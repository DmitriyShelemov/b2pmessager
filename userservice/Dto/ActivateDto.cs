namespace userservice.Dto
{
    public class ActivateDto : GuidDto
    {
        public string? VerificationKey { get; set; }
    }
}
