namespace Core.ApiResponses.Base
{
    public class ErrorResponseBuilder
    {
        public static ErrorResponse Build(string error, int responseType = 0) =>
            new ErrorResponse() { Error = error, ResponseType = 0 };
    }
}
