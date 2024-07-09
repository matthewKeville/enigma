using Entity;
using Enums;

namespace Services.CrosswordInstaller.NYT
{

    public class NYTCrosswordParser 
    {

        public Crossword Parse(int rows, int columns,List<String> solution,List<String> acrossClues, List<String> downClues) {

          // Grid Chars

          List<GridChar> gridChars = new List<GridChar>();
          char[,] answerMatrix = new char[columns, rows];

          int rowIndex = 0;
          foreach ( String rowString in solution ) {
            int columnIndex = 0;
            foreach ( char c in rowString ) {

              answerMatrix[columnIndex, rowIndex] = rowString[columnIndex];
              Char actual = rowString[columnIndex];
              gridChars.Add(new GridChar(){
                  X=columnIndex,
                  Y=rowIndex,
                  C=actual != '#' ? ' ' : '\0'
                });
            
              columnIndex++;
            }
            rowIndex++;
          }

          // Words

          List<Word> Words = new List<Word>();

          int ordinal = 1;

          for (int j = 0; j < rows; j++)
          {
              for (int i = 0; i < columns; i++)
              {

                  if (answerMatrix[i, j] == '#')
                  {
                      continue;
                  }

                  bool wordHit = false;

                  //is word across clue?
                  if ((i != columns) && (i == 0 || answerMatrix[i - 1, j] == '#'))
                  {

                      //match clue to ordinal and position
                      if (acrossClues.Count() == 0)
                      {
                          // Trace.WriteLine("Critical error, no clues left");
                          // Trace.WriteLine($" i,j {i},{j} : is {answerMatrix[i, j]}, prev is {answerMatrix[i - 1, j]}");
                          Environment.Exit(0);
                      }

                      String clue = acrossClues[0];
                      acrossClues.RemoveAt(0);
                      // Trace.WriteLine(
                      //     string.Format("{0} across is {1} at r,c {2},{3}", ordinal, clue, i, j)
                      // );

                      //mine answer
                      int wend = i;
                      String answer = "";
                      while (wend != columns && answerMatrix[wend, j] != '#')
                      {
                          answer += answerMatrix[wend, j];
                          wend++;
                      }
                      // Trace.WriteLine("\tand the answer is " + answer);
                      Words.Add(new Word() {
                          X = i,
                          Y = j,
                          I = ordinal,
                          Direction = Direction.Across,
                          Answer = answer,
                          Clue = clue
                      });
                      wordHit = true;
                  }

                  //is word down clue?
                  if ((j != rows) && (j == 0 || answerMatrix[i, j - 1] == '#'))
                  {
                      if (downClues.Count() == 0)
                      {
                          // Trace.WriteLine("Critical error, no clues left");
                          // Trace.WriteLine($" i,j {i},{j} : is {answerMatrix[i, j]}, prev is {answerMatrix[i - 1, j]}");
                          Environment.Exit(0);
                      }

                      //match clue to ordinal and position
                      String clue = downClues[0];
                      downClues.RemoveAt(0);
                      // Trace.WriteLine(
                      //     string.Format("{0} down is {1} at r,c {2},{3}", ordinal, clue, i, j)
                      // );

                      //mine answer
                      int wend = j;
                      String answer = "";
                      while (wend != rows && answerMatrix[i, wend] != '#')
                      {
                          answer += answerMatrix[i, wend];
                          wend++;
                      }
                      // Trace.WriteLine("\tand the answer is " + answer);
                      Words.Add(new Word(){
                          X=i,
                          Y=j,
                          I=ordinal,
                          Direction=Direction.Down,
                          Answer=answer,
                          Clue=clue
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
            Rows = rows,
            Columns = columns,
          };
          crossword.Type = CrosswordType.NYTIMES;
          crossword.Published = DateTime.Parse("03/21/2021");
          crossword.Title = crossword.Published.ToShortDateString();
          crossword.Words.AddRange(Words);
          crossword.GridChars.AddRange(gridChars);

          return crossword;

        }

    }

}
