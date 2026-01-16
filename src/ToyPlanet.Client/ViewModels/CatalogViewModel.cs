using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ToyPlanet.Client.ViewModels
{
    /// <summary>
    /// Модель для відображення даних про товари
    /// </summary>
    public class ToyItem : IEquatable<ToyItem>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; } = 1;

        public bool Equals(ToyItem other)
        {
            return other != null && this.Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ToyItem);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    /// <summary>
    /// ViewModel для каталогу товарів
    /// </summary>
    public class CatalogViewModel : BaseViewModel
    {
        private ObservableCollection<ToyItem> _toys;
        private ToyItem _selectedToy;
        private string _searchText;

        public CatalogViewModel()
        {
            Title = "Каталог товарів";
            LoadToysAsync();
        }

        /// <summary>
        /// Колекція товарів
        /// </summary>
        public ObservableCollection<ToyItem> Toys
        {
            get => _toys;
            set => SetProperty(ref _toys, value);
        }

        /// <summary>
        /// Обраний товар
        /// </summary>
        public ToyItem SelectedToy
        {
            get => _selectedToy;
            set => SetProperty(ref _selectedToy, value);
        }

        /// <summary>
        /// Текст пошуку
        /// </summary>
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                FilterToys();
            }
        }

        /// <summary>
        /// Завантаження товарів
        /// </summary>
        private async Task LoadToysAsync()
        {
            try
            {
                IsLoading = true;
                
                // Симуляція завантаження даних з API
                await Task.Delay(1000);

                Toys = new ObservableCollection<ToyItem>
                {
                    new ToyItem { Id = 1, Name = "Плюшевий ведмідь", Price = 250m, Category = "М'які іграшки" },
                    new ToyItem { Id = 2, Name = "Конструктор LEGO", Price = 1500m, Category = "Конструктори" },
                    new ToyItem { Id = 3, Name = "Машинка", Price = 150m, Category = "Транспорт" },
                    new ToyItem { Id = 4, Name = "Лялька", Price = 350m, Category = "Ляльки" },
                    new ToyItem { Id = 5, Name = "Головоломка", Price = 100m, Category = "Головоломки" }
                };
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Фільтрування товарів за пошуком
        /// </summary>
        private void FilterToys()
        {
            if (Toys == null || string.IsNullOrWhiteSpace(SearchText))
            {
                _ = LoadToysAsync();
                return;
            }

            var filtered = Toys
                .Where(t => t.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Toys = new ObservableCollection<ToyItem>(filtered);
        }

        /// <summary>
        /// Добавлення товара до кошика
        /// </summary>
        public void AddToCart(ToyItem toy)
        {
            // Реалізація добавлення до кошика
        }
    }
}