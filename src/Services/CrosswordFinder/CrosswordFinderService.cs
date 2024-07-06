using Microsoft.Extensions.Hosting;
using Services.CrosswordFinder.Debug;

namespace Services.CrosswordFinder {

public class CrosswordFinderService {

  private Thread thread;
  private List<CrosswordFinder> finders;

  public CrosswordFinderService(IApplicationLifetime applicationLifetime,
      DatabaseContext dbCtx,DebugFinder dbgFinder) {
    finders = new List<CrosswordFinder>(){
      dbgFinder
    };
    applicationLifetime.ApplicationStopping.Register(Stop);
    thread = new Thread(Find);
    thread.Start();
  }

  public void Stop() {
    Trace.WriteLine("Finder service shutting down");
  }

  public void Find() {
    finders.ForEach( f => {
        f.Search();
    });
  }


}

}
