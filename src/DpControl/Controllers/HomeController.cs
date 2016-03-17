using DpControl.Models;
using DpControl.Utility;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Controllers
{
    public class HomeController:Controller
    {
        public IActionResult Default()
        {
           
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            //Admin Menus
            #region leveal1
            Menu admin_CustomerInfo = new Menu();
            admin_CustomerInfo.MenuName = "客户信息";
            admin_CustomerInfo.MenuUrl = "";
            admin_CustomerInfo.Order = 1;


            #region leveal2
            Menu admin_CustomerInfoSecond = new Menu();
            admin_CustomerInfoSecond.MenuName = "客户信息";
            admin_CustomerInfoSecond.MenuUrl = "";
            admin_CustomerInfoSecond.Order = 1;

            Menu admin_CustomerInfoSupply = new Menu();
            admin_CustomerInfoSupply.MenuName = "供应商信息";
            admin_CustomerInfoSupply.MenuUrl = "/Home/Error";
            admin_CustomerInfoSupply.Order = 2;

            #region leveal3
            Menu admin_CustomerInfo_SearchCustomer = new Menu();
            admin_CustomerInfo_SearchCustomer.MenuName = "查看客户";
            admin_CustomerInfo_SearchCustomer.MenuUrl = "/Home/Error";
            admin_CustomerInfo_SearchCustomer.Order = 1;

            Menu admin_CustomerInfo_SearchControl = new Menu();
            admin_CustomerInfo_SearchControl.MenuName = "查看电机";
            admin_CustomerInfo_SearchControl.MenuUrl = "/Home/Error";
            admin_CustomerInfo_SearchControl.Order = 2;

            List<Menu> leveal3Menus = new List<Menu>();
            leveal3Menus.Add(admin_CustomerInfo_SearchCustomer);
            leveal3Menus.Add(admin_CustomerInfo_SearchControl);


            #endregion
            admin_CustomerInfoSecond.SecondaryMenus = leveal3Menus;


            List<Menu> leveal2Menus = new List<Menu>();
            leveal2Menus.Add(admin_CustomerInfoSecond);
            leveal2Menus.Add(admin_CustomerInfoSupply);
            #endregion
            admin_CustomerInfo.SecondaryMenus = leveal2Menus;
            #endregion

            #region leveal1
            Menu admin_AccountInfo = new Menu();
            admin_AccountInfo.MenuName = "账号信息";
            admin_AccountInfo.MenuUrl = "";
            admin_AccountInfo.Order = 2;

            #region leveal2
            Menu admin_AccountInfo_manage = new Menu();
            admin_AccountInfo_manage.MenuName = "分配账号";
            admin_AccountInfo_manage.MenuUrl = "/Home/Error";
            admin_AccountInfo_manage.Order = 1;

            Menu admin_AccountInfo_role = new Menu();
            admin_AccountInfo_role.MenuName = "Role Management";
            admin_AccountInfo_role.MenuUrl = "/Manage/IndexForRole";
            admin_AccountInfo_role.Order = 2;

            Menu admin_AccountInfo_user = new Menu();
            admin_AccountInfo_user.MenuName = "User Management";
            admin_AccountInfo_user.MenuUrl = "/Manage/IndexForUser";
            admin_AccountInfo_user.Order = 3;


            List<Menu> leveal22Menus = new List<Menu>();
            leveal22Menus.Add(admin_AccountInfo_manage);
            leveal22Menus.Add(admin_AccountInfo_role);
            leveal22Menus.Add(admin_AccountInfo_user);

            #endregion
            admin_AccountInfo.SecondaryMenus = leveal22Menus;
            #endregion

            #region leveal1
            Menu admin_InfoManage = new Menu();
            admin_InfoManage.MenuName = "维护信息";
            admin_InfoManage.MenuUrl = "";
            admin_InfoManage.Order = 3;

            #region leveal2
            Menu admin_InfoManage_alarmDic = new Menu();
            admin_InfoManage_alarmDic.MenuName = "报警字典";
            admin_InfoManage_alarmDic.MenuUrl = "/Home/Error";
            admin_CustomerInfo.Order = 1;

            Menu admin_InfoManage_alarmLog = new Menu();
            admin_InfoManage_alarmLog.MenuName = "报警日志";
            admin_InfoManage_alarmLog.MenuUrl = "/Home/Error";
            admin_InfoManage_alarmLog.Order = 2;

            Menu admin_InfoManage_operlog = new Menu();
            admin_InfoManage_operlog.MenuName = "操作日志";
            admin_InfoManage_operlog.MenuUrl = "/Home/Error";
            admin_InfoManage_operlog.Order = 3;


            List<Menu> leveal222Menus = new List<Menu>();
            leveal222Menus.Add(admin_InfoManage_alarmDic);
            leveal222Menus.Add(admin_InfoManage_alarmLog);
            leveal222Menus.Add(admin_InfoManage_operlog);

            #endregion
            admin_InfoManage.SecondaryMenus = leveal222Menus;
            #endregion
            //all menus
            List<Menu> allMenus = new List<Menu>();
            allMenus.Add(admin_CustomerInfo);
            allMenus.Add(admin_AccountInfo);
            allMenus.Add(admin_InfoManage);

            ViewData["Menus"] = allMenus;

            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Board()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Error()
        {
            return View();
        }

    }

   
    
}
