namespace Services.PluginService {

  /// <summary>
  /// Base exception for service layer
  /// </summary>
  public class PluginServiceException : ServiceException
  {
    public PluginServiceException() {}
    public PluginServiceException(string message) 
      : base(message) {}
    public PluginServiceException(string message,Exception inner) 
      : base(message,inner) {}
  }


  /// <summary>
  /// Plugin could not be cloned
  /// </summary>
  public class PluginCloneException : PluginServiceException
  {
    public PluginCloneException() {}
    public PluginCloneException(string message) 
      : base(message) {}
    public PluginCloneException(string message,Exception inner) 
      : base(message,inner) {}
  }

  /// <summary>
  /// Plugin could not be built
  /// </summary>
  public class PluginBuildException : PluginServiceException
  {
    public PluginBuildException() {}
    public PluginBuildException(string message) 
      : base(message) {}
    public PluginBuildException(string message,Exception inner) 
      : base(message,inner) {}
  }

}

