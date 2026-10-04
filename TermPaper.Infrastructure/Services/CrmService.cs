using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TermPaper.Application.Common;
using TermPaper.Application.Dto.SalesforceDto;
using TermPaper.Application.Interface;
using TermPaper.Domain.Enum;
using TermPaper.Infrastructure.Identity;
using TermPaper.Infrastructure.Mappers;
using TermPaper.Infrastructure.Settings;

namespace TermPaper.Infrastructure.Services;

public class CrmService: ICrmService
{
    private readonly SalesforceSettings _configuration;
    private readonly HttpClient _httpClient;
    private readonly UserManager<AppUser> _userManager;

    public CrmService(IOptions<SalesforceSettings> configuration, HttpClient httpClient,
        UserManager<AppUser> userManager)
    {
        _configuration = configuration.Value;
        _httpClient = httpClient;
        _userManager = userManager;
    }

    public Dictionary<string, string> SalesforceVerificationData()
    {
        return new Dictionary<string, string>
        {
            ["client_id"] = _configuration.ClientId,
            ["client_secret"] = _configuration.ClientSecret,
            ["grant_type"] = "client_credentials",
        };
    }

    public async Task<string> GetSalesforceAuf()
    {
        var data = SalesforceVerificationData();
        var content = new FormUrlEncodedContent(data);
        var url = $"https://{_configuration.MyDomain}/services/oauth2/token";
        var response = await _httpClient.PostAsync(url, content);
        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"Salesforce auth failed: {body}");
        return GetSalesforceToken(body);
    }

    public string GetSalesforceToken(string authResponse)
    {
        using var doc = JsonDocument.Parse(authResponse);
        return doc.RootElement.GetProperty("access_token").GetString()!;
    }

    public async Task<Result> CreateAccountSalesforce(string token, SalesforceAccountCreateDto dto, string userId)
    {
        var url = $"https://{_configuration.InstanceUrl}/services/data/v62.0/composite/tree/Account";
        var data = await CreateDataAccountSalesforce(dto, userId);

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json")
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);
        return response.IsSuccessStatusCode
            ? Result.Success()
            : Result.Failure(ErrorCode.NotFound);   
    }
    public async Task<SalesforceCompositeRequestDto> CreateDataAccountSalesforce(SalesforceAccountCreateDto dto, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return SalesforceMapper.ToCompositeRequestDto(dto, user);
    }
}