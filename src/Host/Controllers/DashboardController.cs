using ApplicationCore.Interfaces;
using ApplicationCore.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.DTOs;
using ApplicationCore.DTOs.User;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Host.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;
        public DashboardController(IDashboardService service)
        {
            _service = service;
        }


        [Route("getData")]
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var result = await _service.GetData();
            return Ok(result);
        }

        [Route("ip")]
        [HttpGet]
        public async Task<IActionResult> GetIpAddress()
        {
            var result = await _service.GetClientIpAddress();
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Response<int>>> create([FromBody] UserTDO request)
        {
            var result = await _service.Create(request);
            return Ok(result);
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult<Response<int>>> Update(int id, [FromBody] UserTDO request)
        {
            var result = await _service.Update(id, request);
            return Ok(result);
        }


        [HttpPost("createLogs")]
        public async Task<ActionResult<Response<int>>> CreateLogs([FromBody] logsDTO request)
        {
            var result = await _service.GetLogsCreate(request);
            return Ok(result);
        }

        [HttpPost("CreateUserlogs")]
        public async Task<ActionResult<Response<int>>>CreateUsuariologs(UserTDO request)
        {
            var result = await _service.Createpersona(request);
            return Ok(result);
        }

        [HttpPut("UpdatePersona")]
        public async Task<ActionResult<Response<int>>> Updatepersona(int id ,[FromBody]UserTDO request)
        {
            var result = await _service.Updatepersona(id, request);
            return Ok(result);

        }

        [HttpDelete("DeletePersona")]
        public async Task<ActionResult<Response<int>>> deletePersona(int id, [FromBody] UserTDO request)
        {
            var result = await _service.Deletepersona(id, request);
            return Ok (result);
        }




    }
}
