using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Tests
{
    public class TestResultBase
    {
        #nullable enable
        public int? Id { get; set; }

        #nullable enable
        public string? Name { get; set; }

        #nullable enable
        public decimal? MinimumScoreRequired { get; set; }

    }
}
