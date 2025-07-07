using System.ComponentModel.DataAnnotations;
using GastroApp.Common;

namespace GastroApp.Domain.Entities;

//Klasa odpowiadajaca za menu
public class MenuItem : BaseEntity
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    public bool IsAvailable { get; set; } = true;

    [MaxLength(100)]
    public string Category { get; set; }
}
