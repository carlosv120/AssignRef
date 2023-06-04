using Sabio.Models.Domain.Seasons;
using Sabio.Models.Domain.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Certifications
{
    public class Certification
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public Season Season { get; set; }

        public bool IsPhysicalRequired { get; set; }

        public bool IsBackgroundCheckRequired { get; set; }

        public bool IsTestRequired { get; set; }

        public TestResultBase Test { get; set; }

        public bool IsFitnessTestRequired { get; set; }

        public bool IsClinicRequired { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsActive { get; set; }

        public BaseUser CreatedBy { get; set; }

        public BaseUser ModifiedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }


    }
}
