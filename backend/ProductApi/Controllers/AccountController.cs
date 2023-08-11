using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Services;
using ProductApi.Helpers;
using ProductApi.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly UserService _userService;
    private readonly RoleService _roleService;

    public AccountController(
        ILogger<AccountController> logger,
        UserService userService,
        RoleService roleService)
    {
        _logger = logger;
        _userService = userService;
        _roleService = roleService;
    }

    // Registers a new user and saves it.
    // Validates user input.
    [HttpPost]
    [Route("Register")]
    [AllowAnonymous]
    public async Task<ActionResult> RegisterUser(UserRegisterCredentials userCredentials)
    {
        if (!userCredentials.Password.Equals(userCredentials.ConfirmPassword))
        {
            return BadRequest("Passwords don't match");
        }

        if (userCredentials.Username.Length < 1 || userCredentials.Username.Length > 30)
        {
            return BadRequest("Username must be 1-30 characters long");
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

        var newUser = new AppUser(userCredentials.Username, userCredentials.Password, userRoles);
        await _userService.AddUserAsync(newUser);

        _logger.LogInformation($"Registered and created a new user with username '{newUser.Username}'");

        // TODO: Change returned response later
        return Ok();
    }

    /*
    // Login a user and validate JWT token.
    [HttpPost]
    [AllowAnonymous]
    public ActionResult LoginUser()
    {
        return NotFound();
    }
    */
}
