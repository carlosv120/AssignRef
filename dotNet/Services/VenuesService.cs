using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Venues;
using Sabio.Models.Requests.Venues;
using Sabio.Services.Interfaces;
using Sabio.Services.Locations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Venue = Sabio.Models.Domain.Venues.Venue;

namespace Sabio.Services
{
    public class VenuesService : IVenuesService, IMapBaseVenue
    {
        IDataProvider _data = null;
        ILocationMapper _locationMapper = null;
        IBaseUserMapper _userMapper = null;
      
        public VenuesService(IDataProvider data, ILocationMapper locationMapper, IBaseUserMapper userMapper)
        {
            _data = data;
            _locationMapper = locationMapper;
            _userMapper = userMapper;
        }
        public int Add(VenueAddRequest model, int userId)
        {
            int id = 0;
            string procName = "[dbo].[Venues_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@CreatedBy", userId);
                col.AddWithValue("@ModifiedBy", userId);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);
            },
            returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;
                int.TryParse(oId.ToString(), out id);
            });
            return id;
        }

        public Venue SelectById(int id)
        {
            string procName = "[dbo].[Venues_Select_ById]";
            Venue venues = null;
            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                venues = MapSingleVenue(reader, ref startingIndex);
            }
            );
            return venues;
        }
        public Paged<Venue> GetAll(int pageIndex, int pageSize)
        {
            Paged<Venue> pagedList = null;
            List <Venue> list = null;
            int totalCount = 0;

            _data.ExecuteCmd("[dbo].[Venues_SelectAll]", (param) =>
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
            }, 
            (reader, recordSetIndex) =>
            {
                int startingIndex = 0;
                Venue venue = MapSingleVenue(reader, ref startingIndex);
                totalCount = reader.GetSafeInt32(startingIndex);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(startingIndex);
                }
                if (list == null)
                {
                    list = new List<Venue>();
                }
                list.Add(venue);
            });
                if (list != null)
            {
                pagedList = new Paged<Venue>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public void Update(VenueUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Venues_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@Id", model.Id);
                col.AddWithValue("@ModifiedBy", userId);
            }, returnParameters: null);
        }
        public Paged<Venue> Search (int pageIndex, int pageSize, string query)
        {
            Paged<Venue> pagedList = null;
            List<Venue> list = null;
            int totalCount = 0;

            _data.ExecuteCmd(
                "[dbo].[Venues_Search]", (SqlParameterCollection col) =>
                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);
                    col.AddWithValue("@Query", query);

                },
                (reader, readerSetIndex) =>
                {
                    int startingIndex = 0;
                    Venue venue = MapSingleVenue(reader, ref startingIndex);
                  
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex);
                    }
                    if (list == null)
                    {
                        list = new List<Venue>();
                    }
                    list.Add(venue);
                });
            if(list != null)
            {
                pagedList =new Paged<Venue>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public void Delete(int id)
        {
            string procName = "[dbo].[Venue_Delete_ById]";
            _data.ExecuteNonQuery(procName, 
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            },
            returnParameters: null);
        }
        
        private  Venue MapSingleVenue(IDataReader reader, ref int startingIndex)
        {
            Venue venue = new Venue();

            venue.Id = reader.GetSafeInt32(startingIndex++);
            venue.Name = reader.GetString(startingIndex++);
            venue.Location = _locationMapper.MapLocation(reader, ref startingIndex);        
            venue.PrimaryImageUrl = reader.GetSafeString(startingIndex++);
            venue.Description = reader.GetString(startingIndex++);
            venue.Url = reader.GetSafeString(startingIndex++);
            venue.CreatedBy = _userMapper.MapBaseUser(reader, ref startingIndex);
            venue.ModifiedBy = _userMapper.MapBaseUser(reader, ref startingIndex);
            venue.DateCreated = reader.GetSafeDateTime(startingIndex++);
            venue.DateModified = reader.GetSafeDateTime(startingIndex++);

            return venue;

        }

        public BaseVenue MapBaseVenue(IDataReader reader, ref int startingIndex)
        {
            BaseVenue venue = new BaseVenue();

            venue.Id = reader.GetSafeInt32(startingIndex++);
            venue.Name = reader.GetString(startingIndex++);
            venue.Location = _locationMapper.MapLocation(reader, ref startingIndex);
            venue.PrimaryImageUrl = reader.GetSafeString(startingIndex++);

            return venue;
        }
        private static void AddCommonParams(VenueAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Name", model.Name);
            col.AddWithValue("@Description", model.Description);
            col.AddWithValue("@LocationId", model.LocationId);
            col.AddWithValue("@Url", model.Url);
            col.AddWithValue("@PrimaryImageUrL", model.PrimaryImageUrL);
        }
    }
}
