using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onlineFoodOrdering.Models;
namespace onlineFoodOrdering.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            resturantEntities r = new resturantEntities();
            return View(r.resturant);
        }
        [HttpPost]
        public ActionResult  check(mambership m)
        {
            resturantEntities r = new resturantEntities();
            IQueryable<mambership> a=(from x in r.mambership where x.username == m.username select x);
            if (m.mpassword == a.First().mpassword||m==null)
            {
                Session["nav"] = a.First();
                return View("Index", r.resturant);
            }
            else
            {
                return View("Error");
            }

        }
        public ActionResult foods(resturant r)
        {
            resturantEntities re = new resturantEntities();
            IQueryable<food> foo = from res in re.resturant
                                   join fr in re.FinR on res.branch_code equals fr.rcode
                                   join fo in re.food on
                                  fr.fcode equals fo.code
                                   where fr.rcode == r.branch_code
                                   select fo;
            Session["now"] = r.branch_code;
            return View("foods",foo );
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        private bool cmp(orders a,orders b)
        {
            return a.code < b.code;
        }
        public ActionResult deliver(food f,int code)
        {
            if (Session["nav"] == null)
                return View("Login");
            resturantEntities re = new resturantEntities();
            IQueryable<orders> or = from o in re.orders select o;
            orders order = new orders();
            order.code = or.First().code - 1;
            order.delivery_code = 1;
            order.orders_time = DateTime.Now;
            order.recieved_time = DateTime.Now;
            order.price = f.price * 1000;
            order.delivery_price = 1000;
            re.orders.Add(order);
            re.SaveChanges();

            ordering orderings = new ordering();
            orderings.costumer =( (mambership)Session["nav"]).username;
            orderings.order_code = order.code;
            orderings.food = f.code;
            orderings.rcode = int.Parse(Session["now"].ToString());
            re.ordering.Add(orderings);
            re.SaveChanges();

            return View();
        }



        public ActionResult Register()
        {

            return View();
        }


        public ActionResult saved(string uname,string fname,string type,string email
          ,  int phone,long cellphone,string provience,string city,string address,int postCode,int dd,int mm,
            int yyyy,string vegeterian,string pass,string lname)
        {
            resturantEntities r = new resturantEntities();
            customer c = new customer();
            DateTime d = new DateTime(yyyy, mm, dd);
            c.birth_date = d;
            c.c_address = address;
            c.c_cellphone_number = cellphone;
            c.c_city = city;
            c.c_phone_number = phone;
            c.c_province = provience;
            c.email = email;
            c.post_code = postCode;
            c.username = uname;
            if (vegeterian == "checked")
                c.vegeterian = true;
            else
                c.vegeterian = false;
            r.customer.Add(c);
            mambership m = new mambership();
            m.username = uname;
            m.mpassword = pass;
            m.mtype = type;
            r.mambership.Add(m);
            r.SaveChangesAsync();
            Session["nav"] = m;
                return View("saved");
        }


        public ActionResult Login()
        {
            return View();
        }
    }
}