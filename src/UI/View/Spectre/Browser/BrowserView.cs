using Context;
using Model;
using Spectre.Console;

namespace UI.View.Spectre.Browser {

  public class BrowserView {

    public BrowserModel browserModel;

    public void setContext(ApplicationContext context) {
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
