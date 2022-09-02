﻿namespace _01_ExclusiveQuery.Contracts.Product
{
    public class ProductQueryModel
    {
        public int Id { get; set; }

        public string Picture { get; set; }

        public string PictureAlt { get; set; }

        public string PictureTitle { get; set; }

        public string Name { get; set; }

        public string Price { get; set; }

        public string PriceWithDiscount { get; set; }

        public int DiscountRate { get; set; }

        public string ProductCategory { get; set; }

        public string Slug { get; set; }
    }
}
