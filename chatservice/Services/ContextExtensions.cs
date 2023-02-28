using Dapper;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace chatservice.Services
{
    public static class ContextExtensions
    {
        private const string Sql = "EXEC SP_SET_SESSION_CONTEXT @key=N'TenantUID', @value = @tenantUID";

        public static void SetTenantUID(this IDbConnection conn, object? tenantUID)
        {
            if (conn == null)
                return;

            if (tenantUID != null)
            {
                if (!Guid.TryParse(tenantUID.ToString(), out var guidParsed))
                {
                    throw new ValidationException("TenantUID is invalid.");
                }

                conn.Execute(Sql, new { tenantUID });

            }
        }
    }
}
