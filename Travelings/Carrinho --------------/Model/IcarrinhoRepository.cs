namespace Travelings.Model
{
    public interface IcarrinhoRepository
    {

        void Add(carrinho carrinho);

        List<carrinho> Get();
        List<carrinho> GetByCliente(int idcliente);
        void Update(carrinho carrinho);
        void Delete(int id);

        carrinho GetById(int id);

    }
}
