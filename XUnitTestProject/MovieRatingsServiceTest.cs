using FluentAssertions;
using Moq;
using MovieRatingsApplication.Core.Interfaces;
using MovieRatingsApplication.Core.Model;
using MovieRatingsApplication.Core.Services;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace XUnitTestProject
{
    public class MovieRatingsServiceTest
    {
        private List<MovieRating> ratings = null;
        private Mock<IMovieRatingsRepository> repoMock;

        public MovieRatingsServiceTest()
        {
            repoMock = new Mock<IMovieRatingsRepository>();
            repoMock.Setup(repo => repo.GetAllMovieRatings()).Returns(() => ratings);
        }

        // returns the number movies which have got the grade N.

        [Theory]
        [InlineData(1, 0)]
        [InlineData(3, 1)]
        [InlineData(5, 2)]
        public void NumberOfMoviesWithGrade(int grade, int expected)
        {
            // arrange
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 1, 3, DateTime.Now),
                new MovieRating(2, 1, 3, DateTime.Now),
                new MovieRating(3, 1, 4, DateTime.Now),

                new MovieRating(3, 5, 5, DateTime.Now),
                new MovieRating(3, 2, 5, DateTime.Now),
                new MovieRating(4, 2, 5, DateTime.Now)
            };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);

            // act
            int result = mrs.NumberOfMoviesWithGrade(grade);

            // assert
            Assert.Equal(expected, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(6)]
        public void NumberOfMoviesWithGradeInvalidExpectArgumentException(int grade)
        {
            // arrange
            Mock<IMovieRatingsRepository> repoMock = new Mock<IMovieRatingsRepository>();
            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);

            // act
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                int result = mrs.NumberOfMoviesWithGrade(grade);
            });

            // assert
            Assert.Equal("Grade must be 1 - 5", ex.Message);
        }


        //  1. On input N, what are the number of reviews from reviewer N? 

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        public void GetNumberOfReviewsFromReviewer(int reviewer, int expected)
        {
            // arrange
            ratings = new List<MovieRating>()
            {
                new MovieRating(2, 1, 3, DateTime.Now),
                new MovieRating(3, 1, 4, DateTime.Now),
                new MovieRating(3, 2, 3, DateTime.Now),
                new MovieRating(4, 1, 4, DateTime.Now)
            };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);

            // act

            int result = mrs.GetNumberOfReviewsFromReviewer(reviewer);

            // assert
            Assert.Equal(expected, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);
        }



        // 2. On input N, what is the average rate that reviewer N had given?
        [Theory]
        [InlineData(1, 4)]
        [InlineData(2, 3.25)]

        public void GetAverageRateFromReviewer(int reviewer, double expected)
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 1, 4, DateTime.Now),
                new MovieRating(2, 1, 4, DateTime.Now),
                new MovieRating(2, 2, 2, DateTime.Now),
                new MovieRating(2, 3, 4, DateTime.Now),
                new MovieRating(2, 4, 3, DateTime.Now)

            };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);

            double result = mrs.GetAverageRateFromReviewer(reviewer);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetAverageRateFromReviewerInvalidException()
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 1, 4, DateTime.Now)
             };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);

            int reviewer = 2;

            Assert.Throws<InvalidOperationException>(() => mrs.GetAverageRateFromReviewer(reviewer));

        }



        // 3. On input N and R, how many times has reviewer N given rate R?
        [Theory]
        [InlineData(1, 4, 1)]
        [InlineData(2, 4, 2)]
        [InlineData(4, 4, 0)]

        public void GetNumberOfRatesByReviewer(int reviewer, int grade, int expected)
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 1, 4, DateTime.Now),
                new MovieRating(2, 1, 4, DateTime.Now),
                new MovieRating(2, 2, 2, DateTime.Now),
                new MovieRating(2, 3, 4, DateTime.Now),
                new MovieRating(2, 4, 3, DateTime.Now),
                new MovieRating(3, 1, 4, DateTime.Now)
            };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);

            int result = mrs.GetNumberOfRatesByReviewer(reviewer, grade);

            Assert.Equal(expected, result);

        }

        // 4. On input N, how many have reviewed movie N?
        [Theory]
        [InlineData(1, 3)]
        [InlineData(2, 1)]
        [InlineData(5, 0)]

        public void GetNumberOfReviews(int movie, int expected)
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 1, 4, DateTime.Now),
                new MovieRating(2, 1, 4, DateTime.Now),
                new MovieRating(2, 2, 2, DateTime.Now),
                new MovieRating(2, 3, 4, DateTime.Now),
                new MovieRating(2, 4, 3, DateTime.Now),
                new MovieRating(3, 1, 4, DateTime.Now)
            };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);

            int result = mrs.GetNumberOfReviews(movie);

            Assert.Equal(expected, result);

        }

        // 5. On input N, what is the average rate the movie N had received?

        [Theory]
        [InlineData(1, 3.67)]
        [InlineData(2, 3.33)]

        public void GetAverageRateOfMovie(int movie, double expected)
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 1, 4, DateTime.Now),
                new MovieRating(2, 1, 4, DateTime.Now),
                new MovieRating(2, 2, 2, DateTime.Now),
                new MovieRating(3, 2, 4, DateTime.Now),
                new MovieRating(4, 1, 3, DateTime.Now),
                new MovieRating(4, 2, 4, DateTime.Now)
            };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);

            double result = mrs.GetAverageRateOfMovie(movie);

            Assert.Equal(expected, result);

        }

        [Fact]
        public void GetAverageRateOfMovieInvalidException()
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 1, 4, DateTime.Now)
             };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);

            int movie = 2;

            Assert.Throws<InvalidOperationException>(() => mrs.GetAverageRateOfMovie(movie));

        }

        // 6. On input N and R, how many times had movie N received rate R?

        [Theory]
        [InlineData(1, 4, 2)]
        [InlineData(2, 2, 1)]
        [InlineData(3, 4, 0)]

        public void GetNumberOfRates(int movie, int grade, int expected)
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 1, 4, DateTime.Now),
                new MovieRating(2, 1, 4, DateTime.Now),
                new MovieRating(2, 2, 2, DateTime.Now),
                new MovieRating(3, 2, 4, DateTime.Now),
                new MovieRating(4, 1, 3, DateTime.Now),
                new MovieRating(4, 2, 4, DateTime.Now)
            };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);

            var result = mrs.GetNumberOfRates(movie, grade);

            Assert.Equal(expected, result);
        }


        //  7. What is the id(s) of the movie(s) with the highest number of top rates (5)? 
        [Fact]
        public void GetMoviesWithHighestNumberOfTopRates()
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 1, 5, DateTime.Now),
                new MovieRating(1, 2, 5, DateTime.Now),

                new MovieRating(2, 1, 4, DateTime.Now),
                new MovieRating(2, 2, 5, DateTime.Now),

                new MovieRating(2, 3, 5, DateTime.Now),
                new MovieRating(3, 3, 5, DateTime.Now),
            };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);

            List<int> expected = new List<int>() { 2, 3 };

            // act
            var result = mrs.GetMoviesWithHighestNumberOfTopRates();

            // assert
            Assert.Equal(expected, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);

        }

        // 8. What reviewer(s) had done most reviews?
        [Fact]
        public void GetMostProductiveReviewers()
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 1, 4, DateTime.Now),
                new MovieRating(2, 1, 4, DateTime.Now),
                new MovieRating(2, 2, 2, DateTime.Now),
                new MovieRating(3, 2, 4, DateTime.Now),
                new MovieRating(4, 1, 3, DateTime.Now),
                new MovieRating(4, 2, 4, DateTime.Now)
            };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);

            List<int> expected = new List<int>() { 2, 4 };

            var result = mrs.GetMostProductiveReviewers();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetMostProductiveReviewersNoReviewers()
        {
            ratings = new List<MovieRating>()
            {

            };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);

            Assert.Throws<InvalidOperationException>(() => mrs.GetMostProductiveReviewers());

        }

        // 9. On input N, what is top N of movies? The score of a movie is its average rate.
        [Fact]

        public void GetTopRatedMovies()
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 1, 1, DateTime.Now),
                new MovieRating(1, 2, 2, DateTime.Now),
                new MovieRating(1, 3, 3, DateTime.Now),
                new MovieRating(2, 1, 4, DateTime.Now),
                new MovieRating(2, 2, 5, DateTime.Now),
                new MovieRating(2, 3, 1, DateTime.Now),
                new MovieRating(3, 1, 2, DateTime.Now),
                new MovieRating(3, 2, 3, DateTime.Now),
                new MovieRating(3, 3, 4, DateTime.Now),
                new MovieRating(3, 4, 5, DateTime.Now)
            };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);
            int numberofMovies = 2;
            List<int> expected = new List<int>() { 4, 2 };

            var result = mrs.GetTopRatedMovies(numberofMovies);

            Assert.Equal(expected, result);

        }

        // 10. On input N, what are the movies that reviewer N has reviewed? The list should be sorted decreasing by rate first, and date secondly
        [Fact]

        public void GetTopMoviesByReviewer()
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 1, 1, DateTime.Now.AddDays(-3)),
                new MovieRating(1, 2, 2, DateTime.Now.AddDays(-6)),
                new MovieRating(1, 3, 2, DateTime.Now.AddDays(-7)),
                new MovieRating(2, 1, 4, DateTime.Now.AddDays(-46)),
                new MovieRating(2, 2, 5, DateTime.Now.AddDays(-1)),
                new MovieRating(2, 3, 1, DateTime.Now.AddDays(-2)),
                new MovieRating(3, 1, 2, DateTime.Now.AddDays(-42)),
                new MovieRating(3, 2, 3, DateTime.Now.AddDays(-24)),
                new MovieRating(3, 3, 4, DateTime.Now.AddDays(-8)),
                new MovieRating(3, 4, 5, DateTime.Now.AddDays(-9))
            };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);
            int reviewer = 1;
            var expected = new List<int>() { 2, 3, 1 };

            var result = mrs.GetTopMoviesByReviewer(reviewer);

            Assert.Equal(expected, result);
        }

        // 11. On input N, who are the reviewers that have reviewed movie N? The list should be sorted decreasing by rate first, and date secondly.
        [Fact]

        public void GetReviewersByMovie()
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 1, 1, DateTime.Now.AddDays(-3)),
                new MovieRating(1, 2, 2, DateTime.Now.AddDays(-6)),
                new MovieRating(1, 3, 2, DateTime.Now.AddDays(-7)),
                new MovieRating(2, 1, 2, DateTime.Now.AddDays(-46)),
                new MovieRating(2, 2, 5, DateTime.Now.AddDays(-1)),
                new MovieRating(2, 3, 1, DateTime.Now.AddDays(-2)),
                new MovieRating(3, 1, 2, DateTime.Now.AddDays(-42)),
                new MovieRating(3, 2, 3, DateTime.Now.AddDays(-24)),
                new MovieRating(3, 3, 4, DateTime.Now.AddDays(-8)),
                new MovieRating(3, 4, 5, DateTime.Now.AddDays(-9))
            };

            MovieRatingsService mrs = new MovieRatingsService(repoMock.Object);
            int movie = 1;
            var expected = new List<int>() { 3, 2, 1 };

            var result = mrs.GetReviewersByMovie(movie);

            Assert.Equal(expected, result);
        }

    }
}
