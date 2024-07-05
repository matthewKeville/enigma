using Enums;

namespace UI.Model.Game {

public record ClueModel(int ordinal, String value);

public class CluesModel : IModel {

  public List<ClueModel> Across;
  public List<ClueModel> Down;
  public (int,Direction) ActiveClue;


  public void NextClue() {
    findNextOrdinal(1);
  }

  public void PrevClue() {
    findNextOrdinal(-1);
  }

  private int FindCurrentIndex() {
    List<ClueModel> clues = ActiveClue.Item2 == Direction.Across ?
      Across : Down;
    return clues.FindIndex( c => c.ordinal == ActiveClue.Item1 );
  }

  private List<ClueModel> CurrentList() {
    return ActiveClue.Item2 == Direction.Across ?
      Across : Down;
  }

  private void findNextOrdinal(int steps) {
    int currentWordIndex = FindCurrentIndex();
    List<ClueModel> clues = CurrentList();

    if ( steps > 0 ) {
      currentWordIndex = Math.Min((currentWordIndex + 1),clues.Count-1);
    } else {
      currentWordIndex = Math.Max(0,currentWordIndex-1);
    }

    ClueModel nextClue = clues[currentWordIndex];
    ActiveClue = (nextClue.ordinal,ActiveClue.Item2);

  }

  public void ChangeOrientation(bool leftwards) {
    if ( !leftwards && ActiveClue.Item2 == Direction.Across ) {
      int index = Math.Min(FindCurrentIndex(),Down.Count-1);
      ActiveClue.Item1 = Down[index].ordinal;
      ActiveClue.Item2 = Direction.Down;
    } else if ( leftwards && ActiveClue.Item2 == Direction.Down ) {
      int index = Math.Min(FindCurrentIndex(),Across.Count-1);
      ActiveClue.Item1 = Across[index].ordinal;
      ActiveClue.Item2 = Direction.Across;
    }
  }

}

}
