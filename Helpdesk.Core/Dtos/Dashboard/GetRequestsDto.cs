using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Core.Dtos.Dashboard
{
    public class GetRequestsDto
    {
        public int? UserId { get; set; }
        public string Status { get; set; }
    }
}
