using Microsoft.AspNetCore.Mvc; using raceday_mvc.Models; using raceday_mvc.Services; using System.Text.Json;
namespace raceday_mvc.Controllers;
public class AccountController(RaceDayApiClient api):Controller
{
 [HttpGet] public IActionResult Login()=>View(new LoginVm());
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Login(LoginVm vm){if(!ModelState.IsValid)return View(vm);var r=await api.SendAsync(HttpMethod.Post,"api/auth/login",new{email=vm.Email,password=vm.Password});if(!r.Ok){ModelState.AddModelError("",r.Message);return View(vm);}try{using var d=JsonDocument.Parse(r.Json!);var root=d.RootElement;HttpContext.Session.SetString("UserName",root.GetProperty("fullName").GetString()??"");HttpContext.Session.SetString("Role",root.GetProperty("role").GetString()??"");HttpContext.Session.SetInt32("UserId",root.GetProperty("userId").GetInt32());}catch{ModelState.AddModelError("", "The API returned an unexpected login response. Please try again.");return View(vm);}return RedirectToAction("Index","Home");}
 [HttpGet] public IActionResult Register()=>View(new RegisterVm());
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Register(RegisterVm vm){if(!ModelState.IsValid)return View(vm);var r=await api.PostAsync("api/auth/register",new{fullname=vm.FullName,email=vm.Email,password=vm.Password,role=vm.Role});if(!r.Ok){ModelState.AddModelError("",r.Message);return View(vm);}TempData["Message"]="Registration successful. Please sign in.";return RedirectToAction(nameof(Login));}
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Logout(){await api.PostAsync("api/auth/logout",new{});api.ClearSession();HttpContext.Session.Clear();return RedirectToAction("Index","Home");}
}
