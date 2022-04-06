using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace ConnectionHandler.Hubs;

public class BaseHub : Hub
{
    public string UserName
    {
        get
        {
            return Context.User.Identity.Name;
        }
    }
    
    public string UserId
    {
        get
        {
            return Context.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
                .Select(x => x.Value)
                .FirstOrDefault();
        }
    }
    
    public List<string> RoleNames
    {
        get
        {
            return Context.User.Claims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();
        }
    }

    public string SessionId
    {
        get
        {
            return Context.User.Claims
                .Where(x => x.Type == "sessionId")
                .Select(x => x.Value)
                .FirstOrDefault();
        }
    }
}