using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Core.ViewModels.Dashboard
{
    public class ClientViewModel
    {
        public int Id { get; set; }
        public string NID { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Telephone_Nr { get; set; }
    }
}
