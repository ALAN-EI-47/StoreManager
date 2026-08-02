using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using StoreManager.Web.Models;


namespace StoreManager.Web.Services;

public class AuthService(HttpClient http, IJSRuntime js)
{
    private const string SessionKey = "authenticated_user";

    public UserDto? CurrentUser { get; private set; }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();

    public async Task<bool> LoginAsync(string email, string password)
    {
        var response = await http.PostAsJsonAsync("api/auth/login", new
        {
            Email = email,
            Password = password
        });

        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        var user = await response.Content.ReadFromJsonAsync<UserDto>();
        if (user is null)
        {
            return false;
        }

        CurrentUser = user;

        var json = JsonSerializer.Serialize(user);
        await js.InvokeVoidAsync("sessionStorage.setItem", SessionKey, json);
        NotifyStateChanged();
        return true;
    }

    public async Task<UserDto?> GetCurrentUserAsync()
    {
        if (CurrentUser is not null)
        {
            return CurrentUser;
        }

        var json = await js.InvokeAsync<string?>("sessionStorage.getItem", SessionKey);
        if (string.IsNullOrEmpty(json))
        {
            return null;
        }

        CurrentUser = JsonSerializer.Deserialize<UserDto>(json);
        return CurrentUser;
    }

    public async Task LogoutAsync()
    {
        CurrentUser = null;
        await js.InvokeVoidAsync("sessionStorage.removeItem", SessionKey);
        NotifyStateChanged();
    }
}