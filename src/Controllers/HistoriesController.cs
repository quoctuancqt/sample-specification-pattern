namespace src.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using src.Domain;
    using src.Dtos;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class HistoriesController : ControllerBase
    {
        private readonly TestDbContext _context;

        public HistoriesController(TestDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context
                .Histories
                .Include(x=>x.User)
                .Select(x=> new HistoryDto
                {
                    EntityName = x.EntityName,
                    FieldName = x.FieldName,
                    OldValue = x.OldValue,
                    NewValue = x.NewValue,
                    ActionType = x.ActionType,
                    ActionTypeValue = x.ActionType.ToString(),
                    CreatedDate = x.CreatedDate,
                    DisplayName = x.User.DisplayName,
                    Email = x.User.Email
                }).ToListAsync());
        }
    }
}
