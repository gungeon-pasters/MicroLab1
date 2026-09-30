using System.Net.Http;
using System.Text;
using System.Text.Json;


const string baseUrl = "https://localhost:7216/api/Trainings";

var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
};
using var httpClient = new HttpClient(handler)
{
    BaseAddress = new Uri(baseUrl),
    Timeout = TimeSpan.FromMinutes(5)  
};

// 100 элементов
Console.WriteLine("Тест 1: Добавление 100 элементов...");
for (int i = 1; i <= 100; i++)
{
    var training = new
    {
        title = $"Тренировка_{i}",
        type = i % 2 == 0 ? "Кардио" : "Силовая",
        durationMinutes = 30 + (i % 60),
        trainingDate = DateTime.Today.AddDays(i % 30).ToString("o"),
        approaches = i % 5 + 1,
        weights = i % 50,
        trainerName = $"Тренер_{i}",
        description = $"Тестовая тренировка {i}"
    };

    var json = JsonSerializer.Serialize(training);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await httpClient.PostAsync("", content);
    if (!response.IsSuccessStatusCode)
    {
        Console.WriteLine($"  Ошибка на элементе {i}: {response.StatusCode}");
    }

    if (i % 10 == 0)
        Console.WriteLine($"  Добавлено {i} тренировок...");
}
Console.WriteLine("Тест 1 завершён. Нажмите Enter для следующего теста.");
Console.ReadLine();

// 100 000 элементов
Console.WriteLine();
Console.WriteLine("Тест 2: Добавление 100 000 элементов...");
Console.WriteLine("Это займёт некоторое время...");

var sw = System.Diagnostics.Stopwatch.StartNew();
for (int i = 1; i <= 100_000; i++)
{
    var training = new
    {
        title = $"Тренировка_{i}",
        type = (i % 3) switch { 0 => "Кардио", 1 => "Силовая", _ => "Гибкость" },
        durationMinutes = 20 + (i % 90),
        trainingDate = new DateTime(2020 + i % 5, (i % 12) + 1, (i % 28) + 1).ToString("o"),
        approaches = i % 6 + 1,
        weights = i % 100,
        trainerName = $"Тренер_{i % 50}",
        description = $"Массовый тест {i}"
    };

    var json = JsonSerializer.Serialize(training);
    var content = new StringContent(json, Encoding.UTF8, "application/json");
    await httpClient.PostAsync("", content);

    if (i % 10_000 == 0)
        Console.WriteLine($"  Добавлено {i} тренировок... (прошло {sw.Elapsed.TotalSeconds:F1} с)");
}
sw.Stop();
Console.WriteLine($"Тест 2 завершён за {sw.Elapsed.TotalMinutes:F1} мин. Нажмите Enter...");
Console.ReadLine();

// Удаление 
Console.WriteLine();
Console.WriteLine("Тест 3: Удаление всех элементов...");
try
{
    var resp = await httpClient.DeleteAsync("all");
    if (resp.IsSuccessStatusCode)
        Console.WriteLine("Все тренировки успешно удалены из базы данных.");
    else if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
        Console.WriteLine("База данных уже была пуста.");
    else
        Console.WriteLine($"Ошибка при удалении: {resp.StatusCode}");
}
catch (Exception ex)
{
    Console.WriteLine($"Исключение при удалении: {ex.Message}");
}

Console.WriteLine();
Console.WriteLine("Все тесты выполнены!");
Console.WriteLine("Нажмите любую клавишу, чтобы закрыть...");
Console.ReadKey();