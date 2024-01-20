using NUnit.Framework;
using WpfApp2;

namespace WpfApp2Test1
{
    [TestFixture]
    public class DashboardTests
    {
        [Test]
        public void ParseString_ValidJson_ReturnsExpectedValue()
        {
            // Arrange
            var json = "{\"key\": \"value\"}";
            var bookJson = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);

            // Act
            var result = Dashboard.ParseString(bookJson, "key");

            // Assert
            Assert.AreEqual("value", result);
        }

        [Test]
        public void ParseString_InvalidJson_ReturnsEmptyString()
        {
            // Arrange
            var json = "{}";
            var bookJson = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);

            // Act
            var result = Dashboard.ParseString(bookJson, "key");

            // Assert
            Assert.AreEqual("", result);
        }

        [Test]
        public void ParseInt_ValidJson_ReturnsExpectedValue()
        {
            // Arrange
            var json = "{\"count\": 42}";
            var bookJson = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);

            // Act
            var result = Dashboard.ParseInt(bookJson, "count");

            // Assert
            Assert.AreEqual(42, result);
        }

        [Test]
        public void ParseInt_InvalidJson_ReturnsZero()
        {
            // Arrange
            var json = "{}";
            var bookJson = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);

            // Act
            var result = Dashboard.ParseInt(bookJson, "count");

            // Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void ParseDouble_ValidJson_ReturnsExpectedValue()
        {
            // Arrange
            var json = "{\"rating\": 4.5}";
            var bookJson = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);

            // Act
            var result = Dashboard.ParseDouble(bookJson, "rating");

            // Assert
            Assert.AreEqual(4.5, result);
        }

        [Test]
        public void ParseDouble_InvalidJson_ReturnsNull()
        {
            // Arrange
            var json = "{}";
            var bookJson = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);

            // Act
            var result = Dashboard.ParseDouble(bookJson, "rating");

            // Assert
            Assert.IsNull(result);
        }
    }
}
