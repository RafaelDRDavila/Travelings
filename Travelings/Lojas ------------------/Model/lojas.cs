using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travelings.Model
{
    [Table("lojas")]
    public class lojas
    {
        [Key]
        public int id { get; set; }
        public string nome { get; set; }
        public string cnpj { get; set; }
        public string descricao { get; set; }
        public string logo { get; set; }
        public string banner { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
        public string endereco { get; set; }

        public lojas(int id, string nome, string cnpj, string descricao = "", string logo = "", string banner = "", string email = "", string telefone = "", string endereco = "")
        {
            this.id = id;
            this.nome = nome;
            this.cnpj = cnpj;
            this.descricao = descricao ?? "";
            this.logo = logo ?? "";
            this.banner = banner ?? "";
            this.email = email ?? "";
            this.telefone = telefone ?? "";
            this.endereco = endereco ?? "";
        }
    }
}

