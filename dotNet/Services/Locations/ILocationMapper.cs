using Sabio.Models.Domain.Locations;
using System.Data;

namespace Sabio.Services.Locations
{
    public interface ILocationMapper
    {
        Location MapLocation(IDataReader reader, ref int startingIndex);
    }
}