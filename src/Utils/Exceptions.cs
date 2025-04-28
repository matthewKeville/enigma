namespace Utils.Exceptions {

  public class RepoParserException : Exception
  {
    public RepoParserException() {}
    public RepoParserException(string message) 
      : base(message) {}
    public RepoParserException(string message,Exception inner) 
      : base(message,inner) {}
  }

}
