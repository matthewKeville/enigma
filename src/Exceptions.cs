
namespace Exceptions {

  public class PluginException : Exception
  {
    public PluginException() {}
    public PluginException(string message) 
      : base(message) {}
    public PluginException(string message,Exception inner) 
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

