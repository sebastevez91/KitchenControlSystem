using CocinaManager.Application.DTOs;

namespace CocinaManager.Application.Interfaces;

public interface IMenuSemanalService
{
    Task<MenuSemanalDto> GetSemanaActualAsync();
}