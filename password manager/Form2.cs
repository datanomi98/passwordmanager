using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;

namespace password_manager
{
    public partial class Form2 : Form
    {
        public string password;
        Class1 functions;
        public SQLiteConnection m_dbConnection;
 
        public Form2(Class1 kek)
        {
            InitializeComponent();
            functions = kek;
            this.FormClosing += Form2_FormClosing;
        }
       

        private void Form2_Load(object sender, EventArgs e)
        {
            dataGridView1.Columns.Add("id", "id");
            dataGridView1.Columns.Add("Sitename", "Sitename");
            dataGridView1.Columns.Add("Password", "Password");
            GetDataFromDB();
            

        }
        private void Form2_FormClosing(Object sender, FormClosingEventArgs e)
        {
            //In case windows is trying to shut down, don't hold the process up
           if (e.CloseReason == CloseReason.WindowsShutDown) return;

            
                if (this.DialogResult == DialogResult.Cancel)
                {
                    
                    m_dbConnection = new SQLiteConnection("Data Source=manager.sqlite;Version=3;");
                    m_dbConnection.Open();
                    m_dbConnection.ChangePassword(functions.pass);
                    m_dbConnection.Close();
                    Form1 kek = new Form1();
                    kek.Close();
                    kek.Dispose();
                }
            
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string sitename = sitename2.Text;
                string pass = pass2.Text;
                if (sitename != "" && pass != "")
                {
                    m_dbConnection = new SQLiteConnection("Data Source=manager.sqlite;Version=3;");
                    m_dbConnection.Open();
                    string UserTable = "insert into pass (sitename, password) values ($sitename,$pass)";
                    SQLiteCommand command = new SQLiteCommand(UserTable, m_dbConnection);
                    command.Parameters.AddWithValue("$sitename", sitename);
                    command.Parameters.AddWithValue("$pass", pass);
                    command.ExecuteNonQuery();
                    command.Dispose();
                    m_dbConnection.Close();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    m_dbConnection.Dispose();
                    SQLiteConnection.ClearAllPools();
                    sitename2.Text = "";
                    pass2.Text = "";
                    GetDataFromDB();
                }
                else
                {
                    MessageBox.Show("password or sitename is empty");
                }

            }
            catch (SQLiteException)
            {
                MessageBox.Show("cannot insert data to database");

            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            GetDataFromDB();
        }
        public void GetDataFromDB()
        {
            try
            {

                m_dbConnection = new SQLiteConnection("Data Source=manager.sqlite;Version=3;");
                m_dbConnection.Open();
                //sitename.Text = functions.masterpass;
                string sql = "select * from pass";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (reader.Read())
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridView1);  // this line was missing
                    row.Cells[0].Value = reader.GetValue(reader.GetOrdinal("id"));
                    row.Cells[1].Value = reader.GetValue(reader.GetOrdinal("sitename"));
                    row.Cells[2].Value = reader.GetValue(reader.GetOrdinal("password"));
                    dataGridView1.Rows.Add(row);
                }
                reader.Close();
                command.Dispose();
                m_dbConnection.Close();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                m_dbConnection.Dispose();
                SQLiteConnection.ClearAllPools();
            }
            catch (SQLiteException)
            {
                MessageBox.Show("cannot get data from database");

            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            int rowindex = dataGridView1.CurrentCell.RowIndex;
            int columnindex = dataGridView1.CurrentCell.ColumnIndex;
            string value = dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString();
            int delete = Int32.Parse(value);
            try
            {

                m_dbConnection = new SQLiteConnection("Data Source=manager.sqlite;Version=3;");
                m_dbConnection.Open();
                //sitename.Text = functions.masterpass;
                string sql = "DELETE FROM pass WHERE id = '" + delete + "'";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteReader();
                command.Dispose();
                m_dbConnection.Close();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                m_dbConnection.Dispose();
                SQLiteConnection.ClearAllPools();
                GetDataFromDB();
            }
            catch (SQLiteException)
            {
                MessageBox.Show("cannot get data from database");

            }
        }
    }
    }
   

