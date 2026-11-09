using Microsoft.AspNetCore.Mvc;using raceday_mvc.Services;using raceday_mvc.Models;using raceday_mvc.Filters;
namespace raceday_mvc.Controllers;
[RequireRole("Participant")]
public class ParticipantController(RaceDayApiClient api):Controller
{
 public async Task<IActionResult> Enrolments()=>View(await api.GetAsync<List<EnrolmentVm>>("api/users/me/enrolments")??new());
 public async Task<IActionResult> Results()=>View(await api.GetAsync<List<ResultVm>>("api/users/me/results")??new());
 [HttpPost,ValidateAntiForgeryToken]public async Task<IActionResult> Cancel(int id){var r=await api.DeleteAsync($"api/enrolments/{id}");TempData[r.Ok?"Message":"Error"]=r.Message;return RedirectToAction(nameof(Enrolments));}
}
