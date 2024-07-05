using Context;
using Spectre.Console;
using Spectre.Console.Rendering;
using UI.Model.Browser;

namespace UI.View.Spectre.Browser {

  public class BrowserView : SpectreView<BrowserModel> {

    public BrowserView(ContextAccessor ctx) {
      Register(ctx);
    }

    override protected IRenderable render() {

      Layout root = new Layout();
      Rows rows = new Rows(
        model.headers.Select( 
          x => { 
            if ( x.puzzleId == model.headers[model.selection].puzzleId ) {
              return new Text($"{x.puzzleId.ToString()} {x.date.ToShortDateString()}", new Style(Color.Green1));  
            } else {
              return new Text($"{x.puzzleId.ToString()} {x.date.ToShortDateString()}");  
            }
          }
        )
      );
      root.Update(rows);
      return root;

    }

  }
}
