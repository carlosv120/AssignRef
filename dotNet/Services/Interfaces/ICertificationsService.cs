using Sabio.Models.Domain.Certifications;
using Sabio.Models;
using Sabio.Models.Requests.Certifications;

namespace Sabio.Services.Interfaces
{
    public interface ICertificationsService
    {
        int Add(CertificationsAddRequest modelCertification, int userId);
        void Update(CertificationsUpdateRequest modelCertification, int userId);
        Paged<Certification> GetBySeasonId(int PageIndex, int PageSize, int SeasonId);
        void Delete(int id, int userId);
        Paged<Certification> GetAll(int pageIndex, int pageSize);
        Paged<Certification> GetByConferenceId(int pageIndex, int pageSize, int conferenceId);
    }
}