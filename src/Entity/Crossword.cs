using Enums;

namespace Entity {

  public class Crossword {

    public int Id { get; set;}
    public CrosswordType Type { get; set; }
    public String Title { get; set; }
    public DateTime Published { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? FinishDate { get; set; }
    public TimeSpan Elapsed { get; set; } = TimeSpan.Zero;
    public int Rows { get; set; }
    public int Columns { get; set; }
    public int WordCheckCount { get; set; } = 0;

    public List<Word> Words { get; } = new List<Word>();
    public List<GridChar> GridChars { get; } = new List<GridChar>();

  }

  public class Word  {
    public int Id { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int I { get; set; }
    public Direction Direction { get; set; }
    public String Answer { get; set; } 
    public String Clue { get; set; } 

    public int CrosswordId { get; set; }
    public Crossword crossword { get; set; }

  }

  public class GridChar {
    public int Id { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public char C { get; set; }

    public int CrosswordId { get; set; }
    public Crossword crossword { get; set; }

  }
}
