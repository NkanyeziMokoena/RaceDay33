using Microsoft.AspNetCore.Mvc;using raceday_mvc.Models;using raceday_mvc.Services;using raceday_mvc.Filters;
namespace raceday_mvc.Controllers;
public class EventsController(RaceDayApiClient api):Controller
{
 public async Task<IActionResult> Index()=>View(await api.GetAsync<List<EventVm>>("api/events")??new());
 public async Task<IActionResult> Details(int id){var e=await api.GetAsync<EventVm>($"api/events/{id}");if(e==null)return NotFound();ViewBag.Categories=await api.GetAsync<List<CategoryVm>>($"api/events/{id}/categories")??new();return View(e);}
 [HttpPost,ValidateAntiForgeryToken,RequireRole("Participant")] public async Task<IActionResult> Enrol(int categoryId){var r=await api.PostAsync($"api/categories/{categoryId}/enrol",new{});TempData[r.Ok?"Message":"Error"]=r.Message;return RedirectToAction("Index","Events");}
}
