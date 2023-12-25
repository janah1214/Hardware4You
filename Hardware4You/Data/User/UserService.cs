using Hardware4You.Data.User;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Hardware4You.Data
{
    public class UserService : AuthenticationStateProvider, IDisposable
    {
        private UserContext _context;
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        private readonly string _userStorageKey = "userIdentity";

        public Models.User CurrentUser { get; private set; } = new();

        public UserService(UserContext context, ProtectedLocalStorage protectedLocalStorage)
        {
            _context = context;
            _protectedLocalStorage = protectedLocalStorage;
            AuthenticationStateChanged += OnAuthenticationStateChangedAsync;
        }

        public IEnumerable<Models.User> GetListUser()
        {
            try
            {
                return _context.ListUser;
            }
            catch
            {
                throw;
            }
        }

        public void DeleteUser(long id)
        {
            try
            {
                Models.User user = _context.ListUser.Find(id);
                _context.ListUser.Remove(user);
                _context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void InsertUser(Models.User user)
        {
            try
            {
                user.Id = _context.ListUser.Count() + 1;

                _context.ListUser.Add(user);
                _context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public Models.User SingleUser(long id)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(long id, Models.User user)
        {
            try
            {
                var local = _context.Set<Models.User>().Local.FirstOrDefault(entry => entry.Id.Equals(user.Id));

                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public async Task LoginAsync(string username, string password)
        {
            var principal = new ClaimsPrincipal();
            var user = LookupUserInDatabase(username, password);

            if (user is not null)
            {
                await PersistUserToBrowserAsync(user);
                principal = ToClaimsPrincipal(user);
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        public async Task LogoutAsync()
        {
            await ClearBrowserUserDataAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
        }

        private Models.User? LookupUserInDatabase(string username, string password)
        {
            var users = GetListUser();

            if (users != null)
            {
                return users.SingleOrDefault(u => u.Username == username && u.Password == password);
            }
            return null;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var principal = new ClaimsPrincipal();
            var user = await FetchUserFromBrowserAsync();

            if (user is not null)
            {
                var userInDatabase = LookupUserInDatabase(user.Username, user.Password);

                if (userInDatabase is not null)
                {
                    principal = ToClaimsPrincipal(userInDatabase);
                    CurrentUser = userInDatabase;
                }
            }

            return new(principal);
        }

        private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task)
        {
            var authenticationState = await task;

            if (authenticationState is not null)
            {
                CurrentUser = FromClaimsPrincipal(authenticationState.User);
            }
        }

        public async Task PersistUserToBrowserAsync(Models.User user)
        {
            string userJson = JsonConvert.SerializeObject(user);
            await _protectedLocalStorage.SetAsync(_userStorageKey, userJson);
        }

        public async Task<Models.User?> FetchUserFromBrowserAsync()
        {
            try
            {
                var storedUserResult = await _protectedLocalStorage.GetAsync<string>(_userStorageKey);

                if (storedUserResult.Success && !string.IsNullOrEmpty(storedUserResult.Value))
                {
                    var user = JsonConvert.DeserializeObject<Models.User>(storedUserResult.Value);

                    return user;
                }
            }
            catch (InvalidOperationException)
            {
            }

            return null;
        }

        public async Task ClearBrowserUserDataAsync() => await _protectedLocalStorage.DeleteAsync(_userStorageKey);

        public ClaimsPrincipal ToClaimsPrincipal(Models.User user) => new(new ClaimsIdentity(new Claim[]
        {
            new (ClaimTypes.Name, user.Username),
            new (ClaimTypes.Hash, user.Password),
            new (ClaimTypes.Role, user.Role),
        }, "ShopIdentity"));

        public static Models.User FromClaimsPrincipal(ClaimsPrincipal principal) => new()
        {
            Username = principal.FindFirstValue(ClaimTypes.Name),
            Password = principal.FindFirstValue(ClaimTypes.Hash),
            Role = principal.FindFirstValue(ClaimTypes.Role)
        };

        public void Dispose() => AuthenticationStateChanged -= OnAuthenticationStateChangedAsync;
    }
}
