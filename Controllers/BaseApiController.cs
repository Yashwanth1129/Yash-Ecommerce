using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers.RequestHelpers;
using WebApplication1.Models;
using WebApplication1.Models.Interfaces;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected async Task<ActionResult> CreatePagesResult<T>(IGnericRepository<T> repo, ISpecification<T> spec,
            int pageIndex, int pageSize) where T: BaseEntity
        {
            var items = await repo.ListAsync(spec);
            var count =await repo.ContAsync(spec);

            var pagination = new Pagination<T>(pageIndex, pageSize, count, items);

            return Ok(pagination);

        }
    }
}
