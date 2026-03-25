using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travelings.Model
{
    [Table("produtos")]
    public class produtos
    {
        [Key]

        public int id { get; set; }
        public string nome { get; set; }
        public decimal preco { get; set; }
        public string descricao { get; set; }
        public int estoque { get; set; }
        public string categoria { get; set; }
        public string? subcategoria { get; set; }
        public string imagem { get; set; }
        public string? midias { get; set; }
        public int idloja { get; set; }
        public bool ativo { get; set; } = true;
        public string tipo { get; set; } = "venda";
        public decimal? precoaluguel { get; set; }
        public int? diasminimo { get; set; }
        public int? diasmaximo { get; set; }
        public int? quantidadedisponivel { get; set; }

        public produtos(int id, string nome, decimal preco, string descricao, int estoque, string categoria, string imagem, int idloja, bool ativo = true, string? subcategoria = null, string? midias = null, string tipo = "venda", decimal? precoaluguel = null, int? diasminimo = null, int? diasmaximo = null, int? quantidadedisponivel = null)
        {
            this.id = id;
            this.nome = nome;
            this.preco = preco;
            this.descricao = descricao;
            this.estoque = estoque;
            this.categoria = categoria;
            this.subcategoria = subcategoria;
            this.imagem = imagem;
            this.midias = midias;
            this.idloja = idloja;
            this.ativo = ativo;
            this.tipo = tipo;
            this.precoaluguel = precoaluguel;
            this.diasminimo = diasminimo;
            this.diasmaximo = diasmaximo;
            this.quantidadedisponivel = quantidadedisponivel;
        }
    }
}
