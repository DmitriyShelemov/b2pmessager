namespace facadeservice.MessageBus.Interfaces
{
    public interface IEventProcessor<TMessage> where TMessage : class
    {
        Task<string?> ProcessEvent(TMessage message, string src);
    }
}