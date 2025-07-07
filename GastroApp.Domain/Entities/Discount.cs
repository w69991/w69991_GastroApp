using System.ComponentModel.DataAnnotations;
using GastroApp.Common;

namespace GastroApp.Domain.Entities;

//Nieuzyta klasa znizki/promocji
public class Discount : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Code { get; set; }

    [MaxLength(250)]
    public string Description { get; set; }

    [Range(0, 100)]
    public decimal Percentage { get; set; }

    public DateTime? ExpiresAt { get; set; }

    [Range(0, double.MaxValue)]
    public decimal MinOrderValue { get; set; } = 0;
}