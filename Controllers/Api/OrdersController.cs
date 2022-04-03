using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Controllers.Api
{
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    //[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : ControllerBase
    {
        private readonly IDutchAdapter _adapater;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<StoreUser> _userManager;

        public OrdersController(IDutchAdapter adapater, 
            ILogger<OrdersController> logger,
            IMapper mapper,
            UserManager<StoreUser> userManager)
        {
            _adapater = adapater;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<OrderViewModel>> Get(bool includeItems = true)
        {
            try
            {
                var username = User.Identity.Name;

                var result = _adapater.GetAllOrdersByUser(username, includeItems);
                return Ok(_mapper.Map<IEnumerable<OrderViewModel>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get orders: {ex}");
                return BadRequest("Failed to get orders");
            }
        }
        
        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<OrderViewModel> Get(int id)
        {
            try
            {
                var order = _adapater.GetOrderById(User.Identity.Name, id);
                if (order != null)
                    return Ok(_mapper.Map<Order, OrderViewModel>(order));
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get order: {ex}");
                return BadRequest("Failed to get order");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Order neworder = _mapper.Map<Order>(model);

                    if (neworder.OrderDate == DateTime.MinValue)
                        neworder.OrderDate = DateTime.Now;

                    var currUser = await _userManager.FindByNameAsync(User.Identity.Name);
                    neworder.User = currUser;

                    _adapater.AddEntity(neworder);

                    if (_adapater.SaveAll())
                        return Created($"/api/orders/{neworder.Id}", _mapper.Map<OrderViewModel>(neworder));
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save an order: {ex}");
            }

            return BadRequest("Failed to save new order");
        }
    }
}
