using Microsoft.AspNetCore.Mvc;
using WebAPIDemo.Data;
using WebAPIDemo.Filters;
using WebAPIDemo.Filters.ActionFilters;
using WebAPIDemo.Filters.AuthFilters;
using WebAPIDemo.Filters.ExceptionFilters;
using WebAPIDemo.Models;
using WebAPIDemo.Models.Repositories;

namespace WebAPIDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
[JwtTokenAuthFilter]
public class ShirtsController(ApplicationDbContext db) : ControllerBase
{
    private readonly ApplicationDbContext _db = db;

    [HttpGet]
    public IActionResult GetShirts()
    {
        return Ok(db.Shirts.ToList());
    }

    [HttpGet("{id:int}")]
    [TypeFilter(typeof(FilterShirtId))]
    public IActionResult GetShirtById(int id)
    {
        return Ok(HttpContext.Items["shirt"]);
    }

    [HttpPost]
    [TypeFilter(typeof(FilterCreateShirt))]
    public IActionResult CreateShirt([FromBody] Shirt shirt)
    {
        this._db.Shirts.Add(shirt);
        this._db.SaveChanges();

        return CreatedAtAction(nameof(GetShirtById), new { id = shirt.ShirtId }, shirt);
    }

    [HttpPut("{id:int}")]
    [TypeFilter(typeof(FilterShirtId))]
    [FilterUpdateShirt]
    [TypeFilter(typeof(HandleUpdateExceptions))]
    public IActionResult UpdateShirt(int id, Shirt shirt)
    {
        var shirtToUpdate = HttpContext.Items["shirt"] as Shirt;
        // Should never be null because of FilterShirtId
        shirtToUpdate.Brand = shirt.Brand;
        shirtToUpdate.Price = shirt.Price;
        shirtToUpdate.Size = shirt.Size;
        shirtToUpdate.Color = shirt.Color;
        shirtToUpdate.Gender = shirt.Gender;

        db.SaveChanges();

        return NoContent();
    }

    [TypeFilter(typeof(FilterShirtId))]
    [HttpDelete("{id:int}")]
    public IActionResult DeleteShirt(int id)
    {
        var shirtToDelete = HttpContext.Items["shirt"] as Shirt;
        db.Shirts.Remove(shirtToDelete);
        db.SaveChanges();

        return Ok(shirtToDelete);
    }
}