using System.Linq.Expressions;

namespace OkrManager.Interfaces
{  
    public interface IRepository<T>
        {
            void Create(T entidade);
            void Update(T entidade);
            void Delete(int id);
            T? GetById(int id);
            List<T> GetAll();
        }
}