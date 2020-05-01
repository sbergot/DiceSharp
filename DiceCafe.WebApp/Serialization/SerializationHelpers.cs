using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiceCafe.WebApp.Serialization
{
    public static class SerializationHelpers
    {
        public static void ConfigureSerialization(this JsonSerializerOptions options)
        {
            options.Converters.Add(new JsonStringEnumConverter());
        }
    }
}