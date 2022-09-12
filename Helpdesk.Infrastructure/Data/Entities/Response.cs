using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Infrastructure.Data.Entities
{
    public class Response
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? RequestId { get; set; }
        public string Response_Content { get; set; }
        public DateTime? Response_Date { get; set; }
        public int? UserId { get; set; }

        public virtual Request Request { get; set; }
        public virtual User User { get; set; }
    }
}
