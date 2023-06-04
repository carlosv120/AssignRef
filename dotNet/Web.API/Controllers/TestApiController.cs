using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.Tests;
using Sabio.Services.Interfaces;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using Sabio.Models.Requests.Tests;
using Sabio.Models;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/tests")]
    [ApiController]
    public class TestApiController : BaseApiController
    {
        private ITestService _service = null;
        private IAuthenticationService<int> _authService = null;

        public TestApiController(ITestService service
            , IAuthenticationService<int> authService
            , ILogger<TestApiController> logger) : base(logger)
        {
            _service = service;

            _authService = authService;
        }

        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<Test>>> Search(int pageIndex, int pageSize, string query)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Test> page = _service.Search(pageIndex, pageSize, query);

                if(page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource not found;");
                }
                else
                {
                    response = new ItemResponse<Paged<Test>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Test>>> GetAll(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Test> page = _service.GetAll(pageIndex, pageSize);

                if(page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Test>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("createdBy")]
        public ActionResult<ItemResponse<Paged<Test>>> GetCreatedBy(int pageIndex, int pageSize, int createdBy)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Test> page = _service.GetCreatedBy(pageIndex, pageSize, createdBy);

                if(page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Test>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Delete(id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(TestUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {

                _service.Update(model);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(TestAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                int id = _service.Add(model, userId);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };

                result = Created201(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Test>> Get(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Test test = _service.Get(id);

                if(test == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource not found");
                }
                else
                {
                    response = new ItemResponse<Test> { Item = test };
                }
            }
            catch (Exception Ex)
            {
                code = 500;
                response = new ErrorResponse($"Exception Error: {Ex.Message}");
            }
            return StatusCode(code, response);
        }

        [HttpGet("conferences/{id:int}")]
        public ActionResult<ItemsResponse<Test>> GetByConferenceId(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<Test> test = _service.GetByConferenceId(id);

                if (test == null)
                {
                    code = 404;
                    response = new ErrorResponse("List not found");
                }
                else
                {
                    response = new ItemsResponse<Test> { Items = test };
                }
            }
            catch (Exception Ex)
            {
                code = 500;
                response = new ErrorResponse($"Exception Error: {Ex.Message}");
            }
            return StatusCode(code, response);
        }

    }
}
