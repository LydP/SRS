using System;
using System.Reflection;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Globalization;
using System.Diagnostics;
using System.IO;

namespace SRS
{
    public partial class Form1 : Form
    {
        private static Deck deck { get; set; }
        private int deckChoice { get; set; }
        private List<int> learningDeck { get; set; }
        private List<int> newDeck { get; set; }
        private SQLiteConnection cardConnection { get; set; }
        private SQLiteConnection programDBConnection { get; set; }

        int viewingTime;
        Random rnd = new Random();
        // used to select cards
        int popularity;    

        public Form1()
        {
            InitializeComponent();            
        }

        /// <summary
        /// "didn't remember" selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wrong_Click_1(object sender, EventArgs e)
        {
            right.Visible = false;
            wrong.Visible = false;
            showCardButton.Visible = true;

            // set riar in statistics and criar in cards to 0            
            int nrev;

            string maxNrev = $"SELECT max(nrev) FROM statistics WHERE popularity={popularity}";

            using (SQLiteCommand cmd = new SQLiteCommand(maxNrev, cardConnection))
            {
                object result = cmd.ExecuteScalar();
                result = (result == DBNull.Value) ? null : result;
                nrev = Convert.ToInt32(result);
            }

            string updateRiar = $"UPDATE statistics SET riar = 0 WHERE nrev={nrev}";

            using (SQLiteCommand cmd = new SQLiteCommand(updateRiar, cardConnection))
            {
                cmd.ExecuteNonQuery();
            }

            string updateCriar = $"UPDATE cards SET criar = 0 WHERE popularity={popularity}";

            using (SQLiteCommand cmd = new SQLiteCommand(updateCriar, cardConnection))
            {
                cmd.ExecuteNonQuery();
            }

            cardBack.Text = "";
            // add properties of this review to sqlite database
            createReview(0);
            
            deck.ManageDeck(0, popularity);
            deckChoice = deck.choice;
            showFront();

            // new card's shown so reset and start timer
            viewingTime = 0;
            viewingTimerLabel.Text = "0:00";
            viewingTimer.Start();
        }

        /// <summary>
        /// "remembered" selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void right_Click_1(object sender, EventArgs e)
        {
            right.Visible = false;
            wrong.Visible = false;
            showCardButton.Visible = true;

            // increment riar in statistics and criar in cards            
            int nrev;

            string maxNrev = $"SELECT max(nrev) FROM statistics WHERE popularity={popularity}";

            using (SQLiteCommand cmd = new SQLiteCommand(maxNrev, cardConnection))
            {
                object result = cmd.ExecuteScalar();
                result = (result == DBNull.Value) ? null : result;
                nrev = Convert.ToInt32(result);
            }

            string updateCriar = $"UPDATE cards SET criar = criar + 1 WHERE popularity={popularity}";

            using (SQLiteCommand cmd = new SQLiteCommand(updateCriar, cardConnection))
            {
                cmd.ExecuteNonQuery();
            }

            cardBack.Text = "";
            // add properties of this review to sqlite database
            createReview(1);
            deck.ManageDeck(1, popularity);
            deckChoice = deck.choice;

            showFront();

            // new card's shown so reset and start timer
            viewingTime = 0;
            viewingTimerLabel.Text = "0:00";
            viewingTimer.Start();
        }

        /// <summary>
        /// shows a card's answer when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showCardButton_Click(object sender, EventArgs e)
        {
            showCardButton.Visible = false;
            right.Visible = true;
            wrong.Visible = true;

            showBack();

            viewingTimer.Stop();
        }

        /// <summary>
        /// Timer to record how long the user spends on a card
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewingTimer_Tick(object sender, EventArgs e)
        {
            viewingTime++;

            int seconds = viewingTime % 60;
            int minutes = viewingTime / 60;

            viewingTimerLabel.Text = $"{minutes}:{seconds.ToString("D2")}";
        }

        /// <summary>
        /// Starts a session
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startButton_Click(object sender, EventArgs e)
        {
            cardConnection = Database.cardConnection;
            programDBConnection = Database.programDBConnection;
            int session = sessionCheck();

            // initialize newDeck and learningDeck so Form1_FormClosing() doesn't bug out when running on a complete session
            newDeck = new List<int>();
            learningDeck = new List<int>();

            switch (session)
            {
                // 0 to not run a session, 1 to only complete previous session, 2 to run new session
                case 0:
                    startButton.Visible = false;
                    sessionCheckPanel.Visible = true;

                    break;
                case 1:
                    tableLayoutPanel1.Visible = true;
                    startButton.Visible = false;       
                    deck = new Deck();
                    learningDeck = deck.learningDeck;
                    newDeck = deck.newDeck;
                    deckChoice = deck.choice;

                    showFront();

                    break;
                case 2:
                    DateTime today = DateTime.Now;
                    int maxSessionCount = getMaxSession();

                    decrementSintvl();

                    // updates sess_count in run_info. Future calls of getMaxSession() should return this updated value
                    string sessionCommand = $"INSERT INTO run_info (sess_begin, sess_count) VALUES ('{today.ToString("MM:dd:yyyy:HH:mm:ss")}', {maxSessionCount + 1})";

                    using (SQLiteCommand cmd = new SQLiteCommand(sessionCommand, programDBConnection))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    tableLayoutPanel1.Visible = true;
                    startButton.Visible = false;
                    deck = new Deck();
                    learningDeck = deck.learningDeck;
                    newDeck = deck.newDeck;                    
                    deckChoice = deck.choice;
                    showFront();

                    break;
                default:
                    break;
            }

            viewingTime = 0;
            viewingTimerLabel.Text = "0:00";
            viewingTimer.Start();
        }

        /// <summary>
        /// Show the front of the card or prepare database for closing if both decks are empty
        /// </summary>
        private void showFront()
        {
            Image front;

            string exeFile = (new System.Uri(Assembly.GetEntryAssembly().Location)).AbsolutePath;
            string exeDir = Path.GetDirectoryName(exeFile);

            switch (deckChoice)
            {
                case 0:
                    // randomly index into chosen deck
                    popularity = learningDeck[rnd.Next(learningDeck.Count)];
                    front = Image.FromFile(Path.Combine(exeDir, $@"card_fronts\image{popularity - 1}.jpg"));

                    cardFront.Image = front;
                    break;
                case 1:
                    popularity = newDeck[rnd.Next(newDeck.Count)];
                    front = Image.FromFile(Path.Combine(exeDir, $@"card_fronts\image{popularity - 1}.jpg"));

                    cardFront.Image = front;
                    break;
                case 2:
                    // if both decks are empty, show finished screen
                    tableLayoutPanel1.Visible = false;
                    viewingTimerLabel.Visible = false;
                    doneLabel.Visible = true;

                    // closing up the program
                    int maxSessionCount = getMaxSession();
                    
                    string sessCountCommand = $"UPDATE statistics SET sess_count={maxSessionCount} WHERE sess_count IS NULL";

                    using (SQLiteCommand cmd = new SQLiteCommand(sessCountCommand, cardConnection))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // store popularity values corresponding to reviews from current session
                    string getPopularity = $"SELECT popularity FROM statistics WHERE sess_count={maxSessionCount}";
                    List<int> temp = new List<int>();
                    int tempPopularity;

                    using (SQLiteCommand cmd = new SQLiteCommand(getPopularity, cardConnection))
                    {
                        using (SQLiteDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                // store popularity values in a temporary list
                                tempPopularity = rdr.GetInt32(rdr.GetOrdinal("popularity"));
                                temp.Add(popularity);
                            }
                        }
                    }

                    // remove duplicate popularity values
                    temp = temp.Distinct().ToList();
                    string cSessCountCommand;

                    // update csess_count in cards for each popularity value present in current session
                    foreach (int t in temp)
                    {
                        cSessCountCommand = $"UPDATE cards SET csess_count={maxSessionCount} WHERE popularity={t}";

                        using (SQLiteCommand cmd = new SQLiteCommand(cSessCountCommand, cardConnection))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    break;
                default:
                    // placeholder
                    popularity = 0;
                    break;
            }
        }

        /// <summary>
        /// Show the back of the card. 
        /// </summary>
        private void showBack()
        {
            string commandBack = $"SELECT back FROM cards WHERE popularity={popularity}";
                        
            using (SQLiteCommand cmd = new SQLiteCommand(commandBack, cardConnection))
            {
                string back = cmd.ExecuteScalar().ToString();

                cardBack.Text = back;
            }
        }        

        /// <summary>
        /// Adds review data to statistics and cards table after each review
        /// </summary>
        /// <param name="rightWrong">0 if user selects wrong button, 1 if user selects right button</param>
        private void createReview(int rightWrong)
        {            
            int sintvl;
            int criar;
                        
            if (rightWrong == 0)
            {
                // if user didn't remember card, always reset its scheduled interval
                sintvl = 0;
                criar = 0;
            }
            else
            {                    
                string getNrevCommand = $"SELECT criar FROM cards WHERE popularity={popularity}";

                using (SQLiteCommand cmd = new SQLiteCommand(getNrevCommand, cardConnection))
                {
                    object result = cmd.ExecuteScalar();
                    result = (result == DBNull.Value) ? null : result;
                    criar = Convert.ToInt32(result);
                }                    
                                
                switch (criar)
                {
                    case 1:
                        sintvl = 0;
                        break;
                    case 2:
                        // completion of first session for new card or after a card has been reset
                        sintvl = 1;
                        break;
                    case 3:
                        sintvl = 2;
                        break;
                    case 4:
                        sintvl = 3;
                        break;
                    default:
                        // sets sintvl=0 for criar>=5. Changed when NN runs
                        sintvl = 0;
                        break;
                }
            }

            int maxSessionCount = getMaxSession();            

            int? iintvl;
            long cnrev;
            string ctime;

            string today = DateTime.Now.ToString("MM:dd:yyyy:HH:mm:ss");                

            // get number of times this card has been reviewed
            string nrevCommand = $"SELECT cnrev FROM cards WHERE popularity={popularity}";

            using (SQLiteCommand cmd = new SQLiteCommand(nrevCommand, cardConnection))
            {
                cnrev = (long)cmd.ExecuteScalar();
            }

            // if newDeck contains popularity, the card has never been seen before
            if (newDeck.Contains(popularity))
            {
                // update card data. ctime and age are the same when a card is first seen
                // don't need to set ciintvl b/c card will be seen at least once more before session ends
                string cardsCommand = $"UPDATE cards SET age='{today}', csintvl={sintvl}, cnrev={cnrev + 1}, ctime='{today}', csess_count={maxSessionCount}, cvtime={viewingTime}, crdr={rightWrong} WHERE popularity={popularity}";

                using (SQLiteCommand cmd = new SQLiteCommand(cardsCommand, cardConnection))
                {
                    cmd.ExecuteNonQuery();
                }

                // record review statistics. first time seeing the card, so iintvl remains NULL and nrev = 0
                string reviewCommand = $"INSERT INTO statistics (rdr, vtime, sintvl, time, nrev, popularity, sess_count, riar) VALUES ({rightWrong}, {viewingTime}, {sintvl}, '{today}', {cnrev + 1}, {popularity}, {maxSessionCount}, {criar})";

                using (SQLiteCommand cmd = new SQLiteCommand(reviewCommand, cardConnection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            else
            {
                string ctimeCommand = $"SELECT ctime FROM cards WHERE popularity={popularity}";
                using (SQLiteCommand cmd = new SQLiteCommand(ctimeCommand, cardConnection))
                {
                    ctime = cmd.ExecuteScalar().ToString();
                }

                // subtract ctime from time of review to get the interval in seconds between current review and the most recent of the past reviews
                DateTime now = DateTime.ParseExact(today, "MM:dd:yyyy:HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime then = DateTime.ParseExact(ctime, "MM:dd:yyyy:HH:mm:ss", CultureInfo.InvariantCulture);
                iintvl = (int)Math.Round(now.Subtract(then).TotalSeconds);

                // update card data
                string cardsCommand = $"UPDATE cards SET ctime='{today}', csintvl={sintvl}, cnrev={cnrev + 1}, ciintvl={iintvl}, csess_count={maxSessionCount}, cvtime={viewingTime}, crdr={rightWrong} WHERE popularity={popularity}";
                using (SQLiteCommand cmd = new SQLiteCommand(cardsCommand, cardConnection))
                {
                    cmd.ExecuteNonQuery();
                }

                // record review statistics
                string reviewCommand = $"INSERT INTO statistics (iintvl, rdr, vtime, sintvl, time, nrev, popularity, sess_count, riar) VALUES ({iintvl}, {rightWrong}, {viewingTime}, {sintvl}, '{today}', {cnrev + 1}, {popularity}, {maxSessionCount}, {criar})";
                using (SQLiteCommand cmd = new SQLiteCommand(reviewCommand, cardConnection))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            // set card_sess_count
            int ccardSessCount;
            string getCcardSessCount = $"SELECT ccard_sess_count FROM cards WHERE popularity={popularity}";
            using (SQLiteCommand cmd = new SQLiteCommand(getCcardSessCount, cardConnection))
            {
                object result = cmd.ExecuteScalar();
                result = (result == DBNull.Value) ? null : result;
                ccardSessCount = Convert.ToInt32(result);
            }

            string cardSessCount = $"UPDATE statistics SET card_sess_count = {ccardSessCount} WHERE popularity={popularity} AND nrev={cnrev + 1}";
            using (SQLiteCommand cmd = new SQLiteCommand(cardSessCount, cardConnection))
            {
                cmd.ExecuteNonQuery();
            }
        }        

        /// <summary>
        /// Determines what kind of session to perform today
        /// </summary>
        /// <returns>0 to not run a session, 1 to only complete previous session, 2 to run new session</returns>
        private int sessionCheck()
        {            
            DateTime today = DateTime.Today;
            int maxSessionCount = getMaxSession();
            DateTime mostRecentSession;
            string sessBegin;

            // if there is no max session, then program has never been run before, so run a full session
            if (maxSessionCount == 0)
            {
                return 2;
            }

            string mostRecentCommand = $"SELECT sess_begin FROM run_info WHERE sess_count={maxSessionCount}";

            using (SQLiteCommand cmd = new SQLiteCommand(mostRecentCommand, programDBConnection))
            {
                sessBegin = cmd.ExecuteScalar().ToString();
                mostRecentSession = DateTime.ParseExact(sessBegin, "MM:dd:yyyy:HH:mm:ss", CultureInfo.InvariantCulture).Date;
            }

            int dateDiff = DateTime.Compare(today, mostRecentSession);

            // if datediff is 0, need to check if there are any incomplete reviews
            if (dateDiff == 0)
            {
                int fin_sessNumber;
                                        
                string checkFinSess = "SELECT COUNT(fin_sess) FROM cards WHERE fin_sess = 0";

                using (SQLiteCommand cmd = new SQLiteCommand(checkFinSess, cardConnection))
                {
                    object result = cmd.ExecuteScalar();
                    result = (result == DBNull.Value) ? null : result;
                    fin_sessNumber = Convert.ToInt32(result);
                }  

                // if fin_sessNumber > 0, then there are incomplete reviews
                if (fin_sessNumber > 0)
                {
                    return 1;
                } else
                {
                    return 0;
                }
            }
            else 
            {
                // if dateDiff != 0, then program has not been run today, so procede with a full session
                return 2;
            }
        }

        /// <summary>
        /// Decrements cards.csintvl and statistics.sintvl by one per day to a minimum of for cards that have completed their session 0
        /// </summary>
        private void decrementSintvl()
        {
            DateTime today = DateTime.Today;
            int maxSessionCount = getMaxSession();
            DateTime mostRecentSession;
            string sessBegin;
            int dateDiff;

            // see when most recent session occurred 
                            
            // if there are no entries in the table, then exit the method
            if (maxSessionCount == 0)
            {
                return;
            }

            string mostRecentCommand = $"SELECT sess_begin FROM run_info WHERE sess_count={maxSessionCount}";

            using (SQLiteCommand cmd = new SQLiteCommand(mostRecentCommand, programDBConnection))
            {
                sessBegin = cmd.ExecuteScalar().ToString();
                mostRecentSession = DateTime.ParseExact(sessBegin, "MM:dd:yyyy:HH:mm:ss", CultureInfo.InvariantCulture).Date;
            }
                
            dateDiff = DateTime.Compare(today, mostRecentSession);
                        
            string decrementCsintvlCommand = $"UPDATE cards SET csintvl = CASE WHEN csintvl - {dateDiff} < 0 THEN 0 ELSE csintvl - {dateDiff} END WHERE csintvl IS NOT NULL AND csintvl > 0 AND fin_sess==1";

            using (SQLiteCommand cmd = new SQLiteCommand(decrementCsintvlCommand, cardConnection))
            {
                cmd.ExecuteNonQuery();
            }
        }        

        /// <summary>
        /// Calls python script that runs neural network to predict algorithm's ease factor
        /// </summary>
        /// <returns></returns>
        private string runPython()
        {
            // https://www.codeproject.com/Articles/5165602/Using-Python-Scripts-from-a-Csharp-Client-Includin
            // https://stackoverflow.com/questions/11779143/how-do-i-run-a-python-script-from-c

            // starting a new process
            ProcessStartInfo start = new ProcessStartInfo();
            // the application to start
            start.FileName = @"...python.exe"; // complete path to python 
            // the command-line arguments to use when starting the above application
            start.Arguments = @"...main.py"; // complete path to python file
            // do not use operating system shell to start process
            start.UseShellExecute = false;
            // redirect textual output of application to standard output stream
            start.RedirectStandardOutput = true;
            // start the process "start" from above
            using (Process process = Process.Start(start))
            {
                // read characters from byte stream output of application 
                using (StreamReader reader = process.StandardOutput)
                {
                    // read from current position to end of stream
                    return reader.ReadToEnd();

                }
            }
        }

        /// <summary>
        /// Gets the most recent session number from program.db
        /// </summary>
        /// <returns>The most recent session number</returns>
        private int getMaxSession()
        {
            int maxSessionCount;

            string maxSessionCommand = "SELECT max(sess_count) FROM run_info";

            using (SQLiteCommand cmd = new SQLiteCommand(maxSessionCommand, programDBConnection))
            {
                object result = cmd.ExecuteScalar();
                result = (result == DBNull.Value) ? null : result;
                maxSessionCount = Convert.ToInt32(result);
            }

            return maxSessionCount;
        }       

        /// <summary>
        /// When program is exited, save which cards have not completed their review so program can incorporate them into next session
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string finSessCommand;
            string deckCommand;
            int criar;
            
            foreach (int card in newDeck)
            {
                finSessCommand = $"UPDATE cards SET fin_sess = 0 WHERE popularity={card}";

                using (SQLiteCommand cmd = new SQLiteCommand(finSessCommand, cardConnection))
                {
                    cmd.ExecuteNonQuery();
                }

                // 1 for newDeck
                deckCommand = $"UPDATE cards SET deck = 1 WHERE popularity={card}";

                using (SQLiteCommand cmd = new SQLiteCommand(deckCommand, cardConnection))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            foreach (int card in learningDeck)
            {
                finSessCommand = $"UPDATE cards SET fin_sess = 0 WHERE popularity={card}";

                using (SQLiteCommand cmd = new SQLiteCommand(finSessCommand, cardConnection))
                {
                    cmd.ExecuteNonQuery();
                }

                // 0 for learningDeck
                deckCommand = $"UPDATE cards SET deck = 0 WHERE popularity={card}";

                using (SQLiteCommand cmd = new SQLiteCommand(deckCommand, cardConnection))
                {
                    cmd.ExecuteNonQuery();
                }
            }                

            // criar >= 5 b/c need to account for two riar's when card is first seen or 
            string getNrevCommand = $"SELECT COUNT(criar) FROM cards WHERE criar>=5";

            using (SQLiteCommand cmd = new SQLiteCommand(getNrevCommand, cardConnection))
            {
                object result = cmd.ExecuteScalar();
                result = (result == DBNull.Value) ? null : result;
                criar = Convert.ToInt32(result);
            }

            // wasn't overwriting DB file before. Moved it out of connection block to see if it will now
            if (criar > 0)
            {
                runPython();
            }

            Database.CloseConnections();
        }
    }
       
}
