using Spectre.Console;
using UI.Model.Help;

namespace UI.View.Spectre.Help {

  public class HelpView : SpectreView<HelpModel> {

    public HelpView() {}

    protected override Layout render() {
      Layout root = new Layout();
      List<Text> texts = new List<Text>();
      foreach ( KeyMapInfo km in model.commandMappings ) {
        texts.Add( new Text($"Key {km.key.ToString()} : {km.command.ToString()}"));
      }
      
      Rows rows = new Rows(texts);
      root.Update(rows);
      return root;
    }

  }
}
