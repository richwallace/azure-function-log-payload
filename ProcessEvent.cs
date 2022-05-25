// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using AZFunction.Log.Payload.Extensions;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace AZFunction.Log.Payload
{
    public class ProcessEvent
    {
        [FunctionName("EventSubscriber")]
        public static void Run([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            if (null == eventGridEvent || null == eventGridEvent.Data || string.IsNullOrWhiteSpace(eventGridEvent.Data.ToString()))
                throw new ArgumentNullException(nameof(eventGridEvent));
            if (null == log) throw new ArgumentNullException(nameof(log));

            using (log.BeginNamedScope("Message",
            (EventTelemetryPropertyName.EventType, eventGridEvent.EventType),
            (EventTelemetryPropertyName.EventSubject, eventGridEvent.Subject),
            (EventTelemetryPropertyName.EventKey, eventGridEvent.Id)))
            {
                try
                {
                    // Log incoming full event object
                    log.LogInformation("EventGrid payload: {" + EventTelemetryPropertyName.EventPayload + "}", 
                        JsonConvert.SerializeObject(eventGridEvent, Formatting.Indented));

                    // Drill into event and log nested event "data" payload
                    log.LogInformation("Event payload: {" + EventTelemetryPropertyName.EventPayload + "}", 
                        JsonConvert.SerializeObject(eventGridEvent.Data, Formatting.Indented));
                }
                catch (Exception ex)
                {
                    log.LogError(ex, "ERROR::" + $"{ex.Message} {ex}" + "{" + EventTelemetryPropertyName.EventPayload + "}", 
                        eventGridEvent.Data.ToString());
                    throw;
                }
            }
        }
    }
}
