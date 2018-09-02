using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestioanireDesStages.Models
{
    public class Personne
    {
        [Required]
        public int PersonneId { get; set; }

        [Required]
        public string Nom { get; set; }

        [Required]
        [Phone]
        public string Telephone { get; set; }

        [Required]
        [EmailAddress]
        public string Courriel { get; set; }

        public bool Administrateur { get; set; } = false;

        public bool Superviseur { get; set; } = false;
        public bool Stagiaire { get; set; } = false;
    }
}
