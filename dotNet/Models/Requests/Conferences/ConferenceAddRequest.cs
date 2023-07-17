using Sabio.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Conferences
{
    public class ConferenceAddRequest
    {
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Name { get; set; }


        [Required]
        [Range(0, Int32.MaxValue)]
        public int LevelId { get; set; }


#nullable enable
        public string? Code { get; set; }
        public string? Logo { get; set; }
        public string? Phone { get; set; }
        public string? SiteUrl { get; set; }

#nullable disable
    }
}
