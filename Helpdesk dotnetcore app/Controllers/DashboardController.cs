using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Helpdesk.Core.Interfaces;
using Helpdesk.Core.ViewModels.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Helpdesk.Controllers
{
    [CheckSession]
    public class DashboardController : Controller
    {
        private IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        // GET: Dashboard
        public async Task<ActionResult> Index(string status)
        {
            decimal seconds = DisplayExecutionTime();

            var userId = (int?)HttpContext.Session.GetInt32("ID");
            var list = await _dashboardRepository.GetRequestsAsync(new Core.Dtos.Dashboard.GetRequestsDto
            {
                Status = status,
                UserId = userId
            });

            ViewBag.ExecutionTime = seconds;
            ViewBag.edit = TempData["EditRequestID"];

            return View(list);
        }

        // GET: Dashboard/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var request = await _dashboardRepository.GetRequestAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        // GET: Dashboard/Create
        //[Authorize(Roles = "Operator")]

        public async Task<ActionResult> Create()
        {
            decimal seconds = DisplayExecutionTime();

            await SetCreateData();

            ViewBag.ExecutionTime = seconds;

            return View();
        }

        // POST: Dashboard/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ClientRequestViewModel requestVM, HttpPostedFileBase RequestDocument)
        {
            if (ModelState.IsValid)
            {
                byte[] bytes = null;
                if (RequestDocument != null)
                {
                    using (BinaryReader br = new BinaryReader(RequestDocument.InputStream))
                    {
                        bytes = br.ReadBytes(RequestDocument.ContentLength);
                    }
                }
                requestVM.Bytes = bytes;
                requestVM.UserId = (int?)HttpContext.Session.GetInt32("ID");
                var success = await _dashboardRepository.CreateRequestAsync(requestVM);
                return RedirectToAction("Index");
            }
            await SetCreateData();
            //  ViewBag.DisplayLinkStatus = "visible";
            return View(requestVM);
        }

        [HttpGet]
        public async Task<ActionResult> DownloadFile(int? id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var request = await _dashboardRepository.GetRequestAsync(id);
                    if (request != null)
                    {
                        var document = (request).Document_Content;

                        var FileVirtualPath = Server.MapPath("~/Files/") + "Dokument kerkese.pdf";
                        if (document != null)
                        {
                            return File(document, "application/pdf", Path.GetFileName(FileVirtualPath));
                        }
                        else
                            return View("Error");
                    }
                }
            }
            catch
            {
                ModelState.AddModelError("", "Document doesn't exist!");
            }
            return View();
        }

        // GET: Dashboard/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            decimal seconds = DisplayExecutionTime();
            if (id == null)
            {
                return BadRequest();
            }

            var hD_Request = await _dashboardRepository.GetRequestAsync(id);
            ClientRequestViewModel hd_Client_Request = new ClientRequestViewModel();

            if (hD_Request == null)
            {
                return NotFound();
            }
            if (hD_Request.Current_Status != "Përfunduar")
            {
                hd_Client_Request.NID = hD_Request.Client.NID;
                hd_Client_Request.FirstName = hD_Request.Client.FirstName;
                hd_Client_Request.Surname = hD_Request.Client.Surname;
                hd_Client_Request.Email = hD_Request.Client.Email;
                hd_Client_Request.Telephone_Nr = hD_Request.Client.Telephone_Nr;
                hd_Client_Request.IDHD_Client = await _dashboardRepository.GetClientIdByNIDAsync(hD_Request.Client.NID);//hD_Request.IDHD_Client;
                hd_Client_Request.IDHD_Request_Type = hD_Request.RequestTypeId;
                hd_Client_Request.IDHD_Program = hD_Request.ProgramId;
                hd_Client_Request.Title = hD_Request.Title;
                hd_Client_Request.Descriptions = hD_Request.Descriptions;
                hd_Client_Request.Reception_Date = hD_Request.Reception_Date;
                hd_Client_Request.Document_Name = hD_Request.Document_Name;
                hd_Client_Request.IDHD_Request = (int)id;
                hd_Client_Request.Current_Status = hD_Request.Current_Status;
                hd_Client_Request.Registration_Date = DateTime.Now;
                hd_Client_Request.IDHD_CommunicationChannel = hD_Request.CommunicationChannelId;
                hd_Client_Request.IDHD_Responsible = hD_Request.ResponsibleId;
                hd_Client_Request.IDHD_Operator = (int?)HttpContext.Session.GetInt32("ID");

            }

            var info = await _dashboardRepository.GetCreateViewModelAsync();

            //VwBag.IDHD_Client = new SelectList(db.HD_Client, "IDHD_Client", "NID", hD_Request.IDHD_Client);
            ViewBag.IDHD_Responsible = new SelectList
            (info.Responsible, "Id", "Username", hD_Request.ResponsibleId);
            ViewBag.IDHD_CommunicationChannel = new SelectList
            (info.CommunicationChannels, "Id", "CommunicationChannelValue", hD_Request.CommunicationChannelId);
            ViewBag.IDHD_Program = new SelectList
            (info.Programs, "Id", "Title", hD_Request.ProgramId);
            ViewBag.IDHD_Request_Type = new SelectList
            (info.RequestTypes, "Id", "Category", hD_Request.RequestTypeId);

            List<SelectListItem> Status = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Regjistruar", Value = "Regjistruar" },
                new SelectListItem { Text = "Në proces", Value = "Në proces" },
                new SelectListItem { Text = "Përfunduar", Value = "Përfunduar" },
            };
            ViewBag.Status = Status;
            ViewBag.Current_Status = hD_Request.Current_Status;
            ViewBag.ExecutionTime = seconds;
            ViewBag.requestID = id;
            return View(hd_Client_Request);

            //return Json(new { status = "fail" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ClientRequestViewModel hD_Request, HttpPostedFileBase RequestDocument)
        {
            if (HttpContext.Session.GetString("ID") != null)
            {
                //HD_Request request = new HD_Request();
                byte[] bytes = null;
                if (RequestDocument != null)
                {
                    using (BinaryReader br = new BinaryReader(RequestDocument.InputStream))
                    {
                        bytes = br.ReadBytes(RequestDocument.ContentLength);
                    }
                }
                hD_Request.Bytes = bytes;

                if (hD_Request.Current_Status != "Përfunduar")
                {
                    if (ModelState.IsValid)
                    {

                        var success = await _dashboardRepository.EditRequestAsync(hD_Request);
                        TempData["EditRequestID"] = hD_Request.IDHD_Request;
                        return RedirectToAction("Index", new { status = hD_Request.Current_Status });
                        // ViewBag.EditRequestID = request.IDHD_Request;
                    }

                    var info = await _dashboardRepository.GetCreateViewModelAsync();

                    //VwBag.IDHD_Client = new SelectList(db.HD_Client, "IDHD_Client", "NID", hD_Request.IDHD_Client);
                    ViewBag.IDHD_Responsible = new SelectList
                    (info.Responsible, "Id", "Username", hD_Request.IDHD_Responsible);
                    ViewBag.IDHD_CommunicationChannel = new SelectList
                    (info.CommunicationChannels, "Id", "CommunicationChannelValue", hD_Request.IDHD_CommunicationChannel);
                    ViewBag.IDHD_Program = new SelectList
                    (info.Programs, "Id", "Title", hD_Request.IDHD_Program);
                    ViewBag.IDHD_Request_Type = new SelectList
                    (info.RequestTypes, "Id", "Category", hD_Request.IDHD_Request_Type);

                    List<SelectListItem> Status = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Regjistruar", Value = "Regjistruar" },
                new SelectListItem { Text = "Në proces", Value = "Në proces" },
                new SelectListItem { Text = "Përfunduar", Value = "Përfunduar" },
            };
                    ViewBag.Status = Status;
                    ViewBag.Current_Status = hD_Request.Current_Status;

                }
                //else return Json(new { status = "fail" });
            }
            return View(hD_Request);
            //else return Json(new { status = "fail" });

        }

        // GET: Dashboard/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var requestVM = await _dashboardRepository.GetRequestAsync(id);
            if (requestVM == null)
            {
                return NotFound();
            }
            return View(requestVM);
        }

        // POST: Dashboard/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var success = await _dashboardRepository.DeleteConfirmedAsync(id);
            return Json(new { status = success ? "success" : "fail" });
        }

        [HttpGet]
        public async Task<ActionResult> Response(int? id)
        {

            if (id == null)
            {
                return BadRequest();
            }
            decimal time = DisplayExecutionTime();
            decimal seconds = time / 1000;

            var hd_Client_Request = await _dashboardRepository.GetResponseAsync(id);

            HttpContext.Session.SetInt32("Request_id", hd_Client_Request.IDHD_Request.Value);

            if (hd_Client_Request == null)
            {
                return NotFound();
            }
            ViewBag.ExecutionTime = seconds;

            return View(hd_Client_Request);
        }


        [HttpPost]
        public async Task<ActionResult> Response(RequestResponseViewModel requestResponseViewModel)
        {
            if (ModelState.IsValid)
            {
                requestResponseViewModel.RequestId = (int?)HttpContext.Session.GetInt32("Request_id");
                requestResponseViewModel.UserId = (int?)HttpContext.Session.GetInt32("ID");

                var success = await _dashboardRepository.ResponseAsync(requestResponseViewModel);
            }

            if (requestResponseViewModel == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }

        public decimal DisplayExecutionTime()
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            for (int i = 0; i < 100000000; i++)
            {

            }
            watch.Stop();
            decimal time = watch.ElapsedMilliseconds;
            decimal seconds = time / 1000;
            return seconds;
        }

        private async Task SetCreateData()
        {
            var createVM = await _dashboardRepository.GetCreateViewModelAsync();

            ViewBag.IDHD_Responsible = createVM.Responsible;
            ViewBag.IDHD_CommunicationChannel = createVM.CommunicationChannels;
            ViewBag.IDHD_Program = createVM.Programs;
            ViewBag.IDHD_Request_Type = createVM.RequestTypes;

            List<SelectListItem> Status = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Regjistruar", Value = "Regjistruar" },
                new SelectListItem { Text = "Në proces", Value = "Në proces" },
                new SelectListItem { Text = "Përfunduar", Value = "Përfunduar" },
            };
            ViewBag.Current_Status = Status;
        }
    }
}