using Front.Domain.Dialogs;

namespace Front
{
    public class StateContainer
    {
        private List<DialogDomainModel> _dialogs = new List<DialogDomainModel>();
        private bool _isAlive;
        private bool _isAuthenticated;
        private bool _isGlobalError;

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



        public event Action? OnChange;


        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
