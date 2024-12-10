using AutoMapper;
using Bayanihand.App.Models;
using Bayanihand.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Bayanihand.App.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper mapper;

        public CartController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        [HttpPost]
        public IActionResult CheckoutResult()
        {
            try
            {
                TempData["CheckoutMessage"] = "success";
            }
            catch (Exception ex)
            {
                TempData["CheckoutMessage"] = "error";
            }

            return RedirectToAction("CheckoutConfirmation");
        }

        public IActionResult CheckoutConfirmation()
        {
            return View();
        }

        public IActionResult Index()
        {
            var cart = GetOrCreateCart();
            foreach (var item in cart.Items)
            {
                if (item.Schedule != null)
                {
                    Console.WriteLine(item.Schedule.Id);
                }
            }

            return View(cart);
        }

        [HttpPost]
        public IActionResult AddServiceToCart(int serviceId, int scheduleId)
        {
            var cart = GetOrCreateCart();
            var service = _context.Services.Find(serviceId);

            if (service == null)
            {
                return NotFound();
            }

            var scheduleInCart = _context.CartCommissions
                .Any(cc => cc.ScheduleId == scheduleId && cc.CartId == cart.Id);

            if (scheduleInCart)
            {
                TempData["Error"] = "This schedule has already been selected and cannot be chosen again.";
                return RedirectToAction("Index", "Service");
            }

            var cartItem = new CartCommission
            {
                Service = service,
                WageRate = service.WageRate,
                ServiceCharge = service.ServiceCharge,
                TotalPrice = service.TotalPrice,
                PaymentOption = service.PaymentOption,
                CartId = cart.Id 
            };

            cart.Items.Add(cartItem);
            _context.SaveChanges();

            return RedirectToAction("SelectSchedule", new { cartItemId = cartItem.Id });
        }

        [HttpGet]
        public IActionResult SelectSchedule(int cartItemId)
        {
            var cartItem = _context.Carts
                .SelectMany(c => c.Items)
                .Include(ci => ci.Service)
                .FirstOrDefault(ci => ci.Id == cartItemId);

            if (cartItem == null)
            {
                return NotFound();
            }

            ViewBag.Schedules = _context.Schedules.ToList();

            return View(cartItem);
        }

        [HttpPost]
        public IActionResult SelectSchedule(int cartItemId, int scheduleId)
        {
            var cartItem = _context.Carts
                .SelectMany(c => c.Items)
                .Include(ci => ci.Service)
                .FirstOrDefault(ci => ci.Id == cartItemId);

            if (cartItem == null)
            {
                return NotFound();
            }

            var schedule = _context.Schedules.Find(scheduleId);
            if (schedule == null)
            {
                return NotFound("Schedule not found.");
            }

            cartItem.ScheduleId = scheduleId;
            cartItem.Schedule = schedule;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving the cart item: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

            return RedirectToAction("Index");
        }

        private Cart GetOrCreateCart()
        {
            try
            {
                var userPhoneNumber = User.Identity.Name; 

                var cart = _context.Carts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Service)
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Schedule)
                    .FirstOrDefault(c => c.PhoneNumber == userPhoneNumber); 

                if (cart == null)
                {
                    cart = new Cart
                    {
                        PhoneNumber = userPhoneNumber 
                    };
                    _context.Carts.Add(cart);
                    _context.SaveChanges();
                }

                return cart;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        public IActionResult History()
        {
            var userPhoneNumber = User.Identity.Name;

            var cart = _context.Carts
                .Include(c => c.Items)  
                .ThenInclude(i => i.Service) 
                .Include(c => c.Items) 
                .ThenInclude(i => i.Schedule)
                .FirstOrDefault(c => c.PhoneNumber == userPhoneNumber);

            if (cart == null)
            {
                return View(new List<HistoryVM>());
            }

            List<HistoryVM> list = cart.Items.Select(i => new HistoryVM
            {
                WageRate = i.WageRate,  
                ServiceCharge = i.ServiceCharge,  
                MaterialCost = i.MaterialCost, 
                TotalPrice = i.TotalPrice,  
                PaymentOption = i.PaymentOption, 
                ScheduleDetails = i.Schedule != null
                    ? $"{i.Schedule.MeetUpLocation} - {i.Schedule.MeetUpDay} - {i.Schedule.MeetUpTime}"
                    : "No Schedule", 
                Visit = i.Schedule?.Visit ?? "No Handyman Name",  
                HandymanPhoneNo = i.Schedule?.HandymanPhoneNo ?? "No Phone Number",  
                ServiceDetails = i.Service != null
                    ? $"{i.Service.ServiceName}" : "No Service"  
            }).ToList();

            return View(list); 
        }

        [HttpGet]
        public IActionResult Booked()
        {
            var userPhoneNumber = User.Identity.Name;

            var cart = _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Service)
                .Include(c => c.Items)
                .ThenInclude(i => i.Schedule)
                .FirstOrDefault(c => c.PhoneNumber == userPhoneNumber);

            if (cart == null)
            {
                return View(new List<HistoryVM>());
            }

            List<HistoryVM> list = cart.Items.Select(i => new HistoryVM
            {
                WageRate = i.WageRate,
                ServiceCharge = i.ServiceCharge,
                MaterialCost = i.MaterialCost,
                TotalPrice = i.TotalPrice,
                PaymentOption = i.PaymentOption,
                ScheduleDetails = i.Schedule != null
                    ? $"{i.Schedule.MeetUpLocation} - {i.Schedule.MeetUpDay} - {i.Schedule.MeetUpTime}"
                    : "No Schedule",
                Visit = i.Schedule?.Visit ?? "No Handyman Name",
                HandymanPhoneNo = i.Schedule?.HandymanPhoneNo ?? "No Phone Number",
                ServiceDetails = i.Service != null
                    ? $"{i.Service.ServiceName}" : "No Service"
            }).ToList();

            return View(list);
        }

        [HttpPost]
        public IActionResult AddRating(HistoryVM model)
        {
            if (ModelState.IsValid)
            {
                var cartCommissions = _context.CartCommissions
                    .Include(c => c.Schedule)
                    .Include(c => c.Service)
                    .ToList();

                var rating = new Rating
                {
                    CartCommissionId = model.CartCommissionId,
                    RatingValue = model.RatingValue,
                    Comment = model.Comment
                };

                _context.Ratings.Add(rating);
                _context.SaveChanges();

                List<HistoryVM> list = cartCommissions.Select(c => new HistoryVM
                {
                    CartCommissionId = c.Id,
                    WageRate = c.WageRate,
                    ServiceCharge = c.ServiceCharge,
                    MaterialCost = c.MaterialCost,
                    TotalPrice = c.TotalPrice,
                    PaymentOption = c.PaymentOption,
                    ScheduleDetails = c.Schedule != null
                        ? $"{c.Schedule.MeetUpLocation} - {c.Schedule.MeetUpDay} - {c.Schedule.MeetUpTime}"
                        : "No Schedule",
                    Visit = c.Schedule?.Visit ?? "No Handyman Name",
                    HandymanPhoneNo = c.Schedule?.HandymanPhoneNo ?? "No Phone Number",
                    ServiceDetails = c.Service != null
                        ? $"{c.Service.ServiceName}" : "No Service"
                }).ToList();

                TempData["SuccessMessage"] = "Rating and comment added successfully!";

                return View("History", list);
            }

            TempData["ErrorMessage"] = "Failed to add rating or comment.";
            return RedirectToAction("History");
        }

        private bool IsScheduleAlreadySelected(Cart cart, int scheduleId)
        {
            return cart.Items.Any(item => item.Schedule.Id == scheduleId);
        }
    }
}

