using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
namespace password_manager
{
    public class Class1
    {
        public byte[] dbFile;
        public string masterpass { get; set; }
        public string pass;
        public SQLiteConnection m_dbConnection;

        public Class1() //oletuskonstruktori
        {
            masterpass = "";
        }
        public string master(string masterpass)
        {
            return masterpass;
        }
        
       

        }
    }
