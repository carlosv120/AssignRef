using Sabio.Models.Domain.Tests;
using System;
using System.Collections.Generic;

namespace Sabio.Models.Domain.Tests
{
    public class Test
    {
        public List<TestQuestion> TestQuestions { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public LookUp StatusType { get; set; }
        public LookUp TestType { get; set; }
        public BaseUser CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int ConferenceId { get; set; }
    }
}
