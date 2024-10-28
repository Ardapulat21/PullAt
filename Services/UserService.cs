using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PullAt.Data;
using PullAt.Interfaces;
using PullAt.Models;

namespace PullAt.Services
{
    public class UserService : IUserService
    {
        HttpClient _httpClient;
        private readonly string? apiUrl;
        public UserService(HttpClient httpClient,IConfiguration configuration){
            _httpClient = httpClient;
            apiUrl = Path.Combine(configuration["ApiSettings:BaseUrl"],"User");
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
        public async Task<bool> Add(User credentials){
            var url = Path.Combine(apiUrl,"Register");
            var response = await _httpClient.PostAsJsonAsync(url, credentials);
            if (response.IsSuccessStatusCode)
                return true;

            return false;
        }
        public async Task<bool?> Validate(User credentials){
            var url = Path.Combine(apiUrl,"Login");
            var response = await _httpClient.PostAsJsonAsync(url, credentials);
            if (response.IsSuccessStatusCode)
                return true;
            return false;
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