using Entity;

namespace Repository {
  public interface IRepository<T> where T : IEntity {
    public T getById(Guid id);
    public T save(T entity);
  }
}
