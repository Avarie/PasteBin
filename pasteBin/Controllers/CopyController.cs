using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using pasteBin.Models;
using pasteBin.Repositories;

namespace pasteBin.Controllers
{
    public class CopyController : Controller
    {
        private readonly ItemRepository _itemRepository;

        public CopyController()
        {
            _itemRepository = new ItemRepository(new db());
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Main()
        {
            return View();
        }

        public JsonResult Publish(string html, string image)
        {
            var item = _itemRepository.Add(new Item
            {
                Text = html,
                Ip = Request.UserHostAddress,
                Image = image,
                Description = String.Empty // reserved
            });

            return Json(item);
        }

        public ActionResult Paste(string id)
        {
            var item = _itemRepository.GetAll().FirstOrDefault(x => x.Id == id);
            if (item == null) return View("Paste", "_NoLayout");

            ViewBag.IsHtml = false;
            ViewBag.Text = item.Text;

            var htmlTags = new List<string> { "<html>", "<body>", "<head>", "</html>", "</body>", "</head>" };
            if (!string.IsNullOrEmpty(item.Text) && htmlTags.All(x => item.Text.Contains(x)))
            {
                ViewBag.IsHtml = true;
                return View("Paste", "_TrueHtml");
            }

            ViewBag.Image = item.Image;

            return View("Paste", "_NoLayout");
        }

    }
}
