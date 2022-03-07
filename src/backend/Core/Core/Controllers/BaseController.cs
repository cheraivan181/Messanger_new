using Core.DbModels;
using Core.Repositories.Interfaces;
using Core.Utils;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

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

        public string UserName
        {
            get
            {
                return User.Identity.Name;
            }
        }

        public long UserId
        {
            get
            {
                var userId = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
                    .Select(x => x.Value)
                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(userId))
                {
                    if (long.TryParse(userId, out long result))
                    {
                        return result;
                    }
                }

                Log.Error($"Cannot parse user id from #({userId})");
                return -1;
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
