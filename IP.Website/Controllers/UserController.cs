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
using System.Dynamic;

namespace IP.Website.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        // GET: SORType
        string Baseurl = "http://ipmasterapi-dev.ap-southeast-2.elasticbeanstalk.com";
        //string Baseurl = "http://localhost:57225/";
        public ActionResult Index()
        {
            try
            {
                List<AccountModel> obj = new List<AccountModel>();
                using (var User = new HttpClient())
                {
                    User.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    int id = 0;
                    int roleId = 1;
                    if (Session["acct"] != null)
                    {
                        if (((AccountModel)Session["acct"]).roles == "Administrator")
                        {
                            id = 0;
                            roleId = 1;
                        }
                        else
                        {
                            id = ((AccountModel)Session["acct"]).Id;
                            roleId = ((AccountModel)Session["acct"]).roleId;
                        }
                    }
                    var responseTask = User.GetAsync("api/User/get/" + id + "/" + roleId);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var response = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the SORType list  
                        obj = JsonConvert.DeserializeObject<List<AccountModel>>(response);
                        ViewBag.StatusList = new SelectList(obj[0].statusTypeList, "ID", "name");
                        ViewBag.MembersList = new SelectList(obj[0].membersList, "ID", "firstName");
                        ViewBag.SubContractorList = new SelectList(obj[0].scList, "ID", "subconName");
                        ViewBag.RolesList = new SelectList(obj[0].rolesList, "Id", "rolesName");
                    }
                }

                return View(obj);
            }
            catch (Exception ex)
            {
                ExceptionLogHandler.LogData(ex);

                throw ex;
            }
        }

        public async Task<ActionResult> Insert(AccountModel user)
        {
            try
            {
                if (Session["acct"] != null)
                {
                    user.userId = ((AccountModel)Session["acct"]).uId;
                }
                AccountModel UserInfo = new AccountModel();
                using (var User = new HttpClient())
                {
                    //Passing service base url
                    User.BaseAddress = new Uri(Baseurl);

                    User.DefaultRequestHeaders.Clear();

                    var obj = JsonConvert.SerializeObject(user);

                    // Define request data format
                    User.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllComapnies using HttpClient 
                    HttpResponseMessage Res = await User.PostAsync("api/User/insert", new StringContent(obj, Encoding.UTF8, "application/json"));

                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var UserResponse = Res.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Company list  
                        UserInfo = JsonConvert.DeserializeObject<AccountModel>(UserResponse);
                    }

                    //returning the company list to view  
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogHandler.LogData(ex);

                throw ex;
            }
        }

        public ActionResult Update(AccountModel user)
        {
            try
            {
                if (Session["acct"] != null)
                {
                    user.userId = ((AccountModel)Session["acct"]).uId;
                }
                List<AccountModel> UserInfo = new List<AccountModel>();
                using (var User = new HttpClient())
                {
                    User.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = User.PutAsJsonAsync("api/User/update", user);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var UserResponse = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Sub SORType  list  
                        UserInfo = JsonConvert.DeserializeObject<List<AccountModel>>(UserResponse);

                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ExceptionLogHandler.LogData(ex);

                throw ex;
            }
        }

        public ActionResult Delete(int ID)
        {
            try
            {
                using (var User = new HttpClient())
                {
                    User.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = User.DeleteAsync("api/User/delete/" + ID);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");

                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogHandler.LogData(ex);

                throw ex;
            }
        }
    }
}