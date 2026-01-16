using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace ToyPlanet.Client.Services
{
    /// <summary>
    /// Сервіс для аутентифікації з використанням Identity Server
    /// </summary>
    public class IdentityServerService
    {
        private readonly string _identityServerUrl = "http://localhost:5000";
        private readonly string _clientId = "toyplanet-client";
        private readonly string _clientSecret = "your-secret-key";
        private string _accessToken;

        /// <summary>
        /// Вхід користувача (Login)
        /// </summary>
        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                var client = new HttpClient();
                var request = new PasswordTokenRequest
                {
                    Address = $"{_identityServerUrl}/connect/token",
                    ClientId = _clientId,
                    ClientSecret = _clientSecret,
                    Scope = "openid profile api",
                    UserName = username,
                    Password = password
                };

                var response = await client.RequestPasswordTokenAsync(request);

                if (response.IsError)
                {
                    Console.WriteLine($"Login failed: {response.Error}");
                    return false;
                }

                _accessToken = response.AccessToken;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during login: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Реєстрація нового користувача
        /// </summary>
        public async Task<bool> RegisterAsync(string username, string email, string password)
        {
            try
            {
                var client = new HttpClient();
                var content = new StringContent(
                    $"{{\"username\":\"{username}\",\"email\":\"{email}\",\"password\":\"{password}\"}}",
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync(
                    $"{_identityServerUrl}/api/account/register",
                    content
                );

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during registration: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Отримання поточного токена доступу
        /// </summary>
        public string GetAccessToken()
        {
            return _accessToken;
        }

        /// <summary>
        /// Вихід користувача (Logout)
        /// </summary>
        public void Logout()
        {
            _accessToken = null;
        }

        /// <summary>
        /// Перевірка наявності токена доступу
        /// </summary>
        public bool IsAuthenticated => !string.IsNullOrEmpty(_accessToken);
    }
}