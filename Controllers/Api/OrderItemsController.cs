using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Controllers.Api
{
    [Route("api/orders/{orderid}/items")]
    [ApiController]
    [Produces("application/json")]
    //[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class OrderItemsController : ControllerBase
    {
        private readonly IDutchAdapter _adapater;
        private readonly ILogger<OrderItemsController> _logger;
        private readonly IMapper _mapper;

        public OrderItemsController(IDutchAdapter adapater, 
            ILogger<OrderItemsController> logger,
            IMapper mapper)
        {
            _adapater = adapater;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<OrderItemViewModel>> Get(int orderId)
        {
            try
            {
                var order = _adapater.GetOrderById(User.Identity.Name, orderId);
                if (order != null)
                    return Ok(_mapper.Map<IEnumerable<OrderItemViewModel>>(order.Items));

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get orderItems: {ex}");
                return BadRequest("Failed to get orderItems");
            }
        }


        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<OrderItemViewModel> Get(int orderId, int id)
        {
            try
            {
                var order = _adapater.GetOrderById(User.Identity.Name, orderId);
                if (order != null)
                {
                    var item = order.Items.Where(i => i.Id == id).FirstOrDefault();

                    if (item != null)
                        return Ok(_mapper.Map<OrderItemViewModel>(item));
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get orderItems: {ex}");
                return BadRequest("Failed to get orderItems");
            }
        }
    }
}
