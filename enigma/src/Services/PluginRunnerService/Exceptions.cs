namespace Services.PluginRunnerService {

  /// <summary>
  /// Base exception PluginRunnerService
  /// </summary>
  public class PluginRunnerServiceException : ServiceException
  {
    public PluginRunnerServiceException() {}
    public PluginRunnerServiceException(string message) 
      : base(message) {}
    public PluginRunnerServiceException(string message,Exception inner) 
      : base(message,inner) {}
  }

  public class FetchFailedException : PluginRunnerServiceException
  {
    public FetchFailedException() {}
    public FetchFailedException(string message) 
      : base(message) {}
    public FetchFailedException(string message,Exception inner) 
      : base(message,inner) {}
  }

  public class PluginResponseSerializationException : PluginRunnerServiceException
  {
    public PluginResponseSerializationException() {}
    public PluginResponseSerializationException(string message) 
      : base(message) {}
    public PluginResponseSerializationException(string message,Exception inner) 
      : base(message,inner) {}
  }

  public class PluginNotFoundException : PluginRunnerServiceException
  {
    public PluginNotFoundException() {}
    public PluginNotFoundException(string message) 
      : base(message) {}
    public PluginNotFoundException(string message,Exception inner) 
      : base(message,inner) {}
  }


}

