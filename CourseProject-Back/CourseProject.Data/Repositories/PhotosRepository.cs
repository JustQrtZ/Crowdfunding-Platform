using System.Collections.Generic;
using System.Linq;
using CourseProject.Data.Abstract;
using CourseProject.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Data.Repositories
{
    public class PhotosRepository : EntityBaseRepository<Photos>, IPhotosRepository
    {
        private readonly CourseProjectContext _context;

        public PhotosRepository(CourseProjectContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllPhotosForCompany(string id)
        {
            return (from photo in _context.Photos
                where photo.CrowdfundingCompany.Id == id
                select new
                {
                    photo.Id,
                    photo.PhotoUrl,
                    CrowdfundingCompanyId = photo.CrowdfundingCompany.Id
                });
        }
    }
}