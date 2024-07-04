using Context;

namespace UI.View {
  public interface IView<T> {
    public T Render();
    public void SetContext(ApplicationContext context);
  }
}
