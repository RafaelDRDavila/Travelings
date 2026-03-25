using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travelings.Model
{
    [Table("clientes")]
    public class clientes
    {
        [Key]

        public int id { get; set; }
        public string nomecompleto { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public string cpf { get; set; }
        public string? telefone { get; set; }
        public string? endereco { get; set; }
        public string? cep { get; set; }
        public string? foto { get; set; }



        public clientes(string nomecompleto, string email, string senha, string cpf, string? telefone, string? endereco, string? cep, string? foto = null)
        {

            this.nomecompleto = nomecompleto;
            this.email = email;
            this.senha = senha;
            this.cpf = cpf;
            this.telefone = telefone;
            this.endereco = endereco;
            this.cep = cep;
            this.foto = foto;
        }
    }
}
