using Amazon.Lambda.Core;
using Slight.Alexa.Framework.Models.Requests;
using Slight.Alexa.Framework.Models.Responses;
using System;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Echo
{
    public class Function
    {
        private const string DefaultStation = "Ava";

        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            Response response;
            IOutputSpeech innerResponse = null;
            var log = context.Logger;

            if (input.GetRequestType() == typeof(Slight.Alexa.Framework.Models.Requests.RequestTypes.ILaunchRequest))
            {
                log.LogLine($"Default LaunchRequest made");

                innerResponse = new PlainTextOutputSpeech();
                (innerResponse as PlainTextOutputSpeech).Text = "Metlink gives you real-time public transport information for Wellington, New Zealand.";
            }
            else if (input.GetRequestType() == typeof(Slight.Alexa.Framework.Models.Requests.RequestTypes.IIntentRequest))
            {
                log.LogLine($"Intent Requested {input.Request.Intent.Name}");

                var station = DefaultStation;

                if (input.Request.Intent.Slots.ContainsKey("station"))
                {
                    station = input.Request.Intent.Slots["station"].Value;
                }

                innerResponse = new PlainTextOutputSpeech();
                (innerResponse as PlainTextOutputSpeech).Text = $"The requested station is {station}.";
            }

            response = new Response();
            response.ShouldEndSession = true;
            response.OutputSpeech = innerResponse;
            SkillResponse skillResponse = new SkillResponse();
            skillResponse.Response = response;
            skillResponse.Version = "1.0";

            return skillResponse;
        }
    }
}
