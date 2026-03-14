namespace Services.CommandServices {

using Services.PluginService;

public class SyncCommandService {

  private PluginService pluginService;

  public SyncCommandService(PluginService pluginService) {
    this.pluginService = pluginService;
  }
  
  public void Sync(String[] args) {
    try {
      pluginService.Sync();
    } catch (PluginServiceException ex) {
      throw new CommandServiceException("",ex);
    }
  }

}

} 

