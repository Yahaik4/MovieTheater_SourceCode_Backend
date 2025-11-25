using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace ApiGateway.DataTransferObject.Parameter
{
    public class CreateMovieFormParam
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public TimeSpan Duration { get; set; }
        public string Publisher { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }

        public IFormFile PosterFile { get; set; }

        public string TrailerUrl { get; set; }
        public string Status { get; set; }

        public List<MovieGenreParam> Genres { get; set; }
        public List<MoviePersonParam> Persons { get; set; }
    }
}
