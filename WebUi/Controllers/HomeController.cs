using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using WebUi.Models;

namespace WebUi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOrderRepository _repoOrder;
        private readonly IOrderItemRepository _repoOrderItem;
        private readonly ILogger<HomeController> _logger;
        private readonly IMemoryCache _cache;

        public HomeController(ILogger<HomeController> logger,
            IMemoryCache cache,
            IOrderRepository repoOrder,
            IOrderItemRepository repoOrderItem)
        {
            _logger = logger;
            _cache = cache;
            _repoOrder = repoOrder;
            _repoOrderItem = repoOrderItem;
        }

        public IActionResult Index()
        {
            var d = LoadOrder(2);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public Order LoadOrder(int id)
        {
            if (!_cache.TryGetValue(id, out Order model))
            {
                model = _repoOrder.Get(id);
                if (model != null)
                {
                    _cache.Set(model.Id, model,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return model;
        }
    }
}
