using Npgsql;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.IO;

namespace ContrackAPI
{
    public enum DatabaseCollection
    {
        Byntra = 0,
        Contrack = 1,
    }
   public class SqlDB : IDisposable
    {
        private static IConfiguration _configuration;
        public NpgsqlConnection Conn { get; private set; }
        static SqlDB()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            _configuration = builder.Build();
        }
       public SqlDB(DatabaseCollection database = DatabaseCollection.Byntra)
        {
            var connectionString = GetDbInfo(database);

            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("Connection string is null or empty.");

            Conn = new NpgsqlConnection(connectionString);

            if (Conn.State != ConnectionState.Open)
                Conn.Open();
        }
       private string GetDbInfo(DatabaseCollection db)
        {
            return db switch
            {
                DatabaseCollection.Byntra => _configuration.GetConnectionString("ByntraDB"),
                DatabaseCollection.Contrack => _configuration.GetConnectionString("ContrackDB"),
                _ => _configuration.GetConnectionString("ByntraDB")
            };
        }
       public DataTable GetDataTable(string sql)
        {
            DataTable tbl = new DataTable();

            using (var cmd = new NpgsqlCommand(sql, Conn))
            using (var da = new NpgsqlDataAdapter(cmd))
            {
                da.Fill(tbl);
            }

            return tbl;
        }
        public int Execute(string sql)
        {
            using (var cmd = new NpgsqlCommand(sql, Conn))
            {
                return cmd.ExecuteNonQuery();
            }
        }
        public object GetValue(string sql)
        {
            using (var cmd = new NpgsqlCommand(sql, Conn))
            {
                return cmd.ExecuteScalar();
            }
        }
        public void Dispose()
        {
            if (Conn != null && Conn.State == ConnectionState.Open)
                Conn.Close();
        }
    }
}