using Terminal.Gui;

namespace Settings.Theme {

  public class Theme {

    //16 Color

    public static ColorScheme GlobalColorScheme = new ColorScheme(
        new Terminal.Gui.Attribute(Color.White,Color.Green),
        new Terminal.Gui.Attribute(Color.White,Color.Green),
        new Terminal.Gui.Attribute(Color.White,Color.Green),
        new Terminal.Gui.Attribute(Color.White,Color.Green),
        new Terminal.Gui.Attribute(Color.White,Color.Green)
    );

    ////////////////////////////////////////////////////////////
    //Clues
    ////////////////////////////////////////////////////////////

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

    ////////////////////////////////////////////////////////////
    //Grid
    ////////////////////////////////////////////////////////////

    public Color GridBackgroundBG = ColorName.Red;

    //Unfocused  Cell Styling

    public Color CellFG = ColorName.Black;
    public Color CellBG = ColorName.White;
    public Color CellCorrectHighlightFG = ColorName.Green;
    public Color CellCorrectHighlightBG = ColorName.White;
    public Color CellIncorrectHighlightFG = ColorName.Red;
    public Color CellIncorrectHighlightBG = ColorName.White;
    public char CellEmptyChar = ' ';

    public Color BlockFG = ColorName.Black;
    public Color BlockBG = ColorName.Black;
    public char BlockChar = ' ';

    //Cursor Cell Styling

    public Color CursorHighlightFG = ColorName.Yellow;
    public Color CursorHighlightBG = ColorName.White;

    public Color CursorEmptyHighlightFG = ColorName.Yellow;
    public Color CursorEmptyHighlightBG = ColorName.White;
    public char CursorEmptyHighlightChar = '*';

    public Color CursorCorrectHighlightFG = ColorName.Green;
    public Color CursorCorrectHighlightBG = ColorName.White;
    public Color CursorInorrectHighlightFG = ColorName.Red;
    public Color CursorInorrectHighlightBG = ColorName.White;


    //Active Cell Styling

    public Color ActiveHighlightFG = ColorName.DarkGray;
    public Color ActiveHighlightBG = ColorName.White;
 
    public Color ActiveEmptyHighlightFG = ColorName.DarkGray;
    public Color ActiveEmptyHighlightBG = ColorName.White;
    public char ActiveEmptyHighlightAcrossChar = '-';
    public char ActiveEmptyHighlightDownChar = '|';

    public Color ActiveCorrectHighlightFG = ColorName.Green;
    public Color ActiveCorrectHighlightBG = ColorName.White;
    public Color ActiveInorrectHighlightFG = ColorName.Red;
    public Color ActiveInorrectHighlightBG = ColorName.White;
 
    //Cross Cell Styling

    public Color CrossHighlightFG = ColorName.Gray;
    public Color CrossHighlightBG = ColorName.White;

    public Color CrossEmptyHighlightFG = ColorName.Gray;
    public Color CrossEmptyHighlightBG = ColorName.White;
    public char CrossEmptyHighlightAcrossChar = '-';
    public char CrossEmptyHighlightDownChar = '|';

    public Color CrossCorrectHighlightFG = ColorName.Green;
    public Color CrossCorrectHighlightBG = ColorName.White;
    public Color CrossIncorrectHighlightFG = ColorName.Red;
    public Color CrossIncorrectHighlightBG = ColorName.White;

  }
}
