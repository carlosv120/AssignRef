using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain;

namespace Sabio.Models.Domain.Tests
{
    public class TestQuestion: BaseTestQuestion
    {
        public List<AnswerOption> AnswerOption { get; set; }

    }
}
