using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Core.ViewModels.Dashboard
{
    public class ResponseViewModel
    {
        public int Id { get; set; }
        public int? RequestId { get; set; }
        public string Response_Content { get; set; }
        public DateTime? Response_Date { get; set; }
        public int? UserId { get; set; }
    }
}
