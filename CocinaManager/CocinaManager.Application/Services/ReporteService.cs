using ClosedXML.Excel;
using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CocinaManager.Application.Services;

public class ReporteService : IReporteService
{
    private readonly IPersonalService _personalService;
    private readonly ITurnoService _turnoService;
    private readonly IProductoService _productoService;
    private readonly IMovimientoStockService _movimientoService;
    private readonly IRecetaService _recetaService;

    public ReporteService(
        IPersonalService personalService,
        ITurnoService turnoService,
        IProductoService productoService,
        IMovimientoStockService movimientoService,
        IRecetaService recetaService)
    {
        _personalService = personalService;
        _turnoService = turnoService;
        _productoService = productoService;
        _movimientoService = movimientoService;
        _recetaService = recetaService;
    }

    public async Task<byte[]> ExportarPersonalAsync()
    {
        var lista = await _personalService.GetAllAsync();

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Personal");

        // Título
        ws.Cell(1, 1).Value = "Reporte de Personal";
        ws.Cell(1, 1).Style.Font.Bold = true;
        ws.Cell(1, 1).Style.Font.FontSize = 14;
        ws.Range(1, 1, 1, 4).Merge();

        ws.Cell(2, 1).Value = $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}";
        ws.Cell(2, 1).Style.Font.Italic = true;
        ws.Cell(2, 1).Style.Font.FontColor = XLColor.Gray;
        ws.Range(2, 1, 2, 4).Merge();

        // Encabezados
        var headers = new[] { "Nombre", "Documento", "Cargo", "Estado" };
        for (int i = 0; i < headers.Length; i++)
        {
            var cell = ws.Cell(4, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#667eea");
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }

        // Datos
        int row = 5;
        foreach (var p in lista)
        {
            ws.Cell(row, 1).Value = p.Nombre;
            ws.Cell(row, 2).Value = p.Documento;
            ws.Cell(row, 3).Value = p.Cargo;
            ws.Cell(row, 4).Value = p.Estado.ToString();

            // Color por estado
            var color = p.Estado.ToString() switch
            {
                "Activo" => XLColor.FromHtml("#d4edda"),
                "Enfermo" => XLColor.FromHtml("#f8d7da"),
                "Licencia" => XLColor.FromHtml("#fff3cd"),
                "Ausente" => XLColor.FromHtml("#e2e3e5"),
                _ => XLColor.White
            };

            ws.Range(row, 1, row, 4).Style.Fill.BackgroundColor = color;

            // Bordes
            for (int col = 1; col <= 4; col++)
                ws.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            row++;
        }

        ws.Columns().AdjustToContents();
        return ToBytes(wb);
    }

    public async Task<byte[]> ExportarTurnosAsync()
    {
        var lista = await _turnoService.GetAllAsync();

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Turnos");

        ws.Cell(1, 1).Value = "Reporte de Turnos";
        ws.Cell(1, 1).Style.Font.Bold = true;
        ws.Cell(1, 1).Style.Font.FontSize = 14;
        ws.Range(1, 1, 1, 3).Merge();

        ws.Cell(2, 1).Value = $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}";
        ws.Cell(2, 1).Style.Font.Italic = true;
        ws.Cell(2, 1).Style.Font.FontColor = XLColor.Gray;
        ws.Range(2, 1, 2, 3).Merge();

        var headers = new[] { "Personal", "Fecha", "Turno" };
        for (int i = 0; i < headers.Length; i++)
        {
            var cell = ws.Cell(4, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#667eea");
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }

        int row = 5;
        foreach (var t in lista.OrderBy(x => x.Fecha))
        {
            ws.Cell(row, 1).Value = t.NombrePersonal;
            ws.Cell(row, 2).Value = t.Fecha.ToString("dd/MM/yyyy");
            ws.Cell(row, 3).Value = t.TipoTurno.ToString();

            var color = t.TipoTurno.ToString() switch
            {
                "Manana" => XLColor.FromHtml("#fff3cd"),
                "Tarde" => XLColor.FromHtml("#cce5ff"),
                "Noche" => XLColor.FromHtml("#e2e3e5"),
                _ => XLColor.White
            };

            ws.Range(row, 1, row, 3).Style.Fill.BackgroundColor = color;

            for (int col = 1; col <= 3; col++)
                ws.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            row++;
        }

        ws.Columns().AdjustToContents();
        return ToBytes(wb);
    }

    public async Task<byte[]> ExportarProductosStockAsync()
    {
        var lista = await _productoService.GetAllAsync();

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Stock");

        ws.Cell(1, 1).Value = "Reporte de Productos y Stock";
        ws.Cell(1, 1).Style.Font.Bold = true;
        ws.Cell(1, 1).Style.Font.FontSize = 14;
        ws.Range(1, 1, 1, 5).Merge();

        ws.Cell(2, 1).Value = $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}";
        ws.Cell(2, 1).Style.Font.Italic = true;
        ws.Cell(2, 1).Style.Font.FontColor = XLColor.Gray;
        ws.Range(2, 1, 2, 5).Merge();

        var headers = new[] { "Nombre", "Unidad", "Stock Actual", "Stock Mínimo", "Estado" };
        for (int i = 0; i < headers.Length; i++)
        {
            var cell = ws.Cell(4, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#11998e");
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }

        int row = 5;
        foreach (var p in lista)
        {
            bool stockBajo = p.StockActual <= p.StockMinimo;

            ws.Cell(row, 1).Value = p.Nombre;
            ws.Cell(row, 2).Value = p.UnidadMedida;
            ws.Cell(row, 3).Value = p.StockActual;
            ws.Cell(row, 4).Value = p.StockMinimo;
            ws.Cell(row, 5).Value = stockBajo ? "Stock bajo" : "OK";

            var color = stockBajo
                ? XLColor.FromHtml("#f8d7da")
                : XLColor.FromHtml("#d4edda");

            ws.Range(row, 1, row, 5).Style.Fill.BackgroundColor = color;

            for (int col = 1; col <= 5; col++)
                ws.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            row++;
        }

        // Fila resumen
        ws.Cell(row + 1, 1).Value = $"Total productos: {lista.Count}";
        ws.Cell(row + 1, 1).Style.Font.Bold = true;
        ws.Cell(row + 2, 1).Value = $"Con stock bajo: {lista.Count(p => p.StockActual <= p.StockMinimo)}";
        ws.Cell(row + 2, 1).Style.Font.Bold = true;
        ws.Cell(row + 2, 1).Style.Font.FontColor = XLColor.Red;

        ws.Columns().AdjustToContents();
        return ToBytes(wb);
    }

    public async Task<byte[]> ExportarMovimientosStockAsync()
    {
        var lista = await _movimientoService.GetAllAsync();

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Movimientos");

        ws.Cell(1, 1).Value = "Reporte de Movimientos de Stock";
        ws.Cell(1, 1).Style.Font.Bold = true;
        ws.Cell(1, 1).Style.Font.FontSize = 14;
        ws.Range(1, 1, 1, 5).Merge();

        ws.Cell(2, 1).Value = $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}";
        ws.Cell(2, 1).Style.Font.Italic = true;
        ws.Cell(2, 1).Style.Font.FontColor = XLColor.Gray;
        ws.Range(2, 1, 2, 5).Merge();

        var headers = new[] { "Producto", "Tipo", "Cantidad", "Fecha", "Observación" };
        for (int i = 0; i < headers.Length; i++)
        {
            var cell = ws.Cell(4, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#4facfe");
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }

        int row = 5;
        foreach (var m in lista.OrderByDescending(x => x.Fecha))
        {
            bool esEntrada = m.TipoMovimiento.ToString() == "Entrada";

            ws.Cell(row, 1).Value = m.NombreProducto;
            ws.Cell(row, 2).Value = m.TipoMovimiento.ToString();
            ws.Cell(row, 3).Value = m.Cantidad;
            ws.Cell(row, 4).Value = m.Fecha.ToString("dd/MM/yyyy HH:mm");
            ws.Cell(row, 5).Value = m.Observacion ?? "-";

            var color = esEntrada
                ? XLColor.FromHtml("#d4edda")
                : XLColor.FromHtml("#fff3cd");

            ws.Range(row, 1, row, 5).Style.Fill.BackgroundColor = color;

            for (int col = 1; col <= 5; col++)
                ws.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            row++;
        }

        ws.Columns().AdjustToContents();
        return ToBytes(wb);
    }

    // Nuevo método que calcula viveres y verifica stock
    public async Task<ViveresResultadoDto> CalcularViveresAsync(DateTime fecha, int comensales = 35)
    {
        var resultado = new ViveresResultadoDto
        {
            Fecha = fecha.Date,
            Comensales = comensales
        };

        // Obtener planes del día
        //var planes = await _planMenuService.GetByFechaAsync(fecha);
        //if (planes == null || !planes.Any())
            //return resultado; // sin recetas para la fecha

        // Cargar todos los productos una vez para consulta de stock
        var productos = await _productoService.GetAllAsync();

        // Acumular requerimientos por ingrediente (nombre + unidad)
        var acumulado = new Dictionary<string, (string Unidad, decimal Cantidad)>(StringComparer.OrdinalIgnoreCase);
        /*
        foreach (var plan in planes)
        {
            // Guardar nombre de receta en resultado
            if (!string.IsNullOrWhiteSpace(plan.NombreReceta) && !resultado.Recetas.Contains(plan.NombreReceta))
                resultado.Recetas.Add(plan.NombreReceta);

            // Obtener receta completa
            var receta = await _recetaService.GetByIdAsync(plan.RecetaId);
            if (receta == null || receta.Ingredientes == null || !receta.Ingredientes.Any())
                continue;

            var factor = receta.Porciones > 0 ? (decimal)comensales / receta.Porciones : 1m;

            foreach (var ing in receta.Ingredientes)
            {
                var clave = $"{ing.Nombre.Trim().ToLowerInvariant()}|{ing.UnidadMedida?.Trim().ToLowerInvariant()}";
                var cantidadNecesaria = Math.Round(ing.Cantidad * factor, 2);

                if (acumulado.ContainsKey(clave))
                {
                    var existing = acumulado[clave];
                    acumulado[clave] = (existing.Unidad, existing.Cantidad + cantidadNecesaria);
                }
                else
                {
                    acumulado[clave] = (ing.UnidadMedida ?? string.Empty, cantidadNecesaria);
                }
            }
        }
        */
        // Construir lista final consultando stock por producto (coincidencia por nombre)
        foreach (var kv in acumulado)
        {
            // clave = "nombre|unidad"
            var parts = kv.Key.Split('|');
            var nombre = parts[0];
            var unidad = kv.Value.Unidad;
            var cantidadNecesaria = kv.Value.Cantidad;

            // Buscar producto por nombre (case-insensitive, contains o exact)
            var producto = productos.FirstOrDefault(p =>
                !string.IsNullOrWhiteSpace(p.Nombre) &&
                p.Nombre.Trim().Equals(nombre, StringComparison.OrdinalIgnoreCase));

            var stockActual = producto?.StockActual ?? 0m;
            var stockMinimo = producto?.StockMinimo ?? 0m;
            var disponible = producto != null && stockActual >= cantidadNecesaria;

            resultado.Items.Add(new ViveresItemDto
            {
                Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nombre),
                UnidadMedida = unidad ?? producto?.UnidadMedida ?? string.Empty,
                CantidadNecesaria = Math.Round(cantidadNecesaria, 2),
                StockActual = stockActual,
                StockMinimo = stockMinimo,
                Disponible = disponible,
                ProductoId = producto?.Id
            });
        }

        // Ordenar por nombre
        resultado.Items = resultado.Items.OrderBy(i => i.Nombre).ToList();

        return resultado;
    }

    // Helper privado que ya existía en la clase.
    private static byte[] ToBytes(XLWorkbook wb)
    {
        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }
}