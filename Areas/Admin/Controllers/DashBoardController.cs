using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers;

[Area("Admin")]
public class DashBoardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}