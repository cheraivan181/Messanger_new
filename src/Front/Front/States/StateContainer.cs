using Front.Domain.Dialogs;
using Front.Domain.Message;

namespace Front
{
    public class StateContainer
    {
        private List<DialogDomainModel> _dialogs = new List<DialogDomainModel>();
        private Dictionary<Guid, string> _lastMessages = new Dictionary<Guid, string>();

        private bool _isAlive;
        private bool _isAuthenticated;
        private bool _isGlobalError;

        private List<Message> _messages = new List<Message>();

        public bool IsAlive
        {
            get => _isAlive;
            set
            {
                _isAlive = value;
                NotifyStateChanged();
            }
        }

        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set
            {
                _isAuthenticated = value;
                NotifyStateChanged();
            }
        }

        public bool IsGlobalError
        {
            get => _isGlobalError;
            set
            {
                _isGlobalError = value;
                NotifyStateChanged();
            }
        }

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

        #endregion
    }
}
    