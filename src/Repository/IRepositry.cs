using Entity;

namespace Repository {
  public interface IRepository<T> where T : IEntity {
    public T GetById(int id);
    public List<T> GetAll();
    public T Save(T entity);
    public bool Has(int id);
  }
}
