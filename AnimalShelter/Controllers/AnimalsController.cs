using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AnimalShelter.Models;

namespace AnimalShelter.Controllers
{
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
  }
}