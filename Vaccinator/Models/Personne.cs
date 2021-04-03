using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vaccinator.Models
{
    
    public class Personne
    {
        [Key]
        public string uuid { get; set;}
        
        [Required]
        public string nom { get; set;}
        
        [Required]
        public string prenom { get; set;}
        
        [Required]
        public char sexe { get; set;}
        
        [Required]
        [Display(Name = "Date de naissance")]
        public DateTime ddn { get; set;}
        
        [Required]
        public string role{ get; set;}
        
        [NotMapped]
        public virtual ICollection<Injection> injections { get; set; }
    }
}