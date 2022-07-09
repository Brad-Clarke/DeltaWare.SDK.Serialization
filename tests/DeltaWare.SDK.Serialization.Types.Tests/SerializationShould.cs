using Shouldly;
using System;
using Xunit;

namespace DeltaWare.SDK.Serialization.Types.Tests
{
    public class SerializationShould
    {
        private readonly IObjectSerializer _serializer = new ObjectSerializer();

        #region DateTime Tests

        [Theory]
        [InlineData("4/7/1995", "1995-07-04")]
        [InlineData("4/7/1995 4:35:20 am", "1995-07-04 04:35:20")]
        public void DeserializeDateTime(string dateTimeString, DateTime expectedDateTime)
        {
            DateTime dateTime = Should.NotThrow(() => _serializer.Deserialize<DateTime>(dateTimeString));

            dateTime.ShouldBe(expectedDateTime);
        }

        [Fact]
        public void DeserializeNullDateTime()
        {
            string value = null;

            DateTime? dateTime = Should.NotThrow(() => _serializer.Deserialize<DateTime?>(value));

            dateTime.ShouldBe(null);
        }

        [Theory]
        [InlineData("1995-07-04", "4/7/1995")]
        [InlineData("1995-07-04 04:35:20", "4/7/1995 4:35:20 am")]
        public void SerializeDateTime(DateTime dateTime, string expectedString)
        {
            string dateTimeString = Should.NotThrow(() => _serializer.Serialize(dateTime));

            dateTimeString.ShouldBe(expectedString);
        }

        [Fact]
        public void SerializeNullDateTime()
        {
            string dateTimeString = Should.NotThrow(() => _serializer.Serialize((DateTime?)null));

            dateTimeString.ShouldBe(null);
        }

        #endregion DateTime Tests

        #region Enum Tests

        public enum Gender
        {
            Male,
            Female,
            Unknown
        }

        [Theory]
        [InlineData("Male", Gender.Male)]
        [InlineData("Female", Gender.Female)]
        [InlineData("Unknown", Gender.Unknown)]
        public void DeserializeEnum(string genderString, Gender expectedGender)
        {
            Gender gender = Should.NotThrow(() => _serializer.Deserialize<Gender>(genderString));

            gender.ShouldBe(expectedGender);
        }

        [Theory]
        [InlineData(Gender.Male, "Male")]
        [InlineData(Gender.Female, "Female")]
        [InlineData(Gender.Unknown, "Unknown")]
        public void SerializeEnum(Gender gender, string expectedString)
        {
            string genderString = Should.NotThrow(() => _serializer.Serialize(gender));

            genderString.ShouldBe(expectedString);
        }

        #endregion Enum Tests

        #region Short Tests

        [Theory]
        [InlineData("171", (short)171)]
        [InlineData("25", (short)25)]
        [InlineData(null, null)]
        public void DeserializeShort(string valueString, short? expectedValue)
        {
            short? value = Should.NotThrow(() => _serializer.Deserialize<short?>(valueString));

            value.ShouldBe(expectedValue);
        }

        [Theory]
        [InlineData((short)171, "171")]
        [InlineData((short)25, "25")]
        [InlineData(null, null)]
        public void SerializeShort(short? value, string expectedString)
        {
            string valueString = Should.NotThrow(() => _serializer.Serialize(value));

            valueString.ShouldBe(expectedString);
        }

        #endregion Short Tests

        #region Int Tests

        [Theory]
        [InlineData("171", 171)]
        [InlineData("10512876", 10512876)]
        [InlineData(null, null)]
        public void DeserializeInt(string valueString, int? expectedValue)
        {
            int? value = Should.NotThrow(() => _serializer.Deserialize<int?>(valueString));

            value.ShouldBe(expectedValue);
        }

        [Theory]
        [InlineData(171, "171")]
        [InlineData(10512876, "10512876")]
        [InlineData(null, null)]
        public void SerializeInt(int? value, string expectedString)
        {
            string valueString = Should.NotThrow(() => _serializer.Serialize(value));

            valueString.ShouldBe(expectedString);
        }

        #endregion Int Tests

        #region Long Tests

        [Theory]
        [InlineData("171", 171)]
        [InlineData("10512876", 10512876)]
        [InlineData(null, null)]
        public void DeserializeLong(string valueString, long? expectedValue)
        {
            long? value = Should.NotThrow(() => _serializer.Deserialize<long?>(valueString));

            value.ShouldBe(expectedValue);
        }

        [Theory]
        [InlineData(171, "171")]
        [InlineData(10512876, "10512876")]
        [InlineData(null, null)]
        public void SerializeLong(long? value, string expectedString)
        {
            string valueString = Should.NotThrow(() => _serializer.Serialize(value));

            valueString.ShouldBe(expectedString);
        }

        #endregion Long Tests
    }
}