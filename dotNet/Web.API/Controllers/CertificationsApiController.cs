using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Certifications;
using Sabio.Models.Requests.Certifications;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using Stripe;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;



namespace Sabio.Web.Api.Controllers
{
    [Route("api/certifications")]
    [ApiController]
    public class CertificationsApiController : BaseApiController
    {
        private ICertificationsService _service = null;
        private IAuthenticationService<int> _authService = null;

        public CertificationsApiController(
        ICertificationsService service,
        ILogger<CertificationsApiController> logger,
        IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(CertificationsAddRequest model)
        {
            IUserAuthData user = _authService.GetCurrentUser();

            int code = 201;
            BaseResponse response = null;

            try
            {
                int id = _service.Add(model, user.Id);

                response = new ItemResponse<int> { Item = id };
            }
            catch (Exception exception)
            {
                code = 500;
                base.Logger.LogError(exception.ToString());

                response = new ErrorResponse(exception.Message);
            }
            return StatusCode(code, response);
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(CertificationsUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;
            IUserAuthData user = _authService.GetCurrentUser();

            try
            {
                _service.Update(model, user.Id);

                response = new SuccessResponse();
            }
            catch (Exception exception)
            {
                code = 500;
                response = new ErrorResponse(exception.Message);
                base.Logger.LogError(exception.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("season")]
        public ActionResult<ItemResponse<Paged<Certification>>> GetBySeason(int pageIndex, int pageSize, int seasonId)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<Certification> page = _service.GetBySeasonId(pageIndex, pageSize, seasonId);
                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Certifications not found");
                }
                else
                {
                    response = new ItemResponse<Paged<Certification>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                code = 500;

                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }
            return StatusCode(code, response);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;
            IUserAuthData user = _authService.GetCurrentUser();

            try
            {
                _service.Delete(id, user.Id);
                response = new SuccessResponse();

            }
            catch (Exception exception)
            {
                code = 500;
                response = new ErrorResponse(exception.Message);
                base.Logger.LogError(exception.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Certification>>> GetPageCertifications(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<Certification> page = _service.GetAll(pageIndex, pageSize);
                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Certifications not found");
                }
                else
                {
                    response = new ItemResponse<Paged<Certification>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                code = 500;

                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }
            return StatusCode(code, response);
        }

        [HttpGet("conference")]
        public ActionResult<ItemResponse<Paged<Certification>>> GetByConference(int pageIndex, int pageSize, int conferenceId)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<Certification> page = _service.GetByConferenceId(pageIndex, pageSize, conferenceId);
                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Certifications not found");
                }
                else
                {
                    response = new ItemResponse<Paged<Certification>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                code = 500;

                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }
            return StatusCode(code, response);
        }

    }
}
