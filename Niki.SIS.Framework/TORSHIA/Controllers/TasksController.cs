using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TORSHIA.Models;
using TORSHIA.Models.Enums;
using TORSHIA.ViewModels.Tasks;

namespace TORSHIA.Controllers
{
    public class TasksController : BaseController
    {
        [HttpGet("/Tasks/Datails")]
        public IHttpResponse Details(int id)
        {
            
            if (this.User == null)
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

            TaskDetailsViewModel model = new TaskDetailsViewModel
            {
                Title = task.Title,
                Level = task.AffectedSectors.Count(),
                Description = task.Description,
                AffectedSectors = string.Join(", ", task.AffectedSectors.Select(x => x.Sector.ToString())),
                Participants = task.Participants,
                DueDate = task.DueDate==null ? "No date set" : task.DueDate.Value.ToString()
            };

            return this.View("/Tasks/Details", model);
        }


        [HttpGet("Tasks/Create")]
        public IHttpResponse Create()
        {
            var user = this.db
                .Users
                .FirstOrDefault(x => x.Username == this.User);
            if (user.Role != Role.Admin)
            {
                return this.Redirect("/Users/Login");
            }
            return this.View("/Tasks/Create");
        }

        [HttpPost("Tasks/Create")]
        public IHttpResponse Create(CreateTaskViewModel model)
        {
            var user = this.db
                .Users
                .FirstOrDefault(x => x.Username == this.User);
            if (user.Role != Role.Admin)
            {
                return this.Redirect("/Users/Login");
            }

            var task = new Task
            {
                DueDate=model.DueDate,
                Title=model.Title,
                Description=model.Description,
                Participants=model.Participants
            };

            if (model.Internal!=null)
            {
                task.AffectedSectors.Add(new AffectedSector { Sector= Sector.Internal });
            }
            if (model.Management != null)
            {
                task.AffectedSectors.Add(new AffectedSector { Sector = Sector.Management });
            }
            if (model.Marketing != null)
            {
                task.AffectedSectors.Add(new AffectedSector { Sector = Sector.Marketing });
            }
            if (model.Finances != null)
            {
                task.AffectedSectors.Add(new AffectedSector { Sector = Sector.Finances });
            }
            if (model.Customers != null)
            {
                task.AffectedSectors.Add(new AffectedSector { Sector = Sector.Customers });
            }


            this.db.Tasks.Add(task);
            this.db.SaveChanges();

            return this.Redirect("/Home/Index");
        }
    }
}
