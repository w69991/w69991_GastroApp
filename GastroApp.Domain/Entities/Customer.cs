using System.ComponentModel.DataAnnotations;
using GastroApp.Common;

namespace GastroApp.Domain.Entities
{


    //Klasa odpowiadajaca za klienta
    public class Customer : BaseEntity
    {
        [Required] [MaxLength(100)] public string FirstName { get; set; }

        [Required] [MaxLength(100)] public string LastName { get; set; }

        [Required] [EmailAddress] public string Email { get; set; }

        // Hash hasla przy uzyciu Bcrypt
        [Required] public string PasswordHash { get; set; }
        //Punkty lojalnosciowe
        public int LoyaltyPoints { get; set; }
        //Lista zlozonych przez klienta zamowien
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}