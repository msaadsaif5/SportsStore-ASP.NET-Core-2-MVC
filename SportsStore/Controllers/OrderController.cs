﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository orderRepository;
        private Cart cart;

        public OrderController(IOrderRepository orderRepository, Cart cart)
        {
            this.orderRepository = orderRepository;
            this.cart = cart;
        }

        public ViewResult Checkout()
        {
            return View(new Order());
        }

        public ViewResult List() => View(orderRepository.Orders.Where(o => !o.Shipped));

        [HttpPost]
        public IActionResult MarkShipped(int orderID)
        {
            Order order = orderRepository.Orders.FirstOrDefault(o => o.OrderID == orderID);
            if (order != null)
            {
                order.Shipped = true;
                orderRepository.SaveOrder(order);
            }
            return RedirectToAction(nameof(List));
        }


        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (cart.Lines.Count() == 0)
                ModelState.AddModelError("", "Sorry, your cart is empty!");

            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                orderRepository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
                return View(order);
        }

        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }
    }
}