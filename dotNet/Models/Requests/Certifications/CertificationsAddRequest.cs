using System;
using System.ComponentModel.DataAnnotations;


namespace Sabio.Models.Requests.Certifications
{
    public class CertificationsAddRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int SeasonId { get; set; }

        [Required]
        public bool IsPhysicalRequired { get; set; }

        [Required]
        public bool IsBackgroundCheckRequired { get; set; }

        [Required] 
        public bool IsTestRequired { get; set; }

        #nullable enable
        [Range(1, Int32.MaxValue)]
        public int? TestId { get; set; }

        public decimal? MinimumScoreRequired { get; set; }
        #nullable disable

        [Required]
        public bool IsFitnessTestRequired { get; set; }
        
        [Required]
        public bool IsClinicRequired { get; set; }
        
        [Required]
        public DateTime DueDate { get; set; }
    }
}
