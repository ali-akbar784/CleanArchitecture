namespace CleanArch.Api.Models
{
    public class AuthenticateResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public AuthenticateResponse(string firstName, string lastName, string token)
        {
            FirstName = firstName;
            LastName = lastName;
            Token = token;
        }
    }
}
