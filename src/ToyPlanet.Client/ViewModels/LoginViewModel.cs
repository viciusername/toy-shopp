using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ToyPlanet.Client.Services;

namespace ToyPlanet.Client.ViewModels
{
    /// <summary>
    /// ViewModel для сторінки входу (Login Page)
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        private readonly IdentityServerService _identityServerService;
        private string _username;
        private string _password;
        private bool _isLoginButtonEnabled = true;
        private string _errorMessage;

        public LoginViewModel()
        {
            _identityServerService = new IdentityServerService();
            LoginCommand = new AsyncRelayCommand(LoginAsync);
            RegisterCommand = new AsyncRelayCommand(RegisterAsync);
        }

        /// <summary>
        /// Ім'я користувача
        /// </summary>
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        /// <summary>
        /// Повідомлення про помилку
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        /// <summary>
        /// Команда для входу
        /// </summary>
        public ICommand LoginCommand { get; }

        /// <summary>
        /// Команда для реєстрації
        /// </summary>
        public ICommand RegisterCommand { get; }

        /// <summary>
        /// Вхід користувача
        /// </summary>
        private async Task LoginAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                _isLoginButtonEnabled = false;

                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Введіть ім'я користувача та пароль";
                    return;
                }

                var result = await _identityServerService.LoginAsync(Username, Password);

                if (result)
                {
                    ErrorMessage = string.Empty;
                    // Перенаправлення на головну сторінку
                }
                else
                {
                    ErrorMessage = "Невірні дані для входу";
                }
            }
            finally
            {
                IsLoading = false;
                _isLoginButtonEnabled = true;
            }
        }

        /// <summary>
        /// Реєстрація користувача
        /// </summary>
        private async Task RegisterAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Введіть ім'я користувача та пароль";
                    return;
                }

                var result = await _identityServerService.RegisterAsync(Username, "", Password);

                if (result)
                {
                    ErrorMessage = "Реєстрація успішна! Тепер увійдіть.";
                }
                else
                {
                    ErrorMessage = "Помилка реєстрації";
                }
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

    /// <summary>
    /// Команда для MVVM паттерну
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object parameter) => _execute();
    }

    /// <summary>
    /// Асинхронна команда для MVVM паттерну
    /// </summary>
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        private bool _isExecuting;

        public event EventHandler CanExecuteChanged;

        public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => !_isExecuting && (_canExecute?.Invoke() ?? true);

        public async void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    _isExecuting = true;
                    await _execute();
                }
                finally
                {
                    _isExecuting = false;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}