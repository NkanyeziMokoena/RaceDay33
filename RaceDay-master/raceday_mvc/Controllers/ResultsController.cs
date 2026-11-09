using Microsoft.AspNetCore.Mvc;using raceday_mvc.Services;using raceday_mvc.Models;
namespace raceday_mvc.Controllers;
public class ResultsController(RaceDayApiClient api):Controller{public async Task<IActionResult> Leaderboard(int categoryId){ViewBag.CategoryId=categoryId;return View(await api.GetAsync<List<ResultVm>>($"api/categories/{categoryId}/results")??new());}}
