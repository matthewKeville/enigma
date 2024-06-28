using Enums;

namespace Model {

public record ClueModel(int ordinal, String value);

public class CluesModel {

  //I dont' love the depence on the GridModel, but i'm not sure
  //how to highlight the active word, without this.
  private GridModel gridModel;
  public List<ClueModel> across = new List<ClueModel>();
  public List<ClueModel> down = new List<ClueModel>();

  public CluesModel(CrosswordModel crosswordModel,GridModel gridModel) {
    this.gridModel = gridModel;
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

  public bool IsActiveClue(ClueModel clueModel) {
    return clueModel.ordinal == gridModel.ActiveWord().i;  
  }

}

}
