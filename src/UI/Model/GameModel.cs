
namespace UI.Model
{

    public class GameModel
    {
      public int CrosswordId;
      public GridModel GridModel;
      public DateTime SessionStartTime;
      public TimeSpan PrevElapsed;

      public int WordCheckCount;
      public int CharacterCheckCount;
      public int PuzzleCheckCount;

      public GameModel(int crosswordId, GridModel gridModel,
          TimeSpan prevElapsed,int wordCheckCount, int characterCheckCount,
          int puzzleCheckCount) {
        CrosswordId = crosswordId;
        GridModel = gridModel;
        SessionStartTime = DateTime.UtcNow;
        PrevElapsed = prevElapsed;
        CharacterCheckCount = characterCheckCount;
        WordCheckCount = wordCheckCount;
        PuzzleCheckCount = puzzleCheckCount;
      }
    }
}
