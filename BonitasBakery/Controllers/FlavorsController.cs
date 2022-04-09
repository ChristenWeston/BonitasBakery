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
  public class FlavorsController : Controller
  {
    private readonly BakeryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public FlavorsController(UserManager<ApplicationUser> UserManager, BakeryContext db)
    {
      _userManager = UserManager;
      _db = db;
    }

    public ActionResult Index()
    {
      List<Flavor> model = _db.Flavors.OrderBy(flavor => flavor.Name).ToList();
      return View(model);
    }

    [Authorize]
    public async Task<ActionResult> Create()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      ViewBag.TreatId = new SelectList(_db.Treats.OrderBy(treat => treat.Name), "TreatId", "Name");
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Flavor flavor, int TreatId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);   
      bool duplicateFlavor = _db.Flavors.Any(theFlavor => theFlavor.Name == flavor.Name);

      if (flavor.Name != null)
      {
        if (duplicateFlavor)
        {
          ViewBag.SuccessMessage = "This Flavor already exists";
          ViewBag.TreatId = new SelectList(_db.Treats.OrderBy(treat => treat.Name), "TreatId", "Name");
          return View();
        }
        else 
        {
          ViewBag.SuccessMessage = "Not Duplicate";
          _db.Flavors.Add(flavor);
          _db.SaveChanges();
        }
        if (TreatId != 0)
        {
          _db.FlavorTreats.Add(new FlavorTreat() { TreatId = TreatId, FlavorId = flavor.FlavorId});
          _db.SaveChanges();
        }
      }
      return RedirectToAction("Index");
    }

    public async Task<ActionResult> Details(int id)
    {

      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      var thisFlavor = _db.Flavors
        .Include(flavor => flavor.JoinEntities)
        .ThenInclude(join => join.Treat)
        .FirstOrDefault(flavor => flavor.FlavorId == id);
      ViewBag.FlavorsTreats = _db.FlavorTreats.ToList();
      return View(thisFlavor);
    }

    [Authorize]
    public ActionResult Edit(int id)
    {
      var thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      return View(thisFlavor);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(Flavor flavor, int TreatId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);

      _db.Entry(flavor).State = EntityState.Modified;
      _db.SaveChanges();
      bool duplicate = _db.FlavorTreats.Any(join => join.TreatId == TreatId && join.FlavorId == flavor.FlavorId);
      if (TreatId !=0 && !duplicate)
      {
        _db.FlavorTreats.Add(new FlavorTreat() { TreatId = TreatId, FlavorId = flavor.FlavorId});
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [Authorize]
    public ActionResult Delete(int id)
    {
      var thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      return View(thisFlavor);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      _db.Flavors.Remove(thisFlavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> DeleteTreat(int joinId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      ViewBag.PageTitle = "Remove this Treat from this Flavor";
      var joinEntry = _db.FlavorTreats.FirstOrDefault(entry => entry.FlavorTreatId == joinId);
      if (joinEntry.User.Id == userId)
      {
        _db.FlavorTreats.Remove(joinEntry);
        _db.SaveChanges();
      }
      else
      {
        Console.WriteLine("You don't have authorization to delete this.");
      }
      return RedirectToAction("Index");
    }
  }
}