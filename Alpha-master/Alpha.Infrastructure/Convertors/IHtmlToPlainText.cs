using System;
using System.Collections.Generic;
using System.Text;

namespace Alpha.Infrastructure.Convertors
{
    public interface IHtmlToPlainText
    {
        string ConvertHtmlToPlainText(string html);
    }
}
