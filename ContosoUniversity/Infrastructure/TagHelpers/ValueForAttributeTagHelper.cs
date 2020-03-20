using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ContosoUniversity.Infrastructure.TagHelpers
{
    [HtmlTargetElement("*", Attributes = ValueForAttributeName)]
    public class ValueForAttributeTagHelper : TagHelper
    {
        private const string ValueForAttributeName = "value-for";

        [HtmlAttributeName(ValueForAttributeName)]
        public ModelExpression ValueFor { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ValueFor == null) return;

            var displayValue = ValueFor.Model != null ? ValueFor.Model.ToString() : string.Empty;

            output.Content.SetHtmlContent(displayValue);
        }
    }
}
