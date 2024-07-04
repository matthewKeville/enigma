using Spectre.Console.Rendering;

namespace UI.View.Spectre {
  public interface ISpectreView<T> : UI.View.IView<T> where T : IRenderable {}
}
