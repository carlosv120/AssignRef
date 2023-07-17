using Sabio.Models.Domain.Candidates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Candidates
{
    public class CandidateAddRequest
    {

        [Required]
        [Range(1, int.MaxValue)]
        public int PrimaryPositionId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int LocationId { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int ConferenceId { get; set; }
        [Required]
        [Range(0, 120)]
        public int YearsExperience { get; set; }
        [Required]
        [Range(0, 120)]
        public int CollegeExperience { get; set; }

    }
}