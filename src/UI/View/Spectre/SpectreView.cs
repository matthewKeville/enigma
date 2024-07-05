using Context;
using Spectre.Console;
using Spectre.Console.Rendering;
using UI.Model;

namespace UI.View.Spectre {

  public abstract class SpectreView<K> : ISpectreView<K> where K : IModel {

    private bool supressMissingModel = false;

    protected void Register(ContextAccessor ctx) {
      SetContext((K)ctx.GetModel<K>());
      Trace.WriteLineIf( model is null,$" Null model for {typeof(K).ToString()}" );

      Trace.WriteLine($"registered context listener for {typeof(K).ToString()}");
      ctx.RaiseContextChangeEvent += (object? sender,EventArgs args) => {
        Trace.WriteLine($"context changed in {typeof(K).ToString()}");
        SetContext((K)ctx.GetModel<K>());
      };
    }

    protected K? model;
    public void SetContext(K model) {
      Trace.WriteLine($"context set for {GetType().ToString()}");
      this.model = model;
    }

    public IRenderable Render() {
      if ( this.model is null ) {
        // don't flood the logs 
        if ( !supressMissingModel ) {
          Trace.WriteLine($"{GetType().ToString()} has a null model");
          supressMissingModel = true;
        }
        return new Layout();
      }
      supressMissingModel = false;
      return render();
    } 

    protected abstract IRenderable render();
  }

}
