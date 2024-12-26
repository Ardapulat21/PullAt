using System.Net.Http.Headers;
using Newtonsoft.Json;
using PullAt.Interfaces;
using PullAt.Models;
namespace PullAt.Services
{
    public class UserService : IUserService
    {
        private readonly string _apiUrl;
        private readonly string _usersFolder;
        HttpClient _httpClient;
        public UserService(HttpClient httpClient,IConfiguration configuration,IWebHostEnvironment env){
            _apiUrl = Path.Combine(configuration["ApiSettings:APIUrl"],"User");
            _usersFolder = Path.Combine(env.ContentRootPath,"Users");
            _httpClient = httpClient;
        }
        public async Task<List<User>?> GetUsers(){
            var url = Path.Combine(_apiUrl,"GetAllUsers");
            
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
            var url = Path.Combine(_apiUrl,"Register");
            var response = await _httpClient.PostAsJsonAsync(url, user);
            if (response.IsSuccessStatusCode)
                return true;

            return false;
        }
        public async Task<string?> Login(User user){
            var url = Path.Combine(_apiUrl,"Login");
            var response = await _httpClient.PostAsJsonAsync(url, user);
            if (response.IsSuccessStatusCode){
                var resourceFolder = Path.Combine(_usersFolder,user.Username);
                if(!Directory.Exists(resourceFolder))
                    Directory.CreateDirectory(resourceFolder);
                    
                var token = await response.Content.ReadAsStringAsync();
                return token;
            }
            return null;
        }
        public async Task Delete(int id){
            var url = Path.Combine(_apiUrl,"Delete",$"{id}");
            var response = await _httpClient.GetAsync(url);
            var data = await response.Content.ReadAsStringAsync();
        }
        public async Task Edit(User credentials,int id){
            var url = Path.Combine(_apiUrl,"Edit",$"{id}");
            var response = await _httpClient.PostAsJsonAsync(url, credentials);
            if (response.IsSuccessStatusCode){
                var data = await response.Content.ReadAsStringAsync();
            }
        }
    }
}