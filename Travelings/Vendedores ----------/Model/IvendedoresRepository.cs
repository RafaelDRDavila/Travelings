using Travelings.Model;

namespace Travelings.Vendedores
{
    public interface IvendedoresRepository
    {

        void Add(vendedores vendedores);

        List<vendedores> Get();

        void Update(vendedores vendedores);
        void Delete(int id);

        vendedores GetById(int id);


    }
}
