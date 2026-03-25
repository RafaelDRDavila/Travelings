using Travelings.Vendas_________________.ViewModel;

namespace Travelings.Model
{
    public interface IvendasRepository
    {

        void Add(vendas vendas);

        List<vendas> Get();
        void Update(vendas vendas);
        void Delete(int id);

        vendas GetById(int id);


    }
}
