using Model;
using Services;

namespace UI.ViewModel {

public class StatusViewModel {

  private ICrosswordProvider crosswordProvider;
  private Crossword crossword { 
    get {
      return crosswordProvider.crossword;
    }
  }

  public StatusViewModel(ICrosswordProvider crosswordProvider) {
    this.crosswordProvider = crosswordProvider;
  }

  public String getTitle() {
    return crossword.name;
  }

}

}
