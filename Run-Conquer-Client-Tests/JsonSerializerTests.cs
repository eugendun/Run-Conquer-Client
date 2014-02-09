using NUnit.Framework;
using System.Diagnostics;
using AssemblyCSharp;

namespace RunConquerClientTests
{
    [TestFixture]
    class JsonSerializerTests
    {
        // TODO objects with serializable attributes

        // TODO objects with only serializable attrubutes

        // TODO objects with partialy serializable properties 

        class SerializableObject
        {
            [JsonSerializable]
            public int Id { get; set; }
            [JsonSerializable]
            public float Position { get; set; }
            [JsonSerializable]
            public double Latitude { get; set; }
            [JsonSerializable]
            public bool JsonAble { get; set; }
        }

        [Test()]
        public void SerializableObjectsDefaultProperties()
        {
            var obj = new SerializableObject();
            var expectedJsonString = "{\"Id\":\"0\", \"Position\":\"0\", \"Latitude\":\"0\", \"JsonAble\":\"false\"}";
            Assert.AreEqual(expectedJsonString, JsonSerializer.Serialize(obj));            
        }
    }
}
