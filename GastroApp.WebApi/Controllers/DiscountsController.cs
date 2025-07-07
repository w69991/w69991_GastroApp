using GastroApp.Domain.Entities;
using GastroApp.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GastroApp.WebApi.Controllers;

//Kontroler znizek nie zostal zaimplementowany
[ApiController]
[Route("api/[controller]")]
public class DiscountsController : ControllerBase
{
    private readonly IAsyncRepository<Discount> _discountRepo;
    public DiscountsController(IAsyncRepository<Discount> discountRepo)
    {
        _discountRepo = discountRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _discountRepo.ListAllAsync());

    [HttpPost]
    public async Task<IActionResult> Create(Discount d)
    {
        await _discountRepo.AddAsync(d);
        return Created($"api/discounts/{d.Id}", d);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, Discount updated)
    {
        var existing = await _discountRepo.GetByIdAsync(id);
        if (existing is null) return NotFound();

        existing.Code = updated.Code;
        existing.Description = updated.Description;
        existing.Percentage = updated.Percentage;
        existing.MinOrderValue = updated.MinOrderValue;
        existing.ExpiresAt = updated.ExpiresAt;
        await _discountRepo.UpdateAsync(existing);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existing = await _discountRepo.GetByIdAsync(id);
        if (existing is null) return NotFound();
        await _discountRepo.DeleteAsync(existing);
        return NoContent();
    }
}
