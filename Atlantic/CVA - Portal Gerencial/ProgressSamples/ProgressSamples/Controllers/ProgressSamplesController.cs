using System.Web.Mvc;
using ProgressSamples;

namespace ProgressSamples.Controllers
{
  public class ProgressSamplesController : Controller
  {
    public ActionResult Progress01()
    {
      MusicGenre model = new MusicGenre();

      return View(model);
    }

    [HttpPost]
    public ActionResult Progress01(MusicGenre model)
    {
      System.Threading.Thread.Sleep(3000);

      return View(model);
    }

    public ActionResult Progress02()
    {
      MusicGenre model = new MusicGenre();

      return View(model);
    }

    [HttpPost]
    public ActionResult Progress02(MusicGenre model)
    {
      System.Threading.Thread.Sleep(3000);

      return View(model);
    }

    public ActionResult Progress03()
    {
      MusicGenre model = new MusicGenre();

      return View(model);
    }

    [HttpPost]
    public ActionResult Progress03(MusicGenre model)
    {
      System.Threading.Thread.Sleep(3000);

      return View(model);
    }

    public ActionResult Progress04()
    {
      MusicGenre model = new MusicGenre();

      return View(model);
    }

    [HttpPost]
    public ActionResult Progress04(MusicGenre model)
    {
      System.Threading.Thread.Sleep(3000);

      return View(model);
    }
  }
}