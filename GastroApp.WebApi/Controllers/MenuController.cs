using GastroApp.Application.Menu.Queries;
using GastroApp.Domain.Entities; 
using GastroApp.Domain.Interfaces; 
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GastroApp.WebApi.Controllers;

//Kontroler menu 
[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAsyncRepository<MenuItem> _menuRepo;
    public MenuController(IMediator mediator, IAsyncRepository<MenuItem> menuRepo)
    {
        _mediator = mediator;
        _menuRepo = menuRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _mediator.Send(new GetMenuQuery());
        return Ok(items);
    }
    
    //Utworzenie pozycji w menu
    [HttpPost]
    public async Task<IActionResult> Create(MenuItem item)
    {
        await _menuRepo.AddAsync(item);
        return Created($"api/menu/{item.Id}", item);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] MenuItem updated)
    {
        var existing = await _menuRepo.GetByIdAsync(id);
        if (existing is null) return NotFound();

        existing.Name        = updated.Name;
        existing.Description = updated.Description;
        existing.Price       = updated.Price;
        existing.Category    = updated.Category;
        existing.IsAvailable = updated.IsAvailable;
        await _menuRepo.UpdateAsync(existing);
        return NoContent();
    }
    
    //Usuwanie pozycji z meniu
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existing = await _menuRepo.GetByIdAsync(id);
        if (existing is null) return NotFound();
        await _menuRepo.DeleteAsync(existing);
        return NoContent();
    }
}