using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ContosoUniversity.Infrastructure.TagHelpers
{
    [HtmlTargetElement("display", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class DisplayTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // set output
            output.TagName = "span"; // omit wrapper tag
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("value-for", For.Name);

            // re-use ValueFor tag helper to centralize formatting
            var tagHelper = new ValueForAttributeTagHelper { ValueFor = For };

            tagHelper.Process(context, output);
        }
    }
}
