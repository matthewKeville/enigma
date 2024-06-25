namespace Model {

public enum Direction {
  Across,
  Down
}

public class Word {
  public int x { get; set; }
  public int y { get; set; }
  public int i { get; set; }
  public Direction direction { get; set; }
  public String answer { get; set; } 
  public String prompt { get; set; } 

  public Word(int x, int y, int i, Direction direction,String answer, String prompt) {
    this.x = x;
    this.y = y;
    this.i = i;
    this.direction = direction;
    this.answer = answer;
    this.prompt = prompt;
  
  }

  public override String ToString() {
    return string.Format(
        "\tlocation : {0},{1}" +
        "\n\tordinal : {2}"  +
        "\n\tdirection : {3}"  +
        "\n\tprompt : {4}",
        x,y,i,direction.ToString(),prompt
      );
  }

}

}
