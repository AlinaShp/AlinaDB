using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Mahat.Server.Models
{
    [JsonConverter(typeof(TableColConverter))]
    public class TableCol
    {
        public string ColName { get; set; }
        public string DataType { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsNullable { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsUnique { get; set; }
        public bool IsForeignKey { get; set; }
    }

    public class TableColConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TableCol);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var isForeignKey = jsonObject["IsForeignKey"]?.ToObject<bool>() ?? false;

            TableCol tableCol = isForeignKey ? new ForeignKeyTableCol() : new TableCol();

            serializer.Populate(jsonObject.CreateReader(), tableCol);
            return tableCol;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
