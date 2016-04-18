using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace AdoDotNetClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ExecuteDataReader();
            //FillDataSetOneQuery();
            //FillDataSetTwoQueries();
            //FillDataSetTwoCommands();
        }

        static void ExecuteDataReader()
        {
            var connectionString =
                ConfigurationManager.ConnectionStrings["rhdb"].ConnectionString;

            var connection =
                new SqlConnection(connectionString);

            connection.Open();

            var command = new SqlCommand("select * from ent_admin", connection);

            var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                var output = "Id {0}: Full Name: {1} {2}";

                while (reader.Read())
                {
                    Debug.WriteLine(output, reader["admin_id"], reader["name_first"], reader["name_last"]);
                }
            }
            else 
            {
                Debug.WriteLine("There are no records!");
            }

            reader.Close();

            connection.Close();
        }

        static void FillDataSetOneQuery()
        {
            var connectionString = 
                ConfigurationManager.ConnectionStrings["rhdb"].ConnectionString;

            var connection = 
                new SqlConnection(connectionString);

            connection.Open();

            var adapter = 
                new SqlDataAdapter("select * from ent_admin where ACTIVE_YN = 'Y'", connection);

            var users = new DataSet("Users");

            adapter.Fill(users, "Active");

            Debug.WriteLine("{0} active users", users.Tables["Active"].Rows.Count);

            connection.Close();
        }

        static void FillDataSetTwoQueries()
        {
            var connectionString =
                ConfigurationManager.ConnectionStrings["rhdb"].ConnectionString;

            var connection =
                new SqlConnection(connectionString);

            connection.Open();

            var adapter =
                new SqlDataAdapter("select * from ent_admin where ACTIVE_YN = 'Y'; select * from ent_admin where ACTIVE_YN = 'N'", connection);

            adapter.TableMappings.Add("Table", "Active");
            adapter.TableMappings.Add("Table1", "Inactive");

            var users = new DataSet("Users");

            adapter.Fill(users);

            Debug.WriteLine("{0} active users", users.Tables["Active"].Rows.Count);

            Debug.WriteLine("{0} inactive users", users.Tables["Inactive"].Rows.Count);

            connection.Close();
        }

        static void FillDataSetTwoCommands()
        {
            var connectionString =
                ConfigurationManager.ConnectionStrings["rhdb"].ConnectionString;

            var connection =
                new SqlConnection(connectionString);

            connection.Open();

            var activeCommand = new SqlCommand("select * from ent_admin where ACTIVE_YN = 'Y'", connection);

            var inactiveCommand = new SqlCommand("select * from ent_admin where ACTIVE_YN = 'N'", connection);
                        
            var users = new DataSet("Users");

            users.Tables.Add("Active");
            users.Tables.Add("Inactive");

            var adapter = new SqlDataAdapter();

            adapter.SelectCommand = activeCommand;
            adapter.Fill(users.Tables["Active"]);

            adapter.SelectCommand = inactiveCommand;
            adapter.Fill(users.Tables["Inactive"]);

            Debug.WriteLine("{0} active users", users.Tables["Active"].Rows.Count);

            Debug.WriteLine("{0} inactive users", users.Tables["Inactive"].Rows.Count);

            connection.Close();
        }
    }
}
