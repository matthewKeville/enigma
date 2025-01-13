using Entity;
using Enums;

namespace UI.Model
{

    public class GridCharModel
    {

        public int X;
        public int Y;
        public char? UserChar;
        public char AnswerChar;
        public GridCharStatus Status;
        public bool IsBlock;

        public GridCharModel? Up;
        public GridCharModel? Down;
        public GridCharModel? Left;
        public GridCharModel? Right;

        public GridCharModel(GridChar gridChar)
        {
            X = gridChar.X;
            Y = gridChar.Y;
            UserChar = gridChar.UserChar;
            AnswerChar = gridChar.AnswerChar;
            Status = gridChar.Status;
            IsBlock = gridChar.IsBlock;
        }

        public override bool Equals(Object? obj)
        {
            if (obj is null)
            {
                return false;
            }
            if (obj is not GridCharModel)
            {
                return false;
            }
            GridCharModel other = (GridCharModel)obj;
            return other.X == X && other.Y == Y;
        }

        public void Dump()
        {
            Trace.WriteLine($"gcm : {X},{Y},{UserChar},{IsBlock}, {Up is null}, {Down is null}, {Right is null}, {Left is null}");
        }

    }

    public class GridClueModel
    {
        public int X;
        public int Y;
        public int I;
        public Direction Direction;
        public int Size;
        public String Prompt;
        public String Answer;

        public GridClueModel(Clue clue)
        {
            this.X = clue.X;
            this.Y = clue.Y;
            this.I = clue.I;
            this.Direction = clue.Direction;
            this.Prompt = clue.Prompt;
            this.Answer = clue.Answer;
            this.Size = clue.Answer.Count();
        }
    }

    public class GridModel
    {

        public List<GridClueModel> GridClueModels;
        public List<GridCharModel> GridCharModels;
        public GridCharModel Selection;

        public int crosswordId;
        public int ColumnCount = 0;
        public int RowCount = 0;
        public Direction Orientation = Direction.Across;

        public int WordCheckCount;

        public GridModel(List<GridChar> gridChars, List<Clue> clues, int rowCount, int columnCount)
        {

            //Clue Models
            GridClueModels = new();
            foreach (Clue clue in clues)
            {
                GridClueModels.Add(new GridClueModel(clue));
            }

            //Char Models
            GridCharModels = new();
            foreach (GridChar gc in gridChars)
            {
                var gcm = new GridCharModel(gc);
                gcm.Status = gc.Status;
                GridCharModels.Add(gcm);
            }

            //Link Char Models
            foreach (GridCharModel gcm in GridCharModels)
            {
                gcm.Up = GridCharModels.FirstOrDefault(m => m.X == gcm.X && m.Y == gcm.Y - 1, null);
                gcm.Down = GridCharModels.FirstOrDefault(m => m.X == gcm.X && m.Y == gcm.Y + 1, null);
                gcm.Left = GridCharModels.FirstOrDefault(m => m.X == gcm.X - 1 && m.Y == gcm.Y, null);
                gcm.Right = GridCharModels.FirstOrDefault(m => m.X == gcm.X + 1 && m.Y == gcm.Y, null);
            }

            Selection = GridCharModels.First();

            ColumnCount = columnCount;
            RowCount = rowCount;
        }


        public List<GridCharModel> ActiveWordChars()
        {
            return getWordChars(Selection, Orientation);
        }

        public List<GridCharModel> CrossWordChars()
        {
            Direction cross = Orientation == Direction.Across ? Direction.Down : Direction.Across;
            return getWordChars(Selection, cross);
        }

        public bool MoveUp()
        {
            if (!Selection.Up?.IsBlock ?? false)
            {
                Selection = Selection.Up!;
                return true;
            }
            return false;
        }

        public bool MoveDown()
        {
            if (!Selection.Down?.IsBlock ?? false)
            {
                Selection = Selection.Down!;
                return true;
            }
            return false;
        }

        public bool MoveRight()
        {
            if (!Selection.Right?.IsBlock ?? false)
            {
                Selection = Selection.Right!;
                return true;
            }
            return false;
        }

        public bool MoveLeft()
        {
            if (!Selection.Left?.IsBlock ?? false)
            {
                Selection = Selection.Left!;
                return true;
            }
            return false;
        }

        //try to move the clue at the ordinal
        public void MoveClue(int i)
        {
            GridClueModel? clueModel = GridClueModels
              .Where(gcm => gcm.Direction == Orientation)
              .Where(gcm => gcm.I == i)
              .FirstOrDefault();
            if (clueModel is null)
            {
                Trace.WriteLine($"no clue found for ordinal {i}");
                return;
            }

            GridCharModel? clueStartCharModel = GridCharModels
              .Find(gcm => gcm.X == clueModel.X && gcm.Y == clueModel.Y);
            if (clueStartCharModel is null)
            {
                Trace.WriteLine($"coudln't find start GridCharModel for clue {i}");
                return;
            }
            Selection = clueStartCharModel;

        }

        public void MoveNextClue()
        {
            MoveClue(false);
        }

        public void MovePrevClue()
        {
            MoveClue(true);
        }

        public void MoveClueEnd()
        {
            MoveInClue(true);
        }

        public void MoveClueStart()
        {
            MoveInClue(false);
        }

        //Move to the next character c in the current word,
        //if it exists
        public void FindChar(char c)
        {
            var wordChars = getWordChars(Selection, Orientation);
            int index = wordChars.IndexOf(Selection) + 1;
            while (index < wordChars.Count)
            {
                GridCharModel gcm = wordChars[index];
                if (gcm.UserChar == c)
                {
                    Selection = gcm;
                    return;
                }
                index++;
            }
        }
        //Move to the prev character c in the current word,
        //if it exists
        public void FindReverseChar(char c)
        {
            var wordChars = getWordChars(Selection, Orientation);
            wordChars.ForEach(wc => wc.Dump());
            int index = wordChars.IndexOf(Selection) - 1;
            while (index >= 0)
            {
                GridCharModel gcm = wordChars[index];
                if (gcm.UserChar == c)
                {
                    Selection = gcm;
                    return;
                }
                index--;
            }
        }

        public void SwapOrientation()
        {
            if (Orientation == Direction.Across)
            {
                Orientation = Direction.Down;
            }
            else
            {
                Orientation = Direction.Across;
            }
        }

        public void InsertChar(char c)
        {
            Selection.UserChar = c;
            if ( Selection.UserChar != c ) {
              Selection.Status = GridCharStatus.UNKNOWN;
            }
            if (Orientation == Direction.Across)
            {
                if (!(Selection.Right?.IsBlock ?? true))
                {
                    Selection = Selection.Right;
                }
            }
            else
            {
                if (!(Selection.Down?.IsBlock ?? true))
                {
                    Selection = Selection.Down;
                }
            }
        }

        public void ReplaceChar(char c)
        {
            if ( Selection.UserChar != c ) {
              Selection.Status = GridCharStatus.UNKNOWN;
            }
            Selection.UserChar = c;
        }

        public void DeleteChar(bool moveBackChar)
        {
            Selection.UserChar = ' ';
            Selection.Status = GridCharStatus.UNKNOWN;
            if (moveBackChar)
            {
                if (Orientation == Direction.Across)
                {
                    if (!Selection.Left?.IsBlock ?? false)
                    {
                        Selection = Selection.Left!;
                    }
                }
                else
                {
                    if (!Selection.Up?.IsBlock ?? false)
                    {
                        Selection = Selection.Up!;
                    }
                }
            }
        }

        // Delete the characters from the current Selection to
        // the end of the current clue
        public void DeleteWord()
        {
            var wordChars = getWordChars(Selection, Orientation);
            var selectIndex = wordChars.IndexOf(Selection);
            while (selectIndex < wordChars.Count())
            {
                wordChars[selectIndex].UserChar = ' ';
                wordChars[selectIndex].Status = GridCharStatus.UNKNOWN;
                selectIndex++;
            }
        }

        // Delete all the characters from the current clue,
        // and move the selection to the beginning word character
        public void DeleteInnerWord()
        {
            var wordChars = getWordChars(Selection, Orientation);
            var index = 0;
            while (index < wordChars.Count())
            {
                wordChars[index].UserChar = ' ';
                wordChars[index].Status = GridCharStatus.UNKNOWN;
                index++;
            }
            Selection = wordChars.First();
        }

        public (GridClueModel?, GridClueModel?) GetActiveClues()
        {

            List<GridCharModel> acrossWordChars = getWordChars(Selection, Direction.Across);
            GridCharModel acrossStart = acrossWordChars.First();
            GridClueModel? acrossClue = GridClueModels.Where(cm => cm.X == acrossStart.X && cm.Y == acrossStart.Y).FirstOrDefault();

            List<GridCharModel> downWordChars = getWordChars(Selection, Direction.Down);
            GridCharModel downStart = downWordChars.First();
            GridClueModel? downClue = GridClueModels.Where(cm => cm.X == downStart.X && cm.Y == downStart.Y).FirstOrDefault();

            return (acrossClue, downClue);

        }

        public bool IsComplete()
        {
            foreach (GridCharModel gcm  in GridCharModels)
            {
              if ( gcm.UserChar != gcm.AnswerChar) {
                return false;
              }
            }
            return true;
        }

        public void CheckChar() {
          if ( Selection.UserChar == ' ' ) {
            return;
          }
          if ( Selection.UserChar == Selection.AnswerChar ) {
            Selection.Status = GridCharStatus.CORRECT;
          } else {
            Selection.Status = GridCharStatus.INCORRECT;
          }
        }

        public void CheckWord() {
          List<GridCharModel> gcms = getWordChars(Selection,Orientation);
          gcms.ForEach( gcm => {
            if ( gcm.UserChar == ' ' ) {
              return;
            }
            if ( gcm.UserChar == gcm.AnswerChar ) {
              gcm.Status = GridCharStatus.CORRECT;
            } else {
              gcm.Status = GridCharStatus.INCORRECT;
            }
          });
        }

        public void CheckPuzzle() {
          GridCharModels.ForEach( gcm => {
            if ( gcm.UserChar == ' ' ) {
              return;
            }
            if ( gcm.UserChar == gcm.AnswerChar ) {
              gcm.Status = GridCharStatus.CORRECT;
            } else {
              gcm.Status = GridCharStatus.INCORRECT;
            }
          });
        }

        //Return the list of GridCharModels that represent the "word" answer to
        //the clue in order.
        private List<GridCharModel> getWordChars(GridCharModel gcm, Direction direction)
        {

            var wordChars = new List<GridCharModel> { };

            if (direction == Direction.Across)
            {

                var cur = gcm;
                while (cur.Left != null && !cur.Left.IsBlock)
                {
                    cur = cur.Left;
                    wordChars.Insert(0, cur);
                }

                wordChars.Add(gcm);
                cur = gcm;

                while (cur.Right != null && !cur.Right.IsBlock)
                {
                    cur = cur.Right;
                    wordChars.Add(cur!);
                }

            }
            else
            {

                var cur = gcm;
                while (cur.Up != null && !cur.Up.IsBlock)
                {
                    cur = cur.Up;
                    wordChars.Insert(0, cur);
                }

                wordChars.Add(gcm);
                cur = gcm;

                while (cur.Down != null && !cur.Down.IsBlock)
                {
                    cur = cur.Down;
                    wordChars.Add(cur!);
                }

            }

            return wordChars;

        }

        //Move the Selection to the starting character of the next clue
        //prev = true, will move to the previous clue
        private void MoveClue(bool prev)
        {

            (GridClueModel? acrossClue, GridClueModel? downClue) = GetActiveClues();
            GridClueModel? clue = Orientation == Direction.Across ? acrossClue : downClue;

            if (clue == null)
            {
                Trace.WriteLine("no clue found");
                return;
            }

            // a bit hacky here...
            GridClueModel? targetClue = GridClueModels
              .Where(cm => cm.Direction == Orientation)
              .Where(cm => prev ? cm.I < clue.I : cm.I > clue.I)
              .OrderBy(cm => cm.I * (prev ? -1 : 1))
              .FirstOrDefault();

            if (targetClue != null)
            {
                Trace.WriteLine($"clue found {targetClue.X},{targetClue.Y}");
                GridCharModel targetCharModel = GridCharModels
                  .Where(gcm => gcm.X == targetClue.X && gcm.Y == targetClue.Y)
                  .First();
                Selection = targetCharModel;
            }
            else
            {
                Trace.WriteLine("clue not found");
            }
        }

        //Move the Selection to the start or end of the current clue
        private void MoveInClue(bool end)
        {
            List<GridCharModel> wordChars = getWordChars(Selection, Orientation);
            Selection = end ? wordChars.Last() : wordChars.First();
        }

    }
}
