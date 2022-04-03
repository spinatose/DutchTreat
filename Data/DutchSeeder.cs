using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _ctx;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(DutchContext ctx, 
            IWebHostEnvironment env,
            UserManager<StoreUser> userManager)
        {
            _ctx = ctx;
            _env = env;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _ctx.Database.EnsureCreated();

            StoreUser user = await _userManager.FindByEmailAsync("scot.pfuntner@radpartners.com");

            if (user == null)
            {
                user = new()
                {
                    FirstName = "scot",
                    LastName = "pfuntner",
                    Email = "scot.pfuntner@radpartners.com",
                    UserName = "scot.pfuntner@radpartners.com"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if (!result.Succeeded)
                    throw new InvalidOperationException("Could not create new user in seeder");
            }

            if (!_ctx.Products.Any())
            {
                // need to create sample data
                var filePth = Path.Combine(_env.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filePth);
                var prods = JsonSerializer.Deserialize<IEnumerable<Product>>(json);

                _ctx.Products.AddRange(prods);

                Order order = new()
                {
                    User = user, 
                    OrderDate = DateTime.Now,
                    OrderNumber = "100000",
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = prods.First(),
                            Quantity = 5,
                            UnitPrice = prods.First().Price,
                        }
                    }
                };

                _ctx.Orders.Add(order);

                _ctx.SaveChanges();
            }
        }
    }
}
