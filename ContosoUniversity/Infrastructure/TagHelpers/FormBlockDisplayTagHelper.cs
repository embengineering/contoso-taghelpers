using System.Linq;
using ContosoUniversity.Infrastructure.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ContosoUniversity.Infrastructure.TagHelpers
{
    [HtmlTargetElement("form-block-display", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class FormBlockDisplayTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";

        private readonly IHtmlGenerator _generator;

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        public FormBlockDisplayTagHelper(IHtmlGenerator generator)
        {
            _generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var label = GenerateLabel();
            var displayField = GenerateDisplayField();

            var classes = output.Attributes.FirstOrDefault(a => a.Name == "class")?.Value;

            // set output
            output.Attributes.SetAttribute("class", $"form-group {classes}");
            output.TagName = "div"; // omit wrapper tag
            output.TagMode = TagMode.StartTagAndEndTag;

            // set content
            output.PreContent.SetHtmlContent(label);
            output.Content.SetHtmlContent(displayField);
        }

        private TagBuilder GenerateLabel() =>
            _generator.GenerateLabel(ViewContext, For.ModelExplorer, For.Name, For.Metadata.DisplayName ?? For.Metadata.Name.BreakUpCamelCase(), new { @class = "control-label"} );

        private TagBuilder GenerateDisplayField()
        {
            var displayField = new TagBuilder("div");
            var text = For.ModelExplorer.GetSimpleDisplayText(); // TODO: set patterns to format by type
            displayField.InnerHtml.SetContent(text);
            return displayField;
        }
    }
}
