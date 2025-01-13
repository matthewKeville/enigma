namespace UI.View.Game.Clues
{
    using Terminal.Gui;
    using UI.KeyMaping;
    using UI.KeyMapping;

    public class KeyBindsView : Window
    {
        private List<Label> _keyBindLabels = new List<Label>();
        
        public KeyBindsView(KeyMaps keyMaps) {

          int lineNumber = 0;


          var normalHeader = new Label();
          normalHeader.X = 2;
          normalHeader.Y = 1 + lineNumber;
          normalHeader.Text = "Normal Mode Key Maps";
          Add(normalHeader);
          _keyBindLabels.Add(normalHeader);
          lineNumber+=2;

          foreach ((List<Key> sequence,UICommand command) in keyMaps.NormalKeyMaps) {
            if ( command.Type == UICommandType.MOVE_CLUE ||
                 command.Type == UICommandType.MOVE_CLUE ||
                 command.Type == UICommandType.REPLACE_CHAR ||
                 command.Type == UICommandType.FIND_CHAR ||
                 command.Type == UICommandType.FIND_REV_CHAR) {
              //ignore
            } else {
              var label = new Label();
              label.X = 2;
              label.Y = 1 + lineNumber;
              label.Text = sequence
                .Aggregate("", (agg,x) => agg += $"{x.KeyCode.ToString()} ");
              label.Text += command.Description;
              Add(label);
              _keyBindLabels.Add(label);
              lineNumber++;
            }
          }

          lineNumber +=2;
          var insertHeader = new Label();
          insertHeader.X = 2;
          insertHeader.Y = 1 + lineNumber;
          insertHeader.Text = "Insert Mode Key Maps";
          Add(insertHeader);
          _keyBindLabels.Add(insertHeader);
          lineNumber+=2;

          foreach ((List<Key> sequence,UICommand command) in keyMaps.InsertKeyMaps) {
            if (  command.Type == UICommandType.INSERT_CHAR) {
              //ignore
            } else {
              var label = new Label();
              label.X = 2;
              label.Y = 1 + lineNumber;
              label.Text = sequence
                .Aggregate("", (agg,x) => agg += $"{x.KeyCode.ToString()} ");
              label.Text += command.Description;
              Add(label);
              _keyBindLabels.Add(label);
              lineNumber++;
            }
          }

          lineNumber +=2;
          var exit1 = new Label();
          exit1.X = 2;
          exit1.Y = 1 + lineNumber;
          exit1.Text = "F1 Exit";
          Add(exit1);
          _keyBindLabels.Add(exit1);

          lineNumber++;
          var exit2 = new Label();
          exit2.X = 2;
          exit2.Y = 1 + lineNumber;
          exit2.Text = "q Exit";
          Add(exit2);
          _keyBindLabels.Add(exit2);

        }

        public override bool OnKeyDown(Key key)
        {
          if ( key == Key.Q || key == Key.F1) {
            Application.RequestStop();
          }
          return true;
        }
    }

}
