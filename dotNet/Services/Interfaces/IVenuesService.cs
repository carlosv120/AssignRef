using Sabio.Models;
using Sabio.Models.Domain.Venues;
using Sabio.Models.Requests.Venues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services.Interfaces
{
    public interface IVenuesService
    {
        int Add(VenueAddRequest model, int userId);
        Venue SelectById(int id);
        Paged<Venue> GetAll(int pageIndex, int pageSize);
        void Update(VenueUpdateRequest model, int userId);
        Paged<Venue> Search(int pageIndex, int pageSize, string query);
        void Delete(int id);
    }
}
