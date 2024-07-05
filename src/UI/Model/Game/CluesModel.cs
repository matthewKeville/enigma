using Enums;

namespace UI.Model.Game {

public record ClueModel(int ordinal, String value);

public class CluesModel : IModel {

  public List<ClueModel> Across;
  public List<ClueModel> Down;
  public (int,Direction) ActiveClue;

}

}
