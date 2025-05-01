
namespace Exceptions {

  /// <summary>
  /// PluginException base
  /// </summary>
  public class PluginException : Exception
  {
    public PluginException() {}
    public PluginException(string message) 
      : base(message) {}
    public PluginException(string message,Exception inner) 
      : base(message,inner) {}
  }

  /// <summary>
  /// the plugin could not be cloned
  /// </summary>
  public class PluginCloneException : PluginException
  {
    public PluginCloneException() {}
    public PluginCloneException(string message) 
      : base(message) {}
    public PluginCloneException(string message,Exception inner) 
      : base(message,inner) {}
  }

  /// <summary>
  /// the plugin could not be built
  /// </summary>
  public class PluginBuildException : PluginException
  {
    public PluginBuildException() {}
    public PluginBuildException(string message) 
      : base(message) {}
    public PluginBuildException(string message,Exception inner) 
      : base(message,inner) {}
  }

  /// <summary>
  /// the plugin responded with an error
  /// </summary>
  public class PluginResponseException : PluginException
  {
    public PluginResponseException() {}
    public PluginResponseException(string message) 
      : base(message) {}
    public PluginResponseException(string message,Exception inner) 
      : base(message,inner) {}
  }

  /// <summary>
  /// the plugin response could not be serialized
  /// </summary>
  public class PluginResponseSerializationException : PluginException
  {
    public PluginResponseSerializationException() {}
    public PluginResponseSerializationException(string message) 
      : base(message) {}
    public PluginResponseSerializationException(string message,Exception inner) 
      : base(message,inner) {}
  }

  public class PluginNotFoundException : PluginException
  {
    public PluginNotFoundException() {}
    public PluginNotFoundException(string message) 
      : base(message) {}
    public PluginNotFoundException(string message,Exception inner) 
      : base(message,inner) {}
  }

  public class ConfigurationException : Exception
  {
    public ConfigurationException() {}
    public ConfigurationException(string message) 
      : base(message) {}
    public ConfigurationException(string message,Exception inner) 
      : base(message,inner) {}
  }

  public class NotImplemented : Exception
  {
    public NotImplemented() {}
    public NotImplemented(string message) 
      : base(message) {}
    public NotImplemented(string message,Exception inner) 
      : base(message,inner) {}
  }
}

