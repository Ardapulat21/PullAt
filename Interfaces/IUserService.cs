using Microsoft.EntityFrameworkCore;
using PullAt.Models;

namespace PullAt.Interfaces
{
    public interface IUserService
    {
        public Task<List<User>?> GetUsers();
        public Task<bool> Add(User credentials);
        public Task Delete(int id);
        public Task Edit(User credentials,int id);
        public Task<bool?> Validate(User credentials);
    }
}