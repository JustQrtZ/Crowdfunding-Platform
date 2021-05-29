using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CourseProject.Api.Services.Abstraction;
using CourseProject.Api.ViewModels.Company;
using CourseProject.Data.Abstract;
using CourseProject.Model.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace CourseProject.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "User,Admin")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyBenefitRepository _companyBenefit;
        private readonly ICompanyRatingRepository _companyRating;
        private readonly ICompanyTagsRepository _companyTags;
        private readonly ICrowdfundingCompanyRepository _crowdfundingCompany;
        private readonly IPaymentsRepository _payments;
        private readonly ITagsRepository _tagsRepository;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IVideosRepository _videos;
        private readonly IUserBenefitRepository _userBenefit;
        private readonly IPhotosRepository _photos;


        public CompanyController(IUserRepository userRepository, ICrowdfundingCompanyRepository crowdfundingCompany,
            ICompanyBenefitRepository companyBenefit, IPaymentsRepository payments,
            ITokenService tokenService, ITagsRepository tagsRepository,
            ICompanyTagsRepository companyTags, IVideosRepository videos, ICompanyRatingRepository companyRating,
            IUserBenefitRepository userBenefit, IPhotosRepository photos)
        {
            _userRepository = userRepository;
            _crowdfundingCompany = crowdfundingCompany;
            _companyBenefit = companyBenefit;
            _payments = payments;
            _tokenService = tokenService;
            _tagsRepository = tagsRepository;
            _companyTags = companyTags;
            _videos = videos;
            _companyRating = companyRating;
            _userBenefit = userBenefit;
            _photos = photos;
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
        [HttpGet("getAllCompanies")]
        public ActionResult GetAllCompanies()
        {
            return Ok(_crowdfundingCompany.GetAllCompanies());
        }

        [HttpPost("deleteCompany")]
        public ActionResult DeleteCompany(DeleteCompanyViewModel companyViewModel)
        {
            if (!ChekAccess(companyViewModel.CompanyId)) return BadRequest(new {access = "no access"});
            try
            {
                _crowdfundingCompany.DeleteWhere(x => x.Id == companyViewModel.CompanyId);
                _photos.DeleteWhere(x => x.CrowdfundingCompany.Id == companyViewModel.CompanyId);
                _companyTags.DeleteWhere(x => x.CrowdfundingCompany.Id == companyViewModel.CompanyId);
                _videos.DeleteWhere(x => x.CrowdfundingCompany.Id == companyViewModel.CompanyId);
                _photos.Commit();
                _companyTags.Commit();
                _videos.Commit();
                _crowdfundingCompany.Commit();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest(new {error = "u can't delete this company"});
            }
        }

        [HttpPost("createPayment")]
        public ActionResult CreatePayment([FromBody] PaymentViewModel paymentViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var principal =
                _tokenService.GetPrincipalFromExpiredToken(Request.Headers[HeaderNames.Authorization].ToString()
                    .Remove(0, 7));
            var userId = _tokenService.GetUserIdFromClaimsPrincipal(principal);
            var owner = _userRepository.GetSingle(x => x.Id == userId);
            if (owner == null) return BadRequest(new {User = "user does not exist"});
            var company = _crowdfundingCompany.GetSingle(x => x.Id == paymentViewModel.CrowdfundingCompanyId);
            if (company == null) return BadRequest(new {User = "company does not exist"});
            var benefit = _companyBenefit.GetSingle(x => x.Id == paymentViewModel.CompanyBenefitId);
            if (benefit == null) return BadRequest(new {User = "benefit does not exist"});

            var userPayment = new Payments
            {
                Id = Guid.NewGuid().ToString(),
                Amount = (decimal) benefit.Cost,
                PaymentDate = DateTime.Now,
                CrowdfundingCompany = company,
                CompanyBenefit = benefit,
                User = owner
            };

            var userBenefit = new UserBenefit
            {
                Id = Guid.NewGuid().ToString(),
                CompanyBenefit = benefit,
                User = owner
            };

            _userBenefit.Add(userBenefit);
            _userBenefit.Commit();
            _payments.Add(userPayment);
            _payments.Commit();

            return Ok("Payment success");
        }

        [HttpPost("createCompany")]
        public ActionResult CreateCompany([FromBody] CreateCompanyViewModel companyViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var principal =
                _tokenService.GetPrincipalFromExpiredToken(Request.Headers[HeaderNames.Authorization].ToString()
                    .Remove(0, 7));
            var userId = _tokenService.GetUserIdFromClaimsPrincipal(principal);
            var owner = _userRepository.GetSingle(x => x.Id == userId);

            if (owner == null) return BadRequest(new {User = "user does not exist"});

            var crowdfundingCompany = new CrowdfundingCompany
            {
                Id = Guid.NewGuid().ToString(),
                Title = companyViewModel.Title,
                Description = companyViewModel.Description,
                RequiredAmount = companyViewModel.RequiredAmount,
                MainPhotoUrl = GetPhotosUrl(new[] {companyViewModel.MainPhotoUrl}).Result.First(),
                EndCompanyDate = DateTime.Parse(companyViewModel.EndCompanyDate, CultureInfo.InvariantCulture),
                User = owner,
                Theme = companyViewModel.Theme
            };

            _crowdfundingCompany.Add(crowdfundingCompany);
            _crowdfundingCompany.Commit();
            
            CompanyTags(companyViewModel.Tags, crowdfundingCompany.Id);
            
            _videos.Add(new Videos
            {
                Id = Guid.NewGuid().ToString(),
                CrowdfundingCompany = crowdfundingCompany,
                VideoUrl = companyViewModel.VideoUrl
            });
            _videos.Commit();
            
            var companyPhotosUrls = GetPhotosUrl(companyViewModel.Photos);
            foreach (var photo in companyPhotosUrls.Result)
            {
                _photos.Add(new Photos
                {
                    CrowdfundingCompany = crowdfundingCompany,
                    Id = Guid.NewGuid().ToString(),
                    PhotoUrl = photo
                });
            }

            _photos.Commit();
            
            return Ok(_crowdfundingCompany.GetUserCompanies(owner.Id));
        }

        public void CompanyTags(string[] tags, string companyId)
        {
            _companyTags.DeleteWhere(x => x.CrowdfundingCompany.Id == companyId);
            var company = _crowdfundingCompany.GetSingle(x => x.Id == companyId);
            foreach (var tag in tags)
                if (_tagsRepository.IsTagUniq(tag))
                {
                    var singleTag = new Tags
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = tag
                    };
                    _tagsRepository.Add(singleTag);
                    _tagsRepository.Commit();
                    _companyTags.Add(new CompanyTags
                    {
                        Id = Guid.NewGuid().ToString(),
                        CrowdfundingCompany = company,
                        Tags = singleTag
                    });
                }
                else
                {
                    var notUniqueTag = _tagsRepository.GetSingle(x => x.Name == tag);
                    _companyTags.Add(new CompanyTags
                    {
                        Id = Guid.NewGuid().ToString(),
                        CrowdfundingCompany = company,
                        Tags = notUniqueTag
                    });
                }

            _companyTags.Commit();
        }

        [AllowAnonymous]
        [HttpGet("getSingleCompany")]
        public ActionResult<IQueryable> GetSingleCompany(string companyId)
        {
            var company = _crowdfundingCompany.GetSingleCompany(companyId);
            return company == null
                ? BadRequest(new {companyId = "company does not exist"})
                : new ActionResult<IQueryable>(company);
        }

        [AllowAnonymous]
        [HttpGet("getAllTags")]
        public IActionResult GetAllTags()
        {
            return Ok(_tagsRepository.GetAll().Select(tag => tag.Name).ToList());
        }

        private static async Task<List<string>> GetPhotosUrl(string[] photosInBase64)
        {
            var result = new List<string>();
            foreach (var image in photosInBase64)
            {
                var client = new HttpClient {BaseAddress = new Uri("https://api.imgbb.com")};
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("key", "1a9908a71a7b2fd666e90eaf49a403e5"),
                    new KeyValuePair<string, string>("image", image)
                });
                var response = await client.PostAsync("/1/upload", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var obj = JObject.Parse(responseContent);
                result.Add(obj["data"]?["url"]?.ToString());
            }

            return result;
        }


        [HttpPatch("editCompany")]
        public ActionResult EditCompany([FromBody] EditCompanyViewModel companyViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!ChekAccess(companyViewModel.CompanyId)) return BadRequest(new {access = "no access"});
            var company = _crowdfundingCompany.GetSingle(
                x => x.Id == companyViewModel.CompanyId);
            if (company == null) return BadRequest(new {company = "company does not exist or userid does not exist"});
            
            company.Title = companyViewModel.Title;
            company.Description = companyViewModel.Description;
            company.RequiredAmount = companyViewModel.RequiredAmount;
            company.MainPhotoUrl = GetPhotosUrl(new[] {companyViewModel.MainPhotoUrl}).Result.First();
            company.EndCompanyDate = DateTime.Parse(companyViewModel.EndCompanyDate, CultureInfo.InvariantCulture);
            company.Theme = companyViewModel.Theme;

            _crowdfundingCompany.Update(company);
            _crowdfundingCompany.Commit();

            //TODO ПЕРЕДЕЛАТЬ
            CompanyTags(companyViewModel.Tags, company.Id);

            var video = _videos.GetSingle(x => x.CrowdfundingCompany.Id == companyViewModel.CompanyId,
                x => x.CrowdfundingCompany);
            if (video == null)
            {
                _videos.Add(new Videos
                {
                    Id = Guid.NewGuid().ToString(),
                    CrowdfundingCompany = company,
                    VideoUrl = companyViewModel.VideoUrl
                });
                _videos.Commit();
            }
            else
            {
                video.VideoUrl = companyViewModel.VideoUrl;
                _videos.Update(video);
                _videos.Commit();
            }

            _photos.DeleteWhere(x => x.CrowdfundingCompany.Id == company.Id);
            var companyPhotosUrls = GetPhotosUrl(companyViewModel.Photos);
            foreach (var photo in companyPhotosUrls.Result)
            {
                _photos.Add(new Photos
                {
                    CrowdfundingCompany = company,
                    Id = Guid.NewGuid().ToString(),
                    PhotoUrl = photo
                });
            }

            _photos.Commit();

            return Ok(_crowdfundingCompany.GetSingleCompany(company.Id));
        }

        [HttpPost("createVideoForCompany")]
        public ActionResult CreateVideoForCompany([FromBody] CreateVideoForCompanyViewModel viewModel)
        {
            var company = _crowdfundingCompany.GetSingle(x => x.Id == viewModel.CrowdfundingCompany);
            var video = new Videos
            {
                Id = Guid.NewGuid().ToString(),
                VideoUrl = viewModel.VideoUrl,
                CrowdfundingCompany = company
            };
            _videos.Add(video);
            _videos.Commit();
            return Ok(video);
        }

        [HttpPost("editVideoForCompany")]
        public ActionResult EditVideoForCompany([FromBody] EditVideoForCompanyViewModel viewModel)
        {
            var video = _videos.GetSingle(
                x => x.Id == viewModel.Id && x.CrowdfundingCompany.Id == viewModel.CrowdfundingCompany,
                x => x.CrowdfundingCompany);
            if (video == null) return BadRequest(new {company = "video or company does not exist"});
            video.VideoUrl = viewModel.VideoUrl;
            _videos.Update(video);
            _videos.Commit();
            return Ok(video);
        }

        [AllowAnonymous]
        [HttpGet("getVideoForCompany")]
        public ActionResult<IQueryable> GetVideoForCompany(string companyId)
        {
            var video = _videos.GetVideoForCompany(companyId);
            return new ActionResult<IQueryable>(video);
        }

        [HttpDelete("deleteVideoForCompany/{id}")]
        public ActionResult DeleteVideoForCompany(string id)
        {
            _videos.DeleteWhere(x => x.Id == id);
            _videos.Commit();
            return Ok();
        }

        [HttpPost("createRatingCompany")]
        public ActionResult CreateRatingCompany([FromBody] CreateRatingCompanyViewModel viewModel)
        {
            var company = _crowdfundingCompany.GetSingle(x => x.Id == viewModel.CrowdfundingCompany);
            if (company == null) return BadRequest(new {company = "Company does not exist"});
            var principal =
                _tokenService.GetPrincipalFromExpiredToken(Request.Headers[HeaderNames.Authorization].ToString()
                    .Remove(0, 7));
            var userId = _tokenService.GetUserIdFromClaimsPrincipal(principal);
            var rating = _companyRating.GetSingle(x => x.CrowdfundingCompany == company && x.User.Id == userId,
                x => x.User);
            if (rating != null)
            {
                rating.Rating = viewModel.Rating;
                _companyRating.Update(rating);
            }
            else
            {
                var companyRating = new CompanyRating
                {
                    Id = Guid.NewGuid().ToString(),
                    CrowdfundingCompany = company,
                    Rating = viewModel.Rating,
                    User = _userRepository.GetSingle(x => x.Id == userId)
                };

                _companyRating.Add(companyRating);
            }

            _companyRating.Commit();
            return Ok();
        }

        [HttpGet("getUserCompanyRating")]
        public IActionResult GetUserCompanyRating(string companyId)
        {
            var principal =
                _tokenService.GetPrincipalFromExpiredToken(Request.Headers[HeaderNames.Authorization].ToString()
                    .Remove(0, 7));
            var userId = _tokenService.GetUserIdFromClaimsPrincipal(principal);
            return Ok(_companyRating.GetUserCompanyRating(userId, companyId));
        }
    }
}