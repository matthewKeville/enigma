
using Context;
using UI.Model;

namespace UI.Controller {

  public abstract class Controller<K> where K : IModel {

    protected K? model;

    protected void Register(ContextAccessor ctx) {

      SetContext((K)ctx.GetModel<K>());
      Trace.WriteLineIf( model is null,$" Null model for controller {typeof(K).ToString()}" );

      Trace.WriteLine($"registered context listener for {typeof(K).ToString()}");
      ctx.RaiseContextChangeEvent += (object? sender,EventArgs args) => {
        Trace.WriteLine($"Context changed in {typeof(K).ToString()}");
        SetContext((K)ctx.GetModel<K>());
      };

    }

    public void SetContext(K model) {
      Trace.WriteLine($"context set for controller {GetType().ToString()}");
      this.model = model;
    }

  }

}
