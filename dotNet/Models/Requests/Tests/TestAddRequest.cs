using Sabio.Models.Domain;
using Sabio.Models.Domain.Tests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Tests
{
    public class TestAddRequest
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(2000)]
        public string Description { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required]
        public int TestTypeId { get; set; }
    }
}
