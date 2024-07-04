using Enums;
using UI.Model;
using UI.Model.Game;

namespace Services
{

    public class NYDebugCrosswordGenerator 
    {

        public CrosswordModel sample1() {

          int rows = 15;
          int cols = 15;

          char[][] answerMatrixTranspose = [
            @"ACED#PLIE#EASES".ToCharArray(),
                @"RULE#RIND#SAINT".ToCharArray(),
                @"CRIBNOTES#QANDA".ToCharArray(),
                @"###TOM#PEW##FUN".ToCharArray(),
                @"UGH#BOTTLEGOURD".ToCharArray(),
                @"GUESSSO##LADLES".ToCharArray(),
                @"GAPE##PASSTO###".ToCharArray(),
                @"#CARRIAGEHOUSE#".ToCharArray(),
                @"###GENZER##LETS".ToCharArray(),
                @"OHNEAT##VERSACE".ToCharArray(),
                @"MOBILEPHONE#THX".ToCharArray(),
                @"ETA##RAE#ZAG###".ToCharArray(),
                @"RAJAS#BABYPROOF".ToCharArray(),
                @"TIARA#STEM#IDLE".ToCharArray(),
                @"ARMED#THEE#NEED".ToCharArray(),
              ];

          char[,] answerMatrix = new char[cols, rows];
          for (int i = 0; i < cols; i++)
          {
              for (int j = 0; j < rows; j++)
              {
                  answerMatrix[i, j] = answerMatrixTranspose[j][i];
              }
          }


          List<String> acrossClues = new List<String> {
                @"Hit a serve past",
                @"Ballet dancer's bend",
                @"Lightens (up)",
                @"Word after golden or slide",
                @"Often-discarded part of a fruit",
                @"Canonized person",
                @"Cheat sheets",
                @"Post-panel sesh",
                @"Male cat",
                @"Big name in public opinion research",
                @"It might be poked",
                @"Cry of disgust that sounds like 24-Down",
                @"Fruit also known as calabash",
                @"""Yeah, I suppose""",
                @"Soup kitchen utensils",
                @"Stare open-mouthed",
                @"Target, as a wide receiver",
                @"Outbuilding for many a historic home",
                @"Millennial's successor, informally",
                @"Tennis do-overs",
                @"""That's pretty nifty!""",
                @"Fashion house whose logo features Medusa",
                @"Counterpart to a landline",
                @"Appreciative text",
                @"Schedule abbr.",
                @"Middle name for Alec Baldwin and Carly Jepsen",
                @"Go the other way",
                @"Indian royals",
                @"Make safer, in a way ... or what the starts of 17-, 27-, 38- and 52-Across might be?",
                @"Pageant topper",
                @"Often-discarded part of a fruit",
                @"Inactive",
                @"One-___ bandit",
                @"Biblical pronoun",
                @"Nonnegotiable thing"
        };

          List<String> downClues = new List<String> {
                @"Airplane's path on a flight map, often",
                @"Junkyard dog",
                @"Roth of ""Inglourious Basterds""",
                @"Red scare?",
                @"Sneak previews",
                @"Happening, in modern parlance",
                @"Bumbling",
                @"Old car make named for Henry Ford's son",
                @"Abbr. on a lawyer's business card",
                @"Highly rated, as a bond",
                @"Iniquitous",
                @"Stick it out",
                @"Array at a farmer's market",
                @"Noggins",
                @"Language in which ""w"" can be a vowel",
                @"Australian boot brand",
                @"Green dip, familiarly",
                @"Purifying filter acronym",
                @"November birthstone",
                @"One purring in Peru",
                @"Nonalcoholic beer brand",
                @"Composer Rachmaninoff",
                @"Ripen",
                @"Kind of motor used in robotics",
                @"Down-to-earth",
                @"Lead-in to mingle or mezzo",
                @"Ticket assignment",
                @"Cut quite a figure?",
                @"Something a prenatal ultrasound can determine",
                @"Mafia code of silence",
                @"Windbag's output",
                @"Classic video game with the catchphrase ""He's on fire!""",
                @"Biological catalyst",
                @"Collect what's been sown",
                @"""Blue Ribbon"" brewer",
                @"Toffee bar brand since 1928",
                @"Beam",
                @"""What ___ the odds?""",
                @"Down in the dumps",
                @"Wax producer",
                @"Shelley's ""To a Skylark,"" for one",
                @"World Cup chant",
                @"Put quarters in, as a meter"
            };


          // Trace.WriteLine($" ACLUES {acrossClues.Count()} DCLUES {downClues.Count()}");

          CrosswordModel crossword = new CrosswordModel(rows, cols);
          crossword.name = "NY TIMES";

          int ordinal = 1;

          for (int j = 0; j < rows; j++)
          {
              for (int i = 0; i < cols; i++)
              {

                  if (answerMatrix[i, j] == '#')
                  {
                      continue;
                  }

                  bool wordHit = false;

                  //is word across clue?
                  if ((i != cols) && (i == 0 || answerMatrix[i - 1, j] == '#'))
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
                      while (wend != cols && answerMatrix[wend, j] != '#')
                      {
                          answer += answerMatrix[wend, j];
                          wend++;
                      }
                      // Trace.WriteLine("\tand the answer is " + answer);
                      crossword.words.Add(new WordModel(i, j, ordinal, Direction.Across, answer, clue));
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
                      crossword.words.Add(new WordModel(i, j, ordinal, Direction.Down, answer, clue));

                      wordHit = true;
                  }


                  if (wordHit)
                  {
                      ordinal++;
                  }

              }
          }

          return crossword;

        }



        public CrosswordModel sample2() {

          int rows = 15;
          int cols = 15;

          char[][] answerMatrixTranspose = [
                @"HOPACAB#EDU#CDC".ToCharArray(),
                @"THATONE#CENTAUR".ToCharArray(),
                @"SOLOACT#HAIRSPA".ToCharArray(),
                @"##EITHEROR#AEON".ToCharArray(),
                @"PRO#SOLE##OVINE".ToCharArray(),
                @"COLA###COMMENTS".ToCharArray(),
                @"STILETTOHEELS##".ToCharArray(),
                @"#ITSRAININGMEN#".ToCharArray(),
                @"##HAUDENOSAUNEE".ToCharArray(),
                @"SUITCASE###GSIX".ToCharArray(),
                @"OPCIT##CRAG#INT".ToCharArray(),
                @"ALDA#BETATEST##".ToCharArray(),
                @"PAINMED#BOOHISS".ToCharArray(),
                @"ETESIAN#ANDOVER".ToCharArray(),
                @"DET#AKA#TEEPEES".ToCharArray(),
              ];

          char[,] answerMatrix = new char[cols, rows];
          for (int i = 0; i < cols; i++)
          {
              for (int j = 0; j < rows; j++)
              {
                  answerMatrix[i, j] = answerMatrixTranspose[j][i];
              }
          }


          List<String> acrossClues = new List<String> {
            @"Eschew the bus or subway, say",
            @"Lead-in to -tainment",
            @"Org. overseeing the Epidemic Intelligence Service",
            @"Words said while pointing",
            @"Person on horseback?",
            @"Something David Copperfield has that Penn and Teller don't",
            @"Salon, fancily",
            @"Some choice words",
            @"Timeline swath",
            @"With 51-Down, part of a golf club",
            @"Fish that may be served meunière",
            @"Like a lamb",
            @"Flavor of some bottle-shaped gummies",
            @"Section often symbolized by a speech bubble",
            @"They don't give you much to stand on",
            @"1980s disco hit that became a gay anthem",
            @"Native name for the Iroquois Confederacy",
            @"Rider on a carousel?",
            @"Germany, France, the U.K., Italy, Spain and Poland, collectively",
            @"Endnote abbr.",
            @"Rock formation",
            @"Kind of shot that's the opposite of a 38-Down in a screenplay",
            @"Actor with the 2007 memoir ""Things I Overheard While Talking to Myself""",
            @"Trial run",
            @"Number in a pharmacy, informally",
            @"Opposite of ""Yay!""",
            @"Kind of wind across the Aegean",
            @"Massachusetts home of Phillips Academy",
            @"Mystery title: Abbr.",
            @"Alias",
            @"Plain lodging",
          };

          List<String> downClues = new List<String> {
            @"Cleveland ___: Abbr.",
            @"""Well, looky there!""",
            @"Vegetables, fruits, nuts, roots and meat, classically",
            @"""Merci ___ aussi""",
            @"Chesterfield and others",
            @"Chili variety",
            @"Nut variety",
            @"Unwanted effect on a recording",
            @"Treasured",
            @"Sports getup, for short",
            @"Like email addresses, practically",
            @"Kevlar developer",
            @"Parts of many an urban skyline",
            @"Drink container that doesn't easily spill",
            @"Catch up",
            @"Acer offerings",
            @"Indian flatbread",
            @"Rolex competitor",
            @"People of NE France",
            @"Lima locale",
            @"Shoe size specification",
            @"Burp, more formally",
            @"""I did it!""",
            @"Draws",
            @"Refusal overseas",
            @"Kind of shot that's the opposite of a 47-Across in a screenplay",
            @"Got sudsy",
            @"Watching TV after midnight, say",
            @"Capital on the Atlantic",
            @"Apologize with actions",
            @"Rock formation",
            @"Proboscis",
            @"Poet ___ St. Vincent Millay",
            @"See 22-Across",
            @"Unaccounted-for, briefly",
            @"""Wasn't I right?""",
            @"Many promgoers: Abbr."
          };


          // Trace.WriteLine($" ACLUES {acrossClues.Count()} DCLUES {downClues.Count()}");

          CrosswordModel crossword = new CrosswordModel(rows, cols);
          crossword.name = "NY TIMES 2020 03 28";

          int ordinal = 1;

          for (int j = 0; j < rows; j++)
          {
              for (int i = 0; i < cols; i++)
              {

                  if (answerMatrix[i, j] == '#')
                  {
                      continue;
                  }

                  bool wordHit = false;

                  //is word across clue?
                  if ((i != cols) && (i == 0 || answerMatrix[i - 1, j] == '#'))
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
                      while (wend != cols && answerMatrix[wend, j] != '#')
                      {
                          answer += answerMatrix[wend, j];
                          wend++;
                      }
                      // Trace.WriteLine("\tand the answer is " + answer);
                      crossword.words.Add(new WordModel(i, j, ordinal, Direction.Across, answer, clue));
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
                      crossword.words.Add(new WordModel(i, j, ordinal, Direction.Down, answer, clue));

                      wordHit = true;
                  }


                  if (wordHit)
                  {
                      ordinal++;
                  }

              }
          }

          return crossword;

        }

    }

}
