# OAuth2 Integration з OpenIddict

## Що було налаштовано:

### 1. NuGet пакети
- `OpenIddict.AspNetCore` - основний пакет
- `OpenIddict.EntityFrameworkCore` - інтеграція з EF Core

### 2. Конфігурація (appsettings.json)
```json
{
  "OAuth2": {
    "Google": {
      "ClientId": "...",
      "ClientSecret": "..."
    },
    "OpenIddict": {
      "Issuer": "https://localhost:5001",
      "EncryptionKey": "DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=",
      "SigningKey": "DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY="
    }
  }
}
```

### 3. Endpoints OAuth2
- **Authorization**: `GET /connect/authorize`
- **Token**: `POST /connect/token`
- **UserInfo**: `GET /connect/userinfo`

### 4. Автоматична ініціалізація
При старті додатку автоматично створюється клієнт:
- **ClientId**: `toyplanet-web`
- **ClientSecret**: `toyplanet-secret`
- **RedirectUris**: `https://localhost:5001/signin-oidc`, `https://localhost:5001/callback`

### 5. Підтримувані потоки
- Authorization Code Flow
- Refresh Token Flow
- Client Credentials Flow

## Як використовувати:

### Для тестування через Postman:

#### 1. Отримати Access Token (Client Credentials):
```http
POST https://localhost:5001/connect/token
Content-Type: application/x-www-form-urlencoded

grant_type=client_credentials
&client_id=toyplanet-web
&client_secret=toyplanet-secret
&scope=openid profile email
```

#### 2. Використати токен:
```http
GET https://localhost:5001/connect/userinfo
Authorization: Bearer {access_token}
```

### Для інтеграції з іншими додатками:

Інші додатки можуть використовувати цей сервер як OAuth2 провайдер:
- **Issuer**: `https://localhost:5001`
- **Client ID**: `toyplanet-web`
- **Client Secret**: `toyplanet-secret`

### Додавання нових клієнтів:

Відредагуйте `OpenIddictWorker.cs` для додавання нових OAuth2 клієнтів.

## Наступні кроки:

1. **Міграція БД**: Потрібно створити міграцію для OpenIddict таблиць
2. **Інтеграція з User**: Зв'язати OAuth2 з таблицею користувачів
3. **Consent Screen**: Додати екран згоди для авторизації
4. **Scopes**: Налаштувати додаткові scopes для API
