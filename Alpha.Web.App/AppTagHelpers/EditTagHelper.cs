using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Linq;

namespace Alpha.Web.App.AppTagHelpers
{
    [HtmlTargetElement("edit")]
    public class EditTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression aspFor { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator _generator { get; set; }

        public EditTagHelper(IHtmlGenerator generator)
        {
            _generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder instance = new TagBuilder("div");
            var propName = aspFor.ModelExplorer.Model.ToString();

            var modelExProp = aspFor.ModelExplorer.Container.Properties.SingleOrDefault(x => x.Metadata.PropertyName.Equals(propName));
            var propValue = modelExProp.Model;
            var propEditFormatString = modelExProp.Metadata.EditFormatString;

            var label = _generator.GenerateLabel(ViewContext, aspFor.ModelExplorer,
                propName, propName, new { @class = "col-md-2 control-label", @type = "email" });

            var typeOfProperty = propValue?.GetType();//aspFor.ModelExplorer.Metadata.ModelType;
            if (typeOfProperty == typeof(Boolean))
            {
                bool isChecked = propValue.ToString().ToLower() == "true";
                instance = _generator.GenerateCheckBox(ViewContext, aspFor.ModelExplorer, propName, isChecked, new { @class = "form-control" });
            }
            else
            {
                instance = _generator.GenerateTextBox(ViewContext, aspFor.ModelExplorer, propName, propValue, propEditFormatString, new { @class = "form-control" });
            }

            TagBuilder validation = _generator.GenerateValidationMessage(
                ViewContext,
                modelExProp, //Microsoft.AspNetCore.Mvc.ViewFeatures.ModelExplorer
                "Err-1", //string expression
                "Error", //string message
                "div",//string tag
                new { @class = "text-danger" }//object htmlAttributes
                );

            TagBuilder inputParent = new TagBuilder("div");
            inputParent.AddCssClass("col-md-10");
            inputParent.InnerHtml.AppendHtml(instance);
            //inputParent.InnerHtml.AppendHtml(validation);

            var parent = new TagBuilder("div");
            parent.AddCssClass("form-group");
            parent.InnerHtml.AppendHtml(label);
            parent.InnerHtml.AppendHtml(inputParent);

            output.Content.SetHtmlContent(parent);
            base.Process(context, output);
        }
    }
    /*
     <div class="row">
    <form asp-action="Edit" method="post">
        @{
            var propertyNames = (List<string>)ViewData["PropertyList"];
            var props = (List<PropertyInfo>)ViewData["props"];
            @*foreach (PropertyInfo item in props)
                {
                    <edit asp-for="@item"></edit>
                }*@
            foreach (var item in propertyNames)
            {
                <edit asp-for="@item"></edit>
            }
        }
        <div class="container-fluid align-content-lg-center">
            <button type="submit" class="btn btn-primary">Save</button>
            <a asp-action="Index" class="btn btn-secondary">Cancel</a>
        </div>

    </form>
</div>
     */
}
