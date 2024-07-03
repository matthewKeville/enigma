using Enums;
using Model;

namespace Services {

  public class DebugCrosswordGenerator {

    /**
      cats#f
      on#two
      bday#g
    */
    public CrosswordModel sample { 
      get {

        CrosswordModel crossword = new CrosswordModel(3,6);
        crossword.name = "DEBUG";

        // ROW 1
                                    //x,y,i
        crossword.words.Add(new WordModel(0,0,1,Direction.Across,"cats","?"));
        crossword.words.Add(new WordModel(0,0,1,Direction.Down,"cob","?"));
        crossword.words.Add(new WordModel(1,0,2,Direction.Down,"and","?"));
        crossword.words.Add(new WordModel(3,0,3,Direction.Down,"sty","?"));
        crossword.words.Add(new WordModel(5,0,4,Direction.Down,"fog","?"));

        // ROW 2

        crossword.words.Add(new WordModel(0,1,5,Direction.Across,"on","?"));
        crossword.words.Add(new WordModel(3,1,6,Direction.Across,"two","?"));

        // ROW 3

        crossword.words.Add(new WordModel(0,2,7,Direction.Across,"bday","?"));
        return crossword;
      }
    }


  }
}
