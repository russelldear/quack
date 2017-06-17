using Amazon.Lambda.Core;
using Slight.Alexa.Framework.Models.Requests;
using Slight.Alexa.Framework.Models.Responses;
using System.Threading.Tasks;

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

            if (input.GetRequestType() == typeof(Slight.Alexa.Framework.Models.Requests.RequestTypes.ILaunchRequest))
            {
                LambdaLogger.Log($"Default LaunchRequest made");

                innerResponse = new PlainTextOutputSpeech();
                (innerResponse as PlainTextOutputSpeech).Text = "Metlink gives you real-time public transport information for Wellington, New Zealand.";
            }
            else if (input.GetRequestType() == typeof(Slight.Alexa.Framework.Models.Requests.RequestTypes.IIntentRequest))
            {
                LambdaLogger.Log($"Intent Requested {input.Request.Intent.Name}");

                var station = DefaultStation;

                var responseText = "";

                if (input.Request.Intent.Slots.ContainsKey("station") && !string.IsNullOrWhiteSpace(input.Request.Intent.Slots["station"].Value))
                {
                    station = input.Request.Intent.Slots["station"].Value;
                }

                Task.Run(async () => responseText = await TrainGetter.Get(station)).Wait();

                innerResponse = new PlainTextOutputSpeech();
                (innerResponse as PlainTextOutputSpeech).Text = responseText;
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
