using MovieRatingsApplication.Core.Interfaces;
using MovieRatingsApplication.Core.Services;
using System;
using System.Diagnostics;
using Xunit;

namespace XUnitTestProject___PerformanceTest
{
    public class MovieRatingsServicePerformanceTest : IClassFixture<TestFixture>
    {

        private IMovieRatingsRepository repo;

        private int reviewerMostReviews;
        private int movieMostReviews;

        public MovieRatingsServicePerformanceTest(TestFixture data)
        {
            repo = data.Repo;
            reviewerMostReviews = data.ReviewerMostReviews;
            movieMostReviews = data.MovieMostReviews;
        }

        private double TimeInSeconds(Action ac)
        {
            Stopwatch sw = Stopwatch.StartNew();
            ac.Invoke();
            sw.Stop();
            return sw.ElapsedMilliseconds / 1000d;
        }
        
        [Fact]
        public void MostRatedMovie()
        {
            MovieRatingsService mrs = new MovieRatingsService(repo);

            double seconds = TimeInSeconds(() =>
            {
                var result = mrs.GetNumberOfReviews(movieMostReviews);
            });

            Assert.True(seconds <= 4);
        }


        [Fact]        
        public void AverageRatingMostRatedMovie()
        {
            MovieRatingsService mrs = new MovieRatingsService(repo);

            double seconds = TimeInSeconds(() =>
            {
                var result = mrs.GetAverageRateOfMovie(movieMostReviews);
            });

            Assert.True(seconds <= 4);
        }

        [Fact]
        public void AverageRateFromReviewer()
        {
            MovieRatingsService mrs = new MovieRatingsService(repo);

            double seconds = TimeInSeconds(() =>
            {
                var result = mrs.GetAverageRateFromReviewer(reviewerMostReviews);
            });

            Assert.True(seconds <= 4);
        }


        [Fact]
        public void TopMoviesByReviewer()
        {
            MovieRatingsService mrs = new MovieRatingsService(repo);

            double seconds = TimeInSeconds(() =>
            {
                var result = mrs.GetTopMoviesByReviewer(movieMostReviews);
            });

            Assert.True(seconds <= 4);
        }


        [Fact]
        public void NumberOfReviewsFromReviewer()
        {
            MovieRatingsService mrs = new MovieRatingsService(repo);

            double seconds = TimeInSeconds(() =>
            {
                var result = mrs.GetNumberOfReviewsFromReviewer(reviewerMostReviews);
            });

            Assert.True(seconds <= 4);
        }

   

    }
}
