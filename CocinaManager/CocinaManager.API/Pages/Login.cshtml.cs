using CocinaManager.Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace CocinaManager.API.Pages;

public class LoginModel : PageModel
{
    private readonly IUsuarioService _usuarioService;

    [BindProperty] public string NombreUsuario { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;
    public string? Error { get; set; }

    public LoginModel(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToPage("/Index");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var (success, rol) = await _usuarioService.ValidarCredencialesAsync(NombreUsuario, Password);

        if (!success)
        {
            Error = "Usuario o contraseña incorrectos.";
            return Page();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, NombreUsuario),
            new(ClaimTypes.Role, rol)
        };

        var identity = new ClaimsIdentity(claims, "CookieAuth");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("CookieAuth", principal);
        return RedirectToPage("/Index");
    }
}