using Model;

namespace Application {


  public class ApplicationContext() {

    private Crossword backingCrossword;
    private Game backingGame;

    public Crossword crossword { 
      get {
        return this.backingCrossword;
      } 
    }
    public Game Game { 
      get {
        return this.backingGame;
      } 
    }
  }
}
