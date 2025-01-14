namespace UI.View.Game.Clues
{
    using Terminal.Gui;
    using UI.KeyMaping;

    public class KeyBindsView : Window
    {
        private List<Label> _keyBindLabels = new List<Label>();
        private const int _seqStringLength = 20;
        
        public KeyBindsView(KeyMaps keyMaps) {

          int lineNumber = 0;
          var normalHeader = new Label();
          normalHeader.X = 2;
          normalHeader.Y = 1 + lineNumber;
          normalHeader.Text = "Normal Mode Key Maps";
          Add(normalHeader);
          _keyBindLabels.Add(normalHeader);
          lineNumber+=2;

          foreach ( KeyMap keyMap in keyMaps.NormalKeyMaps ) {
            var label = new Label();
            label.X = 2;
            label.Y = 1 + lineNumber;
            if ( keyMap is FixedKeyMap ) {
              label.Text = keyMap.Bindings.First().Item1.Aggregate("", (agg,x) => agg += $"{x.KeyCode.ToString()} ");
              label.Text = label.Text.PadRight(_seqStringLength,' ');
            } else {
              ParametricKeyMap parMap = (ParametricKeyMap) keyMap;
              if ( parMap.Posterior ) {
                label.Text = ((ParametricKeyMap) keyMap).PrinicpalSequence.Aggregate("", (agg,x) => agg += $"{x.KeyCode.ToString()} ");
                label.Text += ((ParametricKeyMap) keyMap).ArgSpec;
              } else {
                label.Text = ((ParametricKeyMap) keyMap).ArgSpec;
                label.Text += ((ParametricKeyMap) keyMap).PrinicpalSequence.Aggregate("", (agg,x) => agg += $"{x.KeyCode.ToString()} ");
              }
              label.Text = label.Text.PadRight(_seqStringLength,' ');
            }
            label.Text += keyMap.Description;
            Add(label);
            _keyBindLabels.Add(label);
            lineNumber++;
          }

          lineNumber +=2;
          var insertHeader = new Label();
          insertHeader.X = 2;
          insertHeader.Y = 1 + lineNumber;
          insertHeader.Text = "Insert Mode Key Maps";
          Add(insertHeader);
          _keyBindLabels.Add(insertHeader);
          lineNumber+=2;

          foreach ( KeyMap keyMap in keyMaps.InsertKeyMaps ) {
            var label = new Label();
            label.X = 2;
            label.Y = 1 + lineNumber;
            if ( keyMap is FixedKeyMap ) {
              label.Text = keyMap.Bindings.First().Item1.Aggregate("", (agg,x) => agg += $"{x.KeyCode.ToString()} ");
              label.Text = label.Text.PadRight(_seqStringLength,' ');
            } else {
              ParametricKeyMap parMap = (ParametricKeyMap) keyMap;
              if ( parMap.Posterior ) {
                label.Text = ((ParametricKeyMap) keyMap).PrinicpalSequence.Aggregate("", (agg,x) => agg += $"{x.KeyCode.ToString()} ");
                label.Text += ((ParametricKeyMap) keyMap).ArgSpec;
              } else {
                label.Text = ((ParametricKeyMap) keyMap).ArgSpec;
                label.Text += ((ParametricKeyMap) keyMap).PrinicpalSequence.Aggregate("", (agg,x) => agg += $"{x.KeyCode.ToString()} ");
              }
              label.Text = label.Text.PadRight(_seqStringLength,' ');
            }
            label.Text += keyMap.Description;
            Add(label);
            _keyBindLabels.Add(label);
            lineNumber++;
          }

          lineNumber +=2;
          var exit1 = new Label();
          exit1.X = 2;
          exit1.Y = 1 + lineNumber;
          exit1.Text = "Press F1 or q Exit";
          Add(exit1);
          _keyBindLabels.Add(exit1);

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
