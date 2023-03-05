using Dapper;
using System.Data;

namespace chatservice.Services
{
    public static class ContextExtensions
    {
        private const string Sql = "EXEC SP_SET_SESSION_CONTEXT @key=N'TenantUID', @value = @tenantUID";

        public static void SetTenantUID(this IDbConnection conn, Guid tenantUID)
        {
            if (conn == null)
                return;

            conn.Execute(Sql, new { tenantUID });
        }
    }
}
