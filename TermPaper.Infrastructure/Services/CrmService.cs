using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TermPaper.Application.Common;
using TermPaper.Application.Dto.SalesforceDto;
using TermPaper.Application.Dto.SupportTicketDto;
using TermPaper.Application.Interface;
using TermPaper.Domain.Enum;
using TermPaper.Infrastructure.Context;
using TermPaper.Infrastructure.Identity;
using TermPaper.Infrastructure.Mappers;
using TermPaper.Infrastructure.Settings;

namespace TermPaper.Infrastructure.Services;

public class CrmService : ICrmService
{
    private readonly SalesforceSettings _salesforceSettings;
    private readonly HttpClient _httpClient;
    private readonly UserManager<AppUser> _userManager;
    private readonly EntraSettings _entraSettings;
    private readonly AppDbContext _context;

    public CrmService(IOptions<SalesforceSettings> salesforceSettings, HttpClient httpClient,
        UserManager<AppUser> userManager, IOptions<EntraSettings> entraSettings, AppDbContext context)
    {
        _salesforceSettings = salesforceSettings.Value;
        _httpClient = httpClient;
        _userManager = userManager;
        _entraSettings = entraSettings.Value;
        _context = context;
    }

    //Salesforce

    public Dictionary<string, string> GetSalesforceAuthData()
    {
        return new Dictionary<string, string>
        {
            ["client_id"] = _salesforceSettings.ClientId,
            ["client_secret"] = _salesforceSettings.ClientSecret,
            ["grant_type"] = "client_credentials",
        };
    }

    public async Task<string> GetSalesforceToken()
    {
        var content = new FormUrlEncodedContent(GetSalesforceAuthData());
        var url = $"https://{_salesforceSettings.MyDomain}/services/oauth2/token";
        var response = await _httpClient.PostAsync(url, content);
        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"Salesforce auth failed: {body}");
        return ParseAccessToken(body);
    }

    public string ParseAccessToken(string authResponse)
    {
        using var doc = JsonDocument.Parse(authResponse);
        return doc.RootElement.GetProperty("access_token").GetString()!;
    }

    public async Task<Result> CreateSalesforceAccount(string token, SalesforceAccountCreateDto dto, string userId)
    {
        var url = $"https://{_salesforceSettings.InstanceUrl}/services/data/v62.0/composite/tree/Account";
        var data = await BuildSalesforceRequest(dto, userId);
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json")
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.SendAsync(request);
        return response.IsSuccessStatusCode ? Result.Success() : Result.Failure(ErrorCode.NotFound);
    }

    public async Task<SalesforceCompositeRequestDto> BuildSalesforceRequest(SalesforceAccountCreateDto dto, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new InvalidOperationException($"Unable to load user with ID '{userId}'.");

        if (user.SalesforceAccountId != null && user.SalesforceContactId != null)
            return new SalesforceCompositeRequestDto();

        return SalesforceMapper.ToCompositeRequestDto(dto, user);
    }

    //Power Automate (OneDrive через Microsoft Graph)

    public Dictionary<string, string> GetEntraAuthData()
    {
        return new Dictionary<string, string>
        {
            ["client_id"] = _entraSettings.ClientId,
            ["client_secret"] = _entraSettings.ClientSecret,
            ["grant_type"] = "client_credentials",
            ["scope"] = "https://graph.microsoft.com/.default"
        };
    }

    public async Task<string> GetGraphToken()
    {
        var content = new FormUrlEncodedContent(GetEntraAuthData());
        var url = $"https://login.microsoftonline.com/{_entraSettings.TenantId}/oauth2/v2.0/token";
        var response = await _httpClient.PostAsync(url, content);
        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"Entra auth failed: {body}");
        return ParseAccessToken(body);
    }
    public async Task<Result> CreateSupportTicket(CreateTicketDto createTicketDto, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new InvalidOperationException($"Unable to load user with ID '{userId}'.");
        var roles = await _userManager.GetRolesAsync(user);
        var positionTitle = await GetPositionTitle(createTicketDto);
        var adminEmails = await GetAdminEmails();
        var data = SupportTicketMapper.ToTicket(createTicketDto, user, roles, positionTitle, adminEmails);
        var token = await GetGraphToken();
        var fileName = $"ticket-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}.json";
        var url = $"https://graph.microsoft.com/v1.0/users/{_entraSettings.Email}/drive/root:/support-tickets/{fileName}:/content";
        var request = new HttpRequestMessage(HttpMethod.Put, url)
        { Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json") };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"OneDrive upload failed: {error}");
        }
        return Result.Success();
    }

    public async Task<List<string>> GetAdminEmails()
    {
        var users = await _userManager.GetUsersInRoleAsync("Admin");
        return users
            .Where(x => x.Email != null)
            .Select(x => x.Email!)
            .ToList();
    }

    public async Task<string?> GetPositionTitle(CreateTicketDto dto)
    {
        if (dto.PositionId == null)
            return null;
        return await _context.Positions
            .Where(x => x.Id == dto.PositionId)
            .Select(x => x.Title)
            .FirstOrDefaultAsync();
    }
}