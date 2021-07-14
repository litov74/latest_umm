namespace API.Controllers.Models.Accounts.Response
{
    public class SignInResponse
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
        public string Role { get; set; }
    }
}
