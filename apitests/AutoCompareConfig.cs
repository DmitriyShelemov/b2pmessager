using facadeservice.Dto;

namespace apitests
{
    public static class AutoCompareConfig
    {
        public static void Configure()
        {
            AutoCompare.Comparer.Configure<TenantDto>()
                .Ignore(x => x.TenantUID);

            AutoCompare.Comparer.Configure<ChatDto>()
                .Ignore(x => x.ChatUID);

            AutoCompare.Comparer.Configure<MessageDto>()
                .Ignore(x => x.ChatUID)
                .Ignore(x => x.MessageUID);
        }
    }
}
