using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Enums;

namespace CocinaManager.Application.Services;

public class MenuSemanalService : IMenuSemanalService
{
    private readonly IPlanMenuRepository _planRepo;
    private readonly IRegistroComensalesRepository _comensalesRepo;

    private static readonly TipoMenu[] OrdenComidas =
        { TipoMenu.Desayuno, TipoMenu.Almuerzo, TipoMenu.Merienda, TipoMenu.Cena, TipoMenu.Postre };

    public MenuSemanalService(IPlanMenuRepository planRepo, IRegistroComensalesRepository comensalesRepo)
    {
        _planRepo = planRepo;
        _comensalesRepo = comensalesRepo;
    }

    public async Task<MenuSemanalDto> GetSemanaActualAsync()
    {
        var planes = await _planRepo.GetAllWithItemsAsync();

        var hoy = DateTime.Today;
        var offset = ((int)hoy.DayOfWeek + 6) % 7; // días desde el lunes (DayOfWeek.Sunday=0)
        var lunes = hoy.AddDays(-offset);
        var fechasSemana = Enumerable.Range(0, 7).Select(i => lunes.AddDays(i)).ToList();

        var comensales = await _comensalesRepo.GetByFechasAsync(fechasSemana);

        var dto = new MenuSemanalDto();
        for (int i = 0; i < 7; i++)
        {
            var diaSemana = (DiaSemana)(i + 1);
            var fecha = fechasSemana[i];

            var diaDto = new DiaMenuDto
            {
                DiaSemana = diaSemana,
                Fecha = fecha,
                CantidadComensales = comensales.FirstOrDefault(c => c.Fecha == fecha)?.Cantidad
            };

            foreach (var tipo in OrdenComidas)
            {
                var plan = planes.FirstOrDefault(p => p.DiaSemana == diaSemana && p.TipoMenu == tipo);
                diaDto.Comidas.Add(new ComidaDto
                {
                    TipoMenu = tipo,
                    Recetas = plan?.Items
                        .Select(it => new PlanMenuItemDto(it.Id, it.RecetaId, it.Receta?.Nombre, it.Rol))
                        .ToList() ?? new List<PlanMenuItemDto>()
                });
            }
            dto.Dias.Add(diaDto);
        }
        return dto;
    }
}
