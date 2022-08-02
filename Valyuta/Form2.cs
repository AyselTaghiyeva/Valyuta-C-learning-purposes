using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;


namespace Valyuta
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {



            string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:/Valyuta_mezenneleri.mdb;";
            string commandText = "INSERT INTO Table1 (Valyuta, Kod, Kurs) VALUES (?, ?, ?)";

            using (OleDbConnection connection = new OleDbConnection(connectString))
            {
                OleDbCommand command = new OleDbCommand(commandText, connection);
                command.Parameters.AddWithValue("@Valyuta", textBox1.Text);
                command.Parameters.AddWithValue("@Kod", textBox2.Text);
                command.Parameters.AddWithValue("@Kurs", textBox3.Text);
                try
                {
                    command.Connection.Open();
                    int response = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: {0}" + ex.Message);
                }


            }



        }
    }
}
