using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using Spectre.Console;

namespace UI.View.Spectre {

public class SpectreRenderer {

  private RootView rootView;
  private bool running = true;
  private Thread renderThread;

  public SpectreRenderer(IApplicationLifetime applicationLifetime,RootView rootView) {
    applicationLifetime.ApplicationStopping.Register(stop);
    this.rootView = rootView;
    //Trace.WriteLine("Sleeping (temp fix) ...");
    // Thread.Sleep(1000);
    Trace.WriteLine("SpectreRender starting...");
    renderThread = new Thread(renderLoop);
    renderThread.Start();
  }

  public void stop() {
    Trace.WriteLine("SpectreRender shutting down...");
    running = false;
  }

  public void renderLoop() {
    Layout root = new Layout();
    AnsiConsole.Live(root)
      .Start(ctx =>
      {
        while (running) {
          root.Update(rootView.Render());
          ctx.Refresh();
        }
      });
  }

}

}
