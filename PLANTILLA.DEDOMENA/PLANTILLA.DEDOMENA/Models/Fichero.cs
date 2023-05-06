using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLANTIILLA.DEDOMENA.Models
{
    [Table("files")]
    public class Fichero
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("FileName")]
        public string Name { get; set; }
        [Column("Descripcion")]
        public string Description { get; set; }
        [Column("FilePath")]
        public string Path { get; set; }
        [Column("Img")]
        public string Img { get; set; }

    }
}
