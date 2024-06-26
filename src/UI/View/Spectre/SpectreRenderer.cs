using Spectre.Console;
namespace UI.View.Spectre;

public class SpectreRenderer {

  private RootView rootView;
  public SpectreRenderer(RootView rootView) {
    this.rootView = rootView;
  }

  public void init() {

    Layout root = new Layout();

    AnsiConsole.Live(root)
      .Start(ctx =>
      {
        while (true) {
          root.Update(rootView.Render());
          ctx.Refresh();
        }
      });
  }

}
