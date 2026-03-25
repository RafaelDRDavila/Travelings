using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travelings.Model
{
    [Table("avaliacoes")]
    public class avaliacao
    {
        [Key]
        public int id { get; set; }
        public int idproduto { get; set; }
        public int idcliente { get; set; }
        public int nota { get; set; }
        public string texto { get; set; }
        public string? midias { get; set; }
        public string? nomecliente { get; set; }
        public string? fotocliente { get; set; }
        public DateTime datacriacao { get; set; }

        public avaliacao(int id, int idproduto, int idcliente, int nota, string texto, string? midias, string? nomecliente, string? fotocliente, DateTime datacriacao)
        {
            this.id = id;
            this.idproduto = idproduto;
            this.idcliente = idcliente;
            this.nota = nota;
            this.texto = texto;
            this.midias = midias;
            this.nomecliente = nomecliente;
            this.fotocliente = fotocliente;
            this.datacriacao = datacriacao;
        }
    }
}
