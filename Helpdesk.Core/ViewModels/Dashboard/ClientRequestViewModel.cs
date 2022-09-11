using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Helpdesk.Core.ViewModels.Dashboard
{
    public class ClientRequestViewModel
    {
        //public HD_Client HD_Client { get; set; }
        //public HD_Request HD_Request { get; set; }

        //te dhenat e klientit
        //[Key]
        [Display(Name="NID")]
        //[Required(ErrorMessage = "Ju lutem vendosni NID te klientit!")]
        public string NID { get; set; }

        [Display(Name = "Emër")]
       //[Required(ErrorMessage = "Ju lutem vendosni emrin e klientit!")]

        public string FirstName { get; set; }
        [Display(Name = "Mbiemër")]
      //  [Required(ErrorMessage = "Ju lutem vendosni mbiemrin e klientit!")]

        public string Surname { get; set; }
        // [Required(ErrorMessage = "Ju lutem vendosni email-in e klientit!")]
        [Display(Name = "E-mail")]

        public string Email { get; set; }
        [Display(Name = "Nr. Telefoni")]
       // [Required(ErrorMessage = "Ju lutem vendosni numrin e telefonit te klientit!")]

        public string Telephone_Nr { get; set; }
        public int IDHD_Request { get; set; }

        //te dhenat e kerkeses
        [Display(Name = "Lloji i kërkesës")]
        [Required(ErrorMessage = "Ju lutem zgjidhni llojin e kërkesës!")]

        public Nullable<int> IDHD_Request_Type { get; set; }
        [Display(Name = "Lloji i programit")]
        [Required(ErrorMessage = "Ju lutem zgjidhni llojin e programit!")]
        public Nullable<int> IDHD_Program { get; set; }

        public Nullable<int> IDHD_Client { get; set; }
        [Display(Name = "Titulli i kërkesës")]
        [Required(ErrorMessage = "Ju lutem plotësoni titullin e kërkesës!")]
        public string Title { get; set; }
        [Display(Name = "Përshkrimi i kërkesës")]
       [Required(ErrorMessage = "Ju lutem plotësoni përshkrimin e kërkesës!")]
        public string Descriptions { get; set; }
        [Display(Name = "Data e marrjes së kërkesës")]
        //[DataType(DataType.Date)]
       // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Ju lutem plotësoni daten e marrjes se kërkesës!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
              ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Reception_Date { get; set; }
        [Display(Name = "Emri i dokumentit")]
     //   [Required(ErrorMessage = "Ju lutem vendosni emrin e dokumentit!")]
        public string Document_Name { get; set; }
        [Display(Name = "Dokumenti")]

        public byte[] Document_Content { get; set; }
        [Display(Name = "Data e regjistrimit të kërkesës")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
              ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Registration_Date { get; set; }
        [Display(Name = "Statusi")]
        [Required(ErrorMessage = "Ju lutem zgjidhni statusin e kërkesës!")]

        public string Current_Status { get; set; }
        [Display(Name = "Operatori")]

        public Nullable<int> IDHD_Operator { get; set; }
        [Display(Name = "Përgjegjësi")]

        public Nullable<int> IDHD_Responsible { get; set; }
        [Display(Name = "Kanali i komunikimit")]
        [Required(ErrorMessage = "Ju lutem zgjidhni kanalin e komunikimit!")]

        public Nullable<int> IDHD_CommunicationChannel { get; set; }

        public byte[] Bytes { get; set; }
        public int? UserId { get; set; }
    }
}