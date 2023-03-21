namespace facadeservice.Dto
{
    public class UserActivateDto : GuidDto
    {
        public string? VerificationKey { get; set; }
    }
}
