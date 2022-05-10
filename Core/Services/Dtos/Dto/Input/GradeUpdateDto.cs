using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Dbo.Input
{
    public class GradeUpdateDto
    {
        [Required]
        public Guid Id { get; set; }

        public string Code { get; set; }

        public Guid? FK_Major { get; set; }

        public Guid? FK_HeadTeacher { get; set; }

        public Guid? FK_SchoolId { get; set; }
    }
}
