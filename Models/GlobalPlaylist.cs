using System.ComponentModel.DataAnnotations;

namespace GlobalPlaylist;

public class Song
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Artist is required")]
    public string? Artist { get; set; }

    public string? Album { get; set; }
    public string? Genre { get; set; }
    public int Year { get; set; }
    public string? Link { get; set; }
}
