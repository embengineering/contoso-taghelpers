using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContosoUniversity.Infrastructure
{
    public interface ISelectListOptionsProvider
    {
        Task<IEnumerable<SelectListItem>> GetOptions();
    }
}
