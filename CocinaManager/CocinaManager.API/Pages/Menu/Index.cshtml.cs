using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CocinaManager.API.Pages.Menu;

public class MenuSemanalModel : PageModel
{
    private readonly IMenuSemanalService _menuService;
    private readonly IComensalesService _comensalesService;

    public MenuSemanalDto MenuSemanal { get; set; } = new();

    // Para el orden de filas en la tabla
    public static readonly TipoMenu[] OrdenComidas =
        { TipoMenu.Desayuno, TipoMenu.Almuerzo, TipoMenu.Merienda, TipoMenu.Cena, TipoMenu.Postre };

    public MenuSemanalModel(IMenuSemanalService menuService, IComensalesService comensalesService)
    {
        _menuService = menuService;
        _comensalesService = comensalesService;
    }

    public async Task OnGetAsync()
    {
        MenuSemanal = await _menuService.GetSemanaActualAsync();
    }

    public async Task<IActionResult> OnPostComensalesAsync(DateTime fecha, int cantidad)
    {
        await _comensalesService.RegistrarAsync(fecha, cantidad);
        return RedirectToPage();
    }
}