using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Infrastructure.Data.Entities
{
    public class Request
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        public virtual Client Client { get; set; }
        public virtual CommunicationChannel CommunicationChannel { get; set; }
        public virtual Program Program { get; set; }
        public virtual RequestType RequestType { get; set; }
        public virtual ICollection<Response> Response { get; set; }
    }
}
