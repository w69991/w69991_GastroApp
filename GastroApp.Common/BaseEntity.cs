using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GastroApp.Common
{
    // Bazowa klasa wszystkich encji z domyślnym kluczem Guid
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; protected set; } = Guid.NewGuid();
    }
}