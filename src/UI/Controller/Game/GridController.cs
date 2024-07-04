using System.Diagnostics;
using System.Drawing;
using Context;
using Enums;
using UI.Command;
using UI.Model.Game;
using UI.View.Spectre.Game;

namespace UI.Controller.Game {

public class GridController {

  private GridView gridView;
  private GridModel grid;
  private ContextAccessor contextAccessor;

  public GridController(ContextAccessor contextAccessor, GridView gridView) {
    this.contextAccessor = contextAccessor;
    this.grid = contextAccessor.getContext().gridModel;
    this.gridView = gridView;
    this.gridView.setContext(contextAccessor.getContext());
  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {
    switch ( commandEventArgs.command ) {

      case Command.Command.UPDATE_CONTEXT:
        Trace.WriteLine("puzzle swap triggered in game grid controller");
        this.grid = contextAccessor.getContext().gridModel;
        gridView.setContext(contextAccessor.getContext());
        break;

      case Command.Command.MOVE_LEFT:
        MoveEntry(Move.LEFT);
        break;

      case Command.Command.MOVE_RIGHT:
        MoveEntry(Move.RIGHT);
        break;

      case Command.Command.MOVE_UP:
        MoveEntry(Move.UP);
        break;

      case Command.Command.MOVE_DOWN:
        MoveEntry(Move.DOWN);
        break;

      case Command.Command.MOVE_WORD_START:
        MoveToWordStart();
        break;
      case Command.Command.MOVE_WORD_END:
        MoveToWordEnd();
        break;

      case Command.Command.INSERT_CHAR:
        Trace.WriteLine("inserting character");
        if (commandEventArgs.key is null) {
          Trace.WriteLine("Critical error , INSERT_CHAR command requires a key, it is null");
          Environment.Exit(1);
        } 
        InsertKey((ConsoleKey)commandEventArgs.key);
        break;

      case Command.Command.DEL_CHAR:
        DeleteKey();
        break;

      case Command.Command.DEL_WORD:
        DeleteWord();
        break;

      case Command.Command.SWAP_ORIENTATION:
        SwapOrientation();
        break;
    }
  }



  public void MoveEntry(Move move) {

    int offx = 0;
    int offy = 0;

    //OOB check
    switch (move) {
      case Move.RIGHT:
        if ( grid.entry.X != grid.ColumnCount-1) {
          offx = 1;
        }
        break;
      case Move.UP:
        if ( grid.entry.Y != 0 ) {
          offy = -1;
        }
        break;
      case Move.LEFT:
        if ( grid.entry.X != 0 ) {
          offx = -1;
        }
        break;
      case Move.DOWN:
        if ( grid.entry.Y != grid.RowCount-1 ) {
          offy = 1;
        }
        break;
    }

    //Valid word position?
    Point nextEntry = Point.Add(grid.entry,new System.Drawing.Size(offx,offy));
    if ( grid.IsInWord(nextEntry.X,nextEntry.Y) ) {
      grid.entry = nextEntry;
    }

  }

  public void SwapOrientation() {
    grid.orientation = ( grid.orientation == Direction.Across ) ? Direction.Down : Direction.Across;
  }


  public void MoveToWordStart() {
    WordModel? expectWord = grid.ActiveWord();
    if (expectWord == null) {
      Trace.WriteLine("Critical Error : ActiveWord not found");
      Environment.Exit(0);
    }
    WordModel word = (WordModel) expectWord;
    grid.entry = new Point(word.x,word.y);
  }

  public void MoveToWordEnd() {
    WordModel? expectWord = grid.ActiveWord();
    if (expectWord == null) {
      Trace.WriteLine("Critical Error : ActiveWord not found");
      Environment.Exit(0);
    }
    WordModel word = (WordModel) expectWord;
    grid.entry = word.direction == Direction.Across ? 
      new Point(word.x + word.answer.Count()-1,word.y) :
      new Point(word.x,word.y + word.answer.Count()-1);
  }

  public void InsertKey(ConsoleKey key) {
    grid.charMatrix[grid.entry.X,grid.entry.Y] = (char) key;
    MoveEntry(grid.orientation == Direction.Across ? Move.RIGHT : Move.DOWN);
  }

  public void DeleteKey() {
    grid.charMatrix[grid.entry.X,grid.entry.Y] = ' ';
    MoveEntry(grid.orientation == Direction.Across ? Move.LEFT : Move.UP);
  }

  public void DeleteWord() {
    WordModel? expectWord = grid.ActiveWord();
    if (expectWord == null) {
      Trace.WriteLine("Critical Error : ActiveWord not found");
      Environment.Exit(0);
    }

    WordModel word = (WordModel) expectWord;

    for ( int n = 0; n < word.answer.Count(); n++ ) {
      if ( word.direction == Direction.Across ) {
        grid.charMatrix[word.x + word.answer.Count()-1 - n , word.y] = ' ';
      } else {
        grid.charMatrix[word.x,word.y + word.answer.Count()-1 - n ] = ' ';
      }
    }

  }



  /**

  public void MoveEntry(Move move) {

    int offx = 0;
    int offy = 0;

    //OOB check
    switch (move) {
      case Move.RIGHT:
        if ( grid.entry.X != crossword.colCount-1) {
          offx = 1;
        }
        break;
      case Move.UP:
        if ( grid.entry.Y != 0 ) {
          offy = -1;
        }
        break;
      case Move.LEFT:
        if ( grid.entry.X != 0 ) {
          offx = -1;
        }
        break;
      case Move.DOWN:
        if ( grid.entry.Y != crossword.rowCount-1 ) {
          offy = 1;
        }
        break;
    }

    //Valid word position?
    Point nextEntry = Point.Add(grid.entry,new System.Drawing.Size(offx,offy));
    if ( IsInWord(nextEntry.X,nextEntry.Y) ) {
      grid.entry = nextEntry;
    }

  }

  public void SwapOrientation() {
    orientation = ( orientation == Direction.Across ) ? Direction.Down : Direction.Across;
  }


  public void MoveToWordStart() {
    Word? expectWord = ActiveWord();
    if (expectWord == null) {
      Trace.WriteLine("Critical Error : ActiveWord not found");
      Environment.Exit(0);
    }
    Word word = (Word) expectWord;
    grid.entry = new Point(word.x,word.y);
  }

  public void MoveToWordEnd() {
    Word? expectWord = ActiveWord();
    if (expectWord == null) {
      Trace.WriteLine("Critical Error : ActiveWord not found");
      Environment.Exit(0);
    }
    Word word = (Word) expectWord;
    grid.entry = word.direction == Direction.Across ? 
      new Point(word.x + word.answer.Count()-1,word.y) :
      new Point(word.x,word.y + word.answer.Count()-1);
  }

  public void InsertKey(ConsoleKey key) {
    charMatrix[grid.entry.X,grid.entry.Y] = (char) key;
    MoveEntry(orientation == Direction.Across ? Move.RIGHT : Move.DOWN);
  }

  public void DeleteKey() {
    charMatrix[grid.entry.X,grid.entry.Y] = ' ';
    MoveEntry(orientation == Direction.Across ? Move.LEFT : Move.UP);
  }

  public void DeleteWord() {
    Word? expectWord = ActiveWord();
    if (expectWord == null) {
      Trace.WriteLine("Critical Error : ActiveWord not found");
      Environment.Exit(0);
    }

    Word word = (Word) expectWord;

    for ( int n = 0; n < word.answer.Count(); n++ ) {
      if ( word.direction == Direction.Across ) {
        charMatrix[word.x + word.answer.Count()-1 - n , word.y] = ' ';
      } else {
        charMatrix[word.x,word.y + word.answer.Count()-1 - n ] = ' ';
      }
    }

  }

  */


}

}
