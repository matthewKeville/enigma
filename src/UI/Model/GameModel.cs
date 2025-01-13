
namespace UI.Model
{

    public class GameModel
    {
      public int CrosswordId;
      public GridModel GridModel;
      public DateTime SessionStartTime;
      public TimeSpan PrevElapsed;

      public GameModel(int crosswordId, GridModel gridModel,TimeSpan prevElapsed) {
        CrosswordId = crosswordId;
        GridModel = gridModel;
        SessionStartTime = DateTime.UtcNow;
        PrevElapsed = prevElapsed;
      }
    }
}
