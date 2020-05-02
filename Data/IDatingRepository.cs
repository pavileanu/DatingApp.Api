using System.Threading.Tasks;
using System.Collections.Generic;
using MyApi.Models;

namespace MyApi.Data
{
    public interface IDatingRepository
    {
         void add<T>(T entity) where T: class;
         void remove<T>(T entity) where T: class;

         Task<bool> SaveAll();

         Task<IEnumerable<User>> GetUsers();
         Task<User> GetUser(int id);

         Task<Photo> GetPhoto(int id);

    }
}