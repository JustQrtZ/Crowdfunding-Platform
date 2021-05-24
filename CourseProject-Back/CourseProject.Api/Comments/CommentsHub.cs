using System;
using System.Threading.Tasks;
using CourseProject.Api.ViewModels.Comments;
using CourseProject.Data.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CourseProject.Api.Comments
{
    [Authorize(Roles = "User,Admin")]
    public class CommentsHub : Hub
    {
        private readonly ICommentsRepository _comments;
        private readonly ICompanyNewsRepository _companyNews;
        private readonly IUserRepository _user;

        public CommentsHub(ICommentsRepository context, ICompanyNewsRepository companyNews, IUserRepository user)
        {
            _comments = context;
            _companyNews = companyNews;
            _user = user;
        }

        public async Task JoinGroup(string companyNewsId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, companyNewsId);
            await Clients.Group(companyNewsId).SendAsync("JoinGroup", companyNewsId);
            var allComments = _comments.FindBy(x=>x.CompanyNews.Id == companyNewsId);
            await Clients.Client(Context.ConnectionId).SendAsync("allStickers", allComments);
        }

        public async Task CreateCommentForCompanyNewsViewModel(CreateCommentForCompanyNewsViewModel commentModel)
        {
            var companyNews = _companyNews.GetSingle(x => x.Id == commentModel.CompanyNews);
            var comment = new Model.Entities.Comments
            {
                Id = Guid.NewGuid().ToString(),
                CompanyNews = companyNews,
                Content = commentModel.Content,
                CreationDate = DateTime.Now,
                User = _user.GetSingle(x => x.Id == commentModel.User),
            };
            _comments.Add(comment);
            _comments.Commit();
            await Clients.Group(commentModel.CompanyNews).SendAsync("addCommentSuccess", _comments.GetAll());
        }
    }
}