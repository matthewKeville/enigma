using Model;
using Services;

namespace UI.View.ViewModel {

public record ClueViewModel(int ordinal, String value);

public class CluesViewModel {

  private ICrosswordProvider crosswordProvider;

  private Crossword crossword {
    get {
      return crosswordProvider.crossword;
    }
  }

  public List<ClueViewModel>  across { 
    get {
      return crossword.words
        .Where( w => w.direction == Direction.Across )
        .OrderBy( w => w.i )
        .Select( w => new ClueViewModel(w.i,w.prompt))
        .ToList();
    }
    private set {}
  }

  public List<ClueViewModel>  down { 
    get {
      return crossword.words
        .Where( w => w.direction == Direction.Down )
        .OrderBy( w => w.i )
        .Select( w => new ClueViewModel(w.i,w.prompt))
        .ToList();
    }
    private set {}
  }

  public CluesViewModel(ICrosswordProvider crosswordProvider) {
    this.crosswordProvider = crosswordProvider;
  }

}

}
