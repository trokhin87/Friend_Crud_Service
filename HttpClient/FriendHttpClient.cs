using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DTO;

namespace HttpClients
{
    public class FriendHttpClient
    {
        private readonly HttpClient _httpClient;

        public FriendHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> AddFriendAsync(FriendDTO friendDto)
        {
            var response = await _httpClient.PostAsJsonAsync("add", friendDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveFriendAsync(RemoveFriendDTO removeFriendDto)
        {
            var response = await _httpClient.DeleteAsJsonAsync("delete", removeFriendDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateFriendAsync(FriendDTO friendDto)
        {
            var response = await _httpClient.PutAsJsonAsync("update", friendDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<FriendDTO>> GetFriendsByAppIdAsync(Guid appId)
        {
            return await _httpClient.GetFromJsonAsync<List<FriendDTO>>($"list/{appId}") ?? new List<FriendDTO>();
        }

        public async Task<WishIdDTO?> GetWishIdAsync(AppIDFriendDTO request)
        {
            return await _httpClient.GetFromJsonAsync<WishIdDTO?>($"pozdrik/{request.Username}/{request.AppID}");
        }

        public async Task<bool> AddWishAndInterestAsync(InterestAndWishDTO request)
        {
            var response = await _httpClient.PostAsJsonAsync("pozdrik/add", request);
            return response.IsSuccessStatusCode;
        }
    }
}