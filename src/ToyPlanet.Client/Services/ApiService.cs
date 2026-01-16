using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ToyPlanet.Client.Services
{
    /// <summary>
    /// Сервіс для взаємодії з API ToyPlanet
    /// </summary>
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IdentityServerService _authService;

        public ApiService(string baseUrl, IdentityServerService authService)
        {
            _baseUrl = baseUrl;
            _authService = authService;
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Отримання списку товарів
        /// </summary>
        public async Task<string> GetToysAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/api/toys");
                AddAuthorizationHeader(request);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка при завантаженні товарів: {ex.Message}");
            }
        }

        /// <summary>
        /// Отримання деталей товара
        /// </summary>
        public async Task<string> GetToyAsync(int toyId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/api/toys/{toyId}");
                AddAuthorizationHeader(request);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка при завантаженні товара: {ex.Message}");
            }
        }

        /// <summary>
        /// Отримання списку категорій
        /// </summary>
        public async Task<string> GetCategoriesAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/api/categories");
                AddAuthorizationHeader(request);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка при завантаженні категорій: {ex.Message}");
            }
        }

        /// <summary>
        /// Отримання замовлень користувача
        /// </summary>
        public async Task<string> GetOrdersAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/api/orders");
                AddAuthorizationHeader(request);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка при завантаженні замовлень: {ex.Message}");
            }
        }

        /// <summary>
        /// Створення нового замовлення
        /// </summary>
        public async Task<string> CreateOrderAsync(string orderJson)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/api/orders")
                {
                    Content = new StringContent(orderJson, System.Text.Encoding.UTF8, "application/json")
                };
                AddAuthorizationHeader(request);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка при створенні замовлення: {ex.Message}");
            }
        }

        /// <summary>
        /// Отримання статистики
        /// </summary>
        public async Task<string> GetStatisticsAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/api/statistics");
                AddAuthorizationHeader(request);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка при завантаженні статистики: {ex.Message}");
            }
        }

        /// <summary>
        /// Добавлення токена доступу до заголовків запиту
        /// </summary>
        private void AddAuthorizationHeader(HttpRequestMessage request)
        {
            var token = _authService?.GetAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}