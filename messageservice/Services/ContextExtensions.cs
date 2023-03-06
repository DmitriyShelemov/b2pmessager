using Dapper;
using System.Data;

namespace messageservice.Services
{
    public static class ContextExtensions
    {
        private const string Sql = "EXEC SP_SET_SESSION_CONTEXT @key=N'TenantUID', @value = @tenantUID";

        public static void SetTenantUID(this IDbConnection conn, Guid tenantUID)
        {
            if (conn == null)
                return;

            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            conn.Execute(Sql, new { tenantUID });
        }
    }
}
