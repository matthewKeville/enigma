using Entity;
using Microsoft.EntityFrameworkCore;

public class DatabaseContext : DbContext {

  public DbSet<Crossword> Crosswords { get; set; }
  public DbSet<Clue> Clues { get; set; }
  public DbSet<GridChar> GridChars { get; set; }

  public String DbPath { get; set; }

  public DatabaseContext(Settings.Settings settings) {
    DbPath = settings.appSettings.DbPath;
  }

  protected override void OnConfiguring(DbContextOptionsBuilder options) {
    options.UseSqlite($"Data Source=" + DbPath);
  }


}
