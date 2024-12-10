using Terminal.Gui;

namespace UI.Theme {

  public class Theme {

    // Ivory White
    /**
    static Color IVORY_WHITE = new Color(255, 255, 240);
    static Color BEIGE_CREAM = new Color(245, 245, 220);
    static Color SOFT_GRAY = new Color(211, 211, 211);
    static Color LIGHT_KRAFT_BROWN = new Color(210, 180, 140);
    static Color PALE_MINT = new Color(230, 247, 230);
    static Color MUTED_LAVENDER = new Color(234, 230, 247);
    static Color BLUSH_PINK = new Color(247, 230, 230);
    static Color SLATE_BLUE = new Color(176, 196, 222);

    // Clues View
    public Color CluesHeaderFG = SLATE_BLUE;
    public Color CluesHeaderBG = IVORY_WHITE;

    public Color FocusedClueFG = MUTED_LAVENDER;
    public Color FocusedClueBG = IVORY_WHITE;

    public Color CrossClueFG = PALE_MINT;
    public Color CrossClueBG = IVORY_WHITE;

    public Color InactiveClueFG = ColorName.Gray;
    public Color InactiveClueBG = IVORY_WHITE;
    */

    //16 Color

    //Clues

    public Color CluesBackgroundBG = ColorName.White;

    public Color CluesHeaderFG = ColorName.Black;
    public Color CluesHeaderBG = ColorName.White;

    public Color FocusedClueFG = ColorName.Yellow;
    public Color FocusedClueBG = ColorName.White;

    public Color CrossClueFG = ColorName.Cyan;
    public Color CrossClueBG = ColorName.White;

    public Color InactiveClueFG = ColorName.Gray;
    public Color InactiveClueBG = ColorName.White;

    //Grid

    //GridBackgroundBG

    //CursorHighlightFG
    //CursorHighlightBG
    //CursorEmptyHighlightFG
    //CursorEmptyHighlightBG
    //CursorEmptyHighlightChar

    //ActiveHighlightFG
    //ActiveHighlightBG
    //ActiveEmptyHighlightChar
    //ActiveEmptyHighlightFG
    //ActiveEmptyHighlightBG

    //CrossHighlightFG
    //CrossHighlightBG
    //CrossEmptyHighlightFG
    //CrossEmptyHighlightBG
    //CrossEmptyHighlightChar

    public Theme() {
      //load theme from json
      //or default...
    }

  }
}
