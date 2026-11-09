using Microsoft.AspNetCore.Mvc;
using raceday_mvc.Filters;
using raceday_mvc.Models;
using raceday_mvc.Services;
using System.Text.Json;

namespace raceday_mvc.Controllers;

[RequireRole]
public class ProfileController(RaceDayApiClient api) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var response = await api.SendAsync(HttpMethod.Get, "api/users/me");
        if (!response.Ok || string.IsNullOrWhiteSpace(response.Json))
        {
            TempData["Error"] = "Could not load your profile. Please sign in again.";
            return RedirectToAction("Login", "Account");
        }
        try
        {
            using var json = JsonDocument.Parse(response.Json);
            var root = json.RootElement;
            return View(new ProfileVm { FullName = root.GetProperty("fullName").GetString() ?? "", Email = root.GetProperty("email").GetString() ?? "" });
        }
        catch { return View(new ProfileVm()); }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ProfileVm vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var result = await api.PutAsync("api/users/me", new { fullname = vm.FullName, email = vm.Email });
        if (!result.Ok) { ModelState.AddModelError("", result.Message); return View(vm); }
        HttpContext.Session.SetString("UserName", vm.FullName);
        TempData["Message"] = "Your profile was updated.";
        return RedirectToAction(nameof(Index));
    }
}
