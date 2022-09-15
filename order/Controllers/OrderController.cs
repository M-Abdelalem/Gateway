using Microsoft.AspNetCore.Mvc;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<Order> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Order
            {
                Date = DateTime.Now.AddDays(index),
                cost=100,
                id=1

            })
            .ToArray();
        }
    }
}