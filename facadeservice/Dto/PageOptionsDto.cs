using queuemessagelibrary.MessageBus.Interfaces;

namespace facadeservice.Dto
{
    public class PageOptionsDto : IBaseEvent<CrudActionType>, ITenantContext, IParentContext
    {
        private const uint MaxTake = 50;

        public uint Skip { get; set; }

        public uint Take { get; set; } = MaxTake;

        public CrudActionType EventType { get; set; }

        public Guid TenantUID { get; set; }

        public Guid? ParentUID { get; set; }

        public static PageOptionsDto Build(uint? take, uint? skip)
        {
            var opts = new PageOptionsDto();
            if (take.HasValue) { opts.Take = take.Value > MaxTake || take.Value < 1 ? MaxTake : take.Value; }
            if (skip.HasValue) { opts.Skip = skip.Value; }
            return opts;
        }
    }
}
