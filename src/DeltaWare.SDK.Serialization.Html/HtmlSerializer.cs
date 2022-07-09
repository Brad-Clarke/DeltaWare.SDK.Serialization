using DeltaWare.SDK.Core.Caching;
using DeltaWare.SDK.Core.Helpers;
using DeltaWare.SDK.Serialization.Html.Attributes;
using DeltaWare.SDK.Serialization.Html.Types;
using DeltaWare.SDK.Serialization.Types;
using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DeltaWare.SDK.Serialization.Html
{
    public class HtmlSerializer : IHtmlSerializer
    {
        private readonly IAttributeCache _attributeCache;

        private readonly IObjectSerializer _objectSerializer;

        public HtmlSerializer()
        {
            _attributeCache = new AttributeCache();
            _objectSerializer = new ObjectSerializer(_attributeCache);
        }

        public T Deserialize<T>(Stream stream) where T : class
        {
            HtmlDocument htmlDocument = new HtmlDocument();

            htmlDocument.Load(stream);

            HtmlNodeCollection nodes = htmlDocument.DocumentNode.SelectSingleNode("html").ChildNodes;

            T htmlObject = Activator.CreateInstance<T>();

            htmlObject = (T)ProcessParentObject(nodes, htmlObject);

            return htmlObject;
        }

        #region Processing Methods

        private void ProcessChildObject(HtmlNode node, ElementInfo element, object parent = null)
        {
            object child;

            if (element.ChildElement != null)
            {
                child = element.Property.GetValue(parent);

                child ??= Activator.CreateInstance(element.Type);

                ProcessChildObject(node, element.ChildElement, child);
            }
            else
            {
                if (_objectSerializer.CanSerialize(element.Property))
                {
                    string value = GetInnerText(node, element);

                    child = _objectSerializer.Deserialize(value, element.Property);
                }
                else if (element.Type.ImplementsInterface<IList>())
                {
                    child = ProcessListObject(node.ChildNodes, element);
                }
                else
                {
                    child = ProcessParentObject(node.ChildNodes, Activator.CreateInstance(element.Type));
                }
            }

            element.Property.SetValue(parent, child);
        }

        private IList ProcessListObject(HtmlNodeCollection nodes, ElementInfo element)
        {
            Type type = element.Type.GenericTypeArguments.Single();

            List<ElementIndexDynamicAttribute> dynamicIndexers = GetDynamicIndexers(type);

            IList parentObject = GenericTypeHelper.CreateGenericList(type);

            int startingIndex = GetStartingIndex(type);
            int currentIndex = -1;

            foreach (HtmlNode node in nodes)
            {
                if (node.Name == "#text")
                {
                    continue;
                }

                currentIndex++;

                if (currentIndex < startingIndex || TryBuildDynamicIndexers(node))
                {
                    continue;
                }

                object childObject = ProcessParentObject(node.ChildNodes, Activator.CreateInstance(type));

                if (childObject != null)
                {
                    parentObject.Add(childObject);
                }
            }

            return parentObject;

            #region Helper Methods

            bool TryBuildDynamicIndexers(HtmlNode node)
            {
                if (dynamicIndexers.Count == 0)
                {
                    return false;
                }

                int childNodeIndex = -1;

                foreach (ElementIndexDynamicAttribute dynamicIndexer in dynamicIndexers)
                {
                    dynamicIndexer.Index = null;
                }

                foreach (HtmlNode childNode in node.ChildNodes)
                {
                    if (childNode.Name is "#text" or "#comment")
                    {
                        continue;
                    }

                    childNodeIndex++;

                    ElementIndexDynamicAttribute dynamicIndexer = dynamicIndexers.SingleOrDefault(d => d.DoesMatch(childNode));

                    if (dynamicIndexer != null)
                    {
                        dynamicIndexer.Index = childNodeIndex;
                    }
                }

                dynamicIndexers.Clear();

                return true;
            }

            #endregion Helper Methods
        }

        private object ProcessParentObject(HtmlNodeCollection nodes, object parent)
        {
            Type parentType = parent.GetType();

            Dictionary<string, int> elementIndexes = new Dictionary<string, int>();

            foreach (HtmlNode node in nodes)
            {
                int index = UpdateIndex(node);

                if (node.Name is "#text" or "#comment" || !TryGetAssociatedProperty(node, index, parentType, out ElementInfo element))
                {
                    continue;
                }

                ProcessChildObject(node, element, parent);
            }

            return parent;

            #region Helper Methods

            int UpdateIndex(HtmlNode node)
            {
                if (elementIndexes.ContainsKey(node.Name))
                {
                    elementIndexes[node.Name]++;

                    return elementIndexes[node.Name];
                }

                elementIndexes.Add(node.Name, 0);

                return 0;
            }

            #endregion Helper Methods
        }

        #endregion Processing Methods

        #region Shared Helper Methods

        private ElementInfo GetAssociatedProperty(HtmlNode node, int? index, PropertyInfo[] parentProperties)
        {
            ElementInfo matchingElement = null;

            foreach (PropertyInfo property in parentProperties)
            {
                ElementInfo childElement = null;

                if (_attributeCache.TryGetAttribute(property, out IncludePropertiesAttribute _))
                {
                    childElement = GetAssociatedProperty(node, null, property.PropertyType.GetPublicProperties());

                    if (childElement == null)
                    {
                        continue;
                    }
                }
                else
                {
                    if (_attributeCache.TryGetAttribute(property, out ElementAttribute element))
                    {
                        if (!element.IsNode(node))
                        {
                            continue;
                        }

                        if (_attributeCache.TryGetAttribute(property, out MatchElementWithInnerTextAttribute match))
                        {
                            if (!match.DoesNodeMatch(node))
                            {
                                continue;
                            }
                        }

                        if (_attributeCache.TryGetAttribute(property, out ElementIndexAttribute elementIndex))
                        {
                            if (!elementIndex.DoesIndexMatch(index))
                            {
                                continue;
                            }
                        }

                        if (_attributeCache.TryGetAttribute(property, out ElementIndexDynamicAttribute dynamicIndex))
                        {
                            if (dynamicIndex.Index != index)
                            {
                                continue;
                            }
                        }

                        if (_attributeCache.TryGetAttribute(property, out ElementIdAttribute elementId))
                        {
                            if (!elementId.DoesNodeMatch(node))
                            {
                                continue;
                            }
                        }

                        if (_attributeCache.TryGetAttributes(property, out IgnoreElementWithAttributeAttribute[] ignoreElements))
                        {
                            if (ignoreElements.Any(e => e.MatchAttribute(node)))
                            {
                                continue;
                            }
                        }

                        if (_attributeCache.TryGetAttribute(property, out MatchElementWithAttributeAttribute matchAttribute))
                        {
                            if (!matchAttribute.MatchAttribute(node))
                            {
                                continue;
                            }
                        }
                    }
                    else if (!property.Name.ToLower().Equals(node.Name))
                    {
                        continue;
                    }
                }

                var foundElement = new ElementInfo
                {
                    Property = property,
                    Type = property.PropertyType,
                    ChildElement = childElement
                };

                if (matchingElement != null)
                {
                    throw new Exception("Duplicate properties found for element");
                }

                matchingElement = foundElement;
            }

            return matchingElement;
        }

        private List<ElementIndexDynamicAttribute> GetDynamicIndexers(Type type)
        {
            List<ElementIndexDynamicAttribute> dynamicIndexers = new List<ElementIndexDynamicAttribute>();

            foreach (PropertyInfo property in type.GetPublicProperties())
            {
                if (_attributeCache.TryGetAttribute(property, out ElementIndexDynamicAttribute dynamicIndexer))
                {
                    dynamicIndexers.Add(dynamicIndexer);
                }
            }

            return dynamicIndexers;
        }

        private string GetInnerText(HtmlNode node, ElementInfo element)
        {
            string value = node.InnerHtml.Replace("\r\n", "").Trim();

            if (_attributeCache.TryGetAttribute(element.Property, out TrimInnerTextAttribute trimText))
            {
                value = trimText.TrimStart(value);
            }

            if (!_attributeCache.TryGetAttributes(element.Property, out ReplaceInnerTextAttribute[] replaceInnerTexts))
            {
                return value;
            }

            foreach (ReplaceInnerTextAttribute replaceInnerText in replaceInnerTexts)
            {
                value = replaceInnerText.Replace(value);
            }

            return value;
        }

        private int GetStartingIndex(Type type)
        {
            int index = 0;

            if (_attributeCache.TryGetAttribute(type, out StartsAtElementIndexAttribute startAtElement))
            {
                index = startAtElement.Index;
            }

            return index;
        }

        private bool TryGetAssociatedProperty(HtmlNode node, int? index, Type type, out ElementInfo matchingElement)
        {
            matchingElement = GetAssociatedProperty(node, index, type.GetPublicProperties());

            return matchingElement != null;
        }

        #endregion Shared Helper Methods
    }
}