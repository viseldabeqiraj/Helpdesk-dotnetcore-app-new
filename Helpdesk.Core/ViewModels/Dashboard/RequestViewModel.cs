using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Core.ViewModels.Dashboard
{
    public class RequestViewModel
    {
        public int Id { get; set; }
        public int? RequestTypeId { get; set; }
        public int? ProgramId { get; set; }
        public int? ClientId { get; set; }
        public string Title { get; set; }
        public string Descriptions { get; set; }
        public DateTime? Reception_Date { get; set; }
        public string Document_Name { get; set; }
        public byte[] Document_Content { get; set; }
        public DateTime? Registration_Date { get; set; }
        public DateTime? Completion_Date { get; set; }
        public string Current_Status { get; set; }
        public int? OperatorId { get; set; }
        public int? ResponsibleId { get; set; }
        public int? CommunicationChannelId { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ClientViewModel Client { get; set; }
        public virtual CommunicationChannelViewModel CommunicationChannel { get; set; }
        public virtual ProgramViewModel Program { get; set; }
        public virtual RequestTypeViewModel RequestType { get; set; }
    }
}
