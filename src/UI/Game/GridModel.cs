using Entity;
using Enums;
using Terminal.Gui;

namespace UI.Model.Game {

  public class GridWordModel {
    public int x;
    public int y;
    public int i;
    public Direction direction;
    public String answer ="";
    public String prompt ="";

    public GridWordModel(int x, int y, int i, Direction direction, String answer, String prompt) {
      this.x = x;
      this.y = y;
      this.i = i;
      this.direction = direction;
      this.answer = answer;
      this.prompt = prompt;
    }
  }

  public class GridCharModel {
    public int X;
    public int Y;
    public char C;
    public int Status = 0;
    public bool IsBlock;
    public GridCharModel? Up;
    public GridCharModel? Down;
    public GridCharModel? Left;
    public GridCharModel? Right;

    public GridCharModel(int x, int y, char c, bool isBlock) {
      this.X = x;
      this.Y = y;
      this.C = c;
      this.IsBlock = isBlock;
    }

    public override bool Equals(Object? obj) {
      if (obj is null) {
        return false;
      }
      if (obj is not GridCharModel) {
        return false;
      }
      GridCharModel other = (GridCharModel) obj;
      return other.X == X && other.Y == Y && other.C == C;
    }

    public void Dump() {
      Trace.WriteLine($"gcm : {X},{Y},{C},{IsBlock}, {Up is null}, {Down is null}, {Right is null}, {Left is null}");
    }

  }

  public class GridClueModel {
    public int X;
    public int Y;
    public int I;
    public Direction Direction;
    public int Size;
    public GridClueModel(int x, int y, int i, Direction direction, int size) {
      this.X = x;
      this.Y = y;
      this.I = i;
      this.Direction = direction;
      this.Size = size;
    }
  }

  public class GridModel {

    public List<GridClueModel> GridClueModels;
    public List<GridCharModel> GridCharModels;
    public GridCharModel Selection;

    public int crosswordId;
    public int ColumnCount = 0;
    public int RowCount = 0;
    public Direction Orientation = Direction.Across;

    public int WordCheckCount;

    public GridModel(List<GridChar> gridChars,List<Word> words) {

      //Clue Models

      GridClueModels = new ();
      foreach ( Word word in words ) {
        GridClueModels.Add(new GridClueModel(word.X,word.Y,word.I,word.Direction,word.Answer.Count()));
      }

      //Char Models

      GridCharModels = new ();
      foreach ( GridChar gc in  gridChars ) {
        var gcm = new GridCharModel(gc.X,gc.Y,gc.C,gc.C == '\0');
        GridCharModels.Add(gcm);
      }

      foreach ( GridCharModel gcm in GridCharModels ) {
        gcm.Up = GridCharModels.FirstOrDefault( m => m.X == gcm.X && m.Y == gcm.Y-1,null);
        gcm.Down = GridCharModels.FirstOrDefault( m => m.X == gcm.X && m.Y == gcm.Y+1,null);
        gcm.Left = GridCharModels.FirstOrDefault( m => m.X == gcm.X-1 && m.Y == gcm.Y,null);
        gcm.Right = GridCharModels.FirstOrDefault( m => m.X == gcm.X+1 && m.Y == gcm.Y,null);
      }

      Selection = GridCharModels.First();

    }

    //Return the list of GridCharModels that represent the "word" answer to
    //the clue in order.
    private List<GridCharModel> getWordChars(GridCharModel gcm,Direction direction) {

      var wordChars = new List<GridCharModel> {};

      if ( direction == Direction.Across ) {

        var cur = gcm;
        while ( cur.Left != null && !cur.Left.IsBlock ) {
          cur = cur.Left;
          wordChars.Insert(0,cur);
        }

        wordChars.Add(gcm);
        cur = gcm;

        while ( cur.Right != null && !cur.Right.IsBlock ) {
          cur = cur.Right;
          wordChars.Add(cur!);
        }

      } else {

        var cur = gcm;
        while ( cur.Up != null && !cur.Up.IsBlock ) {
          cur = cur.Up;
          wordChars.Insert(0,cur);
        }

        wordChars.Add(gcm);
        cur = gcm;

        while ( cur.Down != null && !cur.Down.IsBlock ) {
          cur = cur.Down;
          wordChars.Add(cur!);
        }

      }

      return wordChars;

    }

    //Move the Selection to the starting character of the next clue
    //prev = true, will move to the previous clue
    private void MoveClue(bool prev) {

      List<GridCharModel> wordChars = getWordChars(Selection,Orientation);
      GridCharModel start = wordChars.First();

      GridClueModel? clue = GridClueModels.Where( cm => cm.X == start.X && cm.Y == start.Y).FirstOrDefault();
      if ( clue == null ) {
        Trace.WriteLine("no clue found");
        return;
      }

      // a bit hacky here...
      GridClueModel? targetClue = GridClueModels
        .Where( cm => cm.Direction == Orientation )
        .Where( cm => prev ? cm.I < clue.I : cm.I > clue.I )
        .OrderBy( cm => cm.I * (prev ? -1 : 1) )
        .FirstOrDefault();

      if ( targetClue != null ) {
        Trace.WriteLine($"clue found {targetClue.X},{targetClue.Y}");
        GridCharModel targetCharModel = GridCharModels
          .Where( gcm => gcm.X == targetClue.X && gcm.Y == targetClue.Y )
          .First();
        Selection = targetCharModel;
      } else {
        Trace.WriteLine("clue not found");
      }
    }

    public List<GridCharModel> ActiveWordChars() {
      return getWordChars(Selection,Orientation);
    }

    public bool MoveUp() {
      if ( !Selection.Up?.IsBlock ?? false ) {
        Selection = Selection.Up!;
        return true;
      }
      return false;
    }

    public bool MoveDown() {
      if ( !Selection.Down?.IsBlock ?? false ) {
        Selection = Selection.Down!;
        return true;
      }
      return false;
    }

    public bool MoveRight() {
      if ( !Selection.Right?.IsBlock ?? false ) {
        Selection = Selection.Right!;
        return true;
      }
      return false;
    }

    public bool MoveLeft() {
      if ( !Selection.Left?.IsBlock ?? false ) {
        Selection = Selection.Left!;
        return true;
      }
      return false;
    }

    public void MoveNextClue() {
      MoveClue(false);
    }
    public void MovePrevClue() {
      MoveClue(true);
    }



    public void SwapOrientation(){
      if ( Orientation == Direction.Across ) {
        Orientation = Direction.Down;
      } else {
        Orientation = Direction.Across;
      }
    }

    public void InsertChar(char c){
      Selection.C = c;
      if ( Orientation == Direction.Across ) {
        if ( !(Selection.Right?.IsBlock ?? true) ) {
          Selection = Selection.Right;
        }
      } else {
        if ( !(Selection.Down?.IsBlock ?? true)) {
          Selection = Selection.Down;
        }
      }
      //todo advance key ...
    }

    public void ReplaceChar(char c){
      Selection.C = c;
    }

    public void DeleteChar(){
      Selection.C = ' ';
    }

    // Delete the characters from the current Selection to
    // the end of the current clue
    public void DeleteWord(){
      var wordChars = getWordChars(Selection,Orientation);
      var selectIndex = wordChars.IndexOf(Selection);
      while ( selectIndex < wordChars.Count() ) {
        wordChars[selectIndex].C = ' ';
        selectIndex++;
      }
    }

    // Delete all the characters from the current clue,
    // and move the selection to the beginning word character
    public void DeleteInnerWord(){
      var wordChars = getWordChars(Selection,Orientation);
      var index = 0;
      while ( index < wordChars.Count() ) {
        wordChars[index].C = ' ';
        index++;
      }
      Selection = wordChars.First();
    }


    /**
    public void MoveToOrdinal(int ordinal,Direction direction) {
    public void DeleteWord() {
    private void FindCharForward(ConsoleKey key) {
    private void FindCharBackward(ConsoleKey key) {
    public void MoveToWordStart() {
    public void MoveToWordEnd() {
    public void CheckWord() {
    public bool IsComplete() {
    */

}
}
