using System;
using System.Threading.Tasks;
using CourseProject.Api.ViewModels.Comments;
using CourseProject.Data.Abstract;
using CourseProject.Model.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CourseProject.Api.Comments
{
    public class CommentsHub : Hub
    {
        private readonly ICommentsRepository _comments;
        private readonly IUserRepository _user;
        private readonly ICrowdfundingCompanyRepository _company;
        private readonly ILikesOrDislikesRepository _likesOrDislikes;


        public CommentsHub(ICommentsRepository context, IUserRepository user, ICrowdfundingCompanyRepository company,
            ILikesOrDislikesRepository likesOrDislikes)
        {
            _comments = context;
            _user = user;
            _company = company;
            _likesOrDislikes = likesOrDislikes;
        }

        [AllowAnonymous]
        public async Task JoinGroup(string companyId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, companyId);
            await Clients.Group(companyId).SendAsync("JoinGroup", companyId);
            var allComments = _comments.AllCompanyComments(companyId);
            await Clients.Client(Context.ConnectionId).SendAsync("allComments", allComments);
        }

        [Authorize(Roles = "User,Admin")]
        public async Task CreateComment(CreateCommentForCompanyNewsViewModel commentModel)
        {
            var company = _company.GetSingle(x => x.Id == commentModel.Company);
            var comment = new Model.Entities.Comments
            {
                Id = Guid.NewGuid().ToString(),
                Company = company,
                Content = commentModel.Content,
                CreationDate = DateTime.Now,
                User = _user.GetSingle(x => x.Id == commentModel.User),
            };
            _comments.Add(comment);
            _comments.Commit();
            await Clients.Group(commentModel.Company)
                .SendAsync("addCommentSuccess", _comments.AllCompanyComments(company.Id));
        }

        [Authorize(Roles = "User,Admin")]
        public async Task CreateLikeOrDislike(CreateLikeOrDislikeViewModel likeOrDislikeModelodel)
        {
            var like = _likesOrDislikes.GetSingle(
                x => x.Comments.Id == likeOrDislikeModelodel.Comment && x.User.Id == likeOrDislikeModelodel.User,
                x => x.Comments, x => x.User);
            if (like == null)
            {
                _likesOrDislikes.Add(new LikesOrDislikes
                {
                    Comments = _comments.GetSingle(x => x.Id == likeOrDislikeModelodel.Comment),
                    Id = Guid.NewGuid().ToString(),
                    IsLike = likeOrDislikeModelodel.LikeOrDislike,
                    User = _user.GetSingle(x => x.Id == likeOrDislikeModelodel.User)
                });
                _likesOrDislikes.Commit();
            }
            else
            {
                like.IsLike = likeOrDislikeModelodel.LikeOrDislike;
                _likesOrDislikes.Update(like);
                _likesOrDislikes.Commit();
            }

            await Clients.Group(likeOrDislikeModelodel.Company).SendAsync("likeOrDislike",
                _comments.AllCompanyComments(likeOrDislikeModelodel.Company));
        }
    }
}