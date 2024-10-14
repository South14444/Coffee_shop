using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        SqlConnection conn = null;
        DataSet ds;
        SqlDataAdapter adapter;
        public Form1()
        {
            InitializeComponent();
            conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.
                ConnectionStrings["MssqlCoonWarehouse"].ConnectionString;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.DataSource = ds.Tables["Coffee"];
        }

        private void button13_Click(object sender, EventArgs e)
        {
            string str = "select * from Coffee as C join Manufacturer_side as M On c.Manufacturer_sideId=M.ID join Type_of_coffee as T on C.Type_of_coffeeId=T.ID;";
            try
            {
                adapter = new SqlDataAdapter(str, conn);


                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                ds = new DataSet();

                adapter.TableMappings.Add("Table", "Coffee");
                adapter.TableMappings.Add("Table1", "Manufacturer_side");
                adapter.TableMappings.Add("Table2", "Type_of_coffee");
                adapter.Fill(ds);
                dataGridView1.DataSource = null;
                //dataGridView1.DataSource = ds.Tables["Coffee"];
                textBox1.Text = "Подкучились";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataViewManager dvm = new DataViewManager(ds);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            DataViewSetting setting = dvm.DataViewSettings["Coffee"];
            setting.ApplyDefaultSort =  true;
            setting.Sort = "Type_of_coffeeId";
            dataGridView1.DataSource = ds.Tables["Coffee"];
        }


        private void ExecuteQueryAndDisplay(SqlCommand command)
        {
            try
            {
                adapter = new SqlDataAdapter(command);
                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void ExecuteQueryAndDisplay(string query)
        {
            try
            {
                adapter = new SqlDataAdapter(query, conn);
                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string query = "SELECT C.Name, T.Name AS Type_of_coffee, M.Name AS Manufacturer, C.Description, C.Number_of_grams, C.Cost_price " +
                   "FROM Coffee AS C " +
                   "JOIN Manufacturer_side AS M ON C.Manufacturer_sideId = M.ID " +
                   "JOIN Type_of_coffee AS T ON C.Type_of_coffeeId = T.ID " +
                   "WHERE C.Description LIKE N'%вишни%';";
            ExecuteQueryAndDisplay(query);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            decimal minCost = Convert.ToDecimal(textBox2.Text);  // Поле ввода для минимальной себестоимости
            decimal maxCost = Convert.ToDecimal(textBox3.Text);  // Поле ввода для максимальной себестоимости

            string query = $"SELECT C.Name, T.Name AS Type_of_coffee, M.Name AS Manufacturer, C.Description, C.Number_of_grams, C.Cost_price " +
                           $"FROM Coffee AS C " +
                           $"JOIN Manufacturer_side AS M ON C.Manufacturer_sideId = M.ID " +
                           $"JOIN Type_of_coffee AS T ON C.Type_of_coffeeId = T.ID " +
                           $"WHERE C.Cost_price BETWEEN @minCost AND @maxCost;";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@minCost", minCost);
            command.Parameters.AddWithValue("@maxCost", maxCost);

            ExecuteQueryAndDisplay(command);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int minGrams = Convert.ToInt32(textBox2.Text);  // Поле ввода для минимального количества грамм
            int maxGrams = Convert.ToInt32(textBox3.Text);  // Поле ввода для максимального количества грамм

            string query = $"SELECT C.Name, T.Name AS Type_of_coffee, M.Name AS Manufacturer, C.Description, C.Number_of_grams, C.Cost_price " +
                           $"FROM Coffee AS C " +
                           $"JOIN Manufacturer_side AS M ON C.Manufacturer_sideId = M.ID " +
                           $"JOIN Type_of_coffee AS T ON C.Type_of_coffeeId = T.ID " +
                           $"WHERE C.Number_of_grams BETWEEN @minGrams AND @maxGrams;";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@minGrams", minGrams);
            command.Parameters.AddWithValue("@maxGrams", maxGrams);

            ExecuteQueryAndDisplay(command);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string country1 = textBox2.Text;  
            string country2 = textBox3.Text;  

            string query = $"SELECT C.Name, T.Name AS Type_of_coffee, M.Name AS Manufacturer, C.Description, C.Number_of_grams, C.Cost_price " +
                           $"FROM Coffee AS C " +
                           $"JOIN Manufacturer_side AS M ON C.Manufacturer_sideId = M.ID " +
                           $"JOIN Type_of_coffee AS T ON C.Type_of_coffeeId = T.ID " +
                           $"WHERE M.Name IN (@country1, @country2);";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@country1", country1);
            command.Parameters.AddWithValue("@country2", country2);

            ExecuteQueryAndDisplay(command);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string query = "SELECT M.Name AS Country, COUNT(C.ID) AS CoffeeVarieties " +
                   "FROM Coffee AS C " +
                   "JOIN Manufacturer_side AS M ON C.Manufacturer_sideId = M.ID " +
                   "GROUP BY M.Name;";
            ExecuteQueryAndDisplay(query);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string query = "SELECT M.Name AS Country, AVG(C.Number_of_grams) AS AvgGrams " +
                  "FROM Coffee AS C " +
                  "JOIN Manufacturer_side AS M ON C.Manufacturer_sideId = M.ID " +
                  "GROUP BY M.Name;";
            ExecuteQueryAndDisplay(query);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string country = textBox2.Text; 

            string query = "SELECT TOP 3 C.Name, C.Cost_price " +
                           "FROM Coffee AS C " +
                           "JOIN Manufacturer_side AS M ON C.Manufacturer_sideId = M.ID " +
                           "WHERE M.Name = @country " +
                           "ORDER BY C.Cost_price ASC;";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@country", country);

            ExecuteQueryAndDisplay(command);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string country = textBox2.Text; // Поле для ввода страны

            string query = "SELECT TOP 3 C.Name, C.Cost_price " +
                           "FROM Coffee AS C " +
                           "JOIN Manufacturer_side AS M ON C.Manufacturer_sideId = M.ID " +
                           "WHERE M.Name = @country " +
                           "ORDER BY C.Cost_price DESC;";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@country", country);

            ExecuteQueryAndDisplay(command);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string query = "SELECT TOP 3 C.Name, M.Name AS Country, C.Cost_price " +
                   "FROM Coffee AS C " +
                   "JOIN Manufacturer_side AS M ON C.Manufacturer_sideId = M.ID " +
                   "ORDER BY C.Cost_price ASC;";
            ExecuteQueryAndDisplay(query);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string query = "SELECT TOP 3 C.Name, M.Name AS Country, C.Cost_price " +
                   "FROM Coffee AS C " +
                   "JOIN Manufacturer_side AS M ON C.Manufacturer_sideId = M.ID " +
                   "ORDER BY C.Cost_price DESC;";
            ExecuteQueryAndDisplay(query);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            string query = "SELECT TOP 3 M.Name AS Country, COUNT(C.ID) AS CoffeeVarieties " +
                   "FROM Coffee AS C " +
                   "JOIN Manufacturer_side AS M ON C.Manufacturer_sideId = M.ID " +
                   "GROUP BY M.Name " +
                   "ORDER BY CoffeeVarieties DESC;";
            ExecuteQueryAndDisplay(query);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            string query = "SELECT TOP 3 M.Name AS Country, SUM(C.Number_of_grams) AS TotalGrams " +
                   "FROM Coffee AS C " +
                   "JOIN Manufacturer_side AS M ON C.Manufacturer_sideId = M.ID " +
                   "GROUP BY M.Name " +
                   "ORDER BY TotalGrams DESC;";
            ExecuteQueryAndDisplay(query);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string query = "SELECT TOP 3 C.Name, C.Number_of_grams " +
                   "FROM Coffee AS C " +
                   "JOIN Type_of_coffee AS T ON C.Type_of_coffeeId = T.ID " +
                   "WHERE T.Name = N'Арабика' " +
                   "ORDER BY C.Number_of_grams DESC;";
            ExecuteQueryAndDisplay(query);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            string query = "SELECT TOP 3 C.Name, C.Number_of_grams " +
                   "FROM Coffee AS C " +
                   "JOIN Type_of_coffee AS T ON C.Type_of_coffeeId = T.ID " +
                   "WHERE T.Name = N'Робуста' " +
                   "ORDER BY C.Number_of_grams DESC;";
            ExecuteQueryAndDisplay(query);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            string query = "SELECT TOP 3 C.Name, C.Number_of_grams " +
                   "FROM Coffee AS C " +
                   "JOIN Type_of_coffee AS T ON C.Type_of_coffeeId = T.ID " +
                   "WHERE T.Name = N'Купаж/Бленд' " +
                   "ORDER BY C.Number_of_grams DESC;";
            ExecuteQueryAndDisplay(query);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string query = "WITH RankedCoffee AS ( " +
                   "    SELECT C.Name, T.Name AS Type_of_coffee, C.Number_of_grams, " +
                   "           ROW_NUMBER() OVER (PARTITION BY T.Name ORDER BY C.Number_of_grams DESC) AS Rank " +
                   "    FROM Coffee AS C " +
                   "    JOIN Type_of_coffee AS T ON C.Type_of_coffeeId = T.ID " +
                   ") " +
                   "SELECT Name, Type_of_coffee, Number_of_grams " +
                   "FROM RankedCoffee " +
                   "WHERE Rank <= 3;";
            ExecuteQueryAndDisplay(query);
        }
    }
}
