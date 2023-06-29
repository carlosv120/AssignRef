using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Conferences;
using Sabio.Models.Domain.Locations;
using Sabio.Models.Domain.ReplayEntries;
using Sabio.Models.Domain.TeamMembers;
using Sabio.Models.Domain.Teams;
using Sabio.Models.Requests.TeamsRequest;
using Sabio.Services.Interfaces;
using Sabio.Services.Locations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class TeamService : ITeamService, IMapSingleTeam
    {
        IDataProvider _dataProvider = null;
        IMapBaseConference _mapBaseConference = null;
        ILookUpService _lookUpService = null;
        IMapBaseVenue _mapBaseVenue = null;
        ILocationMapper _locationMapper = null;

        public TeamService(IDataProvider dataProvider, ILookUpService lookUpService, IMapBaseConference mapBaseConference, IMapBaseVenue mapBaseVenue, ILocationMapper locationMapper)
        {
            _dataProvider = dataProvider;
            _mapBaseConference = mapBaseConference;
            _mapBaseVenue = mapBaseVenue;
            _lookUpService= lookUpService;
            _locationMapper = locationMapper;
        }
        public Team TeamsGetById(int id)
        {
            string procName = "[dbo].[Teams_SelectByIdV2]";
            Team model = null;

            _dataProvider.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                model = MapSingleTeam(reader, ref startingIndex);

            });
            return model;
        }

        public List<Team> GetAllTeams()
        {
            string procName = "[dbo].[Teams_Get_All]";
            List<Team> list = null;

            _dataProvider.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {
                
            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                Team aTeam = MapSingleTeam(reader, ref startingIndex);

                if(list == null)
                {
                    list = new List<Team>();
                }
                list.Add(aTeam);
            });
            return list;
        }
        public List<Team> TeamsGetByConferenceId(int Id)
        {
            string procName = "[dbo].[Teams_SelectByConferenceIdV2]";
            List<Team> list = null;

            _dataProvider.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", Id);

            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                Team aTeam = MapSingleTeam(reader, ref startingIndex);

                if (list == null)
                {
                    list = new List<Team>();
                }

                list.Add(aTeam);
            }
            );
            return list;
        }
        public Paged<Team> TeamsSelectAll(int pageIndex, int pageSize)
        {
            string proc = "[dbo].[Teams_SelectAllV2]";
            Paged<Team> pagedList = null;
            List<Team> list = null;
            int totalCount = 0;

            _dataProvider.ExecuteCmd(proc, (param) =>
            {

                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
            }, (reader, recordSetIndex) =>
            {
                int index = 0;
                Team model = MapSingleTeam(reader, ref index);
                totalCount = reader.GetSafeInt32(index);

                if (list == null)
                {
                    list = new List<Team>();
                }
                list.Add(model);
            });
            if (list != null)
            {
                pagedList = new Paged<Team>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public Paged<Team> TeamsSearch(int pageIndex, int pageSize, string query)
        {
            string proc = "[dbo].[Teams_SearchV2]";
            Paged<Team> pagedList = null;
            List<Team> list = null;
            int totalCount = 0;

            _dataProvider.ExecuteCmd(proc, (param) =>
            {

                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
                param.AddWithValue("@Query", query);
            }, (reader, recordSetIndex) =>
            {
                int index = 0;
                Team model = MapSingleTeam(reader, ref index);
                if (totalCount == 0)
                { totalCount = reader.GetSafeInt32(index); }


                if (list == null)
                {
                    list = new List<Team>();
                }
                list.Add(model);
            });
            if (list != null)
            {
                pagedList = new Paged<Team>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public Paged<Team> SearchInConference(int pageIndex, int pageSize, string query, int conferenceId)
        {
            string proc = "[dbo].[Teams_SearchByConferenceV2]"; 
            Paged<Team> pagedList = null;
            List<Team> list = null;
            int totalCount = 0;

            _dataProvider.ExecuteCmd(proc, (param) =>
            {

                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
                param.AddWithValue("@Query", query);
                param.AddWithValue("@ConferenceId", conferenceId);

            }, (reader, recordSetIndex) =>
            {
                int index = 0;
                Team model = MapSingleTeam(reader, ref index);
                if (totalCount == 0)
                { totalCount = reader.GetSafeInt32(index); }


                if (list == null)
                {
                    list = new List<Team>();
                }
                list.Add(model);
            });
            if (list != null)
            {
                pagedList = new Paged<Team>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public Paged<Team> GetByConferencePaginate(int pageIndex, int pageSize, int conferenceId)
        {
            string proc = "[dbo].[Teams_SelectByConferenceId_PaginatedV2]";
            Paged<Team> pagedList = null;
            List<Team> list = null;
            int totalCount = 0;

            _dataProvider.ExecuteCmd(proc, (param) =>
            {

                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
                param.AddWithValue("@Id", conferenceId);

            }, (reader, recordSetIndex) =>
            {
                int index = 0;
                Team model = MapSingleTeam(reader, ref index);
                if (totalCount == 0)
                { totalCount = reader.GetSafeInt32(index); }


                if (list == null)
                {
                    list = new List<Team>();
                }
                list.Add(model);
            });
            if (list != null)
            {
                pagedList = new Paged<Team>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public void TeamsUpdate(TeamUpdateRequest request, int id)
        {
            string procname3 = "[dbo].[Teams_Update]";

            _dataProvider.ExecuteNonQuery(procname3, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", request.Id);                                
                AddCommonParams(request, col);

            }, returnParameters: null);
        }
        public int TeamsAdd(TeamAddRequest request, int userId)
        {
            int id = 0;

            string procname4 = "[dbo].[Teams_Insert]";

            _dataProvider.ExecuteNonQuery(procname4, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(request, col);
                col.AddWithValue("@CreatedBy", userId);                

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);
            }, returnParameters: delegate (SqlParameterCollection returnCol)
            {
                object oId = returnCol["@Id"].Value;
                int.TryParse(oId.ToString(), out id);
            });
            return id;
        }
        public void TeamsDelete(int id)
        {
            string procname5 = "[dbo].[Teams_Delete]";

            _dataProvider.ExecuteNonQuery(procname5, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            }, returnParameters: null);
        }
        public Team MapSingleTeam(IDataReader reader, ref int startingIndex)
        {
            Team model = new Team();

            model.Id = reader.GetSafeInt32(startingIndex++);
            model.Name = reader.GetSafeString(startingIndex++);
            model.Code = reader.GetSafeString(startingIndex++);
            model.Logo = reader.GetSafeString(startingIndex++);
            model.Conference = _mapBaseConference.MapBaseConference(reader, ref startingIndex);
            model.Location = _locationMapper.MapLocation(reader, ref startingIndex);
            model.MainVenue = _mapBaseVenue.MapBaseVenue(reader, ref startingIndex);
            model.PrimaryColor = reader.GetSafeString(startingIndex++);
            model.SecondaryColor = reader.GetSafeString(startingIndex++);
            model.Phone = reader.GetSafeString(startingIndex++);
            model.SiteUrl = reader.GetSafeString(startingIndex++);
            model.Status = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            model.TeamMembers = reader.DeserializeObject<List<BaseTeamMember>>(startingIndex++);
            model.DateCreated = reader.GetSafeDateTime(startingIndex++);
            model.DateModified = reader.GetSafeDateTime(startingIndex++);

            return model;
        }
        private static void AddCommonParams(TeamAddRequest request, SqlParameterCollection col)
        {
            col.AddWithValue("@Name", request.Name);
            col.AddWithValue("@Code", request.Code);
            col.AddWithValue("@Logo", request.Logo);
            col.AddWithValue("@ConferenceId", request.ConferenceId);
            col.AddWithValue("@LocationId", request.LocationId);
            col.AddWithValue("@MainVenueId", request.MainVenueId);
            col.AddWithValue("@PrimaryColor", request.PrimaryColor);
            col.AddWithValue("@SecondaryColor", request.SecondaryColor);
            col.AddWithValue("@Phone", request.Phone);
            col.AddWithValue("@SiteUrl", request.SiteUrl);
            col.AddWithValue("@StatusTypeId", request.StatusTypeId);
        }
    }
}
