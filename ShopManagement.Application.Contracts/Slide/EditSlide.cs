﻿namespace ShopManagement.Application.Contracts.Slide
{
    public class EditSlide : CreateSlide
    {
        public int Id { get; set; }

        public string Image { get; set; }
    }
}
