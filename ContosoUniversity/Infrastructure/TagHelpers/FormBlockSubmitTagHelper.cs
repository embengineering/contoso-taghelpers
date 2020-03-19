using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ContosoUniversity.Infrastructure.TagHelpers
{
    [HtmlTargetElement("form-block-submit", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormBlockSubmitTagHelper : TagHelper
    {
        private readonly IHtmlGenerator _generator;

        public FormBlockSubmitTagHelper(IHtmlGenerator generator)
        {
            _generator = generator;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var submitBtn = new TagBuilder("button");
            var outputContent = (await output.GetChildContentAsync()).GetContent();

            submitBtn.AddCssClass("btn btn-primary");
            submitBtn.InnerHtml.SetContent(outputContent);

            var classes = output.Attributes.FirstOrDefault(a => a.Name == "class")?.Value;

            // set output
            output.Attributes.SetAttribute("class", $"form-group {classes}");
            output.TagName = "div"; // omit wrapper tag
            output.TagMode = TagMode.StartTagAndEndTag;

            // set content
            output.Content.SetHtmlContent(submitBtn);
        }
    }
}
