namespace LegacyTest.Models.CompanyAux
{
    public class InvitationCollaborator
    {
        public string Name { get; set; } = string.Empty;    
        public string Email { get; set; } = string.Empty;
        public string? Token { get; set; } = string.Empty;

        public string? Company { get; set; } = string.Empty;

        public string? Sender { get; set; } = string.Empty;

    }
}
