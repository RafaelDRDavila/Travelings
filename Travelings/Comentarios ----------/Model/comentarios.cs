using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travelings.Model
{
    [Table("comentarios")]
    public class comentarios
    {
        [Key]

        public int id { get; set; }
        public int idcliente { get; set; }
        public string corpotexto { get; set; }
        



        public comentarios(int id, int idcliente, string corpotexto)
        {
            this.id = id;
            this.idcliente = idcliente;
            this.corpotexto = corpotexto;
        }
    }
}

