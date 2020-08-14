using System.Collections.Generic;
using System.Linq;
using CommandApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly CommandContext _context;
        public CommandsController(CommandContext context)
        {
            _context = context;
        }

        //GET:      api/commands
        [HttpGet]
        public ActionResult<IQueryable<Command>> GetCommandItems()
        {
            return _context.CommandItems;
        }
    }
}
