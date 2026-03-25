namespace Travelings.Carrinho_______________.ViewModel
{
    public class carrinhoViewModel
    {
        public int id { get; set; }
        public int idproduto { get; set; }
        public int idcliente { get; set; }
        public int quantidade { get; set; }
        public string modalidade { get; set; } = "compra";
        public DateTime? datainicio { get; set; }
        public DateTime? datafim { get; set; }
    }
}
