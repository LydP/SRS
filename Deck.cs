using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;

namespace SRS
{
    class Deck
    {   
        public List<int> learningDeck { get; private set; }
        public List<int> newDeck { get; private set; }
        public int choice { get; set; }

        private SQLiteConnection connection { get; set; }
        private Random rnd { get; set; }

        public Deck()
        {
            learningDeck = new List<int>();
            newDeck = new List<int>();
            connection = Database.cardConnection;
            rnd = new Random();

            CreateDeck();
            ChooseDeck();
        }

        /// <summary>
        /// Initializes cards in learningDeck and newDeck for a session
        /// </summary>
        private void CreateDeck()
        {            
            long minPopularity;
            int popularity;            

            // min() is an aggregate function, which doesn't work if it immediately follows WHERE
            // NB: age = NULL always evaluates to false. Must use age IS NULL to check for NULL values
            string newDeckCommand1 = $"SELECT min(popularity) FROM cards WHERE age IS NULL";
            // get minimum popularity value from unseen cards
            using (SQLiteCommand cmd = new SQLiteCommand(newDeckCommand1, connection))
            {
                object result = cmd.ExecuteScalar();
                result = (result == DBNull.Value) ? null : result;
                minPopularity = Convert.ToInt32(result);
            }

            // BETWEEN is inclusive
            string newDeckCommand2 = $"SELECT popularity FROM cards WHERE popularity BETWEEN {minPopularity} AND ({minPopularity} + 9)";
            // populate newDeck with unseen cards
            using (SQLiteCommand cmd = new SQLiteCommand(newDeckCommand2, connection))
            {
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        // get the ordinal for popularity column, then convert it to a 32-bit int
                        popularity = rdr.GetInt32(rdr.GetOrdinal("popularity"));
                        newDeck.Add(popularity);
                    }
                    rdr.Close();
                }
            }

            string learningDeckCommand = "SELECT popularity FROM cards WHERE csintvl=0";
            // populate learningDeck with cards scheduled for review
            using (SQLiteCommand cmd = new SQLiteCommand(learningDeckCommand, connection))
            {
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        popularity = rdr.GetInt32(rdr.GetOrdinal("popularity"));
                        learningDeck.Add(popularity);
                    }
                }
            }

            // increment ccard_sess_count in cards table
            SetSQLiteVariables();                

            // add cards from previous session to newDeck and learningDeck here b/c don't want to change ccard_sess_count for these cards
            string getDeck1Command = "SELECT popularity FROM cards WHERE deck=1";
            // populate newDeck with unfinished cards from previous session
            using (SQLiteCommand cmd = new SQLiteCommand(getDeck1Command, connection))
            {
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        popularity = rdr.GetInt32(rdr.GetOrdinal("popularity"));
                        newDeck.Add(popularity);
                    }
                }
            }

            string getDeck2Command = "SELECT popularity FROM cards WHERE deck=0";
            // populate learningDeck with cards from previous session
            using (SQLiteCommand cmd = new SQLiteCommand(getDeck2Command, connection))
            {
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        popularity = rdr.GetInt32(rdr.GetOrdinal("popularity"));
                        learningDeck.Add(popularity);
                    }
                }
            }            
        }

        private void SetSQLiteVariables()
        {
            IEnumerable<int> combinedList = learningDeck.Concat(newDeck);
            string incrementCcardSessCount;

            foreach (var item in combinedList)
            {
                // only want to increment ccard_sess_count for cards that have completed their reviews. 
                /*
                 * I'm differentiating between a run and a session. A run is just starting the program, regardless of what happens
                 * A session is the event of reviewing a set of cards. It's possible for a session to be incomplete and for two separate sessions to run simultaneously
                 * b/c a previously incomplete session can be completed in the current run
                */
                incrementCcardSessCount = $"UPDATE cards SET ccard_sess_count = ccard_sess_count + 1 WHERE popularity={item} AND (fin_sess==1 OR fin_sess IS NULL)";
                using (SQLiteCommand command = new SQLiteCommand(incrementCcardSessCount, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Moves items from newDeck to learningDeck and removes items from learningDeck as necessary during a session and updates fin_sess in cards table
        /// </summary>
        /// <param name="rightWrong">0 for wrong button selection, 1 for right button selection</param>
        public void ManageDeck(int rightWrong, int popularity)
        {
            // 0 for learningDeck, 1 for newDeck
            if (choice == 1) 
            {
                learningDeck.Add(popularity);
                // cards will be removed from newDeck once they've been viewed once
                newDeck.Remove(popularity);
            }
            else if ((choice == 0) && (rightWrong == 1))
            {
                learningDeck.Remove(popularity);

                // set fin_sess to 1 b/c this card's review has been completed.                 
                string finSessCommand = $"UPDATE cards SET fin_sess = 1 WHERE popularity={popularity}";

                using (SQLiteCommand cmd = new SQLiteCommand(finSessCommand, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                string updateDeckCommand = "UPDATE cards SET deck = NULL WHERE fin_sess==1";

                using (SQLiteCommand cmd = new SQLiteCommand(updateDeckCommand, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            ChooseDeck();
        }

        /// <summary>
        /// Sets the deck to be working with for a single review: 0 for learningDeck, 1 for newDeck, 2 when both decks are empty
        /// </summary>
        private void ChooseDeck()
        {   
            // 0 for learningDeck, 1 for newDeck
            if (learningDeck.Any() && newDeck.Any())
            {
                // random integer on [0..2)
                choice = rnd.Next(0, 2);
            }
            else if (learningDeck.Any() && !newDeck.Any())
            {
                choice = 0;
            }
            else if (!learningDeck.Any() && newDeck.Any())
            {
                choice = 1;
            }
            else // when both learningDeck and newDeck are empty
            {
                choice = 2;
            }
        }
    }
}
