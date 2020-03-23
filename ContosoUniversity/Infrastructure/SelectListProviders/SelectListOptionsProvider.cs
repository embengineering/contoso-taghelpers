using System.Collections.Generic;
using System.Threading.Tasks;
using ContosoUniversity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContosoUniversity.Infrastructure.SelectListProviders
{
    public abstract class SelectListOptionsProvider : ISelectListOptionsProvider
    {
        public readonly SchoolContext DbContext;

        protected SelectListOptionsProvider(SchoolContext schoolContext)
        {
            DbContext = schoolContext;
        }

        public abstract Task<IEnumerable<SelectListItem>> GetOptions();
    }
}