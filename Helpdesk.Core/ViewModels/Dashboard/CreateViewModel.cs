using Helpdesk.Core.ViewModels.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Core.ViewModels.Dashboard
{
    public class CreateViewModel
    {
        public List<UserViewModel> Responsible { get; set; }
        public List<CommunicationChannelViewModel> CommunicationChannels { get; set; }
        public List<ProgramViewModel> Programs { get; set; }
        public List<RequestTypeViewModel> RequestTypes { get; set; }
    }
}
