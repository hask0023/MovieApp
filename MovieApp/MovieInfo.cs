using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.TMDb;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace MovieApp
{
    /// <summary>
    /// Class containing all of the HTTP requests and JSON parsing necessary for app functionality
    /// Authors: Steve Haskins & Nune Vardanyan
    /// </summary>
    class MovieInfo
    {
        public List<string> asbList;
        public string allGenres;
        public string fullCast;
        public string movieDirector;
        public string runtime;
        public string plot;
        public string title;
        public BitmapImage posterBitMap;

        public MovieInfo()
        {
            asbList = new List<string>();
            allGenres = string.Empty;
            fullCast = "Cast: ";
            movieDirector = "Director: ";
            runtime = string.Empty;
            plot = string.Empty;
            title = string.Empty;
        }

        // task used to search for all movies, used primarily to populate autocomplete suggestions
        public async Task SearchAllMovies(String searchQuery)
        {


            using (var client = new ServiceClient("7ebbe16045b752de5a97201e94696914"))
            {

                // gives us the JSON response for our search
                var movies = await client.Movies.SearchAsync(searchQuery, null, true, null, 1, CancellationToken.None);

                asbList = new List<string>();

                foreach (Movie m in movies.Results)
                {
                    string title = m.Title;
                    asbList.Add(title);

                }

            }

        }

        //task used to find a specific movie and its relevant information for the detailed view screen
        public async Task searchSpecificMovie(String searchQuery)
        {
            byte movieCounter = 0;
            using (var client = new ServiceClient("7ebbe16045b752de5a97201e94696914"))
            {
                var movies = await client.Movies.SearchAsync(searchQuery, null, true, null, 1, CancellationToken.None);
               

                foreach (Movie m in movies.Results)
                {
                    //safeguard in the rare case that there is more than one movie with the exact same name.
                    if (movieCounter > 0)
                    {
                        movieCounter = 0;
                        break;
                    }

                    if (m.Title.Equals(searchQuery))
                    {
                        //http calls to get detailed movie info and pictures
                        var movie = await client.Movies.GetAsync(m.Id, null, true, CancellationToken.None);
                        var images = await client.Movies.GetImagesAsync(m.Id, null, CancellationToken.None);
                        byte counter = 0;

                        
                        // get all genres for movie
                        foreach (System.Net.TMDb.Genre theGenres in movie.Genres)
                        {
                            string value = theGenres.Name.ToString();
                            allGenres = allGenres + value + ", ";

                        }


                        // get all cast members
                        foreach (System.Net.TMDb.MediaCast theCast in movie.Credits.Cast)
                        {
                            // limit to 5 top billed cast members
                            if (counter >= 5)
                            {
                                counter = 0;
                                break;
                            }
                            string value = theCast.Name.ToString();
                            fullCast = fullCast + value + ", ";
                            counter++;
                        }

                        // get the director
                        foreach (System.Net.TMDb.MediaCrew theDirector in movie.Credits.Crew)
                        {
                            string value = theDirector.Name.ToString();
                            movieDirector = movieDirector + value;
                            break;
                        }

                        // formatting results to be ready to be sent back
                        fullCast = fullCast.Remove(fullCast.Length - 2);
                        allGenres = allGenres.Remove(allGenres.Length - 2);
                        runtime = movie.Runtime.ToString() + " min";
                        plot = movie.Overview.ToString();
                        title = movie.Title.ToString();

                        //set the backdrop image for the movie
                        foreach (System.Net.TMDb.Image image in images.Backdrops)
                        {
                            var poster_uri = String.Format("http://image.tmdb.org/t/p/w500/{0}", image.FilePath.ToString());
                            Uri myUri = new Uri(poster_uri, UriKind.RelativeOrAbsolute);
                            BitmapImage bmi = new BitmapImage();
                            bmi.UriSource = myUri;
                            posterBitMap = bmi;
                            break;
                        }
                        movieCounter++;
                    }
                    
                }
            }
        }
    }
}
