using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace ApiTax.Models
{
    public static class GlobalUser
    {
        public static User CurrentUser { get; set; }
        public static Boolean isAdmin { get; set; }
        public static Boolean isLogin { get; set; }
        public static int? user_type { get; set; }
        public static ObjectUser _ObjectUser { get; set; } 
    }

    public class InitRequest
    {
        public void init(System.Security.Principal.IPrincipal User,string _userjson=null)
        {
            try
            {
                ObjectUser _user=new ObjectUser() {  is_api=false};
                var username = "";
                if (_userjson != null)
                {

                     _user = JsonConvert.DeserializeObject<ObjectUser>(_userjson);
                    
                    func func = new func();
                    if (func.IsValid(_user.username, _user.password))
                    {
                        username = _user.username;
                        _user.is_api = true;
                    }
                }
                if (User.Identity.IsAuthenticated || username !="")
                {
                    StoreTerminalSystemEntities db = new StoreTerminalSystemEntities();
                 

                    if(User.Identity.IsAuthenticated)
                    {
                        username = User.Identity.Name;
                    }
                    
                    var CurrentUser = db.Users.Where(r => r.NationalCode == username).FirstOrDefault();

                 
                    GlobalUser.isLogin = true;

                    GlobalUser.user_type = CurrentUser.user_type;
                    GlobalUser._ObjectUser = _user;

                    if (CurrentUser.isAdmin == true)
                    {
                        GlobalUser.isAdmin = true;
                    }
                    else
                    {
                        GlobalUser.isAdmin = false;
                    }
                    GlobalUser.CurrentUser = CurrentUser;
                }
                else
                {
                    GlobalUser.user_type = 0;
                    GlobalUser.isLogin = false;
                    GlobalUser.isAdmin = false;
                    GlobalUser.CurrentUser = new Models.User();
                    
                }

            }
            catch { }
        }
    }
    public class func
    {
        public bool IsValid(string email, string password)
        {
            StoreTerminalSystemEntities db = new StoreTerminalSystemEntities();
            bool IsValid = false;

            var user = db.Users.FirstOrDefault(u => u.NationalCode == email);
            if (user != null)
            {
                if (user.PassWord == password)
                {
                    IsValid = true;
                }
            }

            return IsValid;
        }
    }
    public class ObjectUser
    {
        public string username { get; set; }
        public string password { get; set; }
        public string clientID { get; set; }
        public string key { get; set; }
        public  Boolean is_api { get; set; }
    }
}