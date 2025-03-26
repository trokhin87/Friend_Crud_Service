using System.Text;
using System.Text.Json;
using DTO;
using Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.InteropServices.ComTypes;

namespace Bussines.Services;

public class FriendService:IFriendService
{
    private readonly HttpClient _httpClient;

    public FriendService(HttpClient httpClient)
    {
        _httpClient = httpClient;

    }
    public async Task<bool> AddFriendAsync(FriendDTO dto)
    {
        var json = JsonSerializer.Serialize(dto);
        var content=new StringContent(json,Encoding.UTF8,"application/json");
        
        var repsonse= await _httpClient.PostAsync($"/api/Friends/add",content);
        return repsonse.IsSuccessStatusCode;
    }

    public async Task<bool> RemoveFriendAsync(DeleteFriendDto dto)
    {
        var json = JsonSerializer.Serialize(dto);
        var content = new StringContent(json,Encoding.UTF8,"application/json");
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri($"/api/Friends/delete"),
            Content = content
        };
        var response = await _httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateFriendAsync(FriendDTO dto)
    {
        var json= JsonSerializer.Serialize(dto);
        var content = new StringContent(json,Encoding.UTF8,"application/json");
        var response= await _httpClient.PutAsync($"/api/Friends/update",content);
        return response.IsSuccessStatusCode;
    }

    public async Task<WishIdDTO> GetWishIdAsync(AppIDFriendDTO dto)
    {
        var response= await _httpClient.GetAsync($"/Friends/api/pozdrik/{dto.Username}/{dto.AppID}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<WishIdDTO>(json,new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    //добавляются интересы и пожелания, отдают id поздрика
    private async Task<PozdrikIdDto?> CreateWishIdAsync(AddIntAndPozhDto dto)
    {
        var response =await _httpClient.PostAsJsonAsync("/api/Friends/pozdrik/create",dto);
        if (!response.IsSuccessStatusCode)
        {
            return null;
            
        }
        return await response.Content.ReadFromJsonAsync<PozdrikIdDto>();
    }

    private async Task<bool> SetPozdrIdToFriendAsync(FriendDTO dto ,int pozdrId)
    {
        AddPozdrIdDto addPozdrIdDto = new AddPozdrIdDto {AppId = dto.AppId,FriendUsername = dto.FriendUsername,PozdrikId = pozdrId};
        var response=await  _httpClient.PostAsJsonAsync( "/api/Friends/SetpozdrId",addPozdrIdDto);
        return response.IsSuccessStatusCode;
    }
    
    public async Task<bool> AddWishAndInterestAsync(AddIntAndPozhDto dto, FriendDTO friendDto)
    {
        PozdrikIdDto? pozdrikIdDto = await CreateWishIdAsync(dto);
        
        if (pozdrikIdDto == null || !pozdrikIdDto._pozdrikId.HasValue)
        {
            throw new Exception("PozdrikIdDto is null or does not have a value");
        }
        dto.IdPozdr = pozdrikIdDto._pozdrikId.Value;


        // var json= JsonSerializer.Serialize(dto);
        // var content=new StringContent(json,Encoding.UTF8,"application/json");
        // var response=await _httpClient.PostAsync($"{_baseUrl}/api/Friends/pozdrik/add",content);
        var secondOperation = await SetPozdrIdToFriendAsync(friendDto, dto.IdPozdr);
        return /*pozdrikIdDto!=null && */secondOperation;
    }

    public async Task<List<FriendDTO>> GetFriendsAsync(AppIdDTO dto)
    {
        var response=await  _httpClient.GetAsync($"/api/Friends/list/{dto.Id}");
        if (!response.IsSuccessStatusCode)
        {
            return new List<FriendDTO>();
        }
        var json=await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<FriendDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<bool> AddFriendWithWishAsync(AddFriendWithWishDTO dto)
    {
        var resposne= await _httpClient.PostAsJsonAsync($"/api/Friends/pozdrik/addWithWish",dto);
        return resposne.IsSuccessStatusCode;
    }
    
} 