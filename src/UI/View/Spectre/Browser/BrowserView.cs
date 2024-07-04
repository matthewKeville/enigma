using Context;
using Spectre.Console;
using UI.Model.Browser;

namespace UI.View.Spectre.Browser {

  public class BrowserView : ISpectreView<Layout> {

    private BrowserModel browserModel;

    public void SetContext(ApplicationContext context) {
      this.browserModel = context.browserModel;
    }

    public Layout Render() {
      Layout root = new Layout();
      Rows rows = new Rows(
        browserModel.headers.Select( 
          x => { 
            if ( x.puzzleId == browserModel.headers[browserModel.selection].puzzleId ) {
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
