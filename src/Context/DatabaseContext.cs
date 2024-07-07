using Entity;
using Microsoft.EntityFrameworkCore;

public class DatabaseContext : DbContext {

  public DbSet<Crossword> Crosswords { get; set; }
  public DbSet<Word> Words { get; set; }
  public DbSet<GridChar> GridChars { get; set; }
  public String DbPath { get; set; }

  public DatabaseContext() {
    var folder = Environment.SpecialFolder.LocalApplicationData;
    var path = Environment.GetFolderPath(folder);
    DbPath = System.IO.Path.Join(path, "enigma.db");
  }

  protected override void OnConfiguring(DbContextOptionsBuilder options) {
    //options.UseSqlite($"Data Source={DbPath}");
    options.UseSqlite($"Data Source=enigma.db");
  }

}
