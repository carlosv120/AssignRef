using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Tests
{
    public class TestUpdateRequest : TestAddRequest, IModelIdentifier
    {
        [Required]
        public int Id { get; set; }
    }
}
