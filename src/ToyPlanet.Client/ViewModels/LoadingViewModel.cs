using System;
using System.Threading.Tasks;

namespace ToyPlanet.Client.ViewModels
{
    /// <summary>
    /// ViewModel для анімованого екрану завантаження
    /// </summary>
    public class LoadingViewModel : BaseViewModel
    {
        private string _loadingMessage;
        private double _progress;
        private bool _isAnimating;

        public LoadingViewModel()
        {
            Title = "Завантаження";
            LoadingMessage = "Зазвичай займає кілька секунд...";
            IsAnimating = true;
        }

        /// <summary>
        /// Повідомлення про завантаження
        /// </summary>
        public string LoadingMessage
        {
            get => _loadingMessage;
            set => SetProperty(ref _loadingMessage, value);
        }

        /// <summary>
        /// Прогрес завантаження (0-100)
        /// </summary>
        public double Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
        }

        /// <summary>
        /// Флаг для анімації
        /// </summary>
        public bool IsAnimating
        {
            get => _isAnimating;
            set => SetProperty(ref _isAnimating, value);
        }

        /// <summary>
        /// Запуск анімованого завантаження
        /// </summary>
        public async Task StartLoadingAsync(string message = null)
        {
            try
            {
                IsAnimating = true;
                Progress = 0;

                if (!string.IsNullOrEmpty(message))
                {
                    LoadingMessage = message;
                }

                // Анімація прогресу
                for (int i = 0; i <= 100; i += 10)
                {
                    Progress = i;
                    await Task.Delay(100);
                }

                Progress = 100;
                IsAnimating = false;
            }
            catch (Exception ex)
            {
                LoadingMessage = $"Помилка: {ex.Message}";
                IsAnimating = false;
            }
        }

        /// <summary>
        /// Зупинка завантаження
        /// </summary>
        public void StopLoading()
        {
            IsAnimating = false;
            Progress = 0;
        }
    }
}