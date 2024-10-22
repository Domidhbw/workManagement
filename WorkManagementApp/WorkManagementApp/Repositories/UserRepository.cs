using WorkManagementApp.Models;
using Task = System.Threading.Tasks.Task;

namespace WorkManagementApp.Repositories
{
    public class UserRepository : IRepository<User>
    {
        public Task AddAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
