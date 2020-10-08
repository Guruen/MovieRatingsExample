using MovieRatingJSONRepository;
using MovieRatingsApplication.Core.Interfaces;
using System;

namespace JSONReaderConsole
{
    class Program
    {
        const string JSON_FilePath = @"C:\Users\BBran\source\repos\MovieRatingsExample\ratings.json";
        static void Main(string[] args)
        {
            IMovieRatingsRepository repo = new MovieRatingRepository(JSON_FilePath);

        }
    }
}
