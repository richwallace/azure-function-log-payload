using Newtonsoft.Json.Linq;

namespace AZFunction.Log.Payload
{
    public class EventSubscriberBase
    {
        protected int? EventKey = -1;

        public EventSubscriberBase() {}

        protected void SetEventKeyFromEventPayload(string eventPayload)
        {
            var eventId = JToken.Parse(eventPayload)["id"];
            if (eventId != null)
            {
                int.TryParse(eventId?.ToString(), out int eventKey);
                EventKey = eventKey;
            }
        }
    }
}