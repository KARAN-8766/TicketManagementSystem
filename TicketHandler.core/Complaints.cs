using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketHandler.core
{
    public class Complaints
    {
        [Key]
        [Required]
        public int ComplaintId { get; set; }
        [Required]
        [Display(Name = "Subject")]
        public string Subject { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Created on")]
        public DateTime Created_on { get; set; }
        [Required]
        [Display(Name = "Priority")]
        public string Priority { get; set; }
        [Required]
        [Display(Name = "Expected Date")]
        public DateTime Expected_Date { get; set; }
        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Required]
        [Display(Name = "Employee Name")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Assigned To")]
        public string AssignTo { get; set; }
    }
}
