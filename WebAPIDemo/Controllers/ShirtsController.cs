using Microsoft.AspNetCore.Mvc;
using WebAPIDemo.Filters;
using WebAPIDemo.Filters.ExceptionFilters;
using WebAPIDemo.Models;
using WebAPIDemo.Models.Repositories;

namespace WebAPIDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShirtsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetShirts()
    {
        return Ok(ShirtRepository.GetShirts());
    }

    [HttpGet("{id:int}")]
    [FilterShirtId]
    public IActionResult GetShirtById(int id)
    {
        return Ok(ShirtRepository.GetShirtById(id));
    }

    [HttpPost]
    [FilterCreateShirt]
    public IActionResult CreateShirt([FromBody] Shirt shirt)
    {
        ShirtRepository.AddShirt(shirt);

        return CreatedAtAction(nameof(GetShirtById), new { id = shirt.ShirtId }, shirt);
    }

    [HttpPut("{id:int}")]
    [FilterShirtId]
    [FilterUpdateShirt]
    [HandleUpdateExceptions]
    public IActionResult UpdateShirt(int id, Shirt shirt)
    {
        ShirtRepository.UpdateShirt(shirt);
        return NoContent();
    }

    [FilterShirtId]
    [HttpDelete("{id:int}")]
    public IActionResult DeleteShirt(int id)
    {
        var shirt = ShirtRepository.GetShirtById(id);
        ShirtRepository.DeleteShirt(id);
        return Ok(shirt);
    }
}