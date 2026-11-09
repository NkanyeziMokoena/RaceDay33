using Microsoft.AspNetCore.Mvc; using raceday_mvc.Services; using raceday_mvc.Models;
namespace raceday_mvc.Controllers;
public class HomeController(RaceDayApiClient api):Controller
{
 public async Task<IActionResult> Index(){ViewBag.User=HttpContext.Session.GetString("UserName");ViewBag.Role=HttpContext.Session.GetString("Role");var events=await api.GetAsync<List<EventVm>>("api/events");return View(events??new());}
 public IActionResult Error()=>View();
}
