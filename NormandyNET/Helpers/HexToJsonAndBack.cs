using Newtonsoft.Json;
using System;

namespace NormandyNET
{
    public class HexToJsonAndBack : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(uint) || objectType == typeof(ulong))
            {
                return true;
            }
            return false;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            serializer.Formatting = Formatting.Indented;

            writer.WriteValue($"0x{value:X}");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            serializer.Formatting = Formatting.Indented;

            var str = (string)reader.Value;

            if (str == null || !str.StartsWith("0x"))
            {
                throw new JsonSerializationException();
            }

            if (objectType == typeof(uint))
            {
                var a = Convert.ToUInt32(str, 16);
                return a;
            }

            if (objectType == typeof(ulong))
            {
                var a = Convert.ToUInt64(str, 16);
                return a;
            }

            return 0;
        }
    }
}