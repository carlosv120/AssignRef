using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Locations;
using Sabio.Models.Requests.Locations;
using System.Collections.Generic;
using System.Data;


namespace Sabio.Services.Locations
{
    public interface ILocationsService
    {
        int Add(LocationsAddRequest model, int userId);
        void Delete(int id);
        Location Get(int id);
        Paged<Location> GetAll(int pageIndex, int pageSize);
        Paged<Location> GetByUser(int userId, int pageIndex, int pageSize);
        void Update(LocationsUpdateRequest model, int userId);
        List<Location> GetByGeo(double latitude, double longitude, int radius);
    }
}