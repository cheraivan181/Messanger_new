using Front.Domain.Dialogs;
using Front.Domain.Message;

namespace Front
{
    public class MessageDialogStateContainer
    {
        private List<DialogDomainModel> _dialogs = new List<DialogDomainModel>();
        private Dictionary<Guid, string> _lastMessages = new Dictionary<Guid, string>();
        private List<Message> _messages = new List<Message>();
        private Dictionary<string, bool> _dialogRequests = new Dictionary<string, bool>();

        public List<DialogDomainModel> Dialogs
        {
            get
            {
                return _dialogs;
            }
            set
            {
                _dialogs = value;
                NotifyStateChanged();
            }
        }

        public List<Message> Messages
        {
            get => _messages;
            set
            {
                _messages = value;
                NotifyStateChanged();
            }
        }

        public Dictionary<Guid, string> LastMessages
        {
            get => _lastMessages;
            set
            {
                _lastMessages = value;
                NotifyStateChanged();
            }
        }

        public Dictionary<string, bool> DialogRequests
        {
            get => _dialogRequests;
            set
            {
                _dialogRequests = value;
                NotifyStateChanged();
            }
        }


        public event Action? OnChange;
        public void NotifyStateChanged() => OnChange?.Invoke();


        #region methods

        public void AddDialog(DialogDomainModel dialog)
        {
            Dialogs.Add(dialog);
            NotifyStateChanged();
        }

        public void AddMessage(Message message)
        {
            Messages.Add(message);
            NotifyStateChanged();
        }

        public void AddLastMessage(Guid dialogId, string message)
        {
            _lastMessages[dialogId] = message;
            NotifyStateChanged();
        }

        public void AddDialogRequest(string userName)
        {
            DialogRequests.Add(userName, false);
            NotifyStateChanged();
        }

        public void SetConfirmDialogRequest(string userName)
        {
            DialogRequests[userName] = true;
            NotifyStateChanged();
        }

        #endregion
    }
}
    