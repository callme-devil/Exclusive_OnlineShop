﻿namespace BlogManagement.Application.Contracts.Article
{
    public class EditArticle : CreateArticle
    {
        public int Id { get; set; }

        public string Image { get; set; }
    }
}
