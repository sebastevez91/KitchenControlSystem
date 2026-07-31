using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;
using System.Linq;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Enums;
using CocinaManager.Application.DTOs;

namespace CocinaManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportesController : ControllerBase
{
    private readonly IReporteService _reporteService;
    private readonly ITurnoService _turnoService;
    private readonly IMovimientoStockService _movimientoService;
    private readonly IRecepcionViveresService _recepcionService;

    public ReportesController(
        IReporteService reporteService,
        ITurnoService turnoService,
        IMovimientoStockService movimientoService,
        IRecepcionViveresService recepcionService)
    {
        _reporteService = reporteService;
        _turnoService = turnoService;
        _movimientoService = movimientoService;
        _recepcionService = recepcionService;
    }

    [HttpGet("personal")]
    public async Task<IActionResult> Personal()
    {
        var bytes = await _reporteService.ExportarPersonalAsync();
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"Personal_{DateTime.Now:yyyyMMdd}.xlsx");
    }

    [HttpGet("turnos")]
    public async Task<IActionResult> Turnos()
    {
        var bytes = await _reporteService.ExportarTurnosAsync();
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"Turnos_{DateTime.Now:yyyyMMdd}.xlsx");
    }

    [HttpGet("productos-stock")]
    public async Task<IActionResult> ProductosStock()
    {
        var bytes = await _reporteService.ExportarProductosStockAsync();
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"Stock_{DateTime.Now:yyyyMMdd}.xlsx");
    }

    [HttpGet("movimientos-stock")]
    public async Task<IActionResult> MovimientosStock()
    {
        var bytes = await _reporteService.ExportarMovimientosStockAsync();
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"Movimientos_{DateTime.Now:yyyyMMdd}.xlsx");
    }

    // Nuevo endpoint: genera PDF con los turnos del mes seleccionado con estilo mejorado
    [HttpGet("turnos-mes")]
    public async Task<IActionResult> TurnosMes(int year, int month)
    {
        var inicio = new DateTime(year, month, 1);
        var fin = inicio.AddMonths(1).AddDays(-1);
        // Ordenar por fecha antes de generar el PDF
        var lista = (await _turnoService.GetByRangoAsync(inicio, fin))
                        .OrderBy(t => t.Fecha)
                        .ToList();

        // Helper local para capitalizar la primera letra
        string Capitalize(string s) => string.IsNullOrEmpty(s) ? s : char.ToUpperInvariant(s[0]) + s.Substring(1);

        // Intentar cargar logo desde wwwroot/img/logo-CM-TR.png (si existe)
        byte[]? logoBytes = null;
        try
        {
            var logoPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "logo-chacabuco.png");
            if (System.IO.File.Exists(logoPath))
                logoBytes = System.IO.File.ReadAllBytes(logoPath);
        }
        catch
        {
            // si falla la carga del logo, continuamos sin él
            logoBytes = null;
        }

        byte[] pdfBytes;
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(24);
                page.DefaultTextStyle(x => x.FontSize(11));

                // Header con logo y título
                page.Header().PaddingBottom(8).Row(row =>
                {
                    row.ConstantColumn(80).Height(60).AlignMiddle().AlignLeft().Element(el =>
                    {
                        if (logoBytes != null)
                            el.Image(logoBytes).FitArea();
                        else
                            el.Height(60);
                    });

                    row.RelativeColumn().AlignMiddle().PaddingLeft(8).Column(col =>
                    {
                        col.Item().Text(t => t
                            .Span("Turnos Cocina RPO").FontSize(18).Bold().FontColor(Colors.Blue.Medium)
                        );
                        col.Item().Text(t => t
                            .Span(inicio.ToString("MMMM yyyy")).FontSize(11).SemiBold()
                        );
                        col.Item().AlignRight().Text(t => t.Span($"Generado: {DateTime.Now:dd/MM/yyyy}").FontSize(9));
                    });
                });

                // Contenido: tabla con cabecera destacada y filas
                page.Content().PaddingVertical(8).Column(col =>
                {
                    // Tabla: cabecera
                    col.Item().Text(t => t.Span("Listado de turnos").FontSize(13).Bold());
                    col.Item().PaddingBottom(6);

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3); // Personal
                            columns.RelativeColumn(2); // Fecha
                            columns.RelativeColumn(2); // Turno
                        });

                        // Cabecera con fondo y texto en blanco
                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Grey.Darken1).Padding(6).Text(h => h.Span("Personal").FontColor(Colors.White).SemiBold());
                            header.Cell().Background(Colors.Grey.Darken1).Padding(6).Text(h => h.Span("Fecha").FontColor(Colors.White).SemiBold());
                            header.Cell().Background(Colors.Grey.Darken1).Padding(6).Text(h => h.Span("Turno").FontColor(Colors.White).SemiBold());
                        });

                        // Filas con alternancia de color para mejorar lectura
                        var index = 0;
                        foreach (var t in lista)
                        {
                            var isEven = (index % 2 == 0);
                            var bg = isEven ? Colors.White : Colors.Grey.Lighten4;

                            table.Cell().Background(bg).Padding(6).Text(c => c.Span(t.NombrePersonal));

                            // Fecha en formato "Lunes 2 de Junio 2026"
                            var dayName = CultureInfo.GetCultureInfo("es-ES").DateTimeFormat.GetDayName(t.Fecha.DayOfWeek);
                            var monthName = CultureInfo.GetCultureInfo("es-ES").DateTimeFormat.GetMonthName(t.Fecha.Month);
                            var fechaFormateada = $"{Capitalize(dayName)} {t.Fecha.Day} de {Capitalize(monthName)} {t.Fecha.Year}";

                            table.Cell().Background(bg).Padding(6).Text(c => c.Span(fechaFormateada));
                            table.Cell().Background(bg).Padding(6).Text(c => c.Span(t.TipoTurno.ToString()));

                            index++;
                        }
                    });

                    // Margen inferior
                    col.Item().PaddingTop(8).Element(el => el.LineHorizontal(1).LineColor(Colors.Grey.Medium));
                });

                // Footer con número de página y marca discreta
                page.Footer().AlignCenter().Text(x =>
                {
                    x.DefaultTextStyle(s => s.FontSize(9));
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                    x.EmptyLine();
                    x.Span("Cocina RPO · Turnos").FontColor(Colors.Grey.Darken2);
                });

            });
        });

        using (var ms = new MemoryStream())
        {
            document.GeneratePdf(ms);
            pdfBytes = ms.ToArray();
        }

        return File(pdfBytes, "application/pdf", $"Turnos_{year}{month.ToString("D2")}.pdf");
    }

    // Nuevo endpoint: calcular víveres para una fecha y cantidad de comensales (por defecto 35)
    [HttpGet("viveres-dia")]
    public async Task<IActionResult> ViveresDia(DateTime fecha, int comensales = 35)
    {
        var resultado = await _reporteService.CalcularViveresAsync(fecha, comensales);
        return Ok(resultado);
    }
}