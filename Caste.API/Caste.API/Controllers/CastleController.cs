using Caste.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Caste.API.Controllers
{
    [ApiController]
    [Route("castle")]
    public class CastleController : ControllerBase
    {

        private readonly CastleService _castleService;

        public CastleController(CastleService castleService)
        {
            _castleService = castleService;
        }

        [HttpGet]
        [Route("data")]
        [ProducesResponseType(typeof(List<Castle.ViewModels.CastleResponse>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult> Get()
        {
            var result =  await _castleService.GetCastles();
            return Ok(result);
        }


        [HttpPost()]
        [Route("data")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Consumes("application/json")]
        public async Task<ActionResult> Post([FromBody] Castle.ViewModels.CastleRequest meta)
        {
            var result = await _castleService.createCastle(meta);
            return Ok(result);
        }

        [HttpPost]
        [Route("file")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var result = await _castleService.CreateFile(file);
            return Ok(result);
        }

    }
}
