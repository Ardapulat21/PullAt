using System.Net.Http.Headers;
using Newtonsoft.Json;
using PullAt.Interfaces;
using PullAt.Models;
namespace PullAt.Services
{
    public class UserService : IUserService
    {
        HttpClient _httpClient;
        private readonly string apiUrl;
        private readonly string _usersFolder;
        public UserService(HttpClient httpClient,IConfiguration configuration,IWebHostEnvironment env){
            _httpClient = httpClient;
            apiUrl = Path.Combine(configuration["ApiSettings:APIUrl"],"User");
            _usersFolder = Path.Combine(env.ContentRootPath,"Users");
        }
        public async Task<List<User>?> GetUsers(){
            var url = Path.Combine(apiUrl,"GetAllUsers");
            
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<User>>(data);
                return users;
            }
            return null;
        }
        public async Task<bool> Register(User user){
            var url = Path.Combine(apiUrl,"Register");
            var response = await _httpClient.PostAsJsonAsync(url, user);
            if (response.IsSuccessStatusCode)
                return true;

            return false;
        }
        public async Task<string?> Login(User user){
            var url = Path.Combine(apiUrl,"Login");
            var response = await _httpClient.PostAsJsonAsync(url, user);
            if (response.IsSuccessStatusCode){
                var resourceFolder = Path.Combine(_usersFolder,user.Username);
                if(!Directory.Exists(resourceFolder))
                    Directory.CreateDirectory(resourceFolder);

                var token = await response.Content.ReadAsStringAsync();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return token;
            }
            return null;
        }
        public async Task Delete(int id){
            var url = Path.Combine(apiUrl,"Delete",$"{id}");
            var response = await _httpClient.GetAsync(url);
            var data = await response.Content.ReadAsStringAsync();
        }
        public async Task Edit(User credentials,int id){
            var url = Path.Combine(apiUrl,"Edit",$"{id}");
            var response = await _httpClient.PostAsJsonAsync(url, credentials);
            if (response.IsSuccessStatusCode){
                var data = await response.Content.ReadAsStringAsync();
            }
        }
    }
}