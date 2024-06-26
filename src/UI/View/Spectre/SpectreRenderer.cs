using Spectre.Console;
namespace UI.View.Spectre;

public class SpectreRenderer {

  private MainView mainView;
  public SpectreRenderer(MainView mainView) {
    this.mainView = mainView;
  }

  public void init() {

    Layout root = new Layout();

    AnsiConsole.Live(root)
      .Start(ctx =>
      {
        while (true) {
        root.Update(mainView.Render());
          ctx.Refresh();
        }
      });
  }

}
