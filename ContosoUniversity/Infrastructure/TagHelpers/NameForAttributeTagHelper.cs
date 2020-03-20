using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ContosoUniversity.Infrastructure.TagHelpers
{
    [HtmlTargetElement("*", Attributes = NameForAttributeName)]
    public class NameForAttributeTagHelper : TagHelper
    {
        private const string NameForAttributeName = "name-for";

        [HtmlAttributeName(NameForAttributeName)]
        public ModelExpression NameFor { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (NameFor == null) return;

            var displayName = NameFor.Metadata.DisplayName ?? NameFor.Metadata.Name.BreakUpCamelCase();

            output.Content.SetHtmlContent(displayName);
        }
    }
}
