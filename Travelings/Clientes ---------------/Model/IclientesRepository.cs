namespace Travelings.Model
{
    public interface IclientesRepository
    {

        void Add(clientes clientes);

        List<clientes> Get();

        void Update(clientes clientes);
        void Delete( int id);

        clientes GetById(int id);
    }
}
