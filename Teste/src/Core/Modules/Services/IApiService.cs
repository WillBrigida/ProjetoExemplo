using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.Modules.Services;

public interface IApiService
{
    Task<T> GetAsync<T>(string uri, bool mock = false, int service = 0);
    Task<T> PutAsync<T>(string uri, object obj, bool mock = false);
    Task<T> PostAsync<T>(string uri, object obj, bool mock = false);
    Task<T> DeleteAsync<T>(string uri, bool mock = false);
    void DefaultRequestHeaders(string token);
    void CleanDefaultRequestHeaders();
}

public class ApiService : IApiService
{
    readonly HttpClient? _httpClient;
    readonly IConnectivityService? _connectivityService;

    public ApiService(HttpClient? httpClient, IConnectivityService connectivityService)
    {
        _httpClient = httpClient;
        _httpClient!.Timeout = TimeSpan.FromSeconds(30);
        _connectivityService = connectivityService;
    }

    public ApiService() { }

    public async Task<T> GetAsync<T>(string uri, bool mock, int service)
    {
        if (mock) return await Mock<T>(uri, service);

        if (!_connectivityService!.IsConnected())
            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(new { Successful = false, Error = "Verifique seu acesso à internete tente novamente" })) ??
                throw new Exception();

        try
        {
            var response = await (_httpClient ?? throw new ArgumentException(nameof(_httpClient)))
                    .GetAsync(uri);

            var result = JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), GetJsonSerializerOptions());

            System.Diagnostics.Debug.WriteLine($"<<<== /n{result}");
            return result ?? throw new Exception();
        }
        catch (Exception ex)
        {
            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(new { Successful = false, Error = ex.ToString() })) ??
                throw new Exception();
        }
    }

    public async Task<T> PutAsync<T>(string uri, object obj, bool mock = false)
    {
        if (mock) return await Mock<T>(uri);

        if (!_connectivityService!.IsConnected())
            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(new { Successful = false, Error = "Verifique seu acesso à internete tente novamente" })) ??
                throw new Exception();

        try
        {
            string json = JsonSerializer.Serialize(obj, GetJsonSerializerOptions());
            System.Diagnostics.Debug.WriteLine($"==>>> /n{json}");
            Console.WriteLine($"==>>> /n{json}");

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await (_httpClient ?? throw new ArgumentException(nameof(_httpClient)))
                .PutAsync(uri, stringContent);

            var result = JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), GetJsonSerializerOptions());
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);


            System.Diagnostics.Debug.WriteLine($"<<<== /n{result}");
            Console.WriteLine($"<<<== /n{result}");

            return result ?? throw new Exception();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Put > Exception");

            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(new { Successful = false, Error = ex.ToString() })) ??
                throw new Exception();
        }
    }

    public async Task<T> PostAsync<T>(string uri, object obj, bool mock)
    {
        if (mock) return await Mock<T>(uri);

        if (!_connectivityService!.IsConnected())
            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(new { Successful = false, Error = "Verifique seu acesso à internete tente novamente" })) ??
                throw new Exception();

        try
        {

            var json = JsonSerializer.Serialize(obj, GetJsonSerializerOptions());
            System.Diagnostics.Debug.WriteLine($"==>>> /n{json}");
            Console.WriteLine($"==>>> /n{json}");

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await (_httpClient ?? throw new ArgumentException(nameof(_httpClient)))
                .PostAsync(uri, stringContent);

            var result = JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), GetJsonSerializerOptions());

            System.Diagnostics.Debug.WriteLine($"<<<== /n{result}");
            Console.WriteLine($"<<<== /n{result}");

            return result ?? throw new Exception();

        }
        catch (Exception ex)
        {
            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(new { Successful = false, Error = ex.ToString() })) ??
                 throw new Exception();
        }
    }

    public async Task<T> DeleteAsync<T>(string uri, bool mock)
    {
        if (mock) return await Mock<T>(uri);

        if (!_connectivityService!.IsConnected())
            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(new { Successful = false, Error = "Verifique seu acesso à internete tente novamente" })) ??
                throw new Exception();

        try
        {
            var response = await (_httpClient ?? throw new ArgumentException(nameof(_httpClient)))
                .DeleteAsync(uri);

            var result = JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), GetJsonSerializerOptions());

            System.Diagnostics.Debug.WriteLine($"<<<== /n{result}");

            return result ?? throw new Exception();
        }
        catch (Exception ex)
        {
            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(new { Successful = false, Error = ex.ToString() })) ??
                 throw new Exception();
        }
    }

    private JsonSerializerOptions GetJsonSerializerOptions()
    {
        return new()
        {
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = null
        };
    }

    public void DefaultRequestHeaders(string token)
        => _httpClient!.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    public void CleanDefaultRequestHeaders() => _httpClient!.DefaultRequestHeaders.Authorization = null;

    private async Task<T> Mock<T>(string uri, int service = 1)
    {
        return await Task.Delay(2000).ContinueWith(_ =>
        {
            object obj = new();

            switch (service)
            {

                default: break;
            }

            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(new { Successful = true, Data = obj! })) ?? throw new Exception(); ;
        });
    }

    public static T TypeConvert<T>(object obj)
    {
        try
        {
            return (T)obj; // Tentativa de conversão
        }
        catch (InvalidCastException)
        {
            return default(T); // Retorna o valor padrão de T se a conversão falhar
        }
    }
}