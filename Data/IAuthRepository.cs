using MyApi.Models;
using System.Threading.Tasks;

namespace MyApi.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User username, string password);

         Task<User> Login(string username, string password);

         Task<bool> UserExists(string username); 

    }
}