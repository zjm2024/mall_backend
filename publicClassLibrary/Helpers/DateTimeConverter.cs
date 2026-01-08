using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace publicClassLibrary.Helpers
{

        // 自定义转换器，可放单独文件
        public class DateTimeConverter : System.Text.Json.Serialization.JsonConverter<DateTime>
        {
            private readonly string _format;
            public DateTimeConverter(string format) => _format = format;

            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                => DateTime.Parse(reader.GetString());          // 反序列化按原逻辑

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
                => writer.WriteStringValue(value.ToString(_format)); // 序列化按指定格式
        }

}
