﻿using System.ComponentModel.DataAnnotations;
using facadeservice.Services.Interfaces;

namespace facadeservice.Services
{
    public class TenantResolver : ITenantResolver
    {
        private readonly IHttpContextAccessor _accessor;

        public TenantResolver(
            IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Guid GetTenantUID()
        {
            var tenantUID = _accessor?.HttpContext?.GetRouteValue("tenantUID");
            if (tenantUID == null || !Guid.TryParse(tenantUID.ToString(), out var guidParsed))
            {
                throw new ValidationException("TenantUID is invalid.");
            }

            return guidParsed;
        }
    }
}
