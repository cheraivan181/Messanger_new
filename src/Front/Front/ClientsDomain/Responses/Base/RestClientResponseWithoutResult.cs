using System.Net;

namespace Front.Domain.Responses.Base;

public class RestClientResponseWithoutResult
{
    public bool IsSucess { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public ErrorResponse ErrorResponse { get; set; }
    public  bool IsCanHandleError { get; set; }
}