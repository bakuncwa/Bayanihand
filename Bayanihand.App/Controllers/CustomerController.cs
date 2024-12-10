using AutoMapper;
using Bayanihand.App.Models;
using Bayanihand.DataModel;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Bayanihand.App.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        public CustomerController(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            List<CustomerVM> list = mapper.Map<List<CustomerVM>>(context.Customers.ToList());
            return View(list);
        }

        public IActionResult Add()
        {
            return View(new CustomerVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CustomerVM model)
        {
            Customer entity = mapper.Map<Customer>(model);
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Id)
        {
            var customer = await context.Customers.FindAsync(Id);
            context.Set<Customer>().Remove(customer);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null) return RedirectToAction("Index");
            CustomerVM buyer = mapper.Map<CustomerVM>(await context.Customers.FindAsync(Id));
            return View(buyer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerVM model)
        {
            context.Set<Customer>().Update(mapper.Map<Customer>(model));
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
