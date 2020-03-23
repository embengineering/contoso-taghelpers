using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ContosoUniversity.Data;
using ContosoUniversity.Infrastructure.Attributes;
using ContosoUniversity.Infrastructure.Extensions;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using StackExchange.Profiling.Internal;

namespace ContosoUniversity.Infrastructure.TagHelpers
{
    [HtmlTargetElement("form-block", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class FormBlockTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";

        private readonly IHtmlGenerator _generator;
        private readonly SchoolContext _dbContext;

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        public FormBlockTagHelper(IHtmlGenerator generator, SchoolContext dbContext)
        {
            _generator = generator;
            _dbContext = dbContext;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var label = GenerateLabel();
            var field = await GenerateField();
            var validationSpan = GenerateValidationMessage();

            var classes = output.Attributes.FirstOrDefault(a => a.Name == "class")?.Value;

            // set output
            output.Attributes.SetAttribute("class", $"form-group {classes}");
            output.TagName = "div"; // omit wrapper tag
            output.TagMode = TagMode.StartTagAndEndTag;

            // set content
            output.PreContent.SetHtmlContent(label);
            output.Content.SetHtmlContent(field);
            output.PostContent.SetHtmlContent(validationSpan);
        }

        private TagBuilder GenerateValidationMessage() =>
            _generator.GenerateValidationMessage(ViewContext, For.ModelExplorer, For.Name, string.Empty, "span", new { @class = "text-danger" });

        private TagBuilder GenerateLabel() =>
            _generator.GenerateLabel(ViewContext, For.ModelExplorer, For.Name, For.Metadata.DisplayName ?? For.Metadata.Name.BreakUpCamelCase(), new { @class = "control-label"} );

        private async Task<TagBuilder> GenerateField()
        {
            var modelType = For.ModelExplorer.ModelType;
            var fieldType = GetInputTypeFromModel(modelType);

            if (!fieldType.IsNullOrWhiteSpace())
                return BuildInput(fieldType);

            if (typeof(IEntity).IsAssignableFrom(modelType))
                return await BuildSelect();

            return null;
        }

        private async Task<TagBuilder> BuildSelect()
        {
            var containerType = For.Metadata.ContainerType;
            var property = containerType.GetProperty(For.Metadata.PropertyName);
            var dropDownListAttribute = property.GetCustomAttribute<SelectListAttribute>();
            var items = await dropDownListAttribute.GetOptions(_dbContext);
            var currentValue = ((IEntity) For.Model)?.Id.ToString();

            var tagBuilder = _generator.GenerateSelect(ViewContext, For.ModelExplorer, null, For.Name, items, 
                new List<string> { currentValue }, false, new {@class = "form-control"});

            return tagBuilder;
        }

        private TagBuilder BuildInput(string fieldType)
        {
            var value = GetValueByType();
            var tagBuilder = _generator.GenerateTextBox(ViewContext, For.ModelExplorer, For.Name, value, null,
                new {@class = "form-control", type = fieldType});
            return tagBuilder;
        }

        private object GetValueByType()
        {
            const string isoDateFormat = "yyyy-MM-dd"; // default iso format required for HTML5 date input
            const string numericFormat = "0.00";

            var propertyType = For.ModelExplorer.ModelType;

            if (DateTypes.Contains(propertyType))
            {
                var parsedValue = (DateTime?) For.Model;
                return parsedValue.HasValue
                    ? parsedValue.Value.ToString(isoDateFormat)
                    : string.Empty;
            }
            
            if (DecimalTypes.Contains(propertyType))
            {
                var parsedValue = (decimal?)For.Model;
                return parsedValue.HasValue
                    ? parsedValue.Value.ToString(numericFormat)
                    : string.Empty;
            }

            return For.Model;
        }

        private string GetInputTypeFromModel(Type modelType)
        {
            if (NumericTypes.Contains(modelType))
                return "number";

            if (DateTypes.Contains(modelType))
                return "date";

            if (modelType == typeof(string))
                return "text";

            return string.Empty;
        }

        private IEnumerable<Type> DateTypes =>
            new[]
            {
                typeof(DateTime),
                typeof(DateTime?),
                typeof(DateTimeOffset),
                typeof(DateTimeOffset?)
            };

        private IEnumerable<Type> DecimalTypes =>
            new[]
            {
                typeof(decimal?),
                typeof(decimal)
            };

        private IEnumerable<Type> NumericTypes =>
            new[]
            {
                typeof(int),
                typeof(int?),
                typeof(double),
                typeof(double?),
                typeof(decimal),
                typeof(decimal?)
            };
    }
}
