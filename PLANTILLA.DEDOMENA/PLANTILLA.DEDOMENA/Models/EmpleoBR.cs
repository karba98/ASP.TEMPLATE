using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLANTIILLA.DEDOMENA.Models
{
    [Table("empleo_br")]
    public class EmpleoBR
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("titulo")]
        public string Titulo { get; set; }
        [Column("descripcion")]
        public string Descripcion { get; set; }
        [Column("salario")]
        public int Salario { get; set; }
        [Column("url")]
        public string Url { get; set; }
        [Column("fechapub")]
        public DateTime FechaPub { get; set; }
        [Column("fechastring")]
        public String FechaString { get; set; }
        [Column("provincia")]
        public String Provincia { get; set; }
        [NotMapped]
        public String ProvinciaName { get; set; }
        [Column("categoria")]
        public String Categoria { get; set; }
        [Column("telefono")]
        public String Telefono { get; set; }
        [Column("email")]
        public String Email { get; set; }
        [Column("publicado")]
        public int? Publicado { get; set; }
        [Column("modo")]
        public string? Modo { get; set; }
    }
}
