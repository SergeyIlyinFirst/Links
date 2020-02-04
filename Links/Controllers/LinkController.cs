using System.Collections.Generic;
using System.Threading.Tasks;
using Links.DataLayer;
using Links.Models;
using Microsoft.AspNetCore.Mvc;

namespace Links.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LinkController : ControllerBase
    {
        private readonly LinkService db = new LinkService();

        [HttpGet("{link}")]
        public async Task<IActionResult> GetLink(string link)
        {
            string url = await db.ClickOnTheLink(link);
            return Redirect(url);
        }
        [HttpGet]
        public async Task<Dictionary<string, int>> GetLinks()
        {
            return await db.GetLinks();
        }
        [HttpPut]
        public async Task<string> PutLink([FromBody]Link model)
        {
            return await db.GetLink(model.AbbreviatedTitle);

        }
        [HttpPost]
        public async Task<string> PostLink([FromBody]Link model)
        {
            return await db.Create(model.Title);
        }
    }
}
