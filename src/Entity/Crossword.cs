using Enums;

namespace Entity {

  public class Crossword : IEntity {
    public int Id { get; set;}
    public CrosswordType Type { get; set; }
    public String Title { get; set; }
    public DateTime Published { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? FinishDate { get; set; }
    public TimeSpan? Elapsed { get; set; } = TimeSpan.Zero;
    public List<Word> Words { get; } = new();
    public int Rows { get; set; }
    public int Columns { get; set; }

  }

  public class Word : IEntity {
    public int Id { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int I { get; set; }
    public Direction Direction { get; set; }
    public String Answer { get; set; } 
    public String Clue { get; set; } 

  }
}
