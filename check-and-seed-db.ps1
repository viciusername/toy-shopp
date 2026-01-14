# Скрипт для перевірки та заповнення бази даних
$dbPath = "src/ToyPlanet.Web/ToyPlanet.db"

Write-Host "Перевірка бази даних: $dbPath" -ForegroundColor Cyan

if (Test-Path $dbPath) {
    Write-Host "База даних знайдена!" -ForegroundColor Green
    
    # Використовуємо System.Data.SQLite через .NET
    Add-Type -Path "C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System.Data.SQLite\v4.0_1.0.118.0__db937bc2d44ff139\System.Data.SQLite.dll" -ErrorAction SilentlyContinue
    
    try {
        $connectionString = "Data Source=$dbPath"
        $connection = New-Object System.Data.SQLite.SQLiteConnection($connectionString)
        $connection.Open()
        
        # Перевіряємо наявність товарів
        $command = $connection.CreateCommand()
        $command.CommandText = "SELECT COUNT(*) FROM Toys"
        $count = $command.ExecuteScalar()
        
        Write-Host "Кількість товарів в базі: $count" -ForegroundColor Yellow
        
        if ($count -eq 0) {
            Write-Host "Додаємо тестові товари..." -ForegroundColor Yellow
            
            $toys = @(
                @(1, "Поні 1", 1990),
                @(2, "Поні 2", 2490),
                @(3, "Поні 3", 2290),
                @(4, "Поні 4", 2590),
                @(5, "Поні 5", 2990),
                @(6, "Поні 6", 3990)
            )
            
            foreach ($toy in $toys) {
                $cmd = $connection.CreateCommand()
                $cmd.CommandText = "INSERT INTO Toys (Id, Name, Price) VALUES (@id, @name, @price)"
                $cmd.Parameters.AddWithValue("@id", $toy[0]) | Out-Null
                $cmd.Parameters.AddWithValue("@name", $toy[1]) | Out-Null
                $cmd.Parameters.AddWithValue("@price", $toy[2]) | Out-Null
                $cmd.ExecuteNonQuery() | Out-Null
                Write-Host "Додано: $($toy[1])" -ForegroundColor Green
            }
        }
        
        # Показуємо всі товари
        $command.CommandText = "SELECT Id, Name, Price FROM Toys"
        $reader = $command.ExecuteReader()
        Write-Host "`nТовари в базі:" -ForegroundColor Cyan
        while ($reader.Read()) {
            Write-Host "ID: $($reader['Id']), Назва: $($reader['Name']), Ціна: $($reader['Price'])" -ForegroundColor White
        }
        
        $connection.Close()
    }
    catch {
        Write-Host "Помилка роботи з базою: $_" -ForegroundColor Red
    }
} else {
    Write-Host "База даних НЕ знайдена!" -ForegroundColor Red
}
