using System.Drawing;
using Enums;

namespace Model.Game {

  public class Game {

    //UI / application state / ephemeral
    public Pane activePane = Pane.GRID;
    public Window activeWindow = Window.GAME;
    public Direction orientation = Direction.Across;
    public Point entry = new Point(0,0);

    //functional properties
    //Word activeWord <-> entry + Crossword
    //

    public Guid id;
    public Crossword crossword; //public Guid Crossword
    public DateTime start;
    public char[,] charMatrix; //represent users answers
  }

  public class Crossword {

    public Guid id;
    public DateTime publicationDate;
    public String name;
    public String author;
    public int rowCount;
    public int colCount;
    public List<WordModel> words;

  }

}
