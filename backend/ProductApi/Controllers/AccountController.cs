using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Services;
using ProductApi.Helpers;
using ProductApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ProductApi.Utility;
using Microsoft.Extensions.Options;
using BCrypt.Net;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IOptions<AppJwtConfig> _appJwtConfig;
    private readonly ILogger<AccountController> _logger;
    private readonly UserService _userService;
    private readonly RoleService _roleService;

    public AccountController(
        IOptions<AppJwtConfig> appJwtConfig,
        ILogger<AccountController> logger,
        UserService userService,
        RoleService roleService)
    {
        _appJwtConfig = appJwtConfig;
        _logger = logger;
        _userService = userService;
        _roleService = roleService;
    }

    // Registers a new user and saves it.
    // Validates user input.
    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult> RegisterUser(UserRegisterCredentials userCredentials)
    {
        if (userCredentials.Username.Length < 1 || userCredentials.Username.Length > 30)
        {
            return BadRequest("Username must be 1-30 characters long");
        }

        if (!userCredentials.Password.Equals(userCredentials.PasswordConfirm))
        {
            return BadRequest("Passwords don't match");
        }

        if (userCredentials.Password.Length < 6)
        {
            return BadRequest("Password must be at least 6 characters long");
        }

        // Case sensitive check if username already exists
        var existingUser = await _userService.GetUserByUsernameCaseSensitiveAsync(userCredentials.Username);
        if (existingUser != null)
        {
            if (existingUser.Username.Equals(userCredentials.Username))
            {
                return Conflict("This username is already in use");
            }
        }

        var userRole = await _roleService.GetRoleByNameAsync("User");
        var userRoles = new List<AppRole>();
        userRoles.Add(userRole);

        int rounds = 12;
        string hashedPassword = BCrypt.Net.BCrypt
            .EnhancedHashPassword(userCredentials.Password, HashType.SHA384, rounds);

        var newUser = new AppUser(userCredentials.Username, hashedPassword, userRoles);
        await _userService.AddUserAsync(newUser);

        _logger.LogInformation($"Registered and created a new user with username '{newUser.Username}'");

        // TODO: Change returned response later
        return Ok();
    }

    
    // Logins an existing user.
    // Validates input data and generates a new JWT token.
    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult> LoginUser(UserLoginCredentials userCredentials)
    {
        var user = await _userService.GetUserByUsernameCaseSensitiveAsync(userCredentials.Username);
        if (user == null)
        {
            return NotFound("No user with this username found");
        }

        if (!BCrypt.Net.BCrypt.EnhancedVerify(userCredentials.Password, user.Password))
        {
            return BadRequest("Password is incorrect");
        }

        var claims = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "User")
        });

        var tokenExpirationMinutes = 5;
        var jwtToken = JwtTokenGenerator.GenerateToken(_appJwtConfig.Value, claims, tokenExpirationMinutes);

        return Ok(new { token = jwtToken });
    }

    [HttpGet]
    [Route("Currentuser")]
    public async Task<ActionResult<AppUserDto>> GetCurrentUserData()
    {
        var currentUser = HttpContext.User;
        var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            return user.ToDto();
        }

        return Unauthorized();
    }
}
