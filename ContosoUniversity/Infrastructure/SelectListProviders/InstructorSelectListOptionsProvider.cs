using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Data;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Infrastructure.SelectListProviders
{
    public class InstructorSelectListOptionsProvider : SelectListOptionsProvider
    {
        public InstructorSelectListOptionsProvider(SchoolContext schoolContext) : base(schoolContext)
        {
        }

        public override async Task<IEnumerable<SelectListItem>> GetOptions()
        {
            var items = await DbContext.Instructors
                .OrderBy(x => x.FullName)
                .Select(x => new SelectListItem(x.FullName, x.Id.ToString()))
                .ToArrayAsync();

            return items;
        }
    }
}
