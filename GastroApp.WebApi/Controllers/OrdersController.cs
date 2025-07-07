using GastroApp.Application.Orders.Commands;
using GastroApp.Domain.Entities; 
using GastroApp.Domain.Interfaces; 
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GastroApp.WebApi.Controllers;

//Operacje zwiazane z zamowieniami
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAsyncRepository<Order> _orderRepo;

    public OrdersController(IMediator mediator, IAsyncRepository<Order> orderRepo)
    {
        _mediator = mediator;
        _orderRepo = orderRepo;
    }   
    
    //Sklada nowe zamowienie
    [HttpPost]
    public async Task<IActionResult> PlaceOrder(CreateOrderCommand cmd)
    {
        var id = await _mediator.Send(cmd);
        return Created($"api/orders/{id}", new { id });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var order = await _orderRepo.GetByIdAsync(id);
        return order is null ? NotFound() : Ok(order);
    }

    //Anulowanie
    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var order = await _orderRepo.GetByIdAsync(id);
        if (order is null) return NotFound();
        order.Status = GastroApp.Domain.Enums.OrderStatus.Cancelled;
        await _orderRepo.UpdateAsync(order);
        return NoContent();
    }
    
    //Zwraca historie zamowien
    [HttpGet("byCustomer/{customerId:guid}")]
    public async Task<IActionResult> GetForCustomer(Guid customerId)
    {
        var list = (await _orderRepo.ListAllAsync())
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new
            {
                o.Id,
                o.OrderDate,
                o.Status,
                o.TotalAmount,
                Items = o.Items.Select(i => new
                {
                    i.MenuItem.Name,
                    i.Quantity,
                    i.UnitPrice
                })
            });
        return Ok(list);
    }
}