namespace Travelings.Model
{
    public interface IcomentariosRepository
    {

        void Add(comentarios comentarios);

        List<comentarios> Get();

        void Update(comentarios comentarios);
        void Delete(int id);

        comentarios GetById(int id);


    }
}
