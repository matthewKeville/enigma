using Logging;
using Serilog;
using Terminal.Gui;

namespace UI.KeyMaping {

  public class KeySequenceParser {

    public static ILogger _logger = Logger.For<KeySequenceParser>();

    public static Dictionary<string, Key> specials = new Dictionary<string, Key>()
    {

      { "<Home>" , Key.Home },
      { "<Del>" , Key.Delete },
      { "<Esc>" , Key.Esc },
      { "<Space>" , Key.Space },
      { "<CR>" , Key.Enter },
      { "<BACKSPACE>" , Key.Backspace },
      { "<Tab>" , Key.Tab },

      { "<PGUP>" , Key.PageUp },
      { "<PGDOWN>" , Key.PageDown },

      { "<UP>" , Key.CursorUp },
      { "<DOWN>" , Key.CursorDown },
      { "<LEFT>" , Key.CursorLeft },
      { "<RIGHT>" , Key.CursorRight },

      { "<F1>" , Key.F1 },
      { "<F2>" , Key.F2 },
      { "<F3>" , Key.F3 },
      { "<F4>" , Key.F4 },
      { "<F5>" , Key.F5 },
      { "<F6>" , Key.F6 },
      { "<F7>" , Key.F7 },
      { "<F8>" , Key.F8 },
      { "<F9>" , Key.F9 },
      { "<F10>" , Key.F10 },
      { "<F11>" , Key.F11 },
      { "<F12>" , Key.F12 },

    };

    public static Key? TryParseNormalKey(String keystring) {

      if ( keystring.Length != 1 ) {
        return null;
      }

      char character = keystring[0];

      //should be a safe cast?
      if (  char.IsLetter(character) || 
            char.IsDigit(character) || 
            char.IsPunctuation(character) ||
            char.IsSymbol(character) 
      ) {
        return (Key) character;
      }

      return null;

    }

    public static Key? TryParseSpecialKey(String keystring) {
      if ( specials.ContainsKey(keystring) ) {
        return specials[keystring];
      }
      return null;
    }

    // first pass at implementation, only support one modifier
    // <C-?> <S-?> <A-?>
    public static Key? TryParseModifiedKey(String keystring) {
      // || <M-?> || = 5
      if (keystring.Count() < 5 || keystring[0] != '<' || keystring[2] !='-'|| keystring[keystring.Length-1] != '>' ) {
        return null;
      }

      String innerKeystring = keystring[3..(keystring.Length-1)];
      Key? key = TryParseNormalKey(innerKeystring);
      key??= TryParseSpecialKey($"<{innerKeystring}>");
      if ( key == null ) {
        _logger.Warning($"failed to parse modified key, keystring = {keystring}, innerKeystring = {innerKeystring} ");
        _logger.Warning($"inner string did not map to normal or special key");
        return null;
      }

      switch ( keystring[1] ) {
        case 'C':
          return key.WithCtrl;
        case 'S':
          return key.WithShift;
        case 'A':
          return key.WithAlt;
        default:
          _logger.Warning($"failed to parse modified key, keystring = {keystring}");
          _logger.Warning($"modified prefix not supported, mpx = {keystring[1]}");
          return null;
      }

    }


    public static List<Key>? TryParse(List<String> keysequence) {
      List<Key> keys = new ();

      foreach ( String keystring in keysequence ) {

        Key? key = null;
        //normal
        key = TryParseNormalKey(keystring);
        if ( key != null ) {
          keys.Add(key);
          continue;
        }

        //special 
        key = TryParseSpecialKey(keystring);
        if ( key != null ) {
          keys.Add(key);
          continue;
        }

        //modified
        key = TryParseModifiedKey(keystring);
        if ( key != null ) {
          keys.Add(key);
          continue;
        }

        _logger.Warning($"keysequence {keysequence} could not be parsed");
        return null;
        
      }

      return keys;

    }
  }

}
