using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Conferences;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public interface IUserService
    {
        int Create(object userModel);
        Task<bool> LogInAsync(string email, string password);
        UserAuthData GetCurrentUser();
        int GetCurrentUserId();
        Task LogOut();
        Task<bool> LogInTest(string email, string password, int id, string[] roles = null);
        void ConfirmUser(string token, string email);
        User SelectUserById(int id);
        (int, string) InsertUser(UserAddRequest model);
        (UserBase, string) SelectUserAuthData(string email);
        Paged<User> SelectAllUsers(int pageIndex, int pageSize);
        Paged<User> SearchUsers(string query, int pageIndex, int pageSize);
        string SelectPasswordByEmail(string email);
        void UpdateStatusById(int id, int newStatus);
        void DeleteByToken(string token);
        string InsertToken(int userId, int tokenType);
        List<BaseToken> SelectByTokenType(int tokenType);
        string HashPassword(string password);
        string ForgotPassword(string email);
        void ChangePassword(PasswordChangeRequest model);
        string SendRegistrationInvitation(PersonnelInviteRequest model);
        Paged<UserAdminView> GetAll(int pageIndex, int pageSize);
        Paged<UserAdminView> Search(int pageIndex, int pageSize, string query);
        UserRegistrationInfo SelectRegistrationDetails(string email, string token = "none");
        void DeleteRegistrationInvitation(int userId);
        List<BaseConference> GetUserConferences(int id);
        Task ChangeUserConference(string conferenceId);
        Paged<UserAdminView> GetByStatus(int pageIndex, int pageSize, int status);

    }
}