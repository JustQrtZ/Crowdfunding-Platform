using System;
using CourseProject.Api.Services.Abstraction;
using CourseProject.Api.ViewModels.Company.News;
using CourseProject.Data.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using CompanyNews = CourseProject.Model.Entities.CompanyNews;

namespace CourseProject.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "User,Admin")]
    [ApiController]
    public class CompanyNewsController : ControllerBase
    {
        private readonly ICrowdfundingCompanyRepository _crowdfundingCompany;
        private readonly ICompanyNewsRepository _companyNews;
        private readonly ITokenService _tokenService;

        public CompanyNewsController(ITokenService tokenService, ICompanyNewsRepository companyNews,
            ICrowdfundingCompanyRepository crowdfundingCompany)
        {
            _tokenService = tokenService;
            _companyNews = companyNews;
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
        [HttpGet("getAllCompanyNews")]
        public ActionResult GetAllCompanyNews([FromBody] AllCompanyNewsViewModel viewModel)
        {
            var company = _crowdfundingCompany.GetSingle(crowdfundingCompany =>
                crowdfundingCompany.Id == viewModel.CrowdfundingCompany);
            if (company == null) return BadRequest(new {company = "company does not exist"});
            return Ok(_companyNews.GetAllNewsForCompany(viewModel.CrowdfundingCompany));
        }

        [AllowAnonymous]
        [HttpPost("createNews")]
        public ActionResult CreateNews([FromBody] CreateNewsViewModel createNewsViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var newsId = Guid.NewGuid().ToString();
            var company = _crowdfundingCompany.GetSingle(x => x.Id == createNewsViewModel.CrowdfundingCompany);
            if (company == null) return BadRequest(new {company = "company does not exist"});
            var news = new CompanyNews
            {
                Id = newsId,
                Title = createNewsViewModel.Title,
                ImageUrl = createNewsViewModel.ImageUrl,
                Content = createNewsViewModel.Content,
                CreationDate = DateTime.Now,
                CrowdfundingCompany = company
            };
            _companyNews.Add(news);
            _companyNews.Commit();
            return Ok(news);
        }

        //TODO Remove user properties from response and leave only UserId  
        [AllowAnonymous]
        [HttpPatch("editNews")]
        public ActionResult<EditNewsViewModel> EditNews([FromBody] EditNewsViewModel editNewsViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (ChekAccess(editNewsViewModel.CrowdfundingCompany))
            {
                var companyNews = _companyNews.GetSingle(x => x.Id == editNewsViewModel.Id);
                if (companyNews == null) return BadRequest(new {company = "company does not exist"});

                companyNews.Title = editNewsViewModel.Title;
                companyNews.ImageUrl = editNewsViewModel.ImageUrl;
                companyNews.Content = editNewsViewModel.Content;
                _companyNews.Update(companyNews);
                _companyNews.Commit();
                return Ok(companyNews);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpDelete("deleteNews")]
        public ActionResult DeleteNews([FromBody] DeleteNewsViewModel viewModel)
        {
            if (ChekAccess(viewModel.CrowdfundingCompany))
            {
                _companyNews.DeleteWhere(x => x.Id == viewModel.Id);
                _companyNews.Commit();
                return Ok();
            }
            else
            {
                return StatusCode(403);
            }
        }
    }
}