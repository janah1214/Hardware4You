using Hardware4You.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;

namespace Hardware4You.Data
{
    public class UserService
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        private readonly string _userStorageKey = "userIdentity";

        public UserService(ProtectedLocalStorage protectedLocalStorage)
        {
            _protectedLocalStorage = protectedLocalStorage;
        }

        public User? LookupUserInDatabase(string username, string password)
        {
            //TODO: bind dbo.User dont use local storage
            var usersFromDatabase = new List<User>()
            {
                new()
                {
                    Username = "Admin",
                    Password = "Admin",
                    Roles = {"admin"}
                },
                new()
                {
                    Username = "Lidl",
                    Password = "Lidl",
                    Roles = {"standard"}
                }
            };

            var foundUser = usersFromDatabase.SingleOrDefault(u => u.Username == username && u.Password == password);

            return foundUser;
        }
       
        public async Task PersistUserToBrowserAsync(User user)
        {
            string userJson = JsonConvert.SerializeObject(user);
            await _protectedLocalStorage.SetAsync(_userStorageKey, userJson);
        }

        public async Task<User?> FetchUserFromBrowserAsync()
        {
            try
            {
                var storedUserResult = await _protectedLocalStorage.GetAsync<string>(_userStorageKey);

                if (storedUserResult.Success && !string.IsNullOrEmpty(storedUserResult.Value))
                {
                    var user = JsonConvert.DeserializeObject<User>(storedUserResult.Value);

                    return user;
                }
            }
            catch (InvalidOperationException)
            {
            }

            return null;
        }

        public async Task ClearBrowserUserDataAsync() => await _protectedLocalStorage.DeleteAsync(_userStorageKey);
    }
}
