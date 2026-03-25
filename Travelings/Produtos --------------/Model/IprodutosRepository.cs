namespace Travelings.Model
{
    public interface IprodutosRepository
    {

        void Add(produtos produtos);

        List<produtos> Get();
        void Update(produtos produtos);
        void Delete(int id);

        produtos GetById(int id);


    }
}
