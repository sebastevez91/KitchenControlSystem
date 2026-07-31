using CocinaManager.Application.DTOs;

namespace CocinaManager.Application.Interfaces;

public interface IMenuPdfService
{
    byte[] GenerarPdfSemanal(MenuSemanalDto menu, byte[]? logoBytes);
}