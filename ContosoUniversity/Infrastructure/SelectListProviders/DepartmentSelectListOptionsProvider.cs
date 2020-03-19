using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Infrastructure.SelectListProviders
{
    public class DepartmentSelectListOptionsProvider : SelectListOptionsProvider
    {
        public DepartmentSelectListOptionsProvider(SchoolContext schoolContext) : base(schoolContext)
        {
        }

        public override async Task<IEnumerable<SelectListItem>> GetOptions()
        {
            var items = await DbContext.Departments
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToArrayAsync();

            return items;
        }
    }
}
