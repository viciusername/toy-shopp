using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ToyPlanet.Client.ViewModels
{
    /// <summary>
    /// Базовий клас для всіх ViewModel, які імплементують MVVM паттерн
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        private bool _isLoading;
        private string _title;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Флаг завантаження
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        /// <summary>
        /// Заголовок сторінки
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        /// Встановлення властивості з генерацією события PropertyChanged
        /// </summary>
        protected void SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return;

            backingStore = value;
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Генерація события PropertyChanged
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}