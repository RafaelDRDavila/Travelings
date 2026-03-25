namespace Travelings.Model
{
    public interface IlojasRepository
    {

        void Add(lojas lojas);

        List<lojas> Get();

        void Update(lojas lojas);
        void Delete(int id);

        lojas GetById(int id);


    }
}
