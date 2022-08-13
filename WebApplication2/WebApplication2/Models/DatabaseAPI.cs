using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class DatabaseAPI
    {
        public static bool isArticleIdInArticles(int articleID)
        {
            foreach(Article article in getAllArticles())
            {
                if(article.Id == articleID)
                {
                    return true;
                }
            }
            return false;
        }
        public static List<Article> getAllArticles()
        {
            List<Article> articles = new List<Article>();
            System.Data.DataSet articles_dataset = MvcApplication.DBconn.RunDataSetSQL("SELECT * FROM Articles ORDER BY ArticleID DESC");
            for(int i = 0;i < articles_dataset.Tables[0].Rows.Count;i++)
            {
                Article a = new Article() { Id = int.Parse(articles_dataset.Tables[0].Rows[i]["ArticleID"].ToString()),body_english = articles_dataset.Tables[0].Rows[i]["body_english"].ToString(), body_arabic = articles_dataset.Tables[0].Rows[i]["body_arabic"].ToString(), body_hebrew = articles_dataset.Tables[0].Rows[i]["body_hebrew"].ToString(),title_english = articles_dataset.Tables[0].Rows[i]["title_english"].ToString(),title_arabic= articles_dataset.Tables[0].Rows[i]["title_arabic"].ToString(), title_hebrew =articles_dataset.Tables[0].Rows[i]["title_hebrew"].ToString(),articleImage = articles_dataset.Tables[0].Rows[i]["ArticleImage"].ToString(),articleSubject = articles_dataset.Tables[0].Rows[i]["ArticleSubject"].ToString() };
                articles.Add(a);
            }

            return articles;
        }

        public static List<Comment> GetAllCommentsOfArticle(int articleID)
        {
            List<Comment> comments = new List<Comment>();
            System.Data.DataSet comments_dataset = MvcApplication.DBconn.RunDataSetSQL("SELECT * FROM Comments where ArticleID = " + articleID + " ORDER BY CommentID DESC");
            for(int i = 0;i < comments_dataset.Tables[0].Rows.Count;i++)
            {
                Comment a = new Comment()
                {
                    articleID = articleID,
                    commenter_name = comments_dataset.Tables[0].Rows[i]["CommenterName"].ToString(),
                    comment_id = int.Parse(comments_dataset.Tables[0].Rows[i]["CommentID"].ToString()),
                    comment_text = comments_dataset.Tables[0].Rows[i]["CommentBody"].ToString(),
                    if_replied = bool.Parse(comments_dataset.Tables[0].Rows[i]["is_replied"].ToString()),
                    reply = comments_dataset.Tables[0].Rows[i]["reply_text"].ToString(),
                    PFP_PIC_ID = comments_dataset.Tables[0].Rows[i]["PFP_PIC_ID"].ToString(),
                    TimeStamp = long.Parse(comments_dataset.Tables[0].Rows[i]["TimeStampComment"].ToString())
                };
                comments.Add(a);
            }
            return comments;
        }
    }
}