using Microsoft.AspNetCore.Mvc;
using {{ProjectName}}.Models;

namespace {{ProjectName}}.Controllers;

public class {{Name}}Controller : Controller
{
    private static readonly List<{{Name}}> _items = new();
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
    public IActionResult Create({{Name}} item)
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
