using Microsoft.AspNetCore.Mvc;using raceday_mvc.Services;using raceday_mvc.Models;using raceday_mvc.Filters;
namespace raceday_mvc.Controllers;
[RequireRole("Organiser")]
public class OrganiserController(RaceDayApiClient api):Controller
{
 public async Task<IActionResult> Index(){var id=HttpContext.Session.GetInt32("UserId");var all=await api.GetAsync<List<EventVm>>("api/events")??new();return View(all.Where(e=>e.OrganiserId==id).OrderBy(e=>e.EventDate).ToList());}
 [HttpGet]public IActionResult CreateEvent()=>View(new EventVm{EventDate=DateTime.Today.AddDays(30),EventType="Run"});
 [HttpGet]public async Task<IActionResult> EditEvent(int id){var ev=await api.GetAsync<EventVm>($"api/events/{id}");if(ev==null||ev.OrganiserId!=HttpContext.Session.GetInt32("UserId"))return NotFound();return View(ev);}
 [HttpPost,ValidateAntiForgeryToken]public async Task<IActionResult> EditEvent(EventVm vm){var r=await api.PutAsync($"api/events/{vm.EventId}",new{name=vm.Name,description=vm.Description,event_date=vm.EventDate.ToString("O"),location=vm.Location,event_type=vm.EventType,distance_km=vm.DistanceKm?.ToString(System.Globalization.CultureInfo.InvariantCulture)});if(!r.Ok){ModelState.AddModelError("",r.Message);return View(vm);}TempData["Message"]="Event updated.";return RedirectToAction(nameof(Index));}
 [HttpPost,ValidateAntiForgeryToken]public async Task<IActionResult> CreateEvent(EventVm vm){var r=await api.PostAsync("api/events",new{name=vm.Name,description=vm.Description,event_date=vm.EventDate.ToString("O"),location=vm.Location,event_type=vm.EventType,distance_km=vm.DistanceKm?.ToString(System.Globalization.CultureInfo.InvariantCulture)});if(!r.Ok){ModelState.AddModelError("",r.Message);return View(vm);}TempData["Message"]="Event created.";return RedirectToAction(nameof(Index));}
 [HttpGet]public async Task<IActionResult> Categories(int id){ViewBag.EventId=id;ViewBag.Event=await api.GetAsync<EventVm>($"api/events/{id}");return View(await api.GetAsync<List<CategoryVm>>($"api/events/{id}/categories")??new());}
 [HttpPost,ValidateAntiForgeryToken]public async Task<IActionResult> CreateCategory(int eventId,string name,decimal distanceKm,int maxParticipants){var r=await api.PostAsync($"api/events/{eventId}/categories",new{name,distance_km=distanceKm,max_participants=maxParticipants});TempData[r.Ok?"Message":"Error"]=r.Message;return RedirectToAction(nameof(Categories),new{id=eventId});}
 [HttpGet]public async Task<IActionResult> Enrolments(int categoryId){ViewBag.CategoryId=categoryId;return View(await api.GetAsync<List<EnrolmentVm>>($"api/categories/{categoryId}/enrolments")??new());}
 [HttpPost,ValidateAntiForgeryToken]public async Task<IActionResult> RecordResult(int enrolmentId,string finishTime,int position,int categoryId){var r=await api.PostAsync($"api/enrolments/{enrolmentId}/result",new{finish_time=finishTime,position});TempData[r.Ok?"Message":"Error"]=r.Message;return RedirectToAction(nameof(Enrolments),new{categoryId});}
 [HttpPost,ValidateAntiForgeryToken]public async Task<IActionResult> DeleteEvent(int id){var r=await api.DeleteAsync($"api/events/{id}");TempData[r.Ok?"Message":"Error"]=r.Message;return RedirectToAction(nameof(Index));}
}
