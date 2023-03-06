using System.Data;
using messageservice.Services.Interfaces;

namespace messageservice.Services
{
    public class TenantResolver : ITenantResolver
    {
        private readonly IDbConnection _connection;

        public TenantResolver(IDbConnection connection)
        {
            _connection = connection;
        }

        private Guid TenantUID { get; set; }

        public Guid GetTenantUID() => TenantUID;

        public void SetTenantUID(Guid uid)
        {
            if (uid == Guid.Empty)
                throw new ArgumentOutOfRangeException(nameof(uid));

            _connection.SetTenantUID(uid);
            TenantUID = uid;
        }
    }
}
