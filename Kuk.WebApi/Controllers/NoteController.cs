using Kuk.Common.Messages;
using Kuk.Services.Services.Note.Implementation;
using Kuk.Services.Services.Note.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace Kuk.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : Controller
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpPost]
        [Route("GetAllPaged")]
        public IActionResult GetAllPaged(NoteGetAllPageRequest request)
        {
            var response = _noteService.GetAllPaged(request);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _noteService.GetByIdAsync(id);
            if (response.IsSuccess)
                return Ok(response);
            if (!response.IsSuccess && response.Message == MessagesResource.NotExistData)
                return NotFound(response);
            return BadRequest(response);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(NoteCreateRequest request)
        {
            var response = await _noteService.CreateAsync(request);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(NoteUpdateRequest request)
        {
            var response = await _noteService.UpdateAsync(request);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _noteService.DeleteAsync(id);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }

    }
}
