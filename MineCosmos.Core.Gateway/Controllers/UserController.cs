using MineCosmos.Core.Common.HttpContextUser;
using MineCosmos.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MineCosmos.Core.Gateway.Controllers
{
    [Authorize(AuthenticationSchemes = Permissions.GWName)]
    [Route("/gateway/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;

        public UserController(IUser user)
        {
            _user = user;
        }

        [HttpGet]
        public MessageModel<List<ClaimDto>> MyClaims()
        {
            return new MessageModel<List<ClaimDto>>()
            {
                success = true,
                response = (_user.GetClaimsIdentity().ToList()).Select(d =>
                    new ClaimDto
                    {
                        Type = d.Type,
                        Value = d.Value
                    }
                ).ToList()
            };
        }
    }
    public class ClaimDto
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
