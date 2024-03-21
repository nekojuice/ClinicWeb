using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Controllers
{
    public class FHealthInformationController : Controller
    {
        // 主頁-->預計會有分類的文章清單：婦科、產科、兒科
        public IActionResult FHealthInformation()
        {
            return View();
        }

        // 懷孕藥品分級表，樣式還沒改好，很醜

        //GET:FHealthInformation/PregnancyCategory
        public IActionResult PregnancyCategory() { return View(); }

        //----------------------------婦科----------------------------

        //子宮頸抺片檢查

        //GET:FHealthInformation/Blog2
        public IActionResult Blog2() { return View(); }

        //乳房超音波檢查

        //GET:FHealthInformation/Blog3
        public IActionResult Blog3() { return View(); }

        //----------------------------產科----------------------------

        // 孕吐

        //GET:FHealthInformation/Blog1
        public IActionResult Blog1() { return View(); }

        //高層次超音波檢查

        //GET:FHealthInformation/Blog4
        public IActionResult Blog4() { return View(); }

        //----------------------------兒科----------------------------

        //新生兒黃疸

        //GET:FHealthInformation/Blog5
        public IActionResult Blog5() { return View(); }

        //新生兒感覺神經性聽損基因篩檢

        //GET:FHealthInformation/Blog6
        public IActionResult Blog6() { return View(); }

    }
}
