using System;
using System.Collections.Generic;
using System.Linq;
using Core.DbModels;
using Core.Repositories.Interfaces;
using Core.Utils;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IUserRepository _userRepository;

        public BaseController(IServiceProvider serviceProvider,
            IUserRepository userRepository)
        {
            _serviceProvider = serviceProvider;
            _userRepository = userRepository;
        }


        [NonAction]
        public Task<User> GetUserAsync()
        {
            var allRoles = User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();

            if (User.Identity != null && User.Identity.IsAuthenticated)
                return _userRepository.GetUserByIdAsync(UserId);

            return null;
        }

        [NonAction]
        public IActionResult CreateResult(object obj)
        {
            return StatusCode(201, obj);
        }
        
        public string UserName
        {
            get
            {
                return User.Identity.Name;
            }
        }

        public Guid UserId
        {
            get
            {
                var userId = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
                    .Select(x => x.Value)
                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(userId))
                {
                    if (Guid.TryParse(userId, out Guid result))
                    {
                        return result;
                    }
                }

                Log.Error($"Cannot parse user id from #({userId})");
                throw new Exception("Cannot parse guid");
            }
        }

        public List<string> RoleNames
        {
            get
            {
                return User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();
            }
        }

        public string TokenId
        {
            get
            {
                return User.Claims.Where(x => x.Type == CommonConstants.UniqueClaimName).Select(x => x.Value).FirstOrDefault();
            }
        }

        public string SessionId
        {
            get
            {
                return User.Claims.Where(x => x.Type == CommonConstants.SessionClaimName)
                    .Select(x => x.Value)
                    .FirstOrDefault();
            }
        }
    }
}
