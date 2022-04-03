using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchAdapter : IDutchAdapter
    {
        private readonly DutchContext _ctx;
        private readonly ILogger<DutchAdapter> _logger;

        public DutchAdapter(DutchContext ctx, ILogger<DutchAdapter> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public void AddEntity(object model)
        {
            try
            {
                _ctx.Add(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add entity to db: {ex}");
            }
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems = true)
        {
            _logger.LogInformation("GetAllOrders was called");

            try
            {
                var result = (includeItems) ? 
                    _ctx.Orders.Include(o => o.Items).ThenInclude(i => i.Product).OrderBy(o => o.OrderDate).ToList()
                    : _ctx.Orders.OrderBy(o => o.OrderDate).ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all orders: {ex}");
                return null;
            }
        }

        public IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems = true)
        {
            _logger.LogInformation("GetAllOrdersByUser was called");

            try
            {
                var result = (includeItems) ?
                    _ctx.Orders.Where(o => o.User.UserName == username).Include(o => o.Items).ThenInclude(i => i.Product).OrderBy(o => o.OrderDate).ToList()
                    : _ctx.Orders.Where(o => o.User.UserName == username).OrderBy(o => o.OrderDate).ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all orders: {ex}");
                return null;
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            _logger.LogInformation("GetAllProducts was called");
    
            try
            {
                return _ctx.Products.OrderBy(p => p.Title).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all products: {ex}");
                return null;
            }
        }

        public Order GetOrderById(string username, int id)
        {
            _logger.LogInformation($"GetOrderByID was called with id: {id}");

            try
            {
                return _ctx.Orders.Where(o => o.Id == id && o.User.UserName == username).Include(o => o.Items).ThenInclude(i => i.Product).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get order by id {id}: {ex}");
                return null;
            }
        }

        public IEnumerable<Product> GetProductsByCategory(string category) =>
            _ctx.Products
                .Where(p => p.Category == category)
                .OrderBy(p => p.Title).ToList();

        public bool SaveAll() => _ctx.SaveChanges() > 0;
    }
}
