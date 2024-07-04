using Entity;

namespace Repository {
  public interface IRepository {
    public IEntity getById(Guid id);
    public IEntity save(IEntity entity);
  }
}
