﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Officials
{
    public class OfficialUpdateRequest : OfficialAddRequest, IModelIdentifier
    {
        public int Id { get; set; }
    }
}
