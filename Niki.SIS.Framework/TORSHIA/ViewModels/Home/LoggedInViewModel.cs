using System;
using System.Collections.Generic;
using System.Text;
using TORSHIA.ViewModels.Tasks;

namespace TORSHIA.ViewModels.Home
{
    public class LoggedInViewModel
    {      

        public IEnumerable<TaskListingViewModel> Tasks { get; set; }
    }
}
