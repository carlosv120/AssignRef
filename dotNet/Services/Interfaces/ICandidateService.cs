using Sabio.Models.Domain.Candidates;
using Sabio.Models;
using Sabio.Models.Requests.Candidates;

namespace Sabio.Services.Interfaces
{
    public interface ICandidateService
    {
        int Add(CandidateAddRequest model, int userId);

        Paged<Candidate> SelectAll(int pageIndex, int pageSize, int statusTypeId, int conferenceId);

        Paged<Candidate> SearchPaginationByConferenceId(int pageIndex, int pageSize, int query);

        Paged<Candidate> SearchPaginationByPositionId(int pageIndex, int pageSize, int query);

        Paged<Candidate> SearchPaginationByUserId(int pageIndex, int pageSize, int query);

        Paged<Candidate> SearchPaginationByName(int pageIndex, int pageSize, string query, int statusTypeId, int conferenceId);

        void Update(CandidateUpdateRequest model);

        void Delete(int id);

    }
}