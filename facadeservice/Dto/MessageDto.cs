namespace facadeservice.Dto
{
    public class MessageDto
    {
        public virtual Guid MessageUID { get; set; }

        public virtual Guid ChatUID { get; set; }

        public string? MessageText { get; set; }
    }
}
