using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Dbo.Input
{
    public class StudentFilterDto
    {
        public string SearchKey { get; set; }

        public Guid? FK_SchoolId { get; set; }

        public Guid? FK_GradeId { get; set; }
    }
}
