using Microsoft.SqlServer.Server;
using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Tests;
using Sabio.Models.Domain.Conferences;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Users;
using Sabio.Services.Interfaces;
using Stripe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class UserService : IUserService, IBaseUserMapper
    {
        private IAuthenticationService<int> _authenticationService;
        private IDataProvider _dataProvider;
        private ILookUpService _lookUpService ;

        public UserService(IAuthenticationService<int> authSerice, IDataProvider dataProvider, ILookUpService lookUpService)
        {
            _authenticationService = authSerice;

            _dataProvider = dataProvider;
            
            _lookUpService = lookUpService;
        }

        public async Task<bool> LogInAsync(string email, string password)
        {
            bool isSuccessful = false;

            IUserAuthData response = Get(email, password);

            if (response != null)
            {
                await _authenticationService.LogInAsync(response);
                isSuccessful = true;
            }
            else
            {
                throw new System.Exception("User Credentials are Invalid");
            }

            return isSuccessful;
        }

        public async Task<bool> LogInTest(string email, string password, int id, string[] roles = null)
        {
            bool isSuccessful = false;
            var testRoles = new[] { "User", "Super", "Content Manager" };

            var allRoles = roles == null ? testRoles : testRoles.Concat(roles);

            IUserAuthData response = new UserBase
            {
                Id = id,
                Name = email,
                Roles = allRoles,
                TenantId = 1
            };

            Claim fullName = new Claim("CustomClaim", "Sabio Bootcamp");
            await _authenticationService.LogInAsync(response, new Claim[] { fullName });

            return isSuccessful;
        }

        public int Create(object userModel)
        {
            //make sure the password column can hold long enough string. put it to 100 to be safe

            int userId = 0;
            string password = "Get from user model when you have a concreate class";
            string salt = BCrypt.BCryptHelper.GenerateSalt();
            string hashedPassword = BCrypt.BCryptHelper.HashPassword(password, "");

            //DB provider call to create user and get us a user id

            //be sure to store both salt and passwordHash
            //DO NOT STORE the original password value that the user passed us

            return userId;
        }

        /// <summary>
        /// Gets the Data call to get a give user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        private IUserAuthData Get(string email, string password)
        {
            UserBase user = null;
            (var data, string passwordHash) = SelectUserAuthData(email);
            bool isValidCredentials = BCrypt.BCryptHelper.CheckPassword(password, passwordHash);
            if (isValidCredentials)
            {
                user = new UserBase();
                user.Id = data.Id;
                user.TenantId = data.TenantId;
                user.Name = data.Name;
                user.Roles = from item in data.Roles select item;
            }
            return user;
        }

        public UserAuthData GetCurrentUser()
        {
            IUserAuthData userClaims = _authenticationService.GetCurrentUser();
            int userId = userClaims.Id;
            User user = SelectUserById(userId);
            UserAuthData returnUser = new UserAuthData(userClaims, user);
            returnUser.Conferences = GetUserConferences(userId);
            return returnUser;
        }

        public int GetCurrentUserId()
        {
            return _authenticationService.GetCurrentUserId();
        }

        public async Task LogOut()
        {
            await _authenticationService.LogOutAsync();
        }

        #region Mappers
        public User MapUser(IDataReader reader, ref int startingIndex)
        {
            User user = new User();
            user.Gender = new LookUp();
            user.Id = reader.GetSafeInt32(startingIndex++);
            user.Email = reader.GetSafeString(startingIndex++);
            user.FirstName = reader.GetSafeString(startingIndex++);
            user.LastName = reader.GetSafeString(startingIndex++);
            user.Mi = reader.GetSafeString(startingIndex++);
            user.Gender.Id = reader.GetSafeInt32(startingIndex++);
            user.Gender.Name = reader.GetSafeString(startingIndex++);
            user.AvatarUrl = reader.GetSafeString(startingIndex++);
            user.Phone = reader.GetSafeString(startingIndex++);
            user.isConfirmed = reader.GetSafeBool(startingIndex++);
            user.Status = reader.GetSafeString(startingIndex++);
            user.DateCreated = reader.GetSafeDateTime(startingIndex++);
            user.DateModified = reader.GetSafeDateTime(startingIndex++);
            return user;
        }

        public BaseToken MapToken(IDataReader data)
        {
            int index = 0;
            var token = new BaseToken();
            token.Token = data.GetSafeString(index++);
            token.UserId = data.GetSafeInt32(index++);
            return token;
        }

        public void AddCommonParams(UserAddRequest model, SqlParameterCollection coll)
        {
            coll.AddWithValue("@Email", model.Email);
            coll.AddWithValue("@FirstName", model.FirstName);
            coll.AddWithValue("@LastName", model.LastName);
            coll.AddWithValue("@Mi", model.Mi);
            coll.AddWithValue("@GenderId", model.GenderId);
            coll.AddWithValue("@AvatarUrl", "");
            coll.AddWithValue("@Phone", model.Phone);
            coll.AddWithValue("@Password", model.Password);
            coll.AddWithValue("@StatusId", model.StatusId);
        }
        #endregion

        public void ConfirmUser(string token, string email)
        {
            string procName = "[dbo].[UserTokens_SelectUserByToken]";
            _dataProvider.ExecuteNonQuery(procName,
                delegate (SqlParameterCollection colToSend)
                {
                    colToSend.AddWithValue("@Token", token);
                    colToSend.AddWithValue("@Email", email);
                }, null);
        }

        public User SelectUserById(int id)
        {
            string procName = "[dbo].[Users_Select_ById]";
            User user = new User();
            _dataProvider.ExecuteCmd(procName,
                delegate (SqlParameterCollection colToSend)
                {
                    colToSend.Add(new SqlParameter("@Id", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Input,
                        Value = id
                    });
                },
                delegate (IDataReader dataReader, short set)
                {
                    int index = 0;
                    user = MapUser(dataReader, ref index);
                });
            return user;
        }


        public (int, string) InsertUser(UserAddRequest model)
        {
            string procName = "[dbo].[Users_Insert]";
            int id = 0;
            string token = Guid.NewGuid().ToString();
            _dataProvider.ExecuteNonQuery(procName,
                delegate (SqlParameterCollection colToSend)
                {
                    colToSend.AddWithValue("@Role", model.Role);
                    colToSend.AddWithValue("@Token", token);
                    AddCommonParams(model, colToSend);
                    colToSend.Add(new SqlParameter("@Id", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    });
                },
                delegate (SqlParameterCollection colToReceive)
                {
                    object IdOut = colToReceive["@Id"].Value;
                    int.TryParse(IdOut.ToString(), out id);
                });
            return (id, token);
        }

        public (UserBase, string) SelectUserAuthData(string email)
        {
            string procName = "[dbo].[Users_Select_AuthData]";
            var authData = new UserBase();
            string password = "";
            _dataProvider.ExecuteCmd(procName,
                delegate (SqlParameterCollection colToSend)
                {
                    colToSend.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 255)
                    {
                        Direction = ParameterDirection.Input,
                        Value = email
                    });
                },
                delegate (IDataReader dataReader, short set)
                {
                    int index = 0;
                    authData.Id = dataReader.GetSafeInt32(index++);
                    authData.Name = dataReader.GetSafeString(index++);
                    password = dataReader.GetSafeString(index++);
                    authData.Roles = dataReader.DeserializeObject<List<string>>(index++);
                    authData.TenantId = dataReader.GetSafeInt32(index++);
                });
            return (authData, password);
        }

        public Paged<User> SelectAllUsers(int pageIndex, int pageSize)
        {
            string procName = "[dbo].[Users_SelectAll]";
            List<User> userList = null;
            int totalCount = 0;
            int index = 0;
            _dataProvider.ExecuteCmd(procName,
                delegate (SqlParameterCollection colToSend)
                {
                    colToSend.Add(new SqlParameter("@PageIndex", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Input,
                        Value = pageIndex
                    });
                    colToSend.Add(new SqlParameter("@PageSize", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Input,
                        Value = pageSize
                    });
                },
                delegate (IDataReader dataReader, short set)
                {
                    if (userList == null)
                    {
                        userList = new List<User>();
                        totalCount = dataReader.GetSafeInt32(13);
                    }
                    index = 0;
                    userList.Add(MapUser(dataReader, ref index));
                });
            return new Paged<User>(userList, pageIndex, pageSize, totalCount);
        }

        public Paged<User> SearchUsers( string query, int pageIndex, int pageSize)
        {
            string procName = "[dbo].[Users_Search]";
            Paged<User> pagedList = null;
            List<User> list = null;
            int totalCount = 0;
            _dataProvider.ExecuteCmd(procName,
                delegate (SqlParameterCollection colToSend)
                {
                    colToSend.AddWithValue("@Query", query);
                    colToSend.AddWithValue("@PageIndex", pageIndex);
                    colToSend.AddWithValue("@PageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader dataReader, short set)
                {
                    int index = 0;
                    User aUser = MapUser(dataReader, ref index);

                    if(totalCount == 0)
                    {
                        totalCount = dataReader.GetSafeInt32(index++);
                    }

                    if (list == null)
                    {
                        list = new List<User>();
                    }
                    list.Add(aUser);
                });
            if(list != null)
            {
                pagedList = new Paged<User>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public string SelectPasswordByEmail(string email)
        {
            string procName = "[dbo].[Users_SelectPass_ByEmail]";
            string password = "";
            _dataProvider.ExecuteCmd(procName,
                delegate (SqlParameterCollection colToSend)
                {
                    colToSend.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 255)
                    {
                        Direction = ParameterDirection.Input,
                        Value = email
                    });
                },
                delegate (IDataReader dataReader, short set)
                {
                    password = dataReader.GetSafeString(0);
                });
            return password;
        }

        public void UpdateStatusById(int id, int newStatus)
        {
            string procName = "[dbo].[Users_UpdateStatus]";
            _dataProvider.ExecuteNonQuery(procName,
                delegate (SqlParameterCollection colToSend)
                {
                    colToSend.Add(new SqlParameter("@Id", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Input,
                        Value = id
                    });
                    colToSend.Add(new SqlParameter("@StatusId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Input,
                        Value = newStatus
                    });
                }, null);
        }

        public void DeleteByToken(string token)
        {
            string procName = "[dbo].[UserTokens_Delete_ByToken]";
            _dataProvider.ExecuteNonQuery(procName,
                delegate (SqlParameterCollection colToSend)
                {
                    colToSend.Add(new SqlParameter("@Token", SqlDbType.NVarChar, 200)
                    {
                        Direction = ParameterDirection.Input,
                        Value = token
                    });
                }, null);
        }

        public string InsertToken(int userId, int tokenType)
        {
            string procName = "[dbo].[UserTokens_Insert]";
            string token = "";
            _dataProvider.ExecuteNonQuery(procName,
                delegate (SqlParameterCollection colToSend)
                {
                    colToSend.Add(new SqlParameter("@UserId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Input,
                        Value = userId
                    });
                    colToSend.Add(new SqlParameter("@TokenType", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Input,
                        Value = tokenType
                    });
                    colToSend.Add(new SqlParameter("@Token", SqlDbType.NVarChar, 200)
                    {
                        Direction = ParameterDirection.Output,
                    });
                },
                delegate (SqlParameterCollection colToReceive)
                {
                    object objToken = colToReceive["@Token"].Value;
                    token = objToken.ToString();
                });
            return token;
        }

        public List<BaseToken> SelectByTokenType(int tokenType)
        {
            string procName = "[dbo].[UserTokens_Select_ByTokenType]";
            List<BaseToken> tokenList = null;
            _dataProvider.ExecuteCmd(procName,
                delegate (SqlParameterCollection colToSend)
                {
                    colToSend.Add(new SqlParameter("@TokenType", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Input,
                        Value = tokenType
                    });
                },
                delegate (IDataReader dataReader, short set)
                {
                    if (tokenList == null)
                    {
                        tokenList = new List<BaseToken>();
                    }
                    tokenList.Add(MapToken(dataReader));
                });
            return tokenList;
        }

        public string HashPassword(string password)
        {
            string salt = BCrypt.BCryptHelper.GenerateSalt();
            string passwordHash = BCrypt.BCryptHelper.HashPassword(password, salt);
            return passwordHash;
        }

        public BaseUser MapBaseUser(IDataReader reader, ref int startingIndex)
        {
            BaseUser user = new BaseUser();

            user.Id = reader.GetSafeInt32(startingIndex++);
            user.FirstName = reader.GetSafeString(startingIndex++);
            user.LastName = reader.GetSafeString(startingIndex++);
            user.Mi = reader.GetSafeString(startingIndex++);
            user.AvatarUrl = reader.GetSafeString(startingIndex++);

            return user;
        }

        public string ForgotPassword(string email)
        {
            string token = Guid.NewGuid().ToString();
            string procName = "[dbo].[Users_Forgot_Password]";
            _dataProvider.ExecuteNonQuery(procName,
                delegate (SqlParameterCollection colToSend)
                {
                    colToSend.AddWithValue("@Email", email);
                    colToSend.AddWithValue("@Token", token);
                }, null);
            return token;
        }

        public void ChangePassword(PasswordChangeRequest model)
        {
            string procName = "[dbo].[Users_ChangePassword]";
            string password = HashPassword(model.Password);
            _dataProvider.ExecuteNonQuery(procName,
                delegate (SqlParameterCollection colToSend)
                {
                    colToSend.AddWithValue("@Token", model.Token);
                    colToSend.AddWithValue("@Email", model.Email);
                    colToSend.AddWithValue("@Password", password);
                }, null);
        }

        public string SendRegistrationInvitation(PersonnelInviteRequest model)
        {
            string procName = "[dbo].[UsersRegistrationRequest_Insert]";
            string token = Guid.NewGuid().ToString();
            DataTable table = new DataTable();
            table.Columns.Add("ConferenceId", typeof(Int32));
            foreach (int conference in model.ConferenceIds)
            {
                DataRow dataRow = table.NewRow();
                dataRow.SetField<int>(0, conference);
                table.Rows.Add(dataRow);
            }
            _dataProvider.ExecuteNonQuery(procName,
                delegate (SqlParameterCollection colToSend)
                {
                    colToSend.AddWithValue("@ConferenceId", table);
                    colToSend.AddWithValue("@Email", model.Email);
                    colToSend.AddWithValue("@RoleId", model.RoleId);
                    colToSend.AddWithValue("@PositionId", model.PositionId);
                    colToSend.AddWithValue("@Token", token);
                }, null);
            return token;
        }

        public UserRegistrationInfo SelectRegistrationDetails(string email, string token = "none")
        {
            int index = 0;
            string procName = "[dbo].[UsersRegistrationRequest_Select]";            
            var paramsDelegate = delegate (SqlParameterCollection colToSend)
            {
                colToSend.AddWithValue("@Token", token);
                colToSend.AddWithValue("@Email", email);
            };
            if (token == "none")
            {
                index++;
                procName = "[dbo].[UsersRegistrationRequests_SelectByEmail]";
                paramsDelegate = delegate (SqlParameterCollection colToSend)
                {
                    colToSend.AddWithValue("@Email", email);
                };
            }
            UserRegistrationInfo result = new UserRegistrationInfo();
            _dataProvider.ExecuteCmd(procName,
                paramsDelegate,
                delegate (IDataReader reader, short set)
                {
                    result.RoleId = reader.GetSafeInt32(index++);
                    result.ConferenceId = reader.DeserializeObject<List<int>>(index++);
                    result.PositionId = reader.GetSafeInt32(index++);                    
                });
            return result;
        }

        public void DeleteRegistrationInvitation(int userId)
        {
            string procName = "[dbo].[UsersRegistrationRequests_DeleteByUserId]";
            _dataProvider.ExecuteNonQuery(procName,
                delegate (SqlParameterCollection colToSend)
                {
                    colToSend.AddWithValue("@Id", userId);
                }, null);
        }

        public async Task ChangeUserConference(string conferenceId)
        {
            IUserAuthData currentUser = _authenticationService.GetCurrentUser();
            IUserAuthData newUser = new UserBase()
            {
                TenantId = conferenceId,
                Id = currentUser.Id,
                Name = currentUser.Name,
                Roles = currentUser.Roles
            };
            await _authenticationService.LogOutAsync();
            await _authenticationService.LogInAsync(newUser);
        }

        public List<BaseConference> GetUserConferences(int id)
        {
            string procName = "[dbo].[ConferenceUsers_SelectByUserId]";
            List<BaseConference> list = null;
            BaseConference conference = null;
            _dataProvider.ExecuteCmd(procName,
                delegate (SqlParameterCollection colToSend)
                {
                    colToSend.AddWithValue("@Id", id);
                },
                delegate (IDataReader dataReader, short set)
                {
                    if (list == null)
                    {
                        list = new List<BaseConference>();
                    }
                    conference = new BaseConference();
                    int index = 0;
                    conference.Id = dataReader.GetSafeInt32(index++);
                    conference.Name = dataReader.GetSafeString(index++);
                    conference.Logo = dataReader.GetSafeString(index++);
                    conference.Code = dataReader.GetSafeString(index++);
                    list.Add(conference);
                });
            return list;
        }

        public Paged<UserAdminView> GetAll(int pageIndex, int pageSize)
        {
            Paged<UserAdminView> pagedList = null;

            List<UserAdminView> list = null;

            int totalCount = 0;

            string procName = "[dbo].[Users_SelectAll_V2]";

            _dataProvider.ExecuteCmd(procName, delegate (SqlParameterCollection col)

            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
            }, delegate (IDataReader reader, short set)
            {
                int index = 0;
                UserAdminView user = MapUserV2(reader, ref index);
              
                

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index++);
                }

                if (list == null)
                {
                    list = new List<UserAdminView>();
                }
                list.Add(user);
            });

            if (list != null)
            {
                pagedList = new Paged<UserAdminView>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public Paged<UserAdminView> Search(int pageIndex, int pageSize, string query)
        {
            Paged<UserAdminView> pagedList = null;
            List<UserAdminView> list = null;
            int totalCount = 0;
            string procName = "[dbo].[Users_Search_V2]";

            _dataProvider.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
                param.AddWithValue("@Query", query);
            }, (reader, recordSetIndex) =>
            {
                int index = 0;
                UserAdminView user = MapUserV2(reader, ref index);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index);
                }

                if (list == null)
                {
                    list = new List<UserAdminView>();
                }
                list.Add(user);
            });

            if (list != null)
            {
                pagedList = new Paged<UserAdminView>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<UserAdminView> GetByStatus(int pageIndex, int pageSize,int status)
        {
            Paged<UserAdminView> pagedList = null;

            List<UserAdminView> list = null;

            int totalCount = 0;

            string procName = "[dbo].[Users_SelectAll_ByStatus]";

            _dataProvider.ExecuteCmd(procName, delegate (SqlParameterCollection col)

            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
                col.AddWithValue("@Status", status);
            }, delegate (IDataReader reader, short set)
            {
                int index = 0;
                UserAdminView user = MapUserV2(reader, ref index);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index++);
                }

                if (list == null)
                {
                    list = new List<UserAdminView>();
                }
                list.Add(user);
            });

            if (list != null)
            {
                pagedList = new Paged<UserAdminView>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public UserAdminView MapUserV2(IDataReader reader, ref int startingIndex)
        {
            UserAdminView user = new UserAdminView();
            
            user.Id = reader.GetSafeInt32(startingIndex++);
            user.Email = reader.GetSafeString(startingIndex++);
            user.FirstName = reader.GetSafeString(startingIndex++);
            user.LastName = reader.GetSafeString(startingIndex++);
            user.Mi = reader.GetSafeString(startingIndex++);
            user.Gender= _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            user.AvatarUrl = reader.GetSafeString(startingIndex++);
            user.Phone = reader.GetSafeString(startingIndex++);
            user.isConfirmed = reader.GetSafeBool(startingIndex++);
            user.Roles = reader.DeserializeObject<List<LookUp>>(startingIndex++) ;
            user.Status = reader.GetSafeString(startingIndex++);
            user.DateCreated = reader.GetSafeDateTime(startingIndex++);
            user.DateModified = reader.GetSafeDateTime(startingIndex++);
            return user;
        }
    }
}