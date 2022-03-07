namespace Core.ApiResponses.Base
{
    public class SucessResponse<T> where T:class
    {
        public T Response { get; set; }
        public int ResponseType { get; set; }
    }
}
