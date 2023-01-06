﻿using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController :ControllerBase
    {
        private readonly IBasketRepository _repository;

        public BasketController(IBasketRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("{UserName}",Name ="GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string username)
        {
            var basket = await _repository.GetBasket(username);
            return Ok( basket ?? new ShoppingCart(username));
        }
        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart),(int) HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            var result = await _repository.UpdateBasket(basket);
            return Ok(result);

        }
        [HttpDelete("{Username}",Name ="DeleteBasket")]
        [ProducesResponseType(typeof(void),(int) HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteBasket(string Username)
        {
           await _repository.DeleteBasket(Username);
            return Ok();
        }

    }
}