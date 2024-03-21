using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers;

public class ShirtsController : Controller
{
    private readonly IWebApiExecutor _webApiExecutor;

    public ShirtsController(IWebApiExecutor webApiExecutor)
    {
        _webApiExecutor = webApiExecutor;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _webApiExecutor.InvokeGet<List<Shirt>>("shirts"));
    }

    public IActionResult CreateShirt()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateShirt(Shirt shirt)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var response = await _webApiExecutor.InvokePost("shirts", shirt);
                if (response is not null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (WebApiException e)
            {
                HandleWebApiException(e);
            }

        }

        return View(shirt);
    }

    public async Task<IActionResult> UpdateShirt(int shirtId)
    {
        try
        {
            var shirt = await _webApiExecutor.InvokeGet<Shirt>($"shirts/{shirtId}");
            if (shirt is not null)
                return View(shirt);
        }
        catch (WebApiException e)
        {
            HandleWebApiException(e);
            return View();
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateShirt(Shirt shirt)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _webApiExecutor.InvokePut($"shirts/{shirt.ShirtId}", shirt);
                return RedirectToAction(nameof(Index));
            }
            catch (WebApiException e)
            {
                HandleWebApiException(e);
            }
        }

        return View(shirt);
    }

    public async Task<IActionResult> DeleteShirt(int shirtId)
    {
        try
        {
            await _webApiExecutor.InvokeDelete($"shirts/{shirtId}");
            return RedirectToAction(nameof(Index));
        }
        catch (WebApiException e)
        {
            HandleWebApiException(e);
            return View(nameof(Index), await _webApiExecutor.InvokeGet<List<Shirt>>("Shirts"));
        }

    }

    private void HandleWebApiException(WebApiException e)
    {
        if (e.ErrorResponse is not null &&
            e.ErrorResponse.Errors is not null &&
            e.ErrorResponse.Errors.Count > 0)
        {
            foreach (var error in e.ErrorResponse.Errors)
            {
                ModelState.AddModelError(error.Key, string.Join("; ", error.Value));
            }
        }
    }
}