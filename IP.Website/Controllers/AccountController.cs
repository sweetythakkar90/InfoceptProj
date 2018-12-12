using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IP.Website.Models;
using IP.Website.Exceptions;
using System.Web.Security;

namespace IP.Website.Controllers
{
   
    public class AccountController : Controller
    {
        // GET: Account
       string Baseurl = "http://ipmasterapi-dev.ap-southeast-2.elasticbeanstalk.com/";
       // string Baseurl = "http://localhost:57225/";
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Home()
        {
            return View();
        }
        public async Task<ActionResult> Login(AccountModel accountmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (accountmodel.userName != null && accountmodel.password != null)
                    {
                        using (var client = new HttpClient())
                        {
                            //Passing service base url  
                            client.BaseAddress = new Uri(Baseurl);

                            client.DefaultRequestHeaders.Clear();
                            //Define request data format  
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                            HttpResponseMessage Res = await client.GetAsync("api/Account/Get/" + accountmodel.userName.Trim() + "/" + accountmodel.password.Trim());

                            //Checking the response is successful or not which is sent using HttpClient  
                            if (Res.IsSuccessStatusCode)
                            {
                                //Storing the response details recieved from web api   
                                var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                                AccountModel acct = new AccountModel();
                                acct = JsonConvert.DeserializeObject<AccountModel>(EmpResponse);
                                if (acct.Id > 0 )
                                {
                                    //Get Role based Menu Details
                                    HttpResponseMessage menuRes = await client.GetAsync("api/Menu/Get/" + acct.roleId + "/ G");

                                    //Checking the response is successful or not which is sent using HttpClient  
                                    if (menuRes.IsSuccessStatusCode)
                                    {
                                        var menuResponse = menuRes.Content.ReadAsStringAsync().Result;
                                        List<MenuModel> md = new List<MenuModel>();
                                        md = JsonConvert.DeserializeObject<List<MenuModel>>(menuResponse);
                                        
                                        //Insert Records into Login Details
                                        LoginDetailsModel ld = new LoginDetailsModel();
                                        ld.userId = acct.uId;
                                        ld.loginDate = DateTime.Now;
                                        ld.logoutDate = DateTime.Now;
                                        ld.userType = acct.userType;

                                        var ldtls = JsonConvert.SerializeObject(ld);
                                        HttpResponseMessage Res1 = await client.PostAsync("api/logindetails/insert", new StringContent(ldtls, Encoding.UTF8, "application/json"));

                                        //Checking the response is successful or not which is sent using HttpClient  
                                        if (Res1.IsSuccessStatusCode)
                                        {
                                            if (md.Count > 0)
                                            {
                                                FormsAuthentication.SetAuthCookie(acct.userName, false); // set the formauthentication cookie
                                                Session["acct"] = acct; // Bind the account details to "LoginCredentials" session
                                                Session["MenuDetails"] = md; //Bind the _menus list to MenuMaster session
                                                Session["UserName"] = acct.userName;

                                                return View("Home");
                                            }
                                            else
                                            {
                                                ModelState.AddModelError("", "User rights not assigned");
                                                return View("Index");
                                            }
                                        }
                                    }
                                }

                            }

                        }
                       
                    }
                    ModelState.AddModelError("", "Invalid username or password");
                    return View("Index");
                }
                return View("Index");
            }
            catch (Exception ex)
            {
                ExceptionLogHandler.LogData(ex);

                throw ex;
            }


        }
        public async Task<ActionResult> LogOff()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                LoginDetailsModel ld = new LoginDetailsModel();
                ld.ID= ((AccountModel)Session["acct"]).Id;
                ld.userId = ((AccountModel)Session["acct"]).uId;
                ld.loginDate = DateTime.Now;
                ld.logoutDate = DateTime.Now;
                ld.userType = ((AccountModel)Session["acct"]).userType;
                var ldtls = JsonConvert.SerializeObject(ld);

                HttpResponseMessage Res1 = await client.PutAsync("api/logindetails/update", new StringContent(ldtls, Encoding.UTF8, "application/json"));
                if (Res1.IsSuccessStatusCode)
                {
                    FormsAuthentication.SignOut();

                    Session.Abandon();
                    Session.Clear();
                    Session.RemoveAll();
                    return RedirectToAction("Index", "Account");
                }
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
        }
    }
}