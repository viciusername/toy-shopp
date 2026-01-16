using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToyPlanet.Client.ViewModels
{
    /// <summary>
    /// ViewModel для статистики та графіків
    /// </summary>
    public class StatisticsViewModel : BaseViewModel
    {
        private List<StatisticItem> _categorySalesData;
        private List<DailySalesItem> _dailySalesData;

        public StatisticsViewModel()
        {
            Title = "Статистика";
            LoadStatisticsAsync();
        }

        /// <summary>
        /// Дані про продажі по категоріям
        /// </summary>
        public List<StatisticItem> CategorySalesData
        {
            get => _categorySalesData;
            set => SetProperty(ref _categorySalesData, value);
        }

        /// <summary>
        /// Дані про продажі по дням
        /// </summary>
        public List<DailySalesItem> DailySalesData
        {
            get => _dailySalesData;
            set => SetProperty(ref _dailySalesData, value);
        }

        /// <summary>
        /// Завантаження статистики
        /// </summary>
        private async Task LoadStatisticsAsync()
        {
            try
            {
                IsLoading = true;

                // Симуляція завантаження даних з API
                await Task.Delay(1500);

                // Дані про продажі по категоріям
                CategorySalesData = new List<StatisticItem>
                {
                    new StatisticItem { Category = "М'які іграшки", Sales = 15000, Percentage = 25 },
                    new StatisticItem { Category = "Конструктори", Sales = 22000, Percentage = 37 },
                    new StatisticItem { Category = "Транспорт", Sales = 12000, Percentage = 20 },
                    new StatisticItem { Category = "Ляльки", Sales = 7000, Percentage = 12 },
                    new StatisticItem { Category = "Головоломки", Sales = 4000, Percentage = 6 }
                };

                // Дані про продажі по дням
                DailySalesData = new List<DailySalesItem>
                {
                    new DailySalesItem { Date = DateTime.Now.AddDays(-6), Sales = 5000 },
                    new DailySalesItem { Date = DateTime.Now.AddDays(-5), Sales = 6500 },
                    new DailySalesItem { Date = DateTime.Now.AddDays(-4), Sales = 7200 },
                    new DailySalesItem { Date = DateTime.Now.AddDays(-3), Sales = 8100 },
                    new DailySalesItem { Date = DateTime.Now.AddDays(-2), Sales = 9500 },
                    new DailySalesItem { Date = DateTime.Now.AddDays(-1), Sales = 11000 },
                    new DailySalesItem { Date = DateTime.Now, Sales = 12500 }
                };
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

    /// <summary>
    /// Модель для даних статистики по категоріям
    /// </summary>
    public class StatisticItem
    {
        public string Category { get; set; }
        public decimal Sales { get; set; }
        public int Percentage { get; set; }
    }

    /// <summary>
    /// Модель для даних статистики по дням
    /// </summary>
    public class DailySalesItem
    {
        public DateTime Date { get; set; }
        public decimal Sales { get; set; }
    }
}