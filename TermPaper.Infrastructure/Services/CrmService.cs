using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TermPaper.Application.Common;
using TermPaper.Application.Dto.SalesforceDto;
using TermPaper.Application.Interface;
using TermPaper.Infrastructure.Identity;
using TermPaper.Infrastructure.Settings;

namespace TermPaper.Infrastructure.Services;

public class CrmService
{
    private readonly SalesforceSettings _configuration;
    private readonly HttpClient _httpClient;
    private readonly AppUser _appUser;
    public CrmService(IOptions<SalesforceSettings> configuration,HttpClient httpClient,AppUser user)
    {
        _configuration = configuration.Value;
        _httpClient = httpClient;
        _appUser = user;
    }

    public Dictionary<string,string> SalesforceVerificationData()
    {
        return new Dictionary<string, string>
        {
            ["ClientId"]= _configuration.ClientId,
            ["ClientSecret"] = _configuration.ClientSecret,
        };
    }

    public async Task<string> GetSalesforceToken()
    {
        var data = SalesforceVerificationData();
        var content = new FormUrlEncodedContent(data);
        var url = $"https://{_configuration.MyDomain}/services/oauth2/token";
        var response = await _httpClient.PostAsync(url, content);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<Result> CreateAccountSalesforce(string token,SalesforceProfileDto dto)
    {
        var url = $"https://{_configuration.InstanceUrl}/services/data/v62.0/composite/tree/Account";
        var json = JsonSerializer.Serialize(dto);
        var content = new StringContent(
            json, Encoding.UTF8, "application/json");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = _httpClient.PostAsync(url, content);
        return Result.Success();
    }
}