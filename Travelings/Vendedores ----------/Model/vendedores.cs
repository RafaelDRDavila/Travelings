using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travelings.Model
{
    [Table("vendedores")]
    public class vendedores
    {
        [Key]

        public int id { get;  set; }
        public string nomecompleto { get; set; }
        public string email { get; set; }
        public string senha { get;  set; }

        public string cpf { get; set; }
        public int idloja { get; set; }

    

   

        public vendedores(int id, string nomecompleto, string email, string senha, string cpf, int idloja)
        {
            this.id = id;
            this.nomecompleto = nomecompleto;
            this.email = email;
            this.senha = senha;
            this.cpf = cpf;
            this.idloja = idloja;
        }
    }
}

