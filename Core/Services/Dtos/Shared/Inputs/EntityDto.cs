using System;
using System.ComponentModel.DataAnnotations;

namespace Services.Dtos.Shared.Inputs
{
    public class EntityDto
    {
        [Required]
        public Guid Id { get; set; }
    }
}
