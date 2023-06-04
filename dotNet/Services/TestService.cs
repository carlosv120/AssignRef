using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Tests;
using Sabio.Models.Requests.Tests;
using Sabio.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sabio.Services
{
    public class TestService : ITestService
    {
        IDataProvider _data = null;
        ILookUpService _lookUpService = null;
        IBaseUserMapper _baseUserMapper = null;

        public TestService(IDataProvider data,  ILookUpService lookUpService, IBaseUserMapper baseUserMapper)
        {
            _data = data;
            _lookUpService = lookUpService;
            _baseUserMapper = baseUserMapper;
        }

        public Paged<Test> Search(int pageIndex, int pageSize, string query)
        {
            Paged<Test> pagedList = null;
            List<Test> list = null;
            int totalCount = 0;
            string procName = "[dbo].[Tests_Search]";

            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
                param.AddWithValue("@Query", query);
            }, (reader, recordSetIndex) =>
            {
                int startingIndex = 0;
                Test test = MapTest(reader, ref startingIndex);

                if(totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(startingIndex);
                }

                if (list == null)
                {
                    list = new List<Test>();
                }
                list.Add(test);
            });

            if(list != null)
            {
                pagedList = new Paged<Test>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<Test> GetAll(int pageIndex, int pageSize)
        {
            Paged<Test> pagedList = null;
            List<Test> list= null;
            int totalCount = 0;
            string procName = "[dbo].[Tests_SelectAll]";

            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
            }, (reader, recordSetIndex) =>
            {
                int startingIndex = 0;
                Test test = MapTest(reader, ref startingIndex);

                if(totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(startingIndex);
                }

                if (list == null)
                {
                    list = new List<Test>();
                }
                list.Add(test);
            });

            if(list != null)
            {
                pagedList = new Paged<Test>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<Test> GetCreatedBy(int pageIndex, int pageSize, int createdBy)
        {
            Paged<Test> pagedList = null;
            List<Test> list = null;
            int totalCount = 0;
            string procName = "[dbo].[Tests_Select_ByCreatedBy]";

            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
                param.AddWithValue("@CreatedBy", createdBy);
            }, (reader, recordSetIndex) =>
            {
                int startingIndex = 0;
                Test test = MapTest(reader, ref startingIndex);

                if(totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(startingIndex);
                }
                
                if (list == null)
                {
                    list = new List<Test>();
                }
                list.Add(test);
            });

            if(list != null)
            {
                pagedList = new Paged<Test>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Tests_DeleteById]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            });
        }

        public void Update(TestUpdateRequest model)
        {
            string procName = "[dbo].[Tests_Update]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);
                    col.AddWithValue("@Id", model.Id);
                },
                returnParameters: null);
        }

        public int Add(TestAddRequest model, int userId) 
        {
            int Id = 0;

            string procName = "[dbo].[Tests_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                AddCommonParams(model, col);
                col.AddWithValue("@CreatedBy", userId);
                col.Add(idOut);
            },
            returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;

                int.TryParse(oId.ToString(), out Id);
            });
            return Id;
        }

        private static void AddCommonParams(TestAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Name", model.Name);
            col.AddWithValue("@Description", model.Description);
            col.AddWithValue("@StatusId", model.StatusId);
            col.AddWithValue("@TestTypeId", model.TestTypeId);
        }

        public Test Get(int id)
        {
            string procName = "[dbo].[Tests_SelectById]";

            Test test = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                test = MapTest(reader, ref startingIndex);
            });
            return test;
        }

        public Test MapTest(IDataReader reader, ref int startingIndex)
        {
            Test test = new Test();
           
            test.Id = reader.GetSafeInt32(startingIndex++);
            test.Name = reader.GetSafeString(startingIndex++);
            test.Description = reader.GetSafeString(startingIndex++);
            test.StatusType = new LookUp();
            test.StatusType = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            test.TestType = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            test.TestQuestions = reader.DeserializeObject<List<TestQuestion>>(startingIndex++);
            test.DateCreated = reader.GetDateTime(startingIndex++);
            test.DateModified = reader.GetDateTime(startingIndex++);
            test.CreatedBy = new BaseUser();
            test.CreatedBy.Id = reader.GetSafeInt32(startingIndex++);
            test.CreatedBy.FirstName = reader.GetSafeString(startingIndex++);
            test.CreatedBy.LastName = reader.GetSafeString(startingIndex++);
            test.CreatedBy.Mi = reader.GetSafeString(startingIndex++);
            test.CreatedBy.AvatarUrl = reader.GetSafeString(startingIndex++);

            return test;
        }

        public TestV1 MapTestV1(IDataReader reader, ref int startingIndex)
        {
            TestV1 test = new TestV1();

            test.Id = reader.GetSafeInt32(startingIndex++);
            test.Name = reader.GetSafeString(startingIndex++);
            test.Description = reader.GetSafeString(startingIndex++);
            test.StatusType = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            test.TestType = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            test.CreatedBy = reader.GetSafeInt32(startingIndex++);
            test.DateCreated = reader.GetDateTime(startingIndex++);
            test.DateModified = reader.GetDateTime(startingIndex++);
            return test;
        }

        public List<Test> GetByConferenceId(int id)
        {
            string procName = "[dbo].[Tests_SelectByConferenceId]";
            List<Test> list = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@ConferenceId", id);
            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                Test aTest = MapTestV2(reader, ref startingIndex);

                if (list  == null)
                {
                    list = new List<Test>();
                }
                list.Add(aTest);


            });
            return list;
        }

        public Test MapTestV2(IDataReader reader, ref int startingIndex)
        {
            Test test = new Test();

            test.Id = reader.GetSafeInt32(startingIndex++);
            test.Name = reader.GetSafeString(startingIndex++);
            test.Description = reader.GetSafeString(startingIndex++);
            test.StatusType = new LookUp();
            test.StatusType = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            test.TestType = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            test.TestQuestions = reader.DeserializeObject<List<TestQuestion>>(startingIndex++);

            test.ConferenceId = reader.GetSafeInt32(startingIndex++);

            test.DateCreated = reader.GetDateTime(startingIndex++);
            test.DateModified = reader.GetDateTime(startingIndex++);
            test.CreatedBy = _baseUserMapper.MapBaseUser(reader, ref startingIndex);


            return test;
        }

    }
}
