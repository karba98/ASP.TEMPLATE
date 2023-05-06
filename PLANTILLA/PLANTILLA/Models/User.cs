using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLANTILLA.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("username")]
        public string UserName { get; set; }
        [Column("password")]
        public byte[] Password { get; set; }
        [Column("salt")]
        public String Salt { get; set; }
    }
}
