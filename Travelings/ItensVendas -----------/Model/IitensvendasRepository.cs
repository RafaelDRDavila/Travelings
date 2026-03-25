using Travelings.Vendas_________________.ViewModel;

namespace Travelings.Model
{
    public interface IitensvendasRepository
    {

        void Add(itensvendas itensvendas);

        List<itensvendas> Get();

        void Update(itensvendas itensvendas);
        void Delete(int id);

        itensvendas GetById(int id);


    }
}
