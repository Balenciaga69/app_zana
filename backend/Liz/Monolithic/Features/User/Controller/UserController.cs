using Microsoft.AspNetCore.Mvc;

namespace Monolithic.Features.User.Controller
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        #region User Management

        [HttpGet("{id}")]
        public IActionResult GetUserById(string id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("me")]
        public IActionResult GetMe()
        {
            throw new NotImplementedException();
        }

        [HttpPut("me/nickname")]
        public IActionResult UpdateNickname([FromBody] object body)
        {
            throw new NotImplementedException();
        }

        [HttpGet("me/connections")]
        public IActionResult GetMyConnections()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Status Queries

        [HttpGet("{id}/is-online")]
        public IActionResult IsUserOnline(string id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("online-stats")]
        public IActionResult GetOnlineStats()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}/rooms")]
        public IActionResult GetUserRooms(string id)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Device Management

        [HttpPost("device-register")]
        public IActionResult RegisterDevice([FromBody] object body)
        {
            throw new NotImplementedException();
        }

        [HttpPost("device-verify")]
        public IActionResult VerifyDevice([FromBody] object body)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
