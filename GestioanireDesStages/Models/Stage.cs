using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestioanireDesStages.Models
{
    public class Stage
    {
        public int StageId { get; set; }

        [Required]
        public string Titre { get; set; }

        [DataType(DataType.Date)]
        public DateTime Debut { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fin { get; set; }


        public string Commentaire { get; set; }


        public string Stagiaire { get; set; }
    }
}
