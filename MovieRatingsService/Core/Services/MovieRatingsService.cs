using MovieRatingsApplication.Core.Interfaces;
using MovieRatingsApplication.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRatingsApplication.Core.Services
{
    public class MovieRatingsService
    {
        private readonly IMovieRatingsRepository RatingsRepository;

        public MovieRatingsService(IMovieRatingsRepository repo)
        {
            RatingsRepository = repo;
        }

        public int NumberOfMoviesWithGrade(int grade)
        {
            if (grade < 1 || grade > 5)
            {
                throw new ArgumentException("Grade must be 1 - 5");
            }

            HashSet<int> movies = new HashSet<int>();
            foreach (MovieRating rating in RatingsRepository.GetAllMovieRatings())
            {
                if (rating.Grade == grade)
                {
                    movies.Add(rating.Movie);
                }
            }
            return movies.Count;
        }

        public int GetNumberOfReviewsFromReviewer(int reviewer)
        {
            int count = 0;
            foreach(MovieRating m in RatingsRepository.GetAllMovieRatings())
            {
                if (m.Reviewer == reviewer)
                {
                    count++;
                }
            }
            return count;

            //return RatingsRepository.GetAllMovieRatings()
            //    .Where(r => r.Reviewer == reviewer)
            //    .Count();
        }

        public List<int> GetMoviesWithHighestNumberOfTopRates()
        {
            var movie5 = RatingsRepository.GetAllMovieRatings()
                .Where(r => r.Grade == 5)
                .GroupBy(r => r.Movie)
                .Select(group => new { 
                    Movie = group.Key,
                    MovieGrade5 = group.Count() 
                });

            int max5 = movie5.Max(grp => grp.MovieGrade5);

            return movie5
                .Where(grp => grp.MovieGrade5 == max5)
                .Select(grp => grp.Movie)
                .ToList();
        }

        public double GetAverageRateFromReviewer(int reviewer)
        {
            var averageRating = RatingsRepository.GetAllMovieRatings()
                .Where(r => r.Reviewer == reviewer)
                .Select(rating => rating.Grade).Average();

            return averageRating;
        }

        public int GetNumberOfRatesByReviewer(int reviewer, int grade)
        {
            var numberofReviews = RatingsRepository.GetAllMovieRatings()
                .Where(r => r.Reviewer == reviewer)
                .Where(g => g.Grade == grade).Count();
            
            return numberofReviews;
        }

        public int GetNumberOfReviews(int movie)
        {
            var numberofReviews = RatingsRepository.GetAllMovieRatings()
                .Where(m => m.Movie == movie).Count();

            return numberofReviews;
        }

        public double GetAverageRateOfMovie(int movie)
        {
            var averageRating = RatingsRepository.GetAllMovieRatings()
                .Where(m => m.Movie == movie)
                .Select(rating => rating.Grade).Average();

            double roundedRating = Math.Round(averageRating, 2);
            return roundedRating;
        }

        public object GetNumberOfRates(int movie, int grade)
        {
            var numberofRates = RatingsRepository.GetAllMovieRatings()
                .Where(m => m.Movie == movie)
                .Where(g => g.Grade == grade).Count();

            return numberofRates;
        }

        public List<int> GetMostProductiveReviewers()
        {
            var topReviewer = RatingsRepository.GetAllMovieRatings()
                .GroupBy(r => r.Reviewer)
                .Select(group => new
                {
                    Reviewer = group.Key,
                    reviewCount = group.Count()

                });

            int maxCount = topReviewer.Max(grp => grp.reviewCount);

            return topReviewer
                .Where(grp => grp.reviewCount == maxCount)
                .Select(grp => grp.Reviewer)
                .ToList();

        }

        public List<int> GetTopRatedMovies(int numberofMovies)
        {
            var topratedMovies = RatingsRepository.GetAllMovieRatings()
                .GroupBy(m => m.Movie)
                .Select(group => new
                {
                    Movie = group.Key,
                    avg = group.Average(g => g.Grade)
                }).OrderByDescending(g => g.avg).Take(numberofMovies);

            return topratedMovies
                .Select(m => m.Movie)
                .ToList();

        }

        public List<int> GetTopMoviesByReviewer(int reviewer)
        {
            var sortedbyRating = RatingsRepository.GetAllMovieRatings()
                .Where(r => r.Reviewer == reviewer)
                .OrderByDescending(g => g.Grade)
                .ThenByDescending(d => d.Date);

            return sortedbyRating
                .Select(m => m.Movie)
                .ToList();
        }

        public List<int> GetReviewersByMovie(int movie)
        {
            var sortedbyRating = RatingsRepository.GetAllMovieRatings()
                .Where(m => m.Movie == movie)
                .OrderByDescending(g => g.Grade)
                .ThenByDescending(d => d.Date);

            return sortedbyRating
                .Select(r => r.Reviewer)
                .ToList();
        }
    }
}
