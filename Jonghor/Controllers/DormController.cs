﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jonghor.Models;
using System.Data.Entity.Validation;
using Jonghor.ViewModel;

namespace Jonghor.Controllers
{
    public class DormController : Controller
    {
        private JongHorDBEntities1 db = new JongHorDBEntities1();
        // GET: Dorm
        public ActionResult Index()
        {
            if (Session["Status"] != null)
            {
                if(Session["Status"].ToString() == "Owner")
                {
                    DormDetailViewModel dt = new DormDetailViewModel();
                    dt.SetDorm(Session["UserName"].ToString());
                    return View("Edit", dt);
                }
                else
                {
                    return View("Add");
                }
            }
            else
            {
                return Content("Error");
            }
        }

        public ActionResult addroom(Room room)
        {

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Add(Dorm dorm,String floorcount,String roomcount,string url_picture)
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "User")
            {
                dorm.M_username = Session["UserName"].ToString();
                dorm.Tel = "000";
                dorm.Address = "111";
                Dorm dormSelect = db.Dorm.SingleOrDefault(d => d.Name == dorm.Name);


                if (dormSelect == null)
                {
                    Dorm_Picture dormpic = new Dorm_Picture();
                    dormpic.Dorm_ID = dorm.Dorm_ID;
                    dormpic.URL_Picture = url_picture;
                    db.Dorm_Picture.Add(dormpic);
                    db.Dorm.Add(dorm);
    
                    for(int i=0;i< Int32.Parse(floorcount);i++)
                    {
                        for(int j=1;j<= Int32.Parse(roomcount);j++)
                        {
                            Room r = new Room();
                            r.Dorm_ID = dorm.Dorm_ID;
                            r.Floor = (i+1).ToString();
                            r.Room_number = (j).ToString();
                            r.Type_ID = 1;
                            if(j<10)
                            {
                                r.Room_number = '0' + r.Room_number;
                            }
                            db.Room.Add(r);
                        }
                    }

                    Session["Status"] = "Owner";
                }
                else
                {
                    Response.Write("<script>alert('The dorm name is already exist')</script>");
                    return RedirectToAction("Index", "Home");
                }
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
                Response.Write("<script>alert('The dorm name is added, Status changed to Owner')</script>");
                return RedirectToAction("Index", "Home");
            }
            Response.Write("<script>alert('Error')</script>");
            return RedirectToAction("Index", "Home");
        }



        public ActionResult Edit(Dorm dorm,int dorm_id)
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "Owner")
            {

                dorm.M_username = Session["UserName"].ToString();
                dorm.Tel = "000";
                dorm.Address = "111";

                    var edit = db.Dorm.Find(dorm.Dorm_ID);
                    edit.Name = dorm.Name;
                    edit.information = dorm.information;
                    edit.Line = dorm.Line;
                    edit.Facebook = dorm.Facebook;
                    edit.Instagram = dorm.Instagram;
                    edit.Tel = dorm.Tel;
                    edit.E_Per_Unit = dorm.E_Per_Unit;
                    edit.W_Per_Unit = dorm.W_Per_Unit;
                    edit.Deposit = dorm.Deposit;
                    edit.Internet = dorm.Internet;
                      
                

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
                Response.Write("<script>alert('Dorm Edited')</script>");
                return RedirectToAction("Index","Home");
            }
            Response.Write("<script>alert('Error cannot add new Dorm')</script>");
            return View("Edit");
        }
        public ActionResult Roommanagement()
        {
            RoomListViewModel roomListview = new RoomListViewModel();
            string name = Session["UserName"].ToString();
            roomListview.GetRoomListView(name);

            return View("../Host/RoomManagement_Host", roomListview);
        }

        public ActionResult DeclineReservedRoom(int room_id)
        {
            JongHorDBEntities1 jonghor = new JongHorDBEntities1();

            Room_Reserved roomReserved = jonghor.Room_Reserved.Where(r => r.Room_ID == room_id).First();
            jonghor.Room_Reserved.Remove(roomReserved);

            jonghor.Room.Find(room_id).Status = (int)Status.Avaliable;

            try
            {
                jonghor.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            Response.Write("<script>alert('New Dorm Added')</script>");

            return Roomsort("All");
        }

        public ActionResult AcceptReservedRoom(int room_id)
        {
            JongHorDBEntities1 jonghor = new JongHorDBEntities1();
            Room_Reserved roomReserved = jonghor.Room_Reserved.Where(r => r.Room_ID == room_id).First();

            jonghor.Person.Find(roomReserved.Username).Room_ID = room_id;
            jonghor.Person.Find(roomReserved.Username).Dorm_ID = roomReserved.Room.Dorm_ID;
            jonghor.Room_Reserved.Remove(roomReserved);

            jonghor.Room.Find(room_id).Status = (int)Status.NotAvaliable;

            

            try
            {
                jonghor.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            Response.Write("<script>alert('New Dorm Added')</script>");
            return Roomsort("All");
        }

        public ActionResult Roomsort(string option)
        {
            if (option == "All")
            {
                return RedirectToAction("Roommanagement", "Dorm");
            }
            else if (option == "Wait")
            {
                RoomListViewModel roomListview = new RoomListViewModel();
                string name = Session["UserName"].ToString();
                roomListview.GetRoomListView(name, Status.WaitRoomMate);
                return View("../Host/RoomManagement_Host", roomListview);
            }
            else if (option == "Avaliable")
            {
                RoomListViewModel roomListview = new RoomListViewModel();
                string name = Session["UserName"].ToString();
                roomListview.GetRoomListView(name, Status.Avaliable);
                return View("../Host/RoomManagement_Host", roomListview);
            }
            else if (option == "Not")
            {
                RoomListViewModel roomListview = new RoomListViewModel();
                string name = Session["UserName"].ToString();
                roomListview.GetRoomListView(name, Status.NotAvaliable);
                return View("../Host/RoomManagement_Host", roomListview);
            }
            else if(option == "Reserved")
            {
                RoomListViewModel roomListview = new RoomListViewModel();
                string name = Session["UserName"].ToString();
                roomListview.GetRoomListView(name, Status.Reserved);
                return View("../Host/RoomManagement_Host", roomListview);
            }

            return Content(option);
        }
    }
}