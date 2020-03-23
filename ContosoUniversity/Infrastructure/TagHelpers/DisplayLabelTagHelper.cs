using ContosoUniversity.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ContosoUniversity.Infrastructure.TagHelpers
{
    [HtmlTargetElement("display-label", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class DisplayLabelTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // set output
            output.TagName = "span"; // omit wrapper tag
            output.TagMode = TagMode.StartTagAndEndTag;

            // set content
            output.Content.SetHtmlContent(For.Metadata.DisplayName ?? For.Metadata.Name.BreakUpCamelCase());
        }
    }
}
