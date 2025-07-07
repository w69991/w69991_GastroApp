
using GastroApp.Application.Customers.Commands;
using GastroApp.Application.Customers.Dtos;
using GastroApp.Domain.Entities;     
using GastroApp.Domain.Interfaces;      
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GastroApp.WebApi.Controllers
{
    //Zarzadza kontami klientow: resjestracja, logowanie
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAsyncRepository<Customer> _customerRepo;

        public CustomersController(IMediator mediator, IAsyncRepository<Customer> customerRepo)
        {
            _mediator = mediator;
            _customerRepo = customerRepo;
        }

        //Rejestracja nowego klienta
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCustomerCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var customer = await _customerRepo.GetByIdAsync(id);
            return customer is null ? NotFound() : Ok(customer);
        }
        
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Customer updated)
        {
            var existing = await _customerRepo.GetByIdAsync(id);
            if (existing is null) return NotFound();

            existing.FirstName = updated.FirstName;
            existing.LastName = updated.LastName;
            existing.Email = updated.Email;
            await _customerRepo.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var customer = await _customerRepo.GetByIdAsync(id);
            if (customer is null) return NotFound();
            await _customerRepo.DeleteAsync(customer);
            return NoContent();
        }
        //Logowanie klienta
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var customer = (await _customerRepo.ListAllAsync())
                .FirstOrDefault(c => c.Email == dto.Email);
            if (customer is null || !BCrypt.Net.BCrypt.Verify(dto.Password, customer.PasswordHash))
                return Unauthorized();

            return Ok(new
            {
                customer.Id,
                customer.FirstName,
                customer.LastName, 
                customer.Email,           
                loyaltyPoints = customer.LoyaltyPoints  
            });
        }
    }
}