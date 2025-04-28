
using System.Text.RegularExpressions;
using Utils.Exceptions;

namespace Utils {

  public static class RepoParser {

    /// <exception cref="PluginException">If src can't be parsed into 'author/repo' </exception>
    public static String GetRepoName(String src) {
      string[] tokens = src.Split('/');
      if ( tokens.Count() < 3 ) {
        throw new RepoParserException("Unable to determine repo name");
      }
      string repo = tokens[tokens.Count()-1];
      // rip .git
      if ( repo.Contains(".git") ) {
        repo = repo.Substring(0,repo.Count()-4);
      }
      return $"{repo}";
    }

    public static bool IsRepoUrl(String url) {
      // WARN : begin gpt
      return Regex.IsMatch(url,
      @"^(https?:\/\/|git@)github\.com[:\/](?:[\w\-]+\/)+[\w\-]+(?:\.git)?$",
      RegexOptions.IgnoreCase);
      // WARN : end gpt
    }

  }

}
