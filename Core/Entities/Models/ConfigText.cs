using MicroOrm.Dapper.Repositories.Attributes.LogicalDelete;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Data.Entity
{
    [Table("ConfigTexts", Schema = "dbo")]
    public class ConfigText
    {
        [Key]
        public Guid Id { get; set; }

        [Status, Deleted]
        public bool IsDeleted { get; set; }

        public string ConfigGroup { get; set; }

        public string ConfigKey { get; set; }

        public string ConfigValue { get; set; }

        public string DisplayText { get; set; }

        public int? ConfigOrder { get; set; }
    }
}