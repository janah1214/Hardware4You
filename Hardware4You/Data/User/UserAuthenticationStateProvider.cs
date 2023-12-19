using Hardware4You.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Hardware4You.Data
{
    public class UserAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
    {
        private readonly UserService _userService;

        public User CurrentUser { get; private set; } = new();

        public UserAuthenticationStateProvider(UserService userService)
        {
            _userService = userService;
            AuthenticationStateChanged += OnAuthenticationStateChangedAsync;
        }

        public async Task LoginAsync(string username, string password)
        {
            var principal = new ClaimsPrincipal();
            var user = _userService.LookupUserInDatabase(username, password);

            if (user is not null)
            {
                await _userService.PersistUserToBrowserAsync(user);
                principal = user.ToClaimsPrincipal();
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        public async Task LogoutAsync()
        {
            await _userService.ClearBrowserUserDataAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var principal = new ClaimsPrincipal();
            var user = await _userService.FetchUserFromBrowserAsync();

            if (user is not null)
            {
                var userInDatabase = _userService.LookupUserInDatabase(user.Username, user.Password);

                if (userInDatabase is not null)
                {
                    principal = userInDatabase.ToClaimsPrincipal();
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
                CurrentUser = User.FromClaimsPrincipal(authenticationState.User);
            }
        }

        public void Dispose() => AuthenticationStateChanged -= OnAuthenticationStateChangedAsync;
    }
}
