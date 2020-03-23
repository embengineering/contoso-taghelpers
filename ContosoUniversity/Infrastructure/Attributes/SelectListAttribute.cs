using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Data;
using ContosoUniversity.Infrastructure.SelectListProviders;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContosoUniversity.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class SelectListAttribute : UIHintAttribute
    {
        private readonly Type _selectListProvider;

        public bool InsertBlank { get; set; }

        public SelectListAttribute(Type selectListProvider) : base("SelectList")
        {
            _selectListProvider = selectListProvider;
            InsertBlank = true;
        }

        public async Task<IEnumerable<SelectListItem>> GetOptions(SchoolContext dbContext)
        {
            var provider = (ISelectListOptionsProvider)Activator.CreateInstance(_selectListProvider, dbContext);
            var options = (await provider.GetOptions()).ToArray();

            return InsertBlank ? options.Prepend(new SelectListItem(string.Empty, string.Empty)) : options;
        }
    }
}
