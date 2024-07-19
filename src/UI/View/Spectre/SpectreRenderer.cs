using Microsoft.Extensions.Hosting;
using Spectre.Console;

namespace UI.View.Spectre {

public class SpectreRenderer {

  private RootView rootView;
  private bool running = true;
  private Thread renderThread;

  public const String Rendering = "no";
  public static int Fps;
  private int fps;

  public SpectreRenderer(IApplicationLifetime applicationLifetime,RootView rootView) {
    applicationLifetime.ApplicationStopping.Register(Stop);
    this.rootView = rootView;
    Trace.WriteLine("SpectreRender starting...");
    renderThread = new Thread(RenderLoop);
    renderThread.Start();
  }

  public void Stop() {
    Trace.WriteLine("SpectreRender shutting down...");
    running = false;
  }

  public void RenderLoop() {
    Layout root = new Layout();
    AnsiConsole.Live(root)
      .Start(ctx =>
      {
        fps = 0;
        DateTime lastCheck = DateTime.UtcNow;
        while (running) {
          lock(Rendering) {
            if ( DateTime.UtcNow > lastCheck.AddSeconds(1) ) {
              lastCheck = DateTime.UtcNow;
              Fps = fps;
              fps = 0;
            } 
            root.Update(rootView.Render());
            ctx.Refresh();
            fps++;
          }
        }
      });
  }

}

}
