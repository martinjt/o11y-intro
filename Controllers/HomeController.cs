using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace o11y_intro.Controllers;

public class HomeController : Controller
{
    private readonly HttpClient _httpClient;

    public HomeController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("/calculate")]
    public async Task<int> Calculate(int index = 0)
    {
        // CUSTOM ATTRIBUTES
        Activity.Current?.SetTag("parameter.index", index);

        if (index == 0 || index == 1)
            return 0;
        if (index == 2)
            return 1;

        var resOne = await GetNext(index - 1);
        var resTwo = await GetNext(index - 2);

        // CHILD SPAN (2 sections of code to uncomment, 2 lines total)
        using var span = ActivityHelper.Fibonacci.StartActivity("calculation");

        var fibonacciNumber = resOne + resTwo;
        span?.SetTag("result", fibonacciNumber);
        
        return fibonacciNumber;
    }

    private async Task<int> GetNext(int iv) => 
        int.Parse(await _httpClient.GetStringAsync($"http://{HttpContext.Request.Host}/calculate?index={iv}"));
}
