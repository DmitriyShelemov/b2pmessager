using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace chatservice.Controllers
{
	[Route(TenantRoute + "[controller]")]
	[ApiController]
	//[Authorize]
	public abstract class BaseTenantController : ControllerBase
	{
		public const string TenantRoute = "/api/Tenant/{tenantUID}/";

        public BaseTenantController()
		{
        }
	}
}
