using Entity;
using Microsoft.EntityFrameworkCore;
using Settings;

public class DatabaseContext : DbContext {

  public DbSet<Crossword> Crosswords { get; set; }
  public DbSet<Word> Words { get; set; }
  public DbSet<GridChar> GridChars { get; set; }

  public String DbPath { get; set; }

  public DatabaseContext(AppSettings settings) {
    DbPath = settings.DbPath;
  }

  protected override void OnConfiguring(DbContextOptionsBuilder options) {
    options.UseSqlite($"Data Source=" + DbPath);
  }


}
