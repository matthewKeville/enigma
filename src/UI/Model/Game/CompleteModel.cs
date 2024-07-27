using UI.Commands;

namespace UI.Model.Game.Complete {

  public record KeyMapInfo(ConsoleKey key,CommandType commandType);

  public class CompleteModel : IModel {
    public int crosswordId;
  }

}
