using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Vaccinator.Models
{
    public class Injection
    {
        [Key]
        public string uuid { get; set;}
        
        [Display(Name = "Personne")]
        [ForeignKey("uuid")]
        [Required]
        public virtual Personne Personne { get; set; }
        
        [Required]
        public string Maladie { get; set;}
        
        [Required]
        public string Marque{ get; set;}
        
        [Display(Name = "Numéro de lot")]
        [Required]
        public string NumLot { get; set;}
        
        [Display(Name = "Date d'injection")]
        [Required]
        public DateTime DatePrise { get; set;}
        
        
        [Display(Name = "Date de rappel")]
        public DateTime? DateRappel { get; set;}
        
        
        [Display(Name = "Status rappel")]
        public bool? StatusRappel { get; set;}
    }
}