using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Helpdesk.Core.ViewModels.Dashboard
{
    public class RequestResponseViewModel
    {
        //public HD_Client_HD_Request_Model Request { get; set; }
        //public HD_Response Response { get; set; }
        [Key]
        public Nullable<int> IDHD_Request { get; set; }
        [Display(Name = "Përgjigjja")]
        [Required(ErrorMessage = "Vendosni një pergjigje për kërkesen!")]

        public string Response_Content { get; set; }
        [Display(Name = "Data e përgjigjes")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
              ApplyFormatInEditMode = true)]

        public Nullable<System.DateTime> Response_Date { get; set; }
        public Nullable<int> IDHD_User { get; set; }

        [Display(Name = "NID")]
        //[Required(ErrorMessage = "Ju lutem vendosni NID te klientit!")]
        public string NID { get; set; }

        [Display(Name = "Emër")]
        //[Required(ErrorMessage = "Ju lutem vendosni emrin e klientit!")]

        public string FirstName { get; set; }
        [Display(Name = "Mbiemër")]
        //  [Required(ErrorMessage = "Ju lutem vendosni mbiemrin e klientit!")]

        public string Surname { get; set; }
        // [Required(ErrorMessage = "Ju lutem vendosni email-in e klientit!")]

        public string Email { get; set; }
        [Display(Name = "Nr. Telefoni")]
        // [Required(ErrorMessage = "Ju lutem vendosni numrin e telefonit te klientit!")]

        public string Telephone_Nr { get; set; }
       // public int IDHD_Request { get; set; }

        //te dhenat e kerkeses
        [Display(Name = "Lloji i kërkeses")]
        // [Required(ErrorMessage = "Ju lutem vendosni llojin e kerkeses!")]

        public Nullable<int> IDHD_Request_Type { get; set; }
        [Display(Name = "Lloji i programit")]
        //   [Required(ErrorMessage = "Ju lutem vendosni llojin e programit!")]
        public Nullable<int> IDHD_Program { get; set; }

        public Nullable<int> IDHD_Client { get; set; }
        [Display(Name = "Titulli i kërkeses")]
        //  [Required(ErrorMessage = "Ju lutem vendosni titullin e kerkeses!")]
        public string Title { get; set; }
        [Display(Name = "Përshkrimi i kërkesës")]
        //  [Required(ErrorMessage = "Ju lutem vendosni pershkrimin e kerkeses!")]
        public string Descriptions { get; set; }
        [Display(Name = "Data e marrjes së kërkesës")]
        //   [Required(ErrorMessage = "Ju lutem vendosni daten e marrjes se kerkeses!")]
        public Nullable<System.DateTime> Reception_Date { get; set; }
        [Display(Name = "Emri i dokumentit")]
        //   [Required(ErrorMessage = "Ju lutem vendosni emrin e dokumentit!")]
        public string Document_Name { get; set; }
        [Display(Name = "Dokumenti")]

        public byte[] Document_Content { get; set; }
        [Display(Name = "Data e regjistrimit të kërkesës")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
              ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Registration_Date { get; set; }
        [Display(Name = "Data e përfundimit të kërkesës")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
              ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Completion_Date { get; set; }
        [Display(Name = "Statusi")]

        public string Current_Status { get; set; }
        [Display(Name = "Operatori")]

        public Nullable<int> IDHD_Operator { get; set; }
        [Display(Name = "Personi përgjegjës")]

        public Nullable<int> IDHD_Responsible { get; set; }
        [Display(Name = "Kanali i komunikimit")]

        public Nullable<int> IDHD_CommunicationChannel { get; set; }
        public virtual RequestTypeViewModel Request_Type { get; set; }
        [Display(Name = "Kategoria")]

        public string Category { get; set; }
        [Display(Name = "Programi")]

        public string Program { get; set; }
        [Display(Name = "Kanali i komunikimit")]

        public string Communication_Channel { get; set; }
        [Display(Name = "Përgjegjësi")]

        public string Responsible { get; set; }
        public int? RequestId { get; set; }
        public int? UserId { get; set; }
    }
}