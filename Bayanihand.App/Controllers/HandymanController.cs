using AutoMapper;
using Bayanihand.App.Models;
using Bayanihand.App.Models.Repositories;
using Bayanihand.DataModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bayanihand.App.Controllers
{
    public class HandymanController : Controller
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userMgr;
        private readonly IHandymanRepository handymanRepo;
        public HandymanController(AppDbContext context, IMapper mapper, UserManager<IdentityUser> userMgr, IHandymanRepository handymanRepo)
        {
            this.context = context;
            this.mapper = mapper;
            this.userMgr = userMgr;
            this.handymanRepo = handymanRepo;
        }

        public async Task<IActionResult> Profile()
        {
            var user = await userMgr.GetUserAsync(User);

            if (user == null || string.IsNullOrEmpty(user.PhoneNumber))
            {
                return RedirectToAction("SignIn", "Account");
            }

            var handyman = context.Handymen.FirstOrDefault(h => h.PhoneNo == user.PhoneNumber);

            if (handyman == null)
            {
                return RedirectToAction("CreateProfile");
            }

            var handymanVM = mapper.Map<HandymanVM>(handyman);

            return View(handymanVM);
        }

        public async Task<IActionResult> ViewProfile(string phoneNo)
        {
            var handyman = context.Handymen.FirstOrDefault(h => h.PhoneNo == phoneNo);

            var handymanVM = mapper.Map<HandymanVM>(handyman);

            return View(handymanVM);
        }
        public IActionResult CreateProfile()
        {
            return View(new HandymanVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProfile(HandymanVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var handyman = mapper.Map<Handyman>(model);
                    await handymanRepo.AddAsync(handyman);

                    TempData["SuccessMessage"] = "Profile created successfully.";

                    return RedirectToAction("Profile");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await userMgr.GetUserAsync(User);

            if (user == null || string.IsNullOrEmpty(user.PhoneNumber))
            {
                return RedirectToAction("SignIn", "Account");
            }

            var handyman = context.Handymen.FirstOrDefault(h => h.PhoneNo == user.PhoneNumber);

            if (handyman == null)
            {
                return RedirectToAction("CreateProfile");
            }

            var handymanVM = mapper.Map<HandymanVM>(handyman);

            return View(handymanVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(HandymanVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userMgr.GetUserAsync(User);

            if (user == null || string.IsNullOrEmpty(user.PhoneNumber))
            {
                return RedirectToAction("SignIn", "Account");
            }

            var handyman = context.Handymen.FirstOrDefault(h => h.PhoneNo == user.PhoneNumber);

            if (handyman == null)
            {
                return RedirectToAction("CreateProfile");
            }

            handyman.FirstName = model.FirstName;
            handyman.LastName = model.LastName;
            handyman.Address = model.Address;
            handyman.City = model.City;
            handyman.Region = model.Region;
            handyman.ZIPCode = model.ZIPCode;
            handyman.BarangayNo = model.BarangayNo;
            handyman.BarangayName = model.BarangayName;
            handyman.ServiceCharge = model.ServiceCharge;
            handyman.YearsOfExperience = model.YearsOfExperience;

            context.Handymen.Update(handyman);
            await context.SaveChangesAsync();

            return RedirectToAction("Profile");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Id)
        {
            var handyman = await context.Handymen.FindAsync(Id);
            context.Set<Handyman>().Remove(handyman);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null) return RedirectToAction("Index");
            HandymanVM seller = mapper.Map<HandymanVM>(await context.Handymen.FindAsync(Id));

            return View(seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HandymanVM model)
        {
            context.Set<Handyman>().Update(mapper.Map<Handyman>(model));
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
