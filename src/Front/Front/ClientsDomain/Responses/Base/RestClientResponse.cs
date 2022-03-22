using System.Net;

namespace Front.Domain.Responses.Base;

public class RestClientResponse<T> where T:class
{
    public SucessResponse<T> SucessResponse { get; set; }
    public bool IsSucess { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public ErrorResponse ErrorResponse { get; set; }
    public  bool IsCanHandleError { get; set; }
}