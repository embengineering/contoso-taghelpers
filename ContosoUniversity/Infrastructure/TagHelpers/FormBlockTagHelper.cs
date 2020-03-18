using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ContosoUniversity.Infrastructure.TagHelpers
{
    [HtmlTargetElement("form-block", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class FormBlockTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";

        private readonly IHtmlGenerator _generator;

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        public FormBlockTagHelper(IHtmlGenerator generator)
        {
            _generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var label = GenerateLabel();
            var field = GenerateField();
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
            _generator.GenerateLabel(ViewContext, For.ModelExplorer, For.Name, For.Metadata.DisplayName ?? BreakUpCamelCase(For.Metadata.Name), new { @class = "control-label"} );

        private TagBuilder GenerateField()
        {
            var modelType = For.ModelExplorer.ModelType;
            TagBuilder tagBuilder = null;

            var numberTypes = new[]
            {
                typeof(int),
                typeof(double),
                typeof(decimal)
            };

            var textTypes = new[]
            {
                typeof(string)
            };

            var dateTypes = new[]
            {
                typeof(DateTime),
                typeof(DateTimeOffset)
            };

            if (numberTypes.Contains(modelType))
            {
                tagBuilder = _generator.GenerateTextBox(ViewContext, For.ModelExplorer, For.Name, For.Model, null,
                    new {@class = "form-control", type = "number"});
            }
            else if (textTypes.Contains(modelType))
            {
                tagBuilder = _generator.GenerateTextBox(ViewContext, For.ModelExplorer, For.Name, For.Model, null,
                    new { @class = "form-control", type = "text" });
            }
            else if (dateTypes.Contains(modelType))
            {
                tagBuilder = _generator.GenerateTextBox(ViewContext, For.ModelExplorer, For.Name, For.Model, null,
                    new { @class = "form-control", type = "date" });
            }

            return tagBuilder;
        }

        private string BreakUpCamelCase(string fieldName)
        {
            var patterns = new[]
            {
                "([a-z])([A-Z])",
                "([0-9])([a-zA-Z])",
                "([a-zA-Z])([0-9])"
            };
            var output = patterns.Aggregate(fieldName,
                (current, pattern) => Regex.Replace(current, pattern, "$1 $2", RegexOptions.IgnorePatternWhitespace));
            return output.Replace('_', ' ');
        }
    }
}
