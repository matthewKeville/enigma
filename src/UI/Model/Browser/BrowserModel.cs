using Enums;

namespace UI.Model.Browser {

  public class BrowserModel : IModel {
    public BrowserTab activeTab = BrowserTab.PICKER;
    public void SwitchTab() {
      if ( activeTab == BrowserTab.PICKER ) {
        activeTab = BrowserTab.INSTALLER;
      } else {
        activeTab = BrowserTab.PICKER;
      }
    }
  }

}
