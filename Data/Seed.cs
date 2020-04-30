using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using MyApi.Models;


namespace MyApi.Data
{
    public class Seed
    {
        public static void seedData(DataContext context)
        {
            if(!context.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach(var user in users)
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash("password", out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;

                    context.Users.Add(user);
                }

                context.SaveChanges();
            }
        }

        
        private static void CreatePasswordHash(string password, out byte[]  PasswordHash, out byte[] PasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {   
                PasswordSalt = hmac.Key;
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}