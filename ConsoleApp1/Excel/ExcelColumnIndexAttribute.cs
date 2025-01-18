namespace ConsoleApp1.Excel
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ExcelColumnIndexAttribute(int index) : Attribute
    {
        public int Index { get; } = index;
    }
}
