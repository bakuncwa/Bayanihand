using AutoMapper;
using Bayanihand.App.Models;
using Bayanihand.App.Models.Repositories;
using Bayanihand.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bayanihand.App.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IServiceRepository repo;
        private readonly IMapper mapper;
        private readonly AppDbContext context;
        public ServiceController(IServiceRepository repo, IMapper mapper, AppDbContext context)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(mapper.Map<List<ServiceVM>>(await repo.GetAllSync()));
        }


        public async Task<IActionResult> Manage()
        {
            return View(mapper.Map<List<ServiceVM>>(await repo.GetAllSync()));
        }

        public IActionResult Add()
        {
            return View(new ServiceVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ServiceVM model)
        {
            Service entity = mapper.Map<Service>(model);

            if (ModelState.IsValid)
            {
                await repo.AddAsync(entity);
                return RedirectToAction(nameof(Manage));
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Id)
        {
            var product = await repo.GetAsync(Id);
            if (product == null)
            {
                return RedirectToAction("Manage");
            }

            await repo.DeleteAsync(Id);

            return RedirectToAction("Manage");
        }

        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null) RedirectToAction("Manage");
            ServiceVM product = mapper.Map<ServiceVM>(await repo.GetAsync((int)Id));
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServiceVM model)
        {
            if (ModelState.IsValid)
            {
                await repo.EditAsync(mapper.Map<Service>(model));
                return RedirectToAction("Manage");
            }
            else
            {
                return View(model);
            }
        }
    }
}
