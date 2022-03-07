namespace Core.ApiResponses.Base
{
    public class SucessResponseBuilder
    {
        public static SucessResponse<T> Build<T>(T response, int responseType = 0) where T:class
        {
            return new SucessResponse<T>()
            {
                Response = response,
                ResponseType = responseType
            };
        }
    }
}
