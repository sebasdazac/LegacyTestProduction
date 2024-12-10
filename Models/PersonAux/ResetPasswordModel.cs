namespace LegacyTest.Models.PersonAux
{
    public class ResetPasswordModel
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
