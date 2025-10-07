using Microsoft.AspNetCore.Mvc;
using RoleManagementApi.Dtos;
using RoleManagementApi.Services;
using System.Threading.Tasks;

namespace RoleManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatbotController : ControllerBase
    {
        private readonly IChatbotService _service;

        public ChatbotController(IChatbotService service)
        {
            _service = service;
        }

        [HttpPost("ask")]
        public async Task<ActionResult<ChatbotResponseDto>> Ask([FromBody] ChatbotRequestDto dto)
        {
            var reply = await _service.GetResponseAsync(dto.Message);
            return Ok(new ChatbotResponseDto { Reply = reply });
        }
    }
}
