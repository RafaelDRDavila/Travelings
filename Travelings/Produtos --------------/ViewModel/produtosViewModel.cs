namespace Travelings.Produtos_______________.ViewModel
{
    public class produtosViewModel
    {
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

    }
}
