
namespace PLANTIILLA.DEDOMENA.Models
{ 
    
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;
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
