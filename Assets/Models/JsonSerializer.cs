using System;
using System.Linq;

namespace AssemblyCSharp
{
    public class JsonSerializer
    {
        public static string Serialize(object obj)
        {
            var jsonSerProps = obj.GetType().GetProperties().Where(p => p.IsDefined(typeof(JsonSerializableAttribute), true));

            var json = new SimpleJSON.JSONClass();
            foreach(var property in jsonSerProps) {
                string propType = property.PropertyType.Name.ToLower();
                if(propType.Contains("int")) {
                    json[property.Name].AsInt = (int)property.GetValue(obj, null);
                } else if(propType.Contains("single")) {
                    json[property.Name].AsFloat = (float)property.GetValue(obj, null);
                } else if(propType.Contains("double")) {
                    json[property.Name].AsDouble = (double)property.GetValue(obj, null);
                } else if(propType.Contains("bool")) {
                    json[property.Name].AsBool = (bool)property.GetValue(obj, null);
                } else {
                    json[property.Name] = "complex";
                }
            }
            return json.ToString();
        }
    }
}
