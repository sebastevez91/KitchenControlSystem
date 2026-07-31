using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Enums;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace CocinaManager.Application.Services;

public class MenuPdfService : IMenuPdfService
{
    private static readonly TipoMenu[] OrdenComidas =
        { TipoMenu.Desayuno, TipoMenu.Almuerzo, TipoMenu.Merienda, TipoMenu.Cena, TipoMenu.Postre };

    public byte[] GenerarPdfSemanal(MenuSemanalDto menu, byte[]? logoBytes)
    {
        var document = Document.Create(c =>
        {
            c.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(20);
                page.DefaultTextStyle(x => x.FontSize(9));

                page.Header().Row(row =>
                {
                    row.RelativeColumn().Column(col =>
                    {
                        col.Item().Text("Menú del Destacamento RPO").FontSize(16).Bold();
                        col.Item().Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(8);
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
                        cols.RelativeColumn(1.3f);
                        foreach (var _ in menu.Dias)
                            cols.RelativeColumn(1);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(6).Text(h => h.Span("Comida").SemiBold());
                        foreach (var dia in menu.Dias)
                        {
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(6).Column(col =>
                            {
                                col.Item().Text(h => h.Span(dia.DiaSemana.ToString()).SemiBold());
                                if (dia.CantidadComensales.HasValue)
                                    col.Item().Text($"{dia.CantidadComensales} comensales").FontSize(7).Italic();
                            });
                        }
                    });

                    int filaIdx = 0;
                    foreach (var tipo in OrdenComidas)
                    {
                        var bg = (filaIdx % 2 == 0) ? Colors.White : Colors.Grey.Lighten4;
                        bool esDoble = tipo == TipoMenu.Almuerzo || tipo == TipoMenu.Cena;

                        table.Cell().Background(bg).Padding(6).Text(h => h.Span(tipo.ToString()).SemiBold());

                        foreach (var dia in menu.Dias)
                        {
                            var comida = dia.Comidas.FirstOrDefault(c => c.TipoMenu == tipo);

                            if (esDoble)
                            {
                                var entrada = comida?.Recetas.FirstOrDefault(r => r.Rol == RolPlato.Entrada);
                                var principal = comida?.Recetas.FirstOrDefault(r => r.Rol == RolPlato.PlatoPrincipal);

                                table.Cell().Background(bg).Padding(6).Column(col =>
                                {
                                    col.Item().Text(t =>
                                    {
                                        t.Span("Entrada: ").FontSize(7).SemiBold();
                                        t.Span(entrada?.RecetaNombre ?? "-");
                                    });
                                    col.Item().Text(t =>
                                    {
                                        t.Span("Principal: ").FontSize(7).SemiBold();
                                        t.Span(principal?.RecetaNombre ?? "-");
                                    });
                                });
                            }
                            else
                            {
                                var receta = comida?.Recetas.FirstOrDefault();
                                table.Cell().Background(bg).Padding(6).Text(receta?.RecetaNombre ?? "-");
                            }
                        }
                        filaIdx++;
                    }
                });

                page.Footer().AlignCenter().Text(x =>
                    x.Span("Cocina Manager - Menú generado automáticamente").FontSize(8));
            });
        });

        using var ms = new MemoryStream();
        document.GeneratePdf(ms);
        return ms.ToArray();
    }
}