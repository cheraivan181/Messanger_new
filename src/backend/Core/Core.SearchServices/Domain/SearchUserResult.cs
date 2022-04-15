namespace Core.SearchServices.Domain;

public class SearchUserResult
{
    public List<SearchUserModel> Result { get; set; }
    public bool IsSucess { get; set; }
    public string ErrorMessage { get; set; }

    public void SetSucessResult(List<SearchUserModel> result)
    {
        Result = result;
        IsSucess = true;
    }

    public void SetErrorResult(string message)
    {
        ErrorMessage = message;
    }

    public void SetServerError()
    {
        ErrorMessage = "Server error. Try to be later";
    }
}

public record SearchUserModel(Guid userId, string UserName, bool isHadDialog);