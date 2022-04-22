namespace Core.Repositories.Interfaces;

public interface IDialogSecretRepository
{
    Task<Guid> CreateDialogSecretAsync(Guid dialogId, string key);
    Task<Dictionary<Guid, string>> GetDialogSecretsAsync(IEnumerable<Guid> dialogIds);
}