using Services.Dtos.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Dbo
{
    public class StudentDto
    {
        public Guid Id { get; set; }

        public string StudentCode { get; set; }

        public string Name { get; set; }

        public long? Birthday { get; set; }

        public DictionaryItemDto Gender { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string IdCard { get; set; }

        public string BankAccount { get; set; }

        public string Email { get; set; }

        public string Avatar { get; set; }

        public DictionaryItemDto Status { get; set; }
    }
}
