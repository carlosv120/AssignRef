using Sabio.Models.Domain.Conferences;
using Sabio.Models.Domain.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Officials
{
    public class Official : BaseOfficial
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public LookUp3Col PrimaryPosition { get; set; }
        public Location Location { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public LookUp StatusType { get; set; }
        public List<BaseConference> Conferences { get; set; }

    }
}
