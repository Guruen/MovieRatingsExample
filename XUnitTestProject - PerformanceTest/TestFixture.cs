using MovieRatingJSONRepository;
using MovieRatingsApplication.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUnitTestProject___PerformanceTest
{
    public class TestFixture : IDisposable
    {
        const string JSON_FilePath = @"C:\Users\BBran\source\repos\MovieRatingsExample\ratings.JSON";

        public IMovieRatingsRepository Repo { get; private set; }
        public int ReviewerMostReviews { get; private set; }
        public int MovieMostReviews { get; private set; }

        public TestFixture()
        {
            Repo = new MovieRatingRepository(JSON_FilePath);

            ReviewerMostReviews = Repo.Ratings
                .GroupBy(r => r.Reviewer)
                .Select(grp => new
                {
                    reviewer = grp.Key,
                    reviews = grp.Count()
                })
                .OrderByDescending(grp => grp.reviews)
                .Select(grp => grp.reviewer)
                .FirstOrDefault();

            MovieMostReviews = Repo.Ratings
                .GroupBy(m => m.Movie)
                .Select(grp => new
                {
                    movie = grp.Key,
                    reviews = grp.Count()
                })
                .OrderByDescending(grp => grp.reviews)
                .Select(grp => grp.movie)
                .FirstOrDefault();

        }


        public void Dispose()
        {
           // Empty Spaaaaaace!
        }
    }
}
