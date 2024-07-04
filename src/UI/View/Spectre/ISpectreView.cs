using Spectre.Console.Rendering;
using UI.Model;

namespace UI.View.Spectre {
  public interface ISpectreView<K> : UI.View.IView<IRenderable,K> where K : IModel {}
}
