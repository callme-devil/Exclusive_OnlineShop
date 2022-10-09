﻿using _01_ExclusiveQuery.Contracts.Comment;

namespace _01_ExclusiveQuery.Contracts.Article
{
    public class ArticleQueryModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public string Picture { get; set; }

        public string PictureAlt { get; set; }

        public string PictureTitle { get; set; }

        public string Slug { get; set; }

        public string MetaDescription { get; set; }

        public string Keywords { get; set; }

        public string CanonicalAddress { get; set; }

        public int CategoryId { get; set; }

        public string PublishDate { get; set; }

        public bool IsDeleted { get; set; }

        public string CategoryName { get; set; }

        public string CategorySlug { get; set; }

        public List<string> KeyWordList { get; set; }

        public List<CommentQueryModel> Comments { get; set; }
    }
}
