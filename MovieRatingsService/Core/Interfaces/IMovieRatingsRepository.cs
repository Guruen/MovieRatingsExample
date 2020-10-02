using MovieRatingsApplication.Core.Model;
using System.Collections.Generic;

namespace MovieRatingsApplication.Core.Interfaces
{
    public interface IMovieRatingsRepository
    {
        IList<MovieRating> GetAllMovieRatings();
    }
}
