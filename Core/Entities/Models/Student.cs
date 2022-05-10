using MicroOrm.Dapper.Repositories.Attributes.LogicalDelete;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entity
{
    [Table("Students", Schema = "dbo")]
    public class Student
    {
        [Key]
        public Guid Id { get; set; }

        [Status, Deleted]
        public bool IsDeleted { get; set; }

        public DateTime? CreatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public Guid? ModifiedBy { get; set; }

        public string StudentCode { get; set; }

        public string Name { get; set; }

        public DateTime? Birthday { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }
        
        public string IdCard { get; set; }

        public string BankAccount { get; set; }

        public string Email { get; set; }

        public string Avatar { get; set; }

        public string Status { get; set; }

        public Guid? FK_GradeId { get; set; }

        public virtual Grade Grade { get; set; }
    }
}
