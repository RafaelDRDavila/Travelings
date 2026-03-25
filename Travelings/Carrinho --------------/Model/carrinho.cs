using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travelings.Model
{
    [Table("carrinho")]
    public class carrinho
    {
        [Key]

        public int id { get; set; }
        public int idproduto { get; set; }
        public int idcliente { get; set; }
        public int quantidade { get; set; }
        public string modalidade { get; set; } = "compra";
        public DateTime? datainicio { get; set; }
        public DateTime? datafim { get; set; }

        public carrinho(int id, int idproduto, int idcliente, int quantidade, string modalidade = "compra", DateTime? datainicio = null, DateTime? datafim = null)
        {
            this.id = id;
            this.idproduto = idproduto;
            this.idcliente = idcliente;
            this.quantidade = quantidade;
            this.modalidade = modalidade;
            this.datainicio = datainicio;
            this.datafim = datafim;
        }
    }
}

