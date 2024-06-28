using Enums;

namespace Model {

public record ClueModel(int ordinal, String value);

public class CluesModel {

  public List<ClueModel> across = new List<ClueModel>();
  public List<ClueModel> down = new List<ClueModel>();
  public CluesModel(CrosswordModel crosswordModel) {
    across = crosswordModel.words
          .Where( w => w.direction == Direction.Across )
          .OrderBy( w => w.i )
          .Select( w => new ClueModel(w.i,w.prompt))
          .ToList();
    down = crosswordModel.words
          .Where( w => w.direction == Direction.Down )
          .OrderBy( w => w.i )
          .Select( w => new ClueModel(w.i,w.prompt))
          .ToList();
  }


  /**

  public List<ClueModel>  across { 
    get {
      return crossword.words
        .Where( w => w.direction == Direction.Across )
        .OrderBy( w => w.i )
        .Select( w => new ClueModel(w.i,w.prompt))
        .ToList();
    }
    private set {}
  }

  public List<ClueModel>  down { 
    get {
      return crossword.words
        .Where( w => w.direction == Direction.Down )
        .OrderBy( w => w.i )
        .Select( w => new ClueModel(w.i,w.prompt))
        .ToList();
    }
    private set {}
  }

  */

}

}
