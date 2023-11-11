using Microsoft.AspNetCore.Mvc;
using GlobalPlaylist.Data;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation(Summary = "Get all songs in the playlist",
        Description = "This endpoint retrieves details from every song in the playlist.")]
    public IEnumerable<Song> Get()
    {
        List<Song> songs = _dbContext.Songs.ToList();
        return songs;
    }

    [HttpGet("{id}", Name = "GetSongById")]
    [SwaggerOperation(Summary = "Get a specific song from the playlist",
        Description = "This endpoint retrieves details from a specific song in the playlist.")]
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
    [SwaggerOperation(Summary = "Add a new song to the playlist",
        Description = "This endpoint creates a new song entry and adds it to the playlist.")]
    public IActionResult CreateSong([FromBody] Song newSong)
    {
        if (string.IsNullOrWhiteSpace(newSong.Title))
        {
            ModelState.AddModelError("Title", "Title is required");
        }

        if (string.IsNullOrWhiteSpace(newSong.Artist))
        {
            ModelState.AddModelError("Artist", "Artist is required");
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
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

    [HttpDelete("{id}", Name = "DeleteSongById")]
    [SwaggerOperation(Summary = "Delete a specific song from the playlist",
        Description = "This endpoint deletes a specific song from the playlist.")]
    public IActionResult DeleteSongById(int id)
    {
        var song = _dbContext.Songs.FirstOrDefault(s => s.Id == id);
        if (song == null)
        {
            return NotFound();
        }
        _dbContext.Songs.Remove(song);
        _dbContext.SaveChanges();
        return NoContent();
    }

    [HttpPut("{id}", Name = "UpdateSong")]
    [SwaggerOperation(Summary = "Update a specific song in the playlist",
        Description = "This endpoint updates details for a specific song in the playlist.")]
    public IActionResult UpdateSong(int id, [FromBody] Song updatedSong)
    {    
        var song = _dbContext.Songs.FirstOrDefault(s => s.Id == id);
        if (song == null)
        {
            return NotFound();
        }
        song.Title = updatedSong.Title ?? song.Title;
        song.Artist = updatedSong.Artist ?? song.Artist;
        song.Album = updatedSong.Album ?? song.Album;
        song.Genre = updatedSong.Genre ?? song.Genre;
        song.Year = updatedSong.Year ?? song.Year;
        song.Link = updatedSong.Link ?? song.Link;
        _dbContext.SaveChanges();
        return Ok(song);
    }
}
