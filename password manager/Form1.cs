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

namespace password_manager
{
    public partial class Form1 : Form
    {
        Class1 function = new Class1();
        private SQLiteConnection m_dbConnection;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists("manager.sqlite"))
            {
                login.Visible = true;
                setmaster.Visible = false;
            }
            else
            {
                setmaster.Visible = true;
                login.Visible = false;
            }
          
        }

        private void login_Click(object sender, EventArgs e)
        {
            try
            {
                m_dbConnection = new SQLiteConnection("Data Source=manager.sqlite;Version=3;Password=" + textBox1.Text + ";");
                m_dbConnection.Open();
                m_dbConnection.ChangePassword("");
                m_dbConnection.Close();
                function.pass = textBox1.Text;
                m_dbConnection.Close();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                m_dbConnection.Dispose();
                SQLiteConnection.ClearAllPools();
                Form2 f2 = new Form2(function); // Instantiate a Form2 object.
                f2.ShowDialog(); // Show Form2 and
                this.Dispose();
                this.Close();
            }
            catch(SQLiteException)
            {
                MessageBox.Show("password is incorrect");
                
            }
        }

        private void setmaster_Click(object sender, EventArgs e)
        {
            try
            {
                function.pass = textBox1.Text;
                //MessageBox.Show(textBox1.Text);
                SQLiteConnection.CreateFile("manager.sqlite");
                m_dbConnection =
               new SQLiteConnection("Data Source=manager.sqlite;Version=3;");
                m_dbConnection.Open();
                string sql = "CREATE TABLE pass (id integer primary key,sitename VARCHAR(255), password VARCHAR(255))";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
                command.Dispose();
                m_dbConnection.Close();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                m_dbConnection.Dispose();
                SQLiteConnection.ClearAllPools();
                Form2 f2 = new Form2(function); // Instantiate a Form2 object.
                f2.ShowDialog(); // Show Form2 and
                this.Dispose();
                this.Close();

            }
            catch (SQLiteException)
            {
                MessageBox.Show("something went wrong");

            }
        }
    }
}