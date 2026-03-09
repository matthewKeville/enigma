using Enums;

namespace Entity {

  public class Crossword {

    public int Id { get; set;}

    // Static Puzzle Data

    public String Type { get; set; }
    public String Title { get; set; }
    public DateTime Published { get; set; }
    public int Rows { get; set; }
    public int Columns { get; set; }
    public List<Clue> Clues { get; set; } = new List<Clue>();

    // Application Puzzle Data

    public List<GridChar> GridChars { set; get; } = new List<GridChar>();
    public DateTime? StartDate { get; set; }
    public DateTime? FinishDate { get; set; }
    public TimeSpan Elapsed { get; set; } = TimeSpan.Zero;

    public int WordCheckCount { get; set; } = 0;
    public int CharacterCheckCount { get; set; } = 0;
    public int PuzzleCheckCount { get; set; } = 0;

    public String ToString() {

      String result =  $"Crossword(Type={Type}, Title={Title}, Published={Published}, Rows={Rows}, Columns={Columns}";
      result+=$"\n Clues {Clues.Count()}";
      foreach ( Clue cl in Clues ) {
        result += "\n" + cl.ToString();
      }
      result+=$"\n GridChars {GridChars.Count()}";
      foreach ( GridChar gc in GridChars ) {
        result += "\n" + gc.ToString();
      }

      return result;
        
    }


  }

  public class Clue  {
    public int Id { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int I { get; set; }
    public Direction Direction { get; set; }
    public String Prompt { get; set; } = "";
    public String Answer { get; set; } = "";

    public int CrosswordId { get; set; }

    public String ToString() {
      return $"Clue(Id={Id}, X={X}, Y={Y}, I={I}, Direction={Direction.ToString()}, Prompt={Prompt}, Answer={Answer}";
    }

  }


  public class GridChar {
    public int Id { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public char? UserChar { get; set; }
    public char AnswerChar { get; set; }
    public String KnownChars { get; set; } = "";
    public bool IsBlock { get; set; }

    public int CrosswordId { get; set; }

    public String ToString() {
      return $"GridChar(Id={Id}, X={X}, Y={Y}, IsBlock={IsBlock}, UserChar={UserChar ?? ' '}, AnswerChar={AnswerChar})";
    }

  }

}
