using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace CocinaManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RecepcionController : ControllerBase
{
    private readonly IRecepcionViveresService _recepcionService;
    private readonly IMovimientoStockService _movimientoService;

    public RecepcionController( IRecepcionViveresService recepcionViveresService, IMovimientoStockService movimientoService)
    {
        _recepcionService = recepcionViveresService;
        _movimientoService = movimientoService;
    }

    // GET: descargar PDF de una recepción guardada
    [HttpGet("recepcion/{id:guid}/pdf")]
    public async Task<IActionResult> RecepcionPdf(Guid id)
    {
        var recepcion = await _recepcionService.GetByIdAsync(id);
        if(recepcion == null) return NotFound();

        // Cargar logo
        byte[]? logoBytes = null;
        try
        {
            var logoPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "logo-chacabuco.png");
            if (System.IO.File.Exists(logoPath)) logoBytes = System.IO.File.ReadAllBytes(logoPath);
        }
        catch { logoBytes = null; }

        var document = Document.Create(c =>
        {
            c.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Row(row =>
                {
                    row.RelativeColumn().Column(col =>
                    {
                        col.Item().Text($"Recepción de víveres - {recepcion.Fecha:dd/MM/yyyy}").FontSize(16).Bold();
                        col.Item().Text($"Proveedor: {recepcion.Proveedor ?? "-"}    Planilla/Nro: {recepcion.Remito + "-RPO-2026" ?? "-"}").FontSize(10);
                        col.Item().Text($"Entrega recibida por: {recepcion.RecibidoPor ?? "-"}    DNI: {recepcion.DniRecibidoPor ?? "-"}").FontSize(10);
                        if (!string.IsNullOrEmpty(recepcion.Observaciones))
                            col.Item().Text($"Observaciones: {recepcion.Observaciones}").FontSize(10);
                    });
                    row.ConstantColumn(100).AlignRight().Element(el =>
                    {
                        if (logoBytes != null) el.Image(logoBytes).FitArea();
                    });
                });

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(cols =>
                    {
                        cols.RelativeColumn(4);
                        cols.RelativeColumn(1);
                        cols.RelativeColumn(1);
                        cols.RelativeColumn(3);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(6).Text(h => h.Span("Producto").SemiBold());
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(6).Text(h => h.Span("Cant.").SemiBold());
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(6).Text(h => h.Span("Unidad").SemiBold());
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(6).Text(h => h.Span("Observación / Lote").SemiBold());
                    });

                    int idx = 0;
                    foreach (var l in recepcion.Lineas)
                    {
                        var bg = (idx % 2 == 0) ? Colors.White : Colors.Grey.Lighten4;
                        table.Cell().Background(bg).Padding(6).Text(l.NombreProducto);
                        table.Cell().Background(bg).Padding(6).Text(l.Cantidad.ToString("0.##"));
                        table.Cell().Background(bg).Padding(6).Text(l.Unidad ?? "-");
                        table.Cell().Background(bg).Padding(6).Text(l.Observacion ?? "-");
                        idx++;
                    }

                    // Añadir filas vacías para firma/impresión (coinciden con 4 columnas)
                    for (int i = 0; i < 8; i++)
                    {
                        table.Cell().Text(" ");
                        table.Cell().Text(" ");
                        table.Cell().Text(" ");
                        table.Cell().Text(" ");
                        table.Cell().Text(" ");
                    }
                });

                page.Footer().AlignCenter().Text(x => x.Span("ENTREGADO POR: Firma: __________________________ Aclaración: __________________________ Fecha: ____/____/____").FontSize(10));
            });
        });

        using var ms = new MemoryStream();
        document.GeneratePdf(ms);
        var bytes = ms.ToArray();
        return File(bytes, "application/pdf", $"Recepcion_{recepcion.Fecha:yyyyMMdd}_{recepcion.Id}.pdf");
    }

    // Nuevo endpoint: recepción de víveres (solo entradas) en una fecha específica
    [HttpGet("recepcion-dia")]
    public async Task<IActionResult> RecepcionViveresDia(DateTime fecha)
    {
        var lista = (await _movimientoService.GetByFechaTipoAsync(fecha, TipoMovimiento.Entrada))
                        .OrderBy(m => m.NombreProducto)
                        .ToList();

        // Intentar cargar logo
        byte[]? logoBytes = null;
        try
        {
            var logoPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "CM_Logo.png");
            if (System.IO.File.Exists(logoPath))
                logoBytes = System.IO.File.ReadAllBytes(logoPath);
        }
        catch
        {
            logoBytes = null;
        }

        var document = Document.Create(c =>
        {
            c.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Row(row =>
                {
                    row.RelativeColumn().Column(col =>
                    {
                        col.Item().Text($"Recepción de víveres - {fecha:dd/MM/yyyy}").FontSize(16).Bold();
                        col.Item().Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(9);
                        col.Item().Text("Proveedor: ______________________    Remito/Nro: __________________").FontSize(10);
                        col.Item().Text("Entrega recibida por: ______________________    DNI: ______________").FontSize(10);
                    });
                    row.ConstantColumn(100).AlignRight().Element(el =>
                    {
                        if (logoBytes != null) el.Image(logoBytes).FitArea();
                    });
                });

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(cols =>
                    {
                        cols.RelativeColumn(4); // Producto
                        cols.RelativeColumn(1); // Cantidad
                        cols.RelativeColumn(1); // Unidad (si disponible)
                        cols.RelativeColumn(3); // Observación / Lote
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(6).Text(h => h.Span("Producto").SemiBold());
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(6).Text(h => h.Span("Cant.").SemiBold());
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(6).Text(h => h.Span("Unidad").SemiBold());
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(6).Text(h => h.Span("Observación / Lote").SemiBold());
                    });

                    int idx = 0;
                    foreach (var m in lista)
                    {
                        var bg = (idx % 2 == 0) ? Colors.White : Colors.Grey.Lighten4;
                        table.Cell().Background(bg).Padding(6).Text(m.NombreProducto);
                        table.Cell().Background(bg).Padding(6).Text(m.Cantidad.ToString("0.##"));
                        table.Cell().Background(bg).Padding(6).Text("-"); // unidad no disponible en DTO
                        table.Cell().Background(bg).Padding(6).Text(m.Observacion ?? "-");
                        idx++;
                    }

                    // Añadir filas en blanco para impresión (coinciden con 4 columnas)
                    for (int i = 0; i < 8; i++)
                    {
                        table.Cell().Text(" ");
                        table.Cell().Text(" ");
                        table.Cell().Text(" ");
                        table.Cell().Text(" ");
                        table.Cell().Text(" ");
                    }
                });

                page.Footer().AlignCenter().Text(x => x.Span("Firma receptor: __________________________    Fecha: ____/____/____").FontSize(10));
            });
        });

        using var ms = new MemoryStream();
        document.GeneratePdf(ms);
        var bytes = ms.ToArray();
        return File(bytes, "application/pdf", $"Recepcion_Viveres_{fecha:yyyyMMdd}.pdf");
    }

    // POST: crear y guardar recepción (devuelve Id)
    [HttpPost("recepcion")]
    public async Task<IActionResult> CrearRecepcion([FromBody] CreateRecepcionViveresDto dto)
    {
        if (dto == null) return BadRequest();
        var created = await _recepcionService.CreateAsync(dto);
        return CreatedAtAction(nameof(ObtenerRecepcion), new { id = created.Id }, created);
    }

    // GET: obtener recepción por id
    [HttpGet("/{id:guid}")]
    public async Task<IActionResult> ObtenerRecepcion(Guid id)
    {
        var r = await _recepcionService.GetByIdAsync(id);
        if (r == null) return NotFound();
        return Ok(r);
    }

}
