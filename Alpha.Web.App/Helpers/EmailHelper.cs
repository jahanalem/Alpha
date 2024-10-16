﻿using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Hosting;
using System.IO;


namespace Alpha.Web.App.Helpers
{
    public static class EmailHelper
    {
        public static string GetEmailTemplate(IWebHostEnvironment environment, string templateName)
        {
            var pathToFile = environment.WebRootPath
                             + Path.DirectorySeparatorChar
                             + Paths.Templates
                             + Path.DirectorySeparatorChar
                             + Paths.EmailTemplates
                             + Path.DirectorySeparatorChar
                             + templateName;

            using (StreamReader sourceReader = File.OpenText(pathToFile))
            {
                return sourceReader.ReadToEnd();
            }
        }

        //public static string GetImagesUrl(IConfiguration configuration, HttpRequest request)
        //{
        //    var uriBuilder = new UriBuilder
        //    {
        //        Host = configuration.GetSection("AppSettings")["ExternalHostName"],
        //        Scheme = request.Scheme,
        //        Path = Paths.Images
        //    };

        //    return uriBuilder.ToString();
        //}

        //public static string GetImagesUrl(IConfiguration configuration, string httpRequestScheme)
        //{
        //    var uriBuilder = new UriBuilder
        //    {
        //        Host = configuration.GetSection("AppSettings")["ExternalHostName"],
        //        Scheme = httpRequestScheme,
        //        Path = Paths.Images
        //    };

        //    return uriBuilder.ToString();
        //}
    }
}
