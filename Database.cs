using System.Reflection;
using System.Data.SQLite;
using System.IO;

namespace SRS
{
    static class Database
    {
        public static SQLiteConnection cardConnection { get; set; }
        public static SQLiteConnection programDBConnection { get; set; }
        
        static Database()
        {
            string exeFile = (new System.Uri(Assembly.GetEntryAssembly().Location)).AbsolutePath;
            string exeDir = Path.GetDirectoryName(exeFile);

            string japanesePath = Path.Combine(exeDir, @"db\Japanese.db");
            string programPath = Path.Combine(exeDir, @"db\program.db");

            cardConnection = new SQLiteConnection($"Data Source={japanesePath}");
            programDBConnection = new SQLiteConnection($"Data Source={programPath}");

            cardConnection.Open();
            programDBConnection.Open();
        }

        /// <summary>
        /// Close and dispose SQLite connections
        /// </summary>
        public static void CloseConnections()
        {
            cardConnection.Close();
            programDBConnection.Close();

            cardConnection.Dispose();
            programDBConnection.Dispose();
        }
    }
}
