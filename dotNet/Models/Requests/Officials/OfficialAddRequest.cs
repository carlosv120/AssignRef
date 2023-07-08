using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Officials
{
    public class OfficialAddRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int PrimaryPositionId { get; set; }

        [Required]
        public int LocationId { get; set; }

    }
}
