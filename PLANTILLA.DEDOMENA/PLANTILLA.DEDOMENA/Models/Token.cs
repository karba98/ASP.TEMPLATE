using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLANTIILLA.DEDOMENA.Models
{
    [Table("codes")]
    public class Token
    {
        [Key]
        [Column("Id")]
        
        public int ? Id { get; set; }
        [Column("Key")]
        public string Key { get; set; }
        [Column("Code")]
        public string Code { get; set; }
    }
}
