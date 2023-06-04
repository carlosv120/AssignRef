using Google.Apis.AnalyticsReporting.v4.Data;
using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Certifications;
using Sabio.Models.Domain.Conferences;
using Sabio.Models.Domain.Seasons;
using Sabio.Models.Domain.Tests;
using Sabio.Models.Requests.Certifications;
using Sabio.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace Sabio.Services
{
    public class CertificationsService : ICertificationsService
    {
        IDataProvider _data = null;
        IBaseUserMapper _baseUserMapper = null;
        public CertificationsService(IDataProvider data, IBaseUserMapper userMapper)
        {
            _data = data;
            _baseUserMapper = userMapper;
        }

        public int Add(CertificationsAddRequest modelCertification, int userId)
        {
            int id = 0;

            string procedureName = "dbo.Certifications_Insert";

            _data.ExecuteNonQuery(procedureName,
            inputParamMapper: delegate (SqlParameterCollection collection)
            {
                AddCertificationCommonParams(modelCertification, collection);
                collection.AddWithValue("@CreatedBy", userId);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;
                collection.Add(idOut);
            },
            returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object objectId = returnCollection["@Id"].Value;
                int.TryParse(objectId.ToString(), out id);
            });
            return id;
        }

        public void Update(CertificationsUpdateRequest modelCertification, int userId)
        {
            string procedureName = "dbo.Certifications_Update";

            _data.ExecuteNonQuery(procedureName,
            inputParamMapper: delegate(SqlParameterCollection collection)
            {
                AddCertificationCommonParams(modelCertification, collection);

                collection.AddWithValue("@ModifiedBy", userId);
                collection.AddWithValue("@Id", modelCertification.Id);

            }, returnParameters:null);
        }

        public Paged<Certification> GetBySeasonId(int pageIndex, int pageSize, int seasonId)
        {
            Paged<Certification> pagedList = null;
            List<Certification> list = null;

            int totalCount = 0;
            string procedureName = "dbo.Certifications_SelectBySeasonId";

            _data.ExecuteCmd(procedureName,
            inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@PageIndex", pageIndex);
                collection.AddWithValue("@PageSize", pageSize);
                collection.AddWithValue("@SeasonId", seasonId);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;

                Certification aCertification = MapCertification(reader, ref startingIndex);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(startingIndex);
                }

                if (list == null)
                {
                    list = new List<Certification>();
                }
                list.Add(aCertification);

            });

            if (list!= null)
            {
                pagedList = new Paged<Certification>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public void Delete(int id, int userId)
        {
            string procedureName = "dbo.Certifications_Delete";

            _data.ExecuteNonQuery(procedureName,
            inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@Id", id);
                collection.AddWithValue("@ModifiedBy", userId);
            },
            returnParameters: null);
        }

        public Paged<Certification> GetAll(int pageIndex, int pageSize) 
        {
            Paged<Certification> pagedList = null;
            List<Certification> list = null;

            int totalCount = 0;
            string procedureName = "dbo.Certifications_SelectAll";

            _data.ExecuteCmd(procedureName,
            inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@PageIndex", pageIndex);
                collection.AddWithValue("@PageSize", pageSize);

            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;

                Certification aCertification = MapCertification(reader, ref startingIndex);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(startingIndex);
                }

                if (list == null)
                {
                    list = new List<Certification>();
                }
                list.Add(aCertification);

            });

            if (list != null)
            {
                pagedList = new Paged<Certification>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<Certification> GetByConferenceId(int pageIndex, int pageSize, int conferenceId)
        {
            Paged<Certification> pagedList = null;
            List<Certification> list = null;

            int totalCount = 0;
            string procedureName = "dbo.Certifications_SelectByConferenceId";

            _data.ExecuteCmd(procedureName,
            inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@PageIndex", pageIndex);
                collection.AddWithValue("@PageSize", pageSize);
                collection.AddWithValue("@ConferenceId", conferenceId);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;

                Certification aCertification = MapCertification(reader, ref startingIndex);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(startingIndex);
                }

                if (list == null)
                {
                    list = new List<Certification>();
                }
                list.Add(aCertification);

            });

            if (list != null)
            {
                pagedList = new Paged<Certification>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        private Certification MapCertification(IDataReader reader, ref int startingIndex)
        {
            Certification singleCertification = new Certification();
            
            singleCertification.Season = new Season();
            singleCertification.Season.Conference = new BaseConference();
            singleCertification.Test = new TestResultBase();

            singleCertification.Id = reader.GetSafeInt32(startingIndex++);
            singleCertification.Name = reader.GetSafeString(startingIndex++);
            singleCertification.Season.Id = reader.GetSafeInt32(startingIndex++);
            singleCertification.Season.Name = reader.GetSafeString(startingIndex++);
            singleCertification.Season.Year = reader.GetSafeInt32(startingIndex++);

            singleCertification.Season.Conference.Id = reader.GetSafeInt32(startingIndex++);
            singleCertification.Season.Conference.Name = reader.GetSafeString(startingIndex++);
            singleCertification.Season.Conference.Code = reader.GetSafeString(startingIndex++);
            singleCertification.Season.Conference.Logo = reader.GetSafeString(startingIndex++);
            
            singleCertification.IsPhysicalRequired = reader.GetSafeBool(startingIndex++);
            singleCertification.IsBackgroundCheckRequired = reader.GetSafeBool(startingIndex++);
            singleCertification.IsTestRequired = reader.GetSafeBool(startingIndex++);
            
            singleCertification.Test.Id = reader.GetSafeInt32Nullable(startingIndex++);
            singleCertification.Test.Name = reader.GetSafeString(startingIndex++);
            singleCertification.Test.MinimumScoreRequired = reader.GetSafeDecimalNullable(startingIndex++);
            
            singleCertification.IsFitnessTestRequired = reader.GetSafeBool(startingIndex++);
            singleCertification.IsClinicRequired = reader.GetSafeBool(startingIndex++);
            singleCertification.DueDate = reader.GetSafeDateTime(startingIndex++);
            singleCertification.IsActive = reader.GetSafeBool(startingIndex++);

            singleCertification.CreatedBy = _baseUserMapper.MapBaseUser(reader, ref startingIndex);
            singleCertification.ModifiedBy = _baseUserMapper.MapBaseUser(reader, ref startingIndex);

            singleCertification.DateCreated = reader.GetSafeDateTime(startingIndex++);
            singleCertification.DateModified = reader.GetSafeDateTime(startingIndex++);


            return singleCertification;
        }

        public void AddCertificationCommonParams(CertificationsAddRequest modelCertification, SqlParameterCollection collection)
        {
            collection.AddWithValue("@Name", modelCertification.Name);
            collection.AddWithValue("@SeasonId", modelCertification.SeasonId);
            collection.AddWithValue("@IsPhysicalRequired", modelCertification.IsPhysicalRequired);
            collection.AddWithValue("@IsBackgroundCheckRequired", modelCertification.IsBackgroundCheckRequired);
            collection.AddWithValue("@IsTestRequired", modelCertification.IsTestRequired);
            collection.AddWithValue("@TestId", modelCertification.TestId);
            collection.AddWithValue("@MinimumScoreRequired", modelCertification.MinimumScoreRequired);
            collection.AddWithValue("@IsFitnessTestRequired", modelCertification.IsFitnessTestRequired);
            collection.AddWithValue("@IsClinicRequired", modelCertification.IsClinicRequired);
            collection.AddWithValue("@DueDate", modelCertification.DueDate);
        }

    }
}