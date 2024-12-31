using Microsoft.Data.SqlClient;

namespace Project_PicturesGalleryPlatform.Models.FavoriteModel
{
    public class FavoriteSetting
    {
        private static readonly string connectionString =
            "Server=tcp:group1project.database.windows.net,1433;Initial Catalog=PicturesGallery;Persist Security Info=False;" +
            "User ID=manager;Password=Abcd1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public string userAccount { get; set; }
        public int pictureId { get; set; }
        public bool isFavorite { get; set; }
        public FavoriteSetting(string userAccount, int pictureId)
        {
            this.userAccount = userAccount;
            this.pictureId = pictureId;
        }


        /// <summary>
        /// 判斷是否已加入我的最愛
        /// </summary>
        public void JudgeState()
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                string commandString = "SELECT userAccount, pictureId FROM Favorite WHERE userAccount = @userAccount AND pictureId = @pictureId";
                SqlCommand sqlCommand = _setParameter(commandString, sqlConnection);
                using (var reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {// 已加入我的最愛
                        isFavorite = true;
                    }
                    else
                    {// 未加入我的最愛
                        isFavorite = false;
                    }
                }
            }
        }


        /// <summary>
        /// 根據isFavorite，true則刪除;false則新增。暫無回傳值
        /// </summary>
        public void AddAndDelete()
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                string commandString;
                if (this.isFavorite)
                {
                    //// 已存在，執行刪除
                    commandString = "DELETE FROM Favorite WHERE userAccount = @userAccount AND pictureId = @pictureId";
                    SqlCommand sqlCommand = _setParameter(commandString, sqlConnection);
                    sqlCommand.ExecuteNonQuery();
                }
                else
                {
                    //// 不存在，執行插入
                    commandString = "INSERT INTO Favorite(userAccount, pictureId) VALUES(@userAccount, @pictureId)";
                    SqlCommand sqlCommand = _setParameter(commandString, sqlConnection);
                    sqlCommand.ExecuteNonQuery();
                }
                
            }
        }
        /// <summary>
        /// 建立新sqlcommand、灌參數
        /// </summary>
        /// <returns>sqlCommand</returns>
        private SqlCommand _setParameter(string commandString, SqlConnection sqlConnection)
        {
            SqlCommand sqlCommand = new SqlCommand(commandString, sqlConnection);
            sqlCommand.Parameters.Add(new SqlParameter("@userAccount", this.userAccount));
            sqlCommand.Parameters.Add(new SqlParameter("@pictureId", this.pictureId));
            return sqlCommand;
        }
    }
}
