using Entity;
using Enums;

namespace Services.CrosswordInstaller.NYT
{

    public struct NYTCrosswordData {
      public int rows;
      public int columns;
      public List<String> solution;
      public List<String> acrossClues;
      public List<String> downClues;
      public DateOnly published;
    }

    public class NYTCrosswordParser 
    {

        public Crossword ParseData(NYTCrosswordData data) {

          // Grid Chars

          List<GridChar> gridChars = new List<GridChar>();
          char[,] answerMatrix = new char[data.columns, data.rows];

          int rowIndex = 0;
          foreach ( String rowString in data.solution ) {
            int columnIndex = 0;
            foreach ( char c in rowString ) {

              answerMatrix[columnIndex, rowIndex] = rowString[columnIndex];
              Char actual = rowString[columnIndex];
              gridChars.Add(new GridChar(){
                  X=columnIndex,
                  Y=rowIndex,
                  UserChar=actual != '#' ? ' ' : '\0',       //null byte encodes block
                  AnswerChar=actual != '#' ? actual : '\0',  //null byte encodes block
                  IsBlock = actual == '#'
                });
            
              columnIndex++;
            }
            rowIndex++;
          }

          // Words

          List<Clue> Words = new List<Clue>();

          int ordinal = 1;

          for (int j = 0; j < data.rows; j++)
          {
              for (int i = 0; i < data.columns; i++)
              {

                  if (answerMatrix[i, j] == '#')
                  {
                      continue;
                  }

                  bool wordHit = false;

                  //is word across clue?
                  if ((i != data.columns) && (i == 0 || answerMatrix[i - 1, j] == '#'))
                  {

                      //match clue to ordinal and position
                      if (data.acrossClues.Count() == 0)
                      {
                          // Trace.WriteLine("Critical error, no clues left");
                          // Trace.WriteLine($" i,j {i},{j} : is {answerMatrix[i, j]}, prev is {answerMatrix[i - 1, j]}");
                          Environment.Exit(0);
                      }

                      String clue = data.acrossClues[0];
                      data.acrossClues.RemoveAt(0);
                      // Trace.WriteLine(
                      //     string.Format("{0} across is {1} at r,c {2},{3}", ordinal, clue, i, j)
                      // );

                      //mine answer
                      int wend = i;
                      String answer = "";
                      while (wend != data.columns && answerMatrix[wend, j] != '#')
                      {
                          answer += answerMatrix[wend, j];
                          wend++;
                      }
                      // Trace.WriteLine("\tand the answer is " + answer);
                      Words.Add(new Clue() {
                          X = i,
                          Y = j,
                          I = ordinal,
                          Direction = Direction.Across,
                          Prompt = clue,
                          Answer = answer
                      });
                      wordHit = true;
                  }

                  //is word down clue?
                  if ((j != data.rows) && (j == 0 || answerMatrix[i, j - 1] == '#'))
                  {
                      if (data.downClues.Count() == 0)
                      {
                          // Trace.WriteLine("Critical error, no clues left");
                          // Trace.WriteLine($" i,j {i},{j} : is {answerMatrix[i, j]}, prev is {answerMatrix[i - 1, j]}");
                          Environment.Exit(0);
                      }

                      //match clue to ordinal and position
                      String clue = data.downClues[0];
                      data.downClues.RemoveAt(0);
                      // Trace.WriteLine(
                      //     string.Format("{0} down is {1} at r,c {2},{3}", ordinal, clue, i, j)
                      // );

                      //mine answer
                      int wend = j;
                      String answer = "";
                      while (wend != data.rows && answerMatrix[i, wend] != '#')
                      {
                          answer += answerMatrix[i, wend];
                          wend++;
                      }
                      // Trace.WriteLine("\tand the answer is " + answer);
                      Words.Add(new Clue(){
                          X=i,
                          Y=j,
                          I=ordinal,
                          Direction=Direction.Down,
                          Prompt=clue,
                          Answer=answer
                        });

                      wordHit = true;
                  }


                  if (wordHit)
                  {
                      ordinal++;
                  }

              }
          }

          //Collect

          Crossword crossword = new Crossword() {
            Rows = data.rows,
            Columns = data.columns,
          };
          crossword.Type = CrosswordType.NYTIMES;
          crossword.Clues.AddRange(Words);
          crossword.GridChars.AddRange(gridChars);

          return crossword;

        }

    }

}
