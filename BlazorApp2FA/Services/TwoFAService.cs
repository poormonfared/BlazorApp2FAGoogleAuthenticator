using BlazorApp2FA.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorApp2FA.Services;
public class TwoFAService : ITwoFAService
{
    private readonly HttpClient _httpClient;
    private string token;
    public Setup2FactorAuthemticationInfo setup2FactorAuthemticationInfo { get; private set; }

    public TwoFAService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<Setup2FactorAuthemticationInfo> Setup2FA()
    {
        var result = await _httpClient.GetAsync("https://localhost:7232/GoogleAuthenticator");
        if (result.StatusCode == System.Net.HttpStatusCode.OK)
        {
            setup2FactorAuthemticationInfo = await result.Content.ReadFromJsonAsync<Setup2FactorAuthemticationInfo>();
            return setup2FactorAuthemticationInfo;
        }
        return null;
    }

    public async Task<bool> Validate2FA(Validate2FAPin dto)
    {
        var memoryStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(memoryStream, dto);
        memoryStream.Seek(0, SeekOrigin.Begin);
        var requestContent = new StreamContent(memoryStream);
        requestContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

        HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7232/GoogleAuthenticator");
        httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpRequest.Content = requestContent;

        var result = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        if (result.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return await result.Content.ReadFromJsonAsync<bool>();
        }

        return false;
    }
}
