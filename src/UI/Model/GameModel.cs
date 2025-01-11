
namespace UI.Model
{

    public class GameModel
    {
      public int CrosswordId;
      public GridModel GridModel;

      public GameModel(int crosswordId, GridModel gridModel) {
        CrosswordId = crosswordId;
        GridModel = gridModel;
      }
    }
}
