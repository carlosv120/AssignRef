using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Locations;
using Sabio.Models.Requests.Locations;
using Sabio.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sabio.Services.Locations
{
    public class LocationsService : ILocationsService, ILocationMapper
    {
        IDataProvider _data = null;

        ILookUpService _lookUpService = null;

        public LocationsService(IDataProvider data, ILookUpService lookUpService)
        {
            _data = data;

            _lookUpService = lookUpService;
        }

        public List<Location> GetByGeo(double latitude, double longitude, int radius)
        {
            List<Location> list = null;

            string procName = "[dbo].[Locations_Select_ByGeo]";

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection col)

            {
                col.AddWithValue("@Latitude", latitude);
                col.AddWithValue("@Longitude", longitude);
                col.AddWithValue("@Radius", radius);
            }, delegate (IDataReader reader, short set)
            {
                int index = 0;
                Location location = MapLocation(reader, ref index);

                if (list == null)
                {
                    list = new List<Location>();
                }
                list.Add(location);
            });

            return list;
        }

        public Paged<Location> GetByUser( int pageIndex, int pageSize, int userId)
        {
            Paged<Location> pagedList = null;

            List<Location> list = null;

            int totalCount = 0;

            string procName = "[dbo].[Locations_Select_ByCreatedBy]";

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection col)

            {
                col.AddWithValue("@CreatedBy", userId);
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);

            }, delegate (IDataReader reader, short set)
            {
                int index = 0;
                Location location = MapLocation(reader, ref index);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index++);
                }

                if (list == null)
                {
                    list = new List<Location>();
                }
                list.Add(location);
            });

            if (list != null)
            {
                pagedList = new Paged<Location>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public Location Get(int id)
        {
            string procName = "[dbo].[Locations_Select_ById]";
            Location location = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {
                int index = 0;

                location = MapLocation(reader, ref index);
            });

            return location;
        }

        public Paged<Location> GetAll(int pageIndex, int pageSize)
        {
            Paged<Location> pagedList = null;

            List<Location> list = null;

            int totalCount = 0;

            string procName = "[dbo].[Locations_SelectAll]";

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection col)

                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);
                }, delegate (IDataReader reader, short set)
                {
                    int index = 0;
                    Location location = MapLocation(reader, ref index);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(index++);
                    }

                    if (list == null)
                    {
                        list = new List<Location>();
                    }
                    list.Add(location);
                });

            if (list != null)
            {
                pagedList = new Paged<Location>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public int Add(LocationsAddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[Locations_Insert]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);
                AddCommonLocation(model, col);
                col.AddWithValue("@CreatedBy", userId);
                col.AddWithValue("@ModifiedBy", userId);
            }, returnParameters: delegate (SqlParameterCollection returnCol)
            {
                object oId = returnCol["@Id"].Value;

                int.TryParse(oId.ToString(), out id);
            });

            return id;
        }

        public void Update(LocationsUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Locations_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonLocation(model, col);
                col.AddWithValue("@Id", model.Id);
                col.AddWithValue("@CreatedBy", userId);
                col.AddWithValue("@ModifiedBy", userId);
            }, returnParameters: null);
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Locations_Delete_ById]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            }, returnParameters: null);
        }

        public Location MapLocation(IDataReader reader, ref int startingIndex)
        {
            Location location = new Location();

            location.Id = reader.GetSafeInt32(startingIndex++);
            location.LineOne = reader.GetSafeString(startingIndex++);
            location.LineTwo = reader.GetSafeString(startingIndex++);
            location.City = reader.GetSafeString(startingIndex++);
            location.State = _lookUpService.MapLookUp3Col(reader, ref startingIndex);
            location.Zip = reader.GetSafeString(startingIndex++);
            location.Lat = reader.GetSafeDouble(startingIndex++);
            location.Long = reader.GetSafeDouble(startingIndex++);
            location.LocationType = _lookUpService.MapSingleLookUp(reader, ref startingIndex);


            return location;
        }

        private static void AddCommonLocation(LocationsAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@LocationTypeId", model.LocationTypeId);
            col.AddWithValue("@LineOne", model.LineOne);
            col.AddWithValue("@LineTwo", model.LineTwo);
            col.AddWithValue("@City", model.City);
            col.AddWithValue("@Zip", model.Zip);
            col.AddWithValue("@StateId", model.StateId);
            col.AddWithValue("@Latitude", model.Latitude);
            col.AddWithValue("@Longitude", model.Longitude);
        }
        
    }
}
