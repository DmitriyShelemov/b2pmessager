namespace apitests
{
    public static class Routes
    {
        public static Uri TenantUri = new Uri("Tenant", UriKind.Relative);
        public static Uri GetTenantUri(Guid uid) => new Uri(TenantUri + @"/" + uid.ToString("D"), UriKind.Relative);


        public static Uri ChatUri = new Uri("Chat", UriKind.Relative);
        public static Uri GetChatUri(Guid tenantuid) => new Uri(GetTenantUri(tenantuid).ToString() + @"/" + ChatUri, UriKind.Relative);
        public static Uri GetChatUri(Guid tenantuid, Guid uid) => new Uri(GetTenantUri(tenantuid).ToString() + @"/" + ChatUri + @"/" + uid.ToString("D"), UriKind.Relative);


        public static Uri MessageUri = new Uri("Message", UriKind.Relative);
        public static Uri GetMessageManyUri(Guid tenantuid, Guid puid) => new Uri(GetChatUri(tenantuid, puid).ToString() + @"/" + MessageUri, UriKind.Relative);
        public static Uri GetMessagePostUri(Guid tenantuid) => new Uri(GetTenantUri(tenantuid).ToString() + @"/" + MessageUri, UriKind.Relative);
        public static Uri GetMessageExactUri(Guid tenantuid, Guid uid) => new Uri(GetTenantUri(tenantuid).ToString() + @"/" + MessageUri + @"/" + uid.ToString("D"), UriKind.Relative);
    }
}
