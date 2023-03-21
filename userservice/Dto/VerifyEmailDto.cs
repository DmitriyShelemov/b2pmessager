using queuemessagelibrary.MessageBus.Interfaces;
using System.Text.Json.Serialization;

namespace userservice.Dto
{
    public class VerifyEmailDto : IBaseEvent<CrudActionType>
    {
        public string? To { get; set; }

        public string? Link { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CrudActionType EventType { get; set; }
    }
}
