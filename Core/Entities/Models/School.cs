using MicroOrm.Dapper.Repositories.Attributes.LogicalDelete;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entity
{
    [Table("Schools", Schema = "dbo")]
    public class School
    {
        [Key]
        public Guid Id { get; set; }

        [Status, Deleted]
        public bool IsDeleted { get; set; }

        public DateTime? CreatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public Guid? ModifiedBy { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Address { get; set; }

        public string Hotline { get; set; }

        public string Email { get; set; }

        public virtual Grade[] Grades { get; set; }
    }
}
