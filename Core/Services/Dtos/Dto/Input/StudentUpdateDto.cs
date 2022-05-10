using Microsoft.AspNetCore.Http;
using Services.Dtos.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Dbo.Input
{
    public class StudentUpdateDto
    {
        [Required]
        public Guid Id { get; set; }

        public string StudentCode { get; set; }
        [Required]
        public string Name { get; set; }

        public long? Birthday { get; set; }

        public DictionaryItemDto Gender { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string IdCard { get; set; }

        public string BankAccount { get; set; }

        public string Email { get; set; }

        public IFormFile Avatar { get; set; }

        public DictionaryItemDto Status { get; set; }

        public Guid? FK_GradeId { get; set; }
    }
}
