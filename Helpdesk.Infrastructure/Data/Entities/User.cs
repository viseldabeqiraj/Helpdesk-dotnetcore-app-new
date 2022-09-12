using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Infrastructure.Data.Entities
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Paternity { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool? IsActive { get; set; }
        public int? RoleId { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<Response> Responses { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
