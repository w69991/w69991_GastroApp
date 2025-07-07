using System.ComponentModel.DataAnnotations;
using GastroApp.Common;

namespace GastroApp.Domain.Entities;

//Jedna z pozycji w zamowieniu
public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; }

    public Guid MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal UnitPrice { get; set; }

    //Wartosc pozycji przechowywana tylko w kodzie 
    public decimal Total => Quantity * UnitPrice;
}