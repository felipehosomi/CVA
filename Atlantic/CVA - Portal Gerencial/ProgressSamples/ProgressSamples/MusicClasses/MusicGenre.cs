using System;

namespace ProgressSamples
{
  public class MusicGenre
  {
    public MusicGenre()
    {
      GenreId = 0;
      Genre = string.Empty;
    }

    public int GenreId { get; set; }
    public string Genre { get; set; }
  }
}
