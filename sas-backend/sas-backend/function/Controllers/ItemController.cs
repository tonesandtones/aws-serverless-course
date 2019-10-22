using System;
using System.Linq;
using function.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace function.Controllers
{
    [Route("items")]
    public class ItemController : Controller
    {
        private readonly ILogger<ItemController> _logger;
        private readonly TestDataAccessor _accessor;

        public ItemController(ILogger<ItemController> logger, TestDataAccessor accessor)
        {
            _logger = logger;
            _accessor = accessor;
        }

        [HttpGet]
        public IActionResult GetItems()
        {
            return Ok(_accessor.TestData.Items);
        }

        [HttpGet("id/{id}")]
        public IActionResult GetItem([FromRoute] string id)
        {
            var item = _accessor.TestData.Items.FirstOrDefault(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }
    }
}