using Microsoft.AspNetCore.Mvc;
using GlobalPlaylist.Data;

namespace GlobalPlaylist.Controllers;

[ApiController]
[Route("playlist")]
public class GlobalPlaylistController : ControllerBase
{
    private readonly ILogger<GlobalPlaylistController> _logger;
    private readonly PlaylistDBContext _dbContext;

    public GlobalPlaylistController(PlaylistDBContext dbContext, ILogger<GlobalPlaylistController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet(Name = "GetGlobalPlaylist")]
    public IEnumerable<Song> Get()
    {
        List<Song> songs = _dbContext.Songs.ToList();
        return songs;
    }

    [HttpGet("{id}", Name = "GetSongById")]
    public IActionResult GetSongById(int id)
    {
        var song = _dbContext.Songs.FirstOrDefault(s => s.Id == id);
        if (song == null)
        {
            return NotFound();
        }
        return Ok(song);
    }


    [HttpPost(Name = "CreateNewSong")]
    public IActionResult CreateSong([FromBody] Song newSong)
    {
        if (newSong == null)
        {
            return BadRequest("Invalid song data");
        }
        var song = new Song
        {
            Title = newSong.Title,
            Artist = newSong.Artist,
            Album = newSong.Album,
            Genre = newSong.Genre,
            Year = newSong.Year,
            Link = newSong.Link
        };
        _dbContext.Songs.Add(song);
        _dbContext.SaveChanges();
        return CreatedAtAction("GetSongById", new { id = song.Id }, song);
    }
}
