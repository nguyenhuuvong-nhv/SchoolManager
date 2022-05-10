using MicroOrm.Dapper.Repositories.Attributes.LogicalDelete;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entity
{
    [Table("Grades", Schema = "dbo")]
    public class Grade
    {
        [Key]
        public Guid Id { get; set; }

        [Status, Deleted]
        public bool IsDeleted { get; set; }

        public DateTime? CreatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public Guid? ModifiedBy { get; set; }

        public string Code { get; set; }

        public Guid? FK_Major { get; set; }

        public Guid? FK_HeadTeacher { get; set; }

        public Guid? FK_SchoolId { get; set; }

        public virtual School School { get; set; }

        public virtual Student[] Students { get; set; }
    }
}
