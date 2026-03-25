namespace Travelings.Model
{
    public interface IavaliacaoRepository
    {
        void Add(avaliacao avaliacao);
        List<avaliacao> Get();
        List<avaliacao> GetByProduto(int idproduto);
        avaliacao? GetById(int id);
        avaliacao? GetByClienteAndProduto(int idcliente, int idproduto);
        void Update(avaliacao avaliacao);
        void Delete(int id);
        double GetMediaByProduto(int idproduto);
        int GetCountByProduto(int idproduto);
    }
}
