using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace Alpha.Infrastructure
{
    public class BaseModelBinder<TModel> : IModelBinder where TModel : class, new()
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            throw new NotImplementedException();
        }

        public virtual object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            TModel model = (TModel)bindingContext.Model ?? new TModel();
            //ICollection<string> propertyNames = bindingContext.PropertyMetadata.Keys;
            //foreach (var propertyName in propertyNames)
            //{
            //string value = GetValue(bindingContext, propertyName);
            //value = HttpUtility.HtmlDecode(value);
            //if (bindingContext.PropertyMetadata[propertyName].ModelType.Name == "Boolean")
            //{
            //    if (value.ToLower() == "true,false" || value.ToLower() == "false,true")
            //        value = HttpContext.Current.Request.Form.GetValues(propertyName).First();
            //}
            //model.SetPropertyValue(propertyName, value);
            // }
            return model;
        }

        //private string GetValue(ModelBindingContext context, string name)
        //{
        //    name = (context.ModelName == "" ? "" : context.ModelName + ".") + name;

        //    ValueProviderResult result = context.ValueProvider.GetValue(name);
        //    if (result == null || result.AttemptedValue == "")
        //    {
        //        return null;
        //    }
        //    return result.AttemptedValue;
        //}
    }
}
