using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BFF.ViewModels;
using Common.Types.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using BFF.Hateoas;
using Common.Types.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BFF.Controllers
{
    [Route("[controller]")]
    public class ShowsController : Controller
    {
        private ILogger<ShowsController> _logger;
        private IShowService _showService;

        public ShowsController(IShowService showService, ILogger<ShowsController> logger)
        {
            _logger = logger;
            _showService = showService;
        }

        // GET: /Shows
        [HttpGet(Name = "shows")]
        public async Task<IActionResult> Get(int offset, int limit)
        {
            if(offset < 0 || limit < 0)
            {
                return BadRequest();
            }

            if (limit == 0)
            {
                return Ok(ViewModelToHateoas(offset, limit, new List<ShowViewModel>()));
            }

            var model = await _showService.GetListAsync(offset, limit);
            return Ok(ViewModelToHateoas(offset, limit, GetShowViewModel(model)));
        }
        
        private RootObject<IEnumerable<ShowViewModel>> ViewModelToHateoas(int offset, int limit, IEnumerable<ShowViewModel> viewModel)
        {
            var hateoas = new RootObject<IEnumerable<ShowViewModel>>(viewModel, GetLinks(offset, limit), new { Count = viewModel.Count(), offset, limit });
            return hateoas;
        }

        private IEnumerable<UrlLink> GetLinks(int offset, int limit)
        {
            var links = new List<UrlLink>();
            links.Add(new UrlLink("self", Url.Link("shows", new { offset, limit })));
            links.Add(new UrlLink("first", Url.Link("shows", null)));
            links.Add(new UrlLink("prev", Url.Link("shows", new { offset = Math.Max((offset - limit), 0), limit })));
            links.Add(new UrlLink("next", Url.Link("shows", new { offset = (offset + limit), limit })));
            return links;

        }

        private IEnumerable<ShowViewModel> GetShowViewModel(IEnumerable<Show> model)
        {
            return from show in model
                   select new ShowViewModel
                   {
                       Id = show.Id,
                       Name = show.Name,
                       Cast = show.Cast?.Where(cast => cast.Person != null)
                                        .OrderByDescending(cast => cast.Person.Birthday)
                                        .Select(cast => new CastViewModel { Id = cast.Person.Id, Name = cast.Person.Name, Birthday = cast.Person.Birthday })
                   };
        }
    }
}
