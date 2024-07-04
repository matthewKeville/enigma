using Entity;

namespace Repository {
  public interface IRepository<T> where T : IEntity {
    public T getById(int id);
    public T save(T entity);
    public bool has(int id);
  }
}
