using Terminal.Gui;

namespace Settings.Theme {

  public class Theme {

    //16 Color

    //Clues

    public static ColorScheme GlobalColorScheme = new ColorScheme(
        new Terminal.Gui.Attribute(Color.White,Color.Green),
        new Terminal.Gui.Attribute(Color.White,Color.Green),
        new Terminal.Gui.Attribute(Color.White,Color.Green),
        new Terminal.Gui.Attribute(Color.White,Color.Green),
        new Terminal.Gui.Attribute(Color.White,Color.Green)
    );

    public Color CluesBackgroundBG = ColorName.White;
    public Color CluesTableBackgroundBG = ColorName.White;

    public Color CluesHeaderFG = ColorName.Black;
    public Color CluesHeaderBG = ColorName.White;

    public Color FocusedClueFG = ColorName.Yellow;
    public Color FocusedClueBG = ColorName.White;

    public Color CrossClueFG = ColorName.DarkGray;
    public Color CrossClueBG = ColorName.White;

    public Color InactiveClueFG = ColorName.Gray;
    public Color InactiveClueBG = ColorName.White;

    //Grid

    public Color GridBackgroundBG = ColorName.Red;


    //Unfocused  Cell Styling

    public Color CellFG = ColorName.Black;
    public Color CellBG = ColorName.White;
    public char CellEmptyChar = ' ';

    public Color BlockFG = ColorName.Black;
    public Color BlockBG = ColorName.Black;
    public char BlockChar = ' ';

    //Cursor Cell Styling

    public Color CursorHighlightFG = ColorName.Red;
    public Color CursorHighlightBG = ColorName.White;

    public Color CursorEmptyHighlightFG = ColorName.Red;
    public Color CursorEmptyHighlightBG = ColorName.White;
    public char CursorEmptyHighlightChar = '*';

    //Active Cell Styling

    public Color ActiveHighlightFG = ColorName.DarkGray;
    public Color ActiveHighlightBG = ColorName.White;
 
    public Color ActiveEmptyHighlightFG = ColorName.DarkGray;
    public Color ActiveEmptyHighlightBG = ColorName.White;
    public char ActiveEmptyHighlightAcrossChar = '-';
    public char ActiveEmptyHighlightDownChar = '|';
 
    //Cross Cell Styling

    public Color CrossHighlightFG = ColorName.Gray;
    public Color CrossHighlightBG = ColorName.White;

    public Color CrossEmptyHighlightFG = ColorName.Gray;
    public Color CrossEmptyHighlightBG = ColorName.White;
    public char CrossEmptyHighlightAcrossChar = '-';
    public char CrossEmptyHighlightDownChar = '|';

    public Theme() {
      //load theme from json
      //or default...
    }

  }
}
