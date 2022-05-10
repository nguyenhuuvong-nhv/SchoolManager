using System;
using System.ComponentModel.DataAnnotations;

namespace Services.Dtos.Dbo.Input
{
    public class SchoolUpdateDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Code { get; set; }

        public string Address { get; set; }

        public string Hotline { get; set; }

        public string Email { get; set; }
    }
}
