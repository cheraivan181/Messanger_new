namespace Core.Repositories.Interfaces;

public interface IDialogSecretRepository
{
    Task<Guid> CreateDialogSecretAsync(Guid dialogId, string key, string iv);
    Task<Dictionary<Guid, (string key, string iv)>> GetDialogSecretsAsync(IEnumerable<Guid> dialogIds);
}