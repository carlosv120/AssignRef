using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Tests;
using Sabio.Models.Requests.Tests;
using System.Collections.Generic;
using System.Data;

namespace Sabio.Services.Interfaces
{
    public interface ITestService
    {
        Test Get(int id);
        int Add(TestAddRequest model, int userId);
        void Update(TestUpdateRequest model);
        void Delete(int id);
        Paged<Test> GetCreatedBy(int pageIndex, int pageSize, int createdBy);
        Paged<Test> GetAll(int pageIndex, int pageSize);
        Paged<Test> Search(int pageIndex, int pageSize, string query);
        TestV1 MapTestV1(IDataReader reader, ref int startingIndex);
        List<Test> GetByConferenceId(int conferenceId);
    }
}
