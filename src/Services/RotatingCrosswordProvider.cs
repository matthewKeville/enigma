using System.Diagnostics;
using System.ComponentModel;
using Model;

namespace Services {

  public class RotatingCrosswordProvider : ICrosswordProvider {

    public event PropertyChangedEventHandler PropertyChanged;

    private Crossword internalCrossword;
    public Crossword crossword { 
      get  {
        return internalCrossword;
      }
      set  {
        internalCrossword = value;
        PropertyChanged.Invoke(this,new PropertyChangedEventArgs(nameof(crossword)));
      }
    }

    private Crossword ny;
    private Crossword db;

    public RotatingCrosswordProvider() {
      ny = new NYDebugCrosswordGenerator().crossword;
      db = new DebugCrosswordGenerator().crossword;
      internalCrossword = db;
      Thread swapThread = new Thread(swapCrosswords);
      swapThread.Start();
    }

    public void swapCrosswords() {
      bool isNy = true;
      while ( true ) {
        Thread.Sleep(5000);
        Trace.WriteLine("Swapping threads");
        if ( isNy ) {
          crossword = db;
          Trace.WriteLine("db");
        } else {
          crossword = ny;
          Trace.WriteLine("ny");
        }
        isNy = !isNy;
      }
    }


  }
}
