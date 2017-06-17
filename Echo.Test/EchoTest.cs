using Newtonsoft.Json;
using Slight.Alexa.Framework.Models.Requests;
using Slight.Alexa.Framework.Models.Responses;
using Xunit;

namespace Echo.Test
{
    public class EchoTest
    {
        [Fact]
        public void Can_get_wellington()
        {
            var skillRequest = Given_a_request_for_this_station(@", ""value"": ""wellington""");

            var skillResponse = When_I_send_it_to_the_function(skillRequest);

            Then_the_response_is_valid(skillResponse);
        }

        [Fact]
        public void Can_get_ava()
        {
            var skillRequest = Given_a_request_for_this_station(@", ""value"": ""ava""");

            var skillResponse = When_I_send_it_to_the_function(skillRequest);

            Then_the_response_is_valid(skillResponse);
        }

        [Fact]
        public void Can_default_with_no_station()
        {
            var skillRequest = Given_a_request_for_this_station("");

            var skillResponse = When_I_send_it_to_the_function(skillRequest);

            Then_the_response_is_valid(skillResponse);
        }

        private SkillRequest Given_a_request_for_this_station(string station)
        {
            return JsonConvert.DeserializeObject<SkillRequest>(string.Format(Template, station));
        }

        private SkillResponse When_I_send_it_to_the_function(SkillRequest request)
        {
            var function = new Function();

            return function.FunctionHandler(request, null);
        }

        private void Then_the_response_is_valid(SkillResponse response)
        {
            var output = response.Response.OutputSpeech as PlainTextOutputSpeech;

            Assert.NotNull(output);
            Assert.True(output.Text.Contains("The next departure"));
        }

        private const string Template = @"{{
  ""session"": {{
    ""sessionId"": ""SessionId.998c5b13-09bd-4434-ae44-4576c220963e"",
    ""application"": {{
      ""applicationId"": ""amzn1.ask.skill.b22bd929-3d02-4b86-99d3-1203791adea4""
    }},
    ""attributes"": {{}},
    ""user"": {{
      ""userId"": ""amzn1.ask.account.AHPFKMH3ZUSH725A5ZQWFW665FC2CCSHCA6HJ4IZHTPLDPZWN3K3YMURLPCVO5W23DUQ5YKXJSYMU22ZZR376S2ULHRHETVSOVXWK5P3IRWQCEGP3WGK6IZRG33IONEAMGXLBVANAASXMHOLWTWEN3IWQMDIU4II4G7SKB73SD3S3EM2B2KPCHNZOKOWHRZFWHIOINWXDBEYRMI""
    }},
    ""new"": true
  }},
  ""request"": {{
    ""type"": ""IntentRequest"",
    ""requestId"": ""EdwRequestId.51fb17aa-497f-4308-8a3a-ad72ebdd64b9"",
    ""locale"": ""en-US"",
    ""timestamp"": ""2017-06-17T04:02:07Z"",
    ""intent"": {{
      ""name"": ""TrainIntent"",
      ""slots"": {{
        ""station"": {{
          ""name"": ""station""{0}
        }}
      }}
    }}
  }},
  ""version"": ""1.0""
}}";
    }
}
