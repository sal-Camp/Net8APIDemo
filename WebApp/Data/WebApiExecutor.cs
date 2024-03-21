using System.Text.Json;

namespace WebApp.Data;

public class WebApiExecutor(IHttpClientFactory httpClientFactory) : IWebApiExecutor
{
    private const string ApiName = "ShirtsApi";

    public async Task<T?> InvokeGet<T>(string relativeUrl)
    {
        var httpClient = httpClientFactory.CreateClient(ApiName);
        var request = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
        var response = await httpClient.SendAsync(request);

        await HandlePotentionalError(response);

        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T?> InvokePost<T>(string relativeUrl, T obj)
    {
        var httpClient = httpClientFactory.CreateClient(ApiName);
        var response = await httpClient.PostAsJsonAsync(relativeUrl, obj);

        await HandlePotentionalError(response);

        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task InvokePut<T>(string relativeUrl, T obj)
    {
        var httpClient = httpClientFactory.CreateClient(ApiName);
        var response = await httpClient.PutAsJsonAsync(relativeUrl, obj);

        await HandlePotentionalError(response);
    }

    public async Task InvokeDelete(string relativeUrl)
    {
        var httpClient = httpClientFactory.CreateClient(ApiName);
        var response = await httpClient.DeleteAsync(relativeUrl);

        await HandlePotentionalError(response);
    }

    private async Task HandlePotentionalError(HttpResponseMessage HttpResponse)
    {
        if (!HttpResponse.IsSuccessStatusCode)
        {
            var errorJson = await HttpResponse.Content.ReadAsStringAsync();
            throw new WebApiException(errorJson);
        }
    }
}