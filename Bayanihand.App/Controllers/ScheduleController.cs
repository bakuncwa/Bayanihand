using AutoMapper;
using Bayanihand.App.Models;
using Bayanihand.App.Models.Repositories;
using Bayanihand.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bayanihand.App.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly AppDbContext context;
        private readonly IScheduleRepository repo;
        private readonly IMapper mapper;

        public ScheduleController(AppDbContext context, IScheduleRepository repo, IMapper mapper)
        {
            this.context = context;
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(mapper.Map<List<ScheduleVM>>(await repo.GetAllSync()));
        }

        public IActionResult Add()
        {
            return View(new ScheduleVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ScheduleVM model)
        {
            if (ModelState.IsValid)
            {
                model.DateAdded = DateTime.Now;
                await repo.AddAsync(mapper.Map<Schedule>(model));
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Id)
        {
            var schedule = await repo.GetAsync(Id);
            if (schedule == null)
            {
                return RedirectToAction("Index");
            }

            await repo.DeleteAsync(Id);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null) RedirectToAction("Index");
            ScheduleVM schedule = mapper.Map<ScheduleVM>(await repo.GetAsync((int)Id));
            return View(schedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ScheduleVM model)
        {
            if (ModelState.IsValid)
            {
                model.DateAdded = DateTime.Now;
                model.DateUpdated = DateTime.Now;
                await repo.EditAsync(mapper.Map<Schedule>(model));
                return RedirectToAction("Index");

            }
            else
            {
                return View(model);
            }
        }
    }
}
