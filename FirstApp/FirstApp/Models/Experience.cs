using System;
using System.Collections.Generic;
using System.Text;

using SQLite;

namespace FirstApp.Models
{
    class Experience
    {
        // คลาสสำหรับโมเคล

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string VenueName { get; set; }

        public string VenueCategory { get; set; }

        public float VenueLat { get; set; }

        public float VenueLng { get; set; }

        public override string ToString()
        {
            return Title;
        }

        public static List<Experience> GetExperiences()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabasePath))
            {
                // get data from table
                conn.CreateTable<Experience>();
                return conn.Table<Experience>().ToList();
            }
        }
    }
}
