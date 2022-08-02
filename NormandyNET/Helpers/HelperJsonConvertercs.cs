using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public sealed class HexStringJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(uint).Equals(objectType);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue($"0x{value:x}");
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var str = reader.ReadAsString();
        if (str == null || !str.StartsWith("0x"))
            throw new JsonSerializationException();
        return Convert.ToUInt32(str);
    }
}

public class UintArrayHexStringConverter : JsonConverter
{
    public override void WriteJson(
        JsonWriter writer,
        object value,
        JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        uint[] data = (uint[])value;

        writer.WriteStartArray();

        for (var i = 0; i < data.Length; i++)
        {
            writer.WriteValue($"0x{data[i]:x}");
        }

        writer.WriteEndArray();
    }

    public override object ReadJson(
        JsonReader reader,
        Type objectType,
        object existingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.StartArray)
        {
            var byteList = new List<uint>();

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.String:
                        var str = reader.Value.ToString();
                        if (str == null || !str.StartsWith("0x"))
                            throw new JsonSerializationException();
                        byteList.Add(Convert.ToUInt32(reader.Value));
                        break;

                    case JsonToken.EndArray:
                        return byteList.ToArray();

                    case JsonToken.Comment:

                        break;

                    default:
                        throw new Exception(
                        string.Format(
                            "Unexpected token when reading bytes: {0}",
                            reader.TokenType));
                }
            }

            throw new Exception("Unexpected end when reading bytes.");
        }
        else
        {
            throw new Exception(
                string.Format(
                    "Unexpected token parsing binary. "
                    + "Expected StartArray, got {0}.",
                    reader.TokenType));
        }
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(byte[]);
    }
}