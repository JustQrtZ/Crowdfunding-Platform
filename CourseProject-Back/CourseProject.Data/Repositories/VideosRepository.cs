using System.Collections.Generic;
using System.Linq;
using CourseProject.Data.Abstract;
using CourseProject.Model.Entities;

namespace CourseProject.Data.Repositories
{
    public class VideosRepository : EntityBaseRepository<Videos>, IVideosRepository
    {
        private readonly CourseProjectContext _context;

        public VideosRepository(CourseProjectContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetVideoForCompany(string id)
        {
            return (from video in _context.Videos
                where video.CrowdfundingCompany.Id == id
                select new
                {
                    video.Id,
                    video.VideoUrl,
                    companyId = video.CrowdfundingCompany.Id
                });
        }
        }
    }