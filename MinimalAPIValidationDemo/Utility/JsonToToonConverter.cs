using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MinimalAPIValidationDemo.Utility
{
    public class JsonToToonConverter
    {
        public string Convert(string json)
        {

            var jsonObject = JArray.Parse(json);
            return ConvertToToon(jsonObject);
        }

        private string ConvertToToon(JObject jsonObject, int indentLevel = 0)
        {
            var toonString = string.Empty;
            var indent = new string(' ', indentLevel * 2);
            foreach (var property in jsonObject.Properties())
            {
                toonString += $"{indent}{property.Name}: {property.Value}\n";
            }
            return toonString.TrimEnd('\n');
        }

        private string ConvertToToon(JArray jsonArray, int indentLevel = 0)
        {
            var toonString = string.Empty;
            var indent = new string(' ', indentLevel * 2);
            foreach (var item in jsonArray)
            {
                if (item is JObject jObjectItem)
                {
                    Console.WriteLine("Item Properties:");
                    foreach (JProperty property in jObjectItem.Properties())
                    {
                        string propertyName = property.Name;
                        JToken propertyValue = property.Value; // This is a JToken, cast if needed
                        toonString += $"{indent}{propertyName}: {propertyValue}\n";
                    }
                }
            }
            return toonString.TrimEnd('\n');
        }
    }
}
