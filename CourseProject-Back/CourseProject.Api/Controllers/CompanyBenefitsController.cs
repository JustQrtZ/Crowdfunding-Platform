using System;
using System.Linq;
using CourseProject.Api.Services.Abstraction;
using CourseProject.Api.ViewModels.Company.Benefits;
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
    public class CompanyBenefitsController : ControllerBase
    {
        private readonly ICompanyBenefitRepository _companyBenefit;
        private readonly ICrowdfundingCompanyRepository _crowdfundingCompany;
        private readonly IPaymentsRepository _payments;
        private readonly ITokenService _tokenService;


        public CompanyBenefitsController(ICompanyBenefitRepository companyBenefit,
            ICrowdfundingCompanyRepository crowdfundingCompany, IPaymentsRepository payments,
            ITokenService tokenService)
        {
            _companyBenefit = companyBenefit;
            _crowdfundingCompany = crowdfundingCompany;
            _payments = payments;
            _tokenService = tokenService;
        }

        public bool ChekAccess(string companyId)
        {
            var principal =
                _tokenService.GetPrincipalFromExpiredToken(Request.Headers[HeaderNames.Authorization].ToString()
                    .Remove(0, 7));
            var userId = _tokenService.GetUserIdFromClaimsPrincipal(principal);
            return _crowdfundingCompany.CheckIfHasAccess(userId,companyId);
        }
        
        [AllowAnonymous]
        [HttpGet("getBenefits")]
        public ActionResult GetBenefitsForCompany(string id)
        {
            Console.WriteLine(id);
            return Ok(_companyBenefit.GetAllBenefitsForCompany(id));
        }

        [HttpPost("createBenefitForCompany")]
        public ActionResult CreateBenefitsForCompany([FromBody] CreateBenefitForCompanyViewModel viewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (ChekAccess(viewModel.CrowdfundingCompany))
            {
                var company = _crowdfundingCompany.GetSingle(x => x.Id == viewModel.CrowdfundingCompany);
                if (company == null) return BadRequest(new {company = "Company does not exist"});
                var companyBenefit = new CompanyBenefit
                {
                    Id = Guid.NewGuid().ToString(),
                    Cost = viewModel.Cost,
                    CrowdfundingCompany = company,
                    Name = viewModel.Name
                };
                _companyBenefit.Add(companyBenefit);
                _companyBenefit.Commit();
                return Ok(companyBenefit);
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpPatch("editCompanyBenefit")]
        public ActionResult<CompanyBenefit> EditCompanyBenefit([FromBody] EditCompanyBenefitViewModel viewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (ChekAccess(viewModel.CrowdfundingCompany))
            {
                var company = _crowdfundingCompany.GetSingle(x => x.Id == viewModel.CrowdfundingCompany);
                if (company == null) return BadRequest(new {company = "Company does not exist"});
                var benefit = _companyBenefit.GetSingle(x => x.Id == viewModel.Id);
                benefit.Name = viewModel.Name;
                benefit.Cost = viewModel.Cost;
                _companyBenefit.Update(benefit);
                _companyBenefit.Commit();
                return benefit;
            }
            else
            {
                return StatusCode(403);
            }
        }

        [HttpDelete("deleteCompanyBenefit")]
        public ActionResult<CompanyBenefit> DeleteCompanyBenefit([FromBody] DeleteCompanyBenefitViewModel viewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (ChekAccess(viewModel.CrowdfundingCompany))
            {
                var payments = _payments.FindBy(x => x.CompanyBenefit.Id == viewModel.Id).ToList();
                if (payments.Any())
                {
                    return BadRequest(new
                        {company = "U can't delete this benefit because payment has been made for this benefit "});
                }

                _companyBenefit.DeleteWhere(x =>
                    x.Id == viewModel.Id && x.CrowdfundingCompany.Id == viewModel.CrowdfundingCompany);
                _companyBenefit.Commit();

                return Ok();
            }
            else
            {
                return StatusCode(403);
            }
        }
    }
}