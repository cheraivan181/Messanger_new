namespace Core.IdentityService.Domain
{
    public class SignUpResult
    {
        public bool IsSucess { get; set; }
        public string ErrorMessage { get; set; }
        public string AcessToken { get; set; }
        public string RefreshToken { get; set; }

        public void SetError(string message)
        {
            IsSucess = false;
            ErrorMessage = message;
        }

        public void SetSucessResult(string acessToken, string refreshToken)
        {
            IsSucess = true;
            AcessToken = acessToken;
            RefreshToken = refreshToken;
        }

        public void SetServerError()
        {
            IsSucess = false;
            ErrorMessage = "Server error";
        }
    }
}
