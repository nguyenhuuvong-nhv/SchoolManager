using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Dbo
{
    public class GradeDetailDto
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public SchoolDto School { get; set; }
    }
}
