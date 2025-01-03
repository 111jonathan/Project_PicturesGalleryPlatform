using Dapper;
using Microsoft.Data.SqlClient;

namespace Project_PicturesGalleryPlatform.Repositories.MyFavoritesRepository
{
    public class MyFavoritesRepository : IMyFavoritesRepository
    {
        private readonly string ConnectionString = "Server=tcp:test241214.database.windows.net,1433;Initial Catalog=Test;Persist Security Info=False;User ID=test;Password=Abcd1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private void ExecuteNonQuery(string sqlQuery, object parameters = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute(sqlQuery, parameters);
            }
        }
        public void AddFavorite(string userAccount, int pictureId)
        {
            var sqlQuery = "INSERT INTO favorite (userAccount, pictureId) VALUES (@UserAccount, @PictureId)";
            ExecuteNonQuery(sqlQuery, new { UserAccount = userAccount, PictureId = pictureId });
        }

        public void RemoveFavorite(string userAccount, int pictureId)
        {
            var sqlQuery = "DELETE FROM favorite WHERE userAccount = @UserAccount AND pictureId = @PictureId";
            ExecuteNonQuery(sqlQuery, new { UserAccount = userAccount, PictureId = pictureId });
        }

        public int IsPictureInFavorites(string userAccount, int pictureId)
        {
            var sqlQuery = "SELECT CASE WHEN EXISTS (SELECT 1 FROM favorite WHERE userAccount = @UserAccount AND pictureId = @PictureId) THEN 1 ELSE 0 END";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.ExecuteScalar<int>(sqlQuery, new { UserAccount = userAccount, PictureId = pictureId });
            }
        }

        public List<int> GetUserFavoritePictureIds(string userAccount)
        {
            var sqlQuery = "SELECT pictureId FROM favorite WHERE userAccount = @UserAccount";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<int>(sqlQuery, new { UserAccount = userAccount }).ToList();
            }
        }
    }
}
