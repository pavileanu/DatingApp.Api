using System;
using Microsoft.AspNetCore.Http;

namespace MyApi.Dtos
{
    public class PhotoForCreationDbo
    {
        public string Url { get; set; }

        public IFormFile File {get; set;}

        public string Description { get; set; }
        
        public DateTime DateAdded { get; set; }

        public string PublicId {get; set;}

        public PhotoForCreationDbo()
        {
            DateAdded = DateTime.Now;
        }
    }
}