using System;
using CourseProject.Api.Services.Abstraction;
using CourseProject.Api.ViewModels.Company.Photos;
using CourseProject.Data.Abstract;
using CourseProject.Model.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace CourseProject.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "User,Admin")]
    [ApiController]
    public class CompanyPhotosController : ControllerBase
    {
        private readonly IPhotosRepository _photos;
        private readonly ITokenService _tokenService;
        private readonly ICrowdfundingCompanyRepository _crowdfundingCompany;

        public CompanyPhotosController(IPhotosRepository photos, ITokenService tokenService,
            ICrowdfundingCompanyRepository crowdfundingCompany)
        {
            _photos = photos;
            _tokenService = tokenService;
            _crowdfundingCompany = crowdfundingCompany;
        }

        public bool ChekAccess(string companyId)
        {
            var principal =
                _tokenService.GetPrincipalFromExpiredToken(Request.Headers[HeaderNames.Authorization].ToString()
                    .Remove(0, 7));
            var userId = _tokenService.GetUserIdFromClaimsPrincipal(principal);
            return _crowdfundingCompany.CheckIfHasAccess(userId, companyId);
        }

        [AllowAnonymous]
        [HttpGet("getAllPhotos")]
        public ActionResult GetAllCompanies(string companyId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(_photos.GetAllPhotosForCompany(companyId));
        }

        [HttpPost("CreatePhoto")]
        public ActionResult CreatePhotoForCompany([FromBody] CreatePhotoViewModel viewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (ChekAccess(viewModel.CrowdfundingCompany))
            {
                var crowdfundingCompany =
                    _crowdfundingCompany.GetSingle(company => company.Id == viewModel.CrowdfundingCompany);
                foreach (var photoUrl in viewModel.PhotoUrl)
                {
                    var photo = new Photos
                    {
                        Id = Guid.NewGuid().ToString(),
                        CrowdfundingCompany = crowdfundingCompany,
                        PhotoUrl = photoUrl
                    };
                    _photos.Add(photo);
                }

                _photos.Commit();
                return Ok();
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPatch("editPhoto")]
        public IActionResult EditPhotoForCompany([FromBody] EditPhotoViewModel viewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (ChekAccess(viewModel.CrowdfundingCompany))
            {
                var photo = _photos.GetSingle(p => p.Id == viewModel.Id);
                photo.PhotoUrl = viewModel.PhotoUrl;
                _photos.Update(photo);
                _photos.Commit();
                return Ok(photo);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpDelete("deletePhoto")]
        public IActionResult DeletePhotoForCompany([FromBody] DeletePhotoViewModel viewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (ChekAccess(viewModel.CrowdfundingCompany))
            {
                _photos.DeleteWhere(photo => photo.Id == viewModel.Id);
                _photos.Commit();
                return Ok();
            }
            else
            {
                return StatusCode(403);
            }
        }
    }
}