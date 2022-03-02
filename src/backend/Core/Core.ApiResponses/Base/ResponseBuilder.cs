namespace Core.ApiResponses.Base
{
    public class ResponseBuilder<T> where T:class
    {
        public bool HasError { get; set; }
        public string Error { get; set; }
        public T Response { get; set; }
        public int ResponseType { get; set; }
        public static ResponseBuilder<T> Build(T response, int responseType = 0, bool hasError = false, string error = null)
        {
            return new ResponseBuilder<T>()
            {
                Response = response,
                HasError = hasError,
                Error = error,
                ResponseType = responseType
            };
        }
    }
}
