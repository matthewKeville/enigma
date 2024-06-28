using System.Diagnostics;
using System.ComponentModel;
using Model;

namespace Services {

  public class NYDebugCrosswordProvider : ICrosswordProvider {

    public event PropertyChangedEventHandler PropertyChanged;

    private CrosswordModel internalCrossword;
    public CrosswordModel crossword { 
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
