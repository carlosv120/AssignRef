using Sabio.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain.Seasons;
using Sabio.Data;
using Sabio.Models.Requests.Seasons;
using System.Reflection.PortableExecutable;
using System.Reflection;
using Sabio.Web.Core.Services;
using Sabio.Models.Domain.Conferences;
using Sabio.Services.Interfaces;
using Sabio.Models.Domain.Crews;
using Microsoft.AspNetCore.Http;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.SqlServer.Server;
using Sabio.Models.Domain.Games;
using Sabio.Models;

namespace Sabio.Services
{

    public class SeasonService : ISeasonService, IMapBaseSeason
    {

        IDataProvider _data = null;
        ILookUpService _lookUpService = null;

        public SeasonService(IDataProvider data, ILookUpService lookUpService)
        {
            _data = data;
            _lookUpService = lookUpService;
            //_formCollection = formCollection;
        }

        public Season GetSeasonById(int id)
        {
            string procName = "[dbo].[Seasons_Select_ById]";

            Season season = null;
            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            },
            delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                season = MapSingleSeason(reader, ref startingIndex);
            });
            return season;
        }

        public List<Season> GetSeasonsByConferenceId(int id)
        {
            string procName = "[dbo].[Seasons_Select_ByConferenceId]";

            List<Season> seasons = new List<Season>();
            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            },
            delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                Season season = MapSingleSeason(reader, ref startingIndex);
                seasons.Add(season);
            });
            return seasons;
        }


        public Paged<Season> GetByConferencePaginate(int pageIndex, int pageSize, int conferenceId)
        {
            string proc = "[dbo].[Seasons_Select_ByConferenceIdPaginateV2]";
            Paged<Season> pagedList = null;
            List<Season> list = null;
            int totalCount = 0;

            _data.ExecuteCmd(proc, (param) =>
            {

                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
                param.AddWithValue("@Id", conferenceId);
            }, (reader, recordSetIndex) =>
            {
                int startingIndex = 0;
                Season season = MapSingleSeason(reader, ref startingIndex);
                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(startingIndex++);

                }

                if (list == null)
                {
                    list = new List<Season>();
                }
                list.Add(season);
            });
            if (list != null)
            {
                pagedList = new Paged<Season>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }



        public List<Season> GetAll()
        {
            string procName = "[dbo].[Seasons_SelectAll]";

            List<Season> seasons = new List<Season>();
            _data.ExecuteCmd(procName, null,
                delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    Season season = MapSingleSeason(reader, ref startingIndex);
                    seasons.Add(season);
                }
            );
            return seasons;
        }

        public int AddSeason(SeasonAddRequest model)
        {

            int id = 0;

            string procName = "[dbo].[Seasons_Insert]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);

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

        public void UpdateSeason(SeasonUpdateRequest model)
        {
            string procName = "[dbo].[Seasons_Update]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", model.Id);
                    AddCommonParams(model, col);

                },
                returnParameters: null);
        }

        public void DeleteSeasonById(int id)
        {
            string procName = "[dbo].[Seasons_DeleteV2]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);
                });
        }

        public void UpdateSeasonStatus(int id, int statusId)
        {
            string procName = "[dbo].[Seasons_Update_Status]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);
                    col.AddWithValue("@NewStatus", statusId);
                });
        }

        private Season MapSingleSeason(IDataReader reader, ref int startingIndex)
        {

            var aSeason = new Season
            {
                Id = reader.GetSafeInt32(startingIndex++),
                Name = reader.GetSafeString(startingIndex++),
                Year = reader.GetSafeInt32(startingIndex++),
                Conference = new BaseConference
                {
                    Id = reader.GetSafeInt32(startingIndex++),
                    Name = reader.GetSafeString(startingIndex++),
                    Code = reader.GetSafeString(startingIndex++),
                    Logo = reader.GetSafeString(startingIndex++)
                },
                StatusType = _lookUpService.MapSingleLookUp(reader, ref startingIndex),
                Weeks = reader.GetSafeInt32(startingIndex++),
                DateCreated = reader.GetDateTime(startingIndex++),
                DateModified = reader.GetDateTime(startingIndex++)
            };

            return aSeason;
        }

        public BaseSeason MapBaseSeason(IDataReader reader, ref int startingIndex)
        {
            BaseSeason aSeason = new BaseSeason();
            aSeason.Id = reader.GetSafeInt32(startingIndex++);
            aSeason.Name = reader.GetSafeString(startingIndex++);
            aSeason.Year = reader.GetSafeInt32(startingIndex++);

            return aSeason;
        }

        private static void AddCommonParams(SeasonAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Name", model.Name);
            col.AddWithValue("@Year", model.Year);
            col.AddWithValue("@ConferenceId", model.ConferenceId);
            col.AddWithValue("@Weeks", model.Weeks);
        }

    }
}
