using System.Diagnostics;
using System.ComponentModel;
using Model;

namespace Services {

  public class NYDebugCrosswordProvider : ICrosswordProvider {

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

    public NYDebugCrosswordProvider() {
      internalCrossword = new NYDebugCrosswordGenerator().crossword;
    }


  }
}
