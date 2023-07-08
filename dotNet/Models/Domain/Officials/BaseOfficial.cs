using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Officials
{
    public class BaseOfficial
    {
        public int Id { get; set; }

        public BaseUser User { get; set; }

        public string Gender { get; set; }

        public LookUp3Col Position { get; set; }
    }
}
