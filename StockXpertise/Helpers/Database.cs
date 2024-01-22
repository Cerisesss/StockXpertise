using System;
using MySql.Data.MySqlClient;

namespace StockXpertise.Helpers
{
    public class Database
    {
        // This class is used to:
        // - Do some basic database operations (mainly SQL queries)
        // - other stuff related to the database in general

        MySqlConnection? instance = null;
    }
}
