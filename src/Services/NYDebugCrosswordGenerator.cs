using System.Diagnostics;
using Model;

namespace Services
{

    public class NYDebugCrosswordGenerator : ICrosswordGenerator
    {

        public Crossword crossword
        {
            get
            {

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

                char[,] answerMatrix = new char[cols,rows];
                for ( int i = 0; i < cols; i++ ) {
                  for ( int j = 0; j < rows; j++ ) {
                    answerMatrix[i,j] = answerMatrixTranspose[j][i];
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


                Trace.WriteLine($" ACLUES {acrossClues.Count()} DCLUES {downClues.Count()}");

                Crossword crossword = new Crossword(rows, cols);
                crossword.name = "NY TIMES";

                int ordinal = 1;

                for (int j = 0; j < rows; j++)
                {
                    for (int i = 0; i < cols; i++)
                    {

                        if (answerMatrix[i,j] == '#')
                        {
                            continue;
                        }

                        bool wordHit = false;

                        //is word across clue?
                        if ( (i != cols) && ( i == 0 || answerMatrix[i-1,j] == '#') )
                        {

                            //match clue to ordinal and position
                            if ( acrossClues.Count() == 0 ) {
                              Trace.WriteLine("Critical error, no clues left");
                              Trace.WriteLine($" i,j {i},{j} : is {answerMatrix[i,j]}, prev is {answerMatrix[i-1,j]}");
                              Environment.Exit(0);
                            }
                            
                            String clue = acrossClues[0];
                            acrossClues.RemoveAt(0);
                            Trace.WriteLine(
                                string.Format("{0} across is {1} at r,c {2},{3}", ordinal, clue, i, j)
                            );

                            //mine answer
                            int wend = i;
                            String answer = "";
                            while (wend != cols && answerMatrix[wend,j] != '#')
                            {
                                answer += answerMatrix[wend,j];
                                wend++;
                            }
                            Trace.WriteLine("\tand the answer is " + answer);
                            crossword.words.Add(new Word(i, j, ordinal, Direction.Across, answer, clue));
                            wordHit = true;
                        }

                        //is word down clue?
                        if (( j != rows) && ( j == 0 || answerMatrix[i,j-1] == '#') )
                        {
                            if ( downClues.Count() == 0 ) {
                              Trace.WriteLine("Critical error, no clues left");
                              Trace.WriteLine($" i,j {i},{j} : is {answerMatrix[i,j]}, prev is {answerMatrix[i-1,j]}");
                              Environment.Exit(0);
                            }

                            //match clue to ordinal and position
                            String clue = downClues[0];
                            downClues.RemoveAt(0);
                            Trace.WriteLine(
                                string.Format("{0} down is {1} at r,c {2},{3}", ordinal, clue, i, j)
                            );

                            //mine answer
                            int wend = j;
                            String answer = "";
                            while (wend != rows && answerMatrix[i,wend] != '#')
                            {
                                answer += answerMatrix[i,wend];
                                wend++;
                            }
                            Trace.WriteLine("\tand the answer is " + answer);
                            crossword.words.Add(new Word(i, j, ordinal, Direction.Down, answer, clue));

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

}
