using Sabio.Models;
using Sabio.Models.Domain.Officials;
using Sabio.Models.Requests.Officials;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IOfficialService
    {
        int Add(OfficialAddRequest model , int userId);

        void Delete(int Id);

        Official GetByUser(int userId);

        List<Official> GetByConference(int conferenceId);

        List<Official> GetByPosition(int positionId);

        Paged<Official> GetAll(int pageIndex, int pageSize);

        Paged<Official> Search(int pageIndex, int pageSize, string query);

        void Update(OfficialUpdateRequest model);

        List<BaseOfficial> GetByMissingCertification(int conferenceId, int certificationId);

    }
}