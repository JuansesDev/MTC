using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers;

public class ProductController : Controller
{
    private static readonly List<Product> _items = new();
    private static int _nextId = 1;

    public IActionResult Index()
    {
        return View(_items);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Product item)
    {
        if (ModelState.IsValid)
        {
            item.Id = _nextId++;
            _items.Add(item);
            return RedirectToAction(nameof(Index));
        }
        return View(item);
    }
}
