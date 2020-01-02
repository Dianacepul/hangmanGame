using System.Collections.Generic;
using System.Data.SqlClient;

namespace HangmanMyra
{
    internal class Connection
    {
        public SqlConnection con;

        public void Connect()
        {
            con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog =HangmanProject");
            con.Open();
            using (SqlCommand command = new SqlCommand("IF NOT EXISTS(SELECT * FROM sysobjects WHERE name= 'Words') CREATE TABLE Words(Theme varchar(255), Words varchar(255), Level varchar(255))", con))
            {
                command.ExecuteNonQuery();
            }
        }

        public void InsertData(string topic, List<string> words)
        {
            foreach (var item in words)
            {
                using (SqlCommand command = new SqlCommand("INSERT INTO Words(Theme, Words, Level) VALUES(@Theme,@Words, @Level)", con))
                {
                    command.Parameters.Add(new SqlParameter("@Theme", System.Data.SqlDbType.VarChar));
                    command.Parameters.Add(new SqlParameter("@Words", System.Data.SqlDbType.VarChar));
                    command.Parameters.Add(new SqlParameter("@Level", System.Data.SqlDbType.VarChar));

                    command.Parameters["@Theme"].Value = topic;
                    command.Parameters["@Words"].Value = item;
                    command.Parameters["@Level"].Value = Level.LevelDefinition(item);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<string> GetTopics()
        {
            List<string> topicsData = new List<string>();
            using (SqlCommand command = new SqlCommand("SELECT DISTINCT Theme FROM Words", con))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        topicsData.Add(reader["Theme"].ToString());
                    }
                }
                return topicsData;
            }
        }

        public List<string> GetLevel(string ChosenTheme)
        {
            List<string> leveldata = new List<string>();
            using (SqlCommand command = new SqlCommand($"SELECT DISTINCT Level FROM Words where Theme ='{ChosenTheme}'", con))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        leveldata.Add(reader["Level"].ToString());
                    }
                    return leveldata;
                }
            }
        }

        public List<string> GetWords(string inputLevel, string inputTopic)
        {
            List<string> wordsList = new List<string>();
            using (SqlCommand command = new SqlCommand($"SELECT Words from Words where Theme ='{inputTopic}'and Level = '{inputLevel}'", con))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        wordsList.Add(reader["Words"].ToString());
                    }
                    return wordsList;
                }
            }
        }
    }
}