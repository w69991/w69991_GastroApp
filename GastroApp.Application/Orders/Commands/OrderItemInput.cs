
using GastroApp.Domain.Entities;
using GastroApp.Domain.Enums;
using GastroApp.Domain.Interfaces;
using MediatR;

namespace GastroApp.Application.Orders.Commands
{
    
    public class CreateOrderCommand : IRequest<Guid>
    {
        public Guid CustomerId { get; init; }
        public IReadOnlyList<OrderItemDto> Items { get; init; } = new List<OrderItemDto>();

        public string? DiscountCode { get; init; }
        public bool   UseLoyaltyPoints { get; init; } 
        public record OrderItemDto(Guid MenuItemId, int Quantity);
    }

    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IAsyncRepository<Customer> _customerRepo;
        private readonly IAsyncRepository<MenuItem> _menuRepo;
        private readonly IAsyncRepository<Order>    _orderRepo;

        public CreateOrderHandler(
            IAsyncRepository<Customer> customerRepo,
            IAsyncRepository<MenuItem> menuRepo,
            IAsyncRepository<Order> orderRepo)
        {
            _customerRepo = customerRepo;
            _menuRepo     = menuRepo;
            _orderRepo    = orderRepo;
        }

        public async Task<Guid> Handle(CreateOrderCommand cmd, CancellationToken _)
        {
            //Klient
            var customer = await _customerRepo.GetByIdAsync(cmd.CustomerId)
                           ?? throw new InvalidOperationException("Customer not found");

            //Items
            var menu  = (await _menuRepo.ListAllAsync()).ToDictionary(m => m.Id);
            var items = cmd.Items.Select(i => new OrderItem
            {
                MenuItemId = i.MenuItemId,
                Quantity   = i.Quantity,
                UnitPrice  = menu[i.MenuItemId].Price
            }).ToList();

            decimal total = items.Sum(i => i.Quantity * i.UnitPrice);

            //–10 zł za 100 pkt
            if (cmd.UseLoyaltyPoints && customer.LoyaltyPoints >= 100)
            {
                total = Math.Max(0, total - 10m);
                customer.LoyaltyPoints -= 100;
            }

            //+10 pkt za zamówienie
            customer.LoyaltyPoints += 10;
            await _customerRepo.UpdateAsync(customer);

            //zapis zamówienia
            var order = new Order
            {
                CustomerId  = customer.Id,
                OrderDate   = DateTime.UtcNow,
                Status      = OrderStatus.New,
                TotalAmount = total,
                Items       = items
            };
            await _orderRepo.AddAsync(order);
            return order.Id;
        }
    }
}