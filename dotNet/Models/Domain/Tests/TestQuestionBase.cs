using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Tests
{
    public class BaseTestQuestion
    {
        public int Id { get; set; }
        public LookUp QuestionType { get; set; }
        public string Question { get; set; }
        public string HelpText { get; set; }
        public bool IsRequired { get; set; }
        public int SortOrder { get; set; }
    }
}
