using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Dbo.Input
{
    public class GradeFilterDto
    {
        public string SearchKey { get; set; }

        public Guid? FK_Major { get; set; }

        public Guid? FK_HeadTeacher { get; set; }

        public Guid? FK_SchoolId { get; set; }
    }
}
