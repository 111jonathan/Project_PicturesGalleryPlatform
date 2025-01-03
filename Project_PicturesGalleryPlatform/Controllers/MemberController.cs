using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Project_PicturesGalleryPlatform.Models;
using System.Linq;

namespace Project_PicturesGalleryPlatform.Controllers
{
    public class MemberController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;

        public MemberController(IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        // 顯示會員資料
        public IActionResult Member()
        {
            // 尝试获取会员对象
            Member member = null;

            if (Request.Cookies.ContainsKey("UserAccount")) // 检查是否存在有效的 Cookie
            {
                string userAccount = Request.Cookies["UserAccount"]; // 获取 UserAccount 的值
                member = _dbContext.Members.FirstOrDefault(m => m.account == userAccount); // 从数据库查询对应的会员信息
            }

            if (member == null) // 若会员为空，传递一个空对象
            {
                member = new Member
                {
                    account = "", // 默认值为空字符串
                    name = "",
                    email = ""
                };
            }

            return View("Member", member); // 返回视图，无论是否找到会员都传递 Member 模型
        }


        // 進入修改會員資料頁面
        public IActionResult MemberModify()
        {

            if (Request.Cookies.ContainsKey("UserAccount"))
            {
                string userAccount = Request.Cookies["UserAccount"];
                var member = _dbContext.Members.FirstOrDefault(m => m.account == userAccount);

                if (member != null)
                {
                    return View("MemberModify", member);
                }
            }

            // 若無法找到會員或沒有有效的 Cookie，返回首頁或其他提示頁
            return RedirectToAction("Index", "Home");
        }

        // 儲存修改後的會員資料
        [HttpPost]
        public IActionResult SaveModifiedMember(Member modifiedMember)
        {
            if (ModelState.IsValid)
            {
                var member = _dbContext.Members.FirstOrDefault(m => m.account == modifiedMember.account);

                if (member != null)
                {
                    member.name = modifiedMember.name;
                    member.email = modifiedMember.email;

                    // 檢查密碼一致性
                    if (!string.IsNullOrEmpty(modifiedMember.password) &&
                        modifiedMember.password == modifiedMember.passwordConfirm)
                    {
                        member.password = modifiedMember.password;
                    }
                    else if (!string.IsNullOrEmpty(modifiedMember.password))
                    {
                        ModelState.AddModelError("passwordConfirm", "密碼與確認密碼不一致");
                        return View("MemberModify", modifiedMember);
                    }

                    _dbContext.SaveChanges();
                    
                    // 先前有登入, 先強制 Logout
                    if (Request.Cookies.ContainsKey("UserAccount"))
                                {
                                 Response.Cookies.Delete("UserAccount");
                                }
                    return RedirectToAction("Index", "Home");
                }
            }

            // 如果數據無效，返回修改頁面並提示錯誤
            return View("MemberModify", modifiedMember);
        }
    }
}
