
namespace PLANTIILLA.DEDOMENA.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("empleo_filter")]
    public class Word
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("words")]
        public string word { get; set; }
    }
    
}
