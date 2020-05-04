using System.Threading.Tasks;
using System.Collections.Generic;
using MyApi.Models;
using MyApi.Helpers;

namespace MyApi.Data
{
    public interface IDatingRepository
    {
         void add<T>(T entity) where T: class;
         void remove<T>(T entity) where T: class;

         Task<bool> SaveAll();

         Task<PageList<User>> GetUsers(UserParams userParams);
         Task<User> GetUser(int id);

         Task<Photo> GetPhoto(int id);

         Task<Photo> GetMainPhotoForUser(int userId);

         Task<Like> GetLike(int userId, int recipientId);
         Task<Message> GetMessage(int id);
         Task<PageList<Message>> GetMessagesForUser(MessageParams messageParams);  
         Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);
    }
}