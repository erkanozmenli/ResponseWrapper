using Microsoft.AspNetCore.Mvc;

namespace ResponseWrapper.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        [Route("ExceptionTest")]
        public IActionResult ExceptionTest()
        {
            int value = 5;
            var result = value / 0;

            return Ok(result);
        }

        [HttpPost]
        [Route("NoContentTest")]
        public IActionResult NoContentTest()
        {
            string result = null;
            return Ok(result);
        }

        [HttpPost]
        [Route("NotFoundTest")]
        public IActionResult NotFoundTest()
        {
            return NotFound();
        }
    }
}
