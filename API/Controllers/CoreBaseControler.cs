using System.Linq;
using System.Net;
using System.Security.Claims;
using API.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public abstract class CoreBaseControler: ControllerBase
    {
       
        protected OkObjectResult OKResponse<T>(T data, string message = null)
        {
            return Ok(new ResponseJson<T>(data, message));
        }

        protected IActionResult UnauthorizedResponse(string error)
        {
            return new ObjectResult(new ResponseJson<object>(null, error, System.Net.HttpStatusCode.Unauthorized, false))
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
        }

        protected BadRequestObjectResult BadRequestResponse(string error, object data = null)
        {
            return BadRequest(new ResponseJson<object>(data, error, HttpStatusCode.BadRequest, false));            
        }

        protected string? GetUserId()
        {
            return HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }

        protected string GetUserAgent()
        {
            return  HttpContext.Request.Headers["User-Agent"].FirstOrDefault();
        }
        protected string GetUserPlatform()
        {
            string ua = GetUserAgent();
            if (string.IsNullOrEmpty(ua))
            {
                return "Unknown";
            }
            if (ua.Contains("Android"))
                return string.Format("Android {0}", GetMobileVersion(ua, "Android"));

            if (ua.Contains("iPad"))
                return string.Format("iPad OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("iPhone"))
                return string.Format("iPhone OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("Linux") && ua.Contains("KFAPWI"))
                return "Kindle Fire";

            if (ua.Contains("RIM Tablet") || (ua.Contains("BB") && ua.Contains("Mobile")))
                return "Black Berry";

            if (ua.Contains("Windows Phone"))
                return string.Format("Windows Phone {0}", GetMobileVersion(ua, "Windows Phone"));

            if (ua.Contains("Mac OS"))
                return "Mac OS";

            if (ua.Contains("Windows NT 5.1") || ua.Contains("Windows NT 5.2"))
                return "Windows XP";

            if (ua.Contains("Windows NT 6.0"))
                return "Windows Vista";

            if (ua.Contains("Windows NT 6.1"))
                return "Windows 7";

            if (ua.Contains("Windows NT 6.2"))
                return "Windows 8";

            if (ua.Contains("Windows NT 6.3"))
                return "Windows 8.1";

            if (ua.Contains("Windows NT 10"))
                return "Windows 10";

            return "Unknown";
        }

        private string GetMobileVersion(string userAgent, string device)
        {
            var temp = userAgent.Substring(userAgent.IndexOf(device) + device.Length).TrimStart();
            var version = string.Empty;

            foreach (var character in temp)
            {
                var validCharacter = false;
                int test = 0;

                if (int.TryParse(character.ToString(), out test))
                {
                    version += character;
                    validCharacter = true;
                }

                if (character == '.' || character == '_')
                {
                    version += '.';
                    validCharacter = true;
                }

                if (validCharacter == false)
                    break;
            }

            return version;
        }
    }
}
