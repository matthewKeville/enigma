using Spectre.Console;
using UI.Model.Game.Complete;

namespace UI.View.Spectre.Game.Complete {

  public class CompleteView : SpectreView<CompleteModel> {

    public CompleteView() {}

    protected override Layout render() {
      Layout root = new Layout();

      List<Text> texts = new List<Text>();
      texts.Add( new Text("Complete") );
      texts.Add( new Text("Press q to exit to main menu") );
      
      Rows rows = new Rows(texts);
      root.Update(rows);
      return root;
    }

  }
}
