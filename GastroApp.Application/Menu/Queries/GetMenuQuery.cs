using GastroApp.Domain.Entities;
using GastroApp.Domain.Interfaces;
using MediatR;

namespace GastroApp.Application.Menu.Queries;
//Zapytanie o pelne dostepne menu 
public record GetMenuQuery() : IRequest<IReadOnlyList<MenuItemDto>>;
//Dto wysylane do front-endu
public record MenuItemDto(Guid Id, string Name, string Description, decimal Price, string Category, bool IsAvailable);

//Handler odczytuje wszystkie pozycje z repozytorium, filtruje niedostepne i mapuje na MenuItemDTO
public class GetMenuHandler : IRequestHandler<GetMenuQuery, IReadOnlyList<MenuItemDto>>
{
    private readonly IAsyncRepository<MenuItem> _menuRepo;

    public GetMenuHandler(IAsyncRepository<MenuItem> menuRepo)
    {
        _menuRepo = menuRepo;
    }

    public async Task<IReadOnlyList<MenuItemDto>> Handle(GetMenuQuery request, CancellationToken cancellationToken)
    {
        var items = await _menuRepo.ListAllAsync();
        return items
            .Where(i => i.IsAvailable)
            .Select(i => new MenuItemDto(i.Id, i.Name, i.Description, i.Price, i.Category, i.IsAvailable))
            .ToList();
    }
}