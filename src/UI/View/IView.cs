using UI.Model;

namespace UI.View {
  public interface IView<T,K> where K : IModel {
    public T Render();
    public void SetContext(K model);
  }
}
