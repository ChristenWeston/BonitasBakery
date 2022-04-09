using Microsoft.AspNetCore.Mvc;
using Bakery.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using System;
using System.Web;
using System.Net;

namespace Bakery.Controllers
{
  public class TreatsController : Controller
  {
    private readonly BakeryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public TreatsController(UserManager<ApplicationUser> UserManager, BakeryContext db)
    {
      _userManager = UserManager;
      _db = db;
    }

    public ActionResult Index()
    {
      List<Treat> model = _db.Treats.OrderBy(treat => treat.Name).ToList();
      return View(model);
    }

    [Authorize]
    public async Task<ActionResult> Create()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      ViewBag.FlavorId = new SelectList(_db.Flavors.OrderBy(flavor => flavor.Name), "FlavorId", "Name");
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Treat treat, int FlavorId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);   
      bool duplicateTreat = _db.Treats.Any(theTreat => theTreat.Name == treat.Name);

      if (treat.Name != null)
      {
        if (duplicateTreat)
        {
          ViewBag.SuccessMessage = "This Treat already exists";
          ViewBag.FlavorId = new SelectList(_db.Flavors.OrderBy(flavor => flavor.Name), "FlavorId", "Name");
          return View();
        }
        else 
        {
          ViewBag.SuccessMessage = "Not Duplicate";
          _db.Treats.Add(treat);
          _db.SaveChanges();
        }
        if (FlavorId != 0)
        {
          _db.FlavorTreats.Add(new FlavorTreat() { FlavorId = FlavorId, TreatId = treat.TreatId});
          _db.SaveChanges();
        }
      }
      return RedirectToAction("Index");
    }

    public async Task<ActionResult> Details(int id)
    {

      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      var thisTreat = _db.Treats
        .Include(treat => treat.JoinEntities)
        .ThenInclude(join => join.Flavor)
        .FirstOrDefault(treat => treat.TreatId == id);
      ViewBag.FlavorsTreats = _db.FlavorTreats.Where(entry => entry.TreatId == id).ToList();
      return View(thisTreat);
    }

    [Authorize]
    public ActionResult Edit(int id)
    {
      var thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(Treat treat, int FlavorId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);

      _db.Entry(treat).State = EntityState.Modified;
      _db.SaveChanges();
      bool duplicate = _db.FlavorTreats.Any(join => join.FlavorId == FlavorId && join.TreatId == treat.TreatId);
      if (FlavorId !=0 && !duplicate)
      {
        _db.FlavorTreats.Add(new FlavorTreat() { FlavorId = FlavorId, TreatId = treat.TreatId});
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [Authorize]
    public ActionResult Delete(int id)
    {
      var thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisTreat= _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      _db.Treats.Remove(thisTreat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> DeleteFlavor(int joinId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      ViewBag.PageTitle = "Remove this Flavor from this Treat";
      var joinEntry = _db.FlavorTreats.FirstOrDefault(entry => entry.FlavorTreatId == joinId);
      // if (joinEntry.User.Id == userId)
      // {
        _db.FlavorTreats.Remove(joinEntry);
        _db.SaveChanges();
      // }
      // else
      // {
      //   Console.WriteLine("You don't have authorization to delete this.");
      // }
      return RedirectToAction("Index");
    }
  }
}