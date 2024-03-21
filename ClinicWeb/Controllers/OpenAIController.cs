using Microsoft.AspNetCore.Mvc;
using OpenAI_API.Completions;
using OpenAI_API;

namespace ClinicWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenAIController : ControllerBase
    {
        [HttpGet]
        [Route("UseChatGPT")]
        public async Task<IActionResult> UseChatGPT(string query)
        {
            string outputResult = "";
            var openai = new OpenAIAPI("sess-1AqwEyVKkBveLwAFxww04sjdtYuCFulI4GJgrmLa");
            CompletionRequest completionRequest = new CompletionRequest();
            completionRequest.Prompt = query;
            //需要將Model切換至"ChatGPTTurboInstruct"
            completionRequest.Model = OpenAI_API.Models.Model.ChatGPTTurboInstruct;
            completionRequest.MaxTokens = 1024;

            var completions = await openai.Completions.CreateCompletionAsync(completionRequest);

            foreach (var completion in completions.Completions)
            {
                outputResult += completion.Text;
            }

            return Ok(outputResult);

        }
    }
}
