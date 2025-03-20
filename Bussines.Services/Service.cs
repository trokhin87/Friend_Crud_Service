using System.Text;
using System.Text.Json;
using DTO;
using Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.InteropServices.ComTypes;

namespace Bussines.Services;

public class Service:IFriendService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public Service(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["ProxyMicroservice:BaseUrl"];
    }
    public async Task<bool> AddFriendAsync(FriendDTO dto)
    {
        var json = JsonSerializer.Serialize(dto);
        var content=new StringContent(json,Encoding.UTF8,"application/json");
        
        var repsonse= await _httpClient.PostAsync($"{_baseUrl}/api/Friends/add",content);
        return repsonse.IsSuccessStatusCode;
    }

    public async Task<bool> RemoveFriendAsync(DeleteFriendDto dto)
    {
        var json = JsonSerializer.Serialize(dto);
        var content = new StringContent(json,Encoding.UTF8,"application/json");
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri($"{_baseUrl}/api/Friends/delete"),
            Content = content
        };
        var response = await _httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateFriendAsync(FriendDTO dto)
    {
        var json= JsonSerializer.Serialize(dto);
        var content = new StringContent(json,Encoding.UTF8,"application/json");
        var response= await _httpClient.PutAsync($"{_baseUrl}/api/Friends/update",content);
        return response.IsSuccessStatusCode;
    }

    public async Task<WishIdDTO> GetWishIdAsync(AppIDFriendDTO dto)
    {
        var response= await _httpClient.GetAsync($"{_baseUrl}/Friends/api/pozdrik/{dto.Username}/{dto.AppID}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<WishIdDTO>(json,new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    private async Task<PozdrikIdDto?> CreateWishIdAsync(InterestAndWishDTO dto)
    {
        var response = _httpClient.PostAsJsonAsync(_baseUrl + "/api/Friends/pozdrik/create",dto);
        if (!response.IsCompletedSuccessfully)
        {
            return null;
        }
        return await response.Result.Content.ReadFromJsonAsync<PozdrikIdDto>();
    }

    private async Task<bool> SetPozdrIdToFriendAsync(FriendDTO dto ,int pozdrId)
    {
        var response=await  _httpClient.PostAsJsonAsync(_baseUrl + "/api/Friends/pozdrik/create",dto);

    }
    
    public async Task<bool> AddWishAndInterestAsync(InterestAndWishDTO dto, FriendDTO friendDto)
    {
        PozdrikIdDto? pozdrikIdDto = await CreateWishIdAsync(dto);

        if (pozdrikIdDto != null) dto.IdWish = pozdrikIdDto._pozdrikId;
        else
        {
            throw new Exception("PozdrikIdDto is null");
        }

        var json= JsonSerializer.Serialize(dto);
        var content=new StringContent(json,Encoding.UTF8,"application/json");
        var response=await _httpClient.PostAsync($"{_baseUrl}/api/Friends/pozdrik/add",content);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<FriendDTO>> GetFriendsAsync(AppIdDTO dto)
    {
        var response=await  _httpClient.GetAsync($"{_baseUrl}/api/Friends/list/{dto.Id}");
        if (!response.IsSuccessStatusCode)
        {
            return new List<FriendDTO>();
        }
        var json=await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<FriendDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
    
    
}