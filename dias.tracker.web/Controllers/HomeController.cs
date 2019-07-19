﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dias.tracker.web.Models;
using System.Net.Http;

namespace dias.tracker.web.Controllers {
  public class HomeController : Controller {
    private static readonly HttpClient _client = new HttpClient();
    private static readonly string _remoteUrl = "https://dias-tracker-api.azurewebsites.net";
    public IActionResult Index() {
      var data = _client.GetStringAsync($"{_remoteUrl}/api/Values");

      return Json(data);
    }

    public IActionResult Privacy() {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
