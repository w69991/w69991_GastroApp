using GastroApp.Common;
using GastroApp.Domain.Enums;

namespace GastroApp.Domain.Entities;

//Klasa odpowiadajaca za zamowienie zlozone przez klienta
public class Order : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public OrderStatus Status { get; set; } = OrderStatus.New;

    public decimal TotalAmount { get; set; }
    //Szczegolowa lista pozycji
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}