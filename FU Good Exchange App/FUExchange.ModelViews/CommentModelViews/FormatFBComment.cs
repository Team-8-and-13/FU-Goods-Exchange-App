
namespace FUExchange.ModelViews.CommentModelViews
{
    public class FormatFBComment
    {
        public required CommentModelView comment { get; set; }
        public required ICollection<CommentModelView> ReplyList { get; set; }
    }
}
