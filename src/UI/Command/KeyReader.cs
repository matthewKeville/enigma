using Microsoft.Extensions.Hosting;

namespace UI.Commands {

  public class KeyInputEventArgs : EventArgs {
    public ConsoleKey key;
    public KeyInputEventArgs(ConsoleKey key) {
      this.key = key;
    }
  }

  public class KeyDispatcher {
    public event EventHandler<KeyInputEventArgs> RaiseKeyInputEvent;
    public void DispatchKeyInputEvent(ConsoleKey key) {
      RaiseKeyInputEvent(this,new KeyInputEventArgs(key));
    }
  }

  public class KeyReader {

    private Thread interpreterThread;
    private bool running = true;
    private KeyDispatcher keyDispatcher;

    public KeyReader(IApplicationLifetime applicationLifetime,KeyDispatcher keyDispatcher) {
      this.keyDispatcher = keyDispatcher;
      applicationLifetime.ApplicationStopping.Register(stop);
      Trace.WriteLine("KeyReader starting ...");
      interpreterThread = new Thread(processIO);
      interpreterThread.Start();
    }

    void processIO() {
      while ( running ) {
        if ( Console.KeyAvailable ) {
          ConsoleKeyInfo keyInfo = Console.ReadKey(true);
          Trace.WriteLine("dispatching key");
          keyDispatcher.DispatchKeyInputEvent(keyInfo.Key);
        }
      }
    }

    void stop() {
      Trace.WriteLine("CommandInterpeter stopping ...");
      running = false;
    }

  }

}
