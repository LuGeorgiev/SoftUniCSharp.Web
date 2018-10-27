using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System;
using System.Linq;
using TORSHIA.Models;
using TORSHIA.Models.Enums;
using TORSHIA.ViewModels.Reports;

namespace TORSHIA.Controllers
{
    public class ReportsController : BaseController
    {
        private readonly Random rnd;

        public ReportsController()
        {
            this.rnd = new Random();
        }

        [HttpGet("/Reports/Close")]
        public IHttpResponse Close(int id)
        {
            var user = this.db
                .Users
                .FirstOrDefault(x => x.Username == this.User);
            if (user == null)
            {
                return this.Redirect("/Users/Login");
            }

            var task = this.db
                .Tasks
                .FirstOrDefault(x => x.Id == id);
            if (task == null)
            {
                this.BadRequestError($"Task with Id: {task.Id} was not found");
            }


            var statusInt = rnd.Next(100);
            var status = Status.Completed;
            if (statusInt < 25)
            {
                status = Status.Archived;
            }

            var report = new Report
            {
                ReportedOn = DateTime.UtcNow,
                Status = status,
                TaskId = task.Id,
                ReporterId = user.Id,
            };
            task.IsReported = true;

            this.db.Reports.Add(report);
            this.db.Tasks.Update(task);
            db.SaveChanges();

            return this.Redirect("/Home/Index");
        }

        [HttpGet("/Reports/All")]
        public IHttpResponse All()
        {
            var user = this.db
               .Users
               .FirstOrDefault(x => x.Username == this.User);
            if (user.Role != Role.Admin)
            {
                return this.Redirect("/Users/Login");
            }

            var reports = this.db
                .Reports
                .Select(x => new ReportListingModel
                {
                    Id = x.Id,
                    Level = x.Task.AffectedSectors.Count(),
                    Task = x.Task.Title,
                    Status = x.Status.ToString()
                })
                .ToList();
            var model = new AllReportsListingModel
            {
                Reports = reports
            };

            return this.View("/Reports/All", model);
        }

        [HttpGet("/Reports/Details")]
        public IHttpResponse Details(int id)
        {
            var user = this.db
             .Users
             .FirstOrDefault(x => x.Username == this.User);
            if (user.Role != Role.Admin)
            {
                return this.Redirect("/Users/Login");
            }

            var report = this.db
                .Reports
                .FirstOrDefault(x => x.Id == id);
            if (report==null)
            {
                this.BadRequestError($"Report with Id: {id} was not found!");
            }

            var model = new ReportDetailsViewModel
            {
                 Id=report.Id,                 
                 Description=report.Task.Description,
                 DueDate=report.Task.DueDate==null?"No date": report.Task.DueDate.Value.ToString("dd/MM/yyyy"),
                 Level = report.Task.AffectedSectors.Count(),
                 Participants = report.Task.Participants,
                 ReportedBy = this.User,
                 ReportedDate = report.ReportedOn.ToString("dd/MM/yyyy"),
                 TaskTitle = report.Task.Title,
                 Status=report.Status.ToString()
            };
            var affectedSectors = "None";
            if (report.Task.AffectedSectors.Count>0)
            {
                affectedSectors = string.Join(", ", report.Task.AffectedSectors.Select(x => x.Sector.ToString()));
            }

            model.AffectedSectors = affectedSectors;

            return this.View("/Reports/Details", model);
        }

    }
}
