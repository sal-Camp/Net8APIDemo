using System.Net.Http.Headers;
using System.Text.Json;
using Newtonsoft.Json;

namespace WebApp.Data;

public class WebApiExecutor(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IWebApiExecutor
{
    private const string ApiName = "ShirtsApi";
    private const string AuthApiName = "AuthorityApi";

    public async Task<T?> InvokeGet<T>(string relativeUrl)
    {
        var httpClient = httpClientFactory.CreateClient(ApiName);
        await AddJwtToHeader(httpClient);
        var request = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
        var response = await httpClient.SendAsync(request);

        await HandlePotentionalError(response);

        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T?> InvokePost<T>(string relativeUrl, T obj)
    {
        var httpClient = httpClientFactory.CreateClient(ApiName);
        await AddJwtToHeader(httpClient);
        var response = await httpClient.PostAsJsonAsync(relativeUrl, obj);

        await HandlePotentionalError(response);

        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task InvokePut<T>(string relativeUrl, T obj)
    {
        var httpClient = httpClientFactory.CreateClient(ApiName);
        await AddJwtToHeader(httpClient);
        var response = await httpClient.PutAsJsonAsync(relativeUrl, obj);

        await HandlePotentionalError(response);
    }

    public async Task InvokeDelete(string relativeUrl)
    {
        var httpClient = httpClientFactory.CreateClient(ApiName);
        await AddJwtToHeader(httpClient);
        var response = await httpClient.DeleteAsync(relativeUrl);

        await HandlePotentionalError(response);
    }

    private async Task HandlePotentionalError(HttpResponseMessage httpResponse)
    {
        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorJson = await httpResponse.Content.ReadAsStringAsync();
            throw new WebApiException(errorJson);
        }
    }

    private async Task AddJwtToHeader(HttpClient httpClient)
    {
        JwtToken? token = null;
        string? strToken = httpContextAccessor.HttpContext?.Session.GetString("access_token");

        if (!string.IsNullOrEmpty(strToken))
        {
            token = JsonConvert.DeserializeObject<JwtToken>(strToken);
        }

        if (token is null || token.ExpiresAt <= DateTime.UtcNow)
        {
            var clientId = configuration.GetValue<string>("ClientId");
            var secret = configuration.GetValue<string>("Secret");

            // Authenticate
            var authorityClient = httpClientFactory.CreateClient(AuthApiName);
            var response = await authorityClient.PostAsJsonAsync("auth", new AppCredential
            {
                ClientId = clientId,
                Secret = secret
            });
            response.EnsureSuccessStatusCode();

            // Get Jwt token from authority
            strToken = await response.Content.ReadAsStringAsync();
            token = JsonConvert.DeserializeObject<JwtToken>(strToken);

            httpContextAccessor.HttpContext?.Session.SetString("access_token", strToken);
        }

        // Pass Jwt token to endpoints through http headers
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken);
    }
}