using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
 //Using for XML comments with swagger
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AnimalShelter.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace AnimalShelter.Controllers
{
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [Route("api/[controller]")]
  [ApiController]
  [Produces("application/json")]
  public class AnimalsController : ControllerBase 
  {
    private readonly AnimalShelterContext _db;

    public AnimalsController(AnimalShelterContext db)
    {
      _db = db;
    }
    ///<summary>
    /// Get a specific animal.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Animal>>> Get(string name, string species, string breed)
    {
      var query = _db.Animals.AsQueryable();

      if( name != null)
      {
        query = query.Where(entry => entry.Name == name);
      }
      if( species != null)
      {
        query = query.Where(entry => entry.Species == species);
      }
      if( breed != null)
      {
        query = query.Where(entry => entry.Breed == breed);
      }
      return await query.ToListAsync();
    }
    ///<summary>
    /// Get a specific animal by Id.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Animal>>GetAnimal(int id)
    {
      var review = await _db.Animals.FindAsync(id);
      
      if (review == null)
      {
        return NotFound();
      }
      return review;
    }
    ///<summary>
    /// Add a new animal.
    /// </summary>
    /// <param name="animal"></param>
    /// <returns>A newly created Animal</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Animals
    ///     {
    ///        "name": "AnimalName",
    ///        "species": "SpeciesName",
    ///        "breed": "BreedName",
    ///        "PostDate": "date",
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created review</response>
    /// <response code="400">If the review is null</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Animal>> Post(Animal animal)
    {
      DateTime date = DateTime.Now;
      var longDateValue = date.ToLongDateString();
      animal.PostDate = longDateValue;
      _db.Animals.Add(animal);
      await _db.SaveChangesAsync();

      return CreatedAtAction(nameof(GetAnimal), new { id = animal.AnimalId}, animal);
    }
    ///<summary>
    /// Update a specific animal.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Animal animal)
    {
      if (id != animal.AnimalId)
      {
        return BadRequest();
      }
      _db.Entry(animal).State = EntityState.Modified;

      try
      {
        await _db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!AnimalExists(id))
        {
          return NotFound();
        }
        else 
        {
          throw;
        }
      }
      return NoContent();
    }
    private bool AnimalExists(int id)
    {
      return _db.Animals.Any(e => e.AnimalId == id);
    }
    ///<summary>
    /// Deletes a specific animal.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAnimal(int id)
    {
      var animal = await _db.Animals.FindAsync(id);
      if (animal == null)
      {
        return NotFound();
      }
        _db.Animals.Remove(animal);
        await _db.SaveChangesAsync();

        return NoContent();
    }
  }
}