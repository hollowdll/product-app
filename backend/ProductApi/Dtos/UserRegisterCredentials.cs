namespace ProductApi.Dtos;

public class UserRegisterCredentials
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string PasswordConfirm { get; set; } = null!;
}