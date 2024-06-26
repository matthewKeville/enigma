namespace Model {

public struct AnswerBlank {
  public int ordinal;
  public int size;
  public Direction direction;
}

public class Game {

  public Dictionary<int,char[]> across;
  public Dictionary<int,char[]> down;

  public Game(List<AnswerBlank> answerBlanks) {
    across = new Dictionary<int, char[]>();
    down = new Dictionary<int, char[]>();
    foreach ( AnswerBlank answerBlank in answerBlanks ) {
      if ( answerBlank.direction == Direction.Across ) {
        across.Add(answerBlank.ordinal,new Char[answerBlank.size]);
      }
      else {
        down.Add(answerBlank.ordinal,new Char[answerBlank.size]);
      }
    }

  }
}

}
