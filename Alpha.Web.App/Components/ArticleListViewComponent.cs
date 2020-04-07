﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.Infrastructure.PaginationUtility;
using Alpha.Services;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace Alpha.Web.App.Components
{
    public class ArticleListViewComponent : ViewComponent
    {
        private readonly IArticleService _articleService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int pageNumber = 1;
        private int? tagId = null;
        private string niceUrl = string.Empty;
        public ArticleListViewComponent(IArticleService articleService, IHttpContextAccessor httpContextAccessor)
        {
            _articleService = articleService;
            _httpContextAccessor = httpContextAccessor;
            //var result = 1;
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                string queryString = (_httpContextAccessor.HttpContext.Request.Query[QueryStringParameters.PageNumber]);
                string tId = (_httpContextAccessor.HttpContext.Request.Query[QueryStringParameters.TagId]);
                if (queryString == null && tId == null)
                {
                    var routeValues = _httpContextAccessor.HttpContext.Request.RouteValues;
                    if (routeValues["tagId"] != null)
                    {
                        tagId = int.Parse(routeValues["tagId"].ToString());
                    }
                    if (routeValues["pageNumber"] != null)
                    {
                        pageNumber = int.Parse(routeValues["pageNumber"].ToString());
                    }
                }
                if (queryString != null)
                {
                    Int32.TryParse(queryString, out pageNumber);
                }

                if (tId != null)
                {
                    tagId = int.Parse(tId);
                }
                ViewBag.PageNumber = pageNumber;
            }
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var key = $"TotalItems-TagId-{tagId}";
            if (TempData[key] == null)
            {
                TempData[key] = await _articleService.FilterByTag(tagId).CountAsync();
            }

            var result = await _articleService.FilterByTagAsync(tagId, pageNumber);
            var x = Url.Action(niceUrl);
            result.Pagination.Init(new Pagination
            {
                PagingInfo = new PagingInfo
                {
                    TotalItems = int.Parse(TempData[key].ToString()),
                    ItemsPerPage = PagingInfo.DefaultItemsPerPage,
                    CurrentPage = pageNumber
                },
                Url = Url.Action(action: "Index", controller: "Article", new { tagId = tagId, pageNumber = pageNumber })
            });
            return View(result);
        }
    }
}
