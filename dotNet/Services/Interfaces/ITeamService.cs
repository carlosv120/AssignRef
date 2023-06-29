using Sabio.Models;
using Sabio.Models.Domain.Teams;
using Sabio.Models.Requests.TeamsRequest;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface ITeamService
    {
        void TeamsDelete(int id);
        List<Team> TeamsGetByConferenceId(int id);
        Team TeamsGetById(int id);
        Paged<Team> TeamsSelectAll(int pageIndex, int pageSize);
        void TeamsUpdate(TeamUpdateRequest request, int id);
        int TeamsAdd(TeamAddRequest request, int userId);
        Paged<Team> TeamsSearch(int pageIndex, int pageSize, string query);
        Paged<Team> SearchInConference(int pageIndex, int pageSize, string query, int conferenceId);
        Paged<Team> GetByConferencePaginate(int pageIndex, int pageSize, int conferenceId);

        public List<Team> GetAllTeams();
    }
}
