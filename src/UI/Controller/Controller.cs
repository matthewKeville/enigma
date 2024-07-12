using UI.Model;

namespace UI.Controller {

  public abstract class Controller<K> where K : IModel {

    public K? model;

    public void SetContext(K model) {
      Trace.WriteLine($"context set for controller {GetType().ToString()}");
      this.model = model;
    }

  }

}
