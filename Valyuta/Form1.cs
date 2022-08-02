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
    public partial class Form1 : Form
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:/Valyuta_mezenneleri.mdb;";
        private OleDbConnection myConnection;
        ComboBox comboBox2 = new ComboBox(); //for KURS column
        ComboBox comboBox3 = new ComboBox(); //for KOD column
        ComboBox comboBox4 = new ComboBox(); //for ID column
        public string secilmishID ;
        public string secilmishkurs;
        public string secilmishkod;
       
        public bool datagetirildi = false; // ACCESS-den data goturulmeden evvel comboBox1_SelectedValueChanged gicleyir,ona nezaret ucun yaratdiq bu deyiseni

        public Form1()
        {
            InitializeComponent();
        }






        public void datagetir() // Data gətirən funksiya yaradırıq
        {

            //Qoşuntu yaradırıq, Access-dən Combobox 1,2,3,4ə uyğun olaraq - valyuta, kurs,kod,id sütunlarını əlavə edirik.
            myConnection = new OleDbConnection(connectString);
            myConnection.Open();
            OleDbCommand cmd = new OleDbCommand("select * From Table1", myConnection);
            OleDbDataReader odr = null;
            odr = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(odr);
            comboBox1.DataSource = table;
            comboBox1.BindingContext = this.BindingContext;
            comboBox1.DisplayMember = "Table1";
            comboBox1.ValueMember = "Valyuta";
            comboBox1.SelectedIndex = -1;


            comboBox2.DataSource = table;
            comboBox2.BindingContext = this.BindingContext;
            comboBox2.DisplayMember = "Table1";
            comboBox2.ValueMember = "Kurs";


            comboBox3.DataSource = table;
            comboBox3.BindingContext = this.BindingContext;
            comboBox3.DisplayMember = "Table1";
            comboBox3.ValueMember = "Kod";

            comboBox4.DataSource = table;
            comboBox4.BindingContext = this.BindingContext;
            comboBox4.DisplayMember = "Table1";
            comboBox4.ValueMember = "ID";

        }






        private void Form1_Load_1(object sender, EventArgs e)
        {

            datagetir(); // Form1 yüklənəndə datamızı gətirmək üçün yaratdığımız funksiyanı çağırırırıq 
            datagetirildi = true; // Datamız gələndən sonra comboBox1_SelectedValueChanged üçün yaratdığımız dəyişəni aktivləşdiririk

        }



        private void comboBox1_SelectedValueChanged(object sender, EventArgs e) // Valyuta seçildikdə
        {

            if (datagetirildi == true) // Datamız gətirilibsə

            {
                int secilmisvalyutaindeksi = comboBox1.SelectedIndex;


                secilmishkurs = comboBox2.GetItemText(comboBox2.Items[secilmisvalyutaindeksi]);
                secilmishkod = comboBox3.GetItemText(comboBox3.Items[secilmisvalyutaindeksi]);
                secilmishID = comboBox4.GetItemText(comboBox4.Items[secilmisvalyutaindeksi]);

                textBox3.Text = secilmishkurs;
                label1.Text = secilmishkod;


                string secilmishfayl = secilmishkod.ToLower(); // Valyuta KOD-ları böyük hərflərlədir amma İcon adları balacadır deyə KODU kiçildib istifadə edirik
                Image myIcon = (Image)global::Valyuta.Properties.Resources.ResourceManager.GetObject(secilmishfayl);
                pictureBox1.BackgroundImage = myIcon;
            }


        }





        private void button1_Click(object sender, EventArgs e) // Çevir
        {

            
            bool a = double.TryParse(textBox1.Text, out double x);//Düzgün dəyər daxil edildiyini yoxlamaq üçün
            bool b = double.TryParse(textBox3.Text, out double y);//Düzgün dəyər daxil edildiyini yoxlamaq üçün
            if (a== true && b == true) //Düzgün dəyər daxil edildiyini yoxlamaq üçün
            { 
                textBox2.Text = Convert.ToString(Convert.ToDouble(textBox1.Text) * Convert.ToDouble(textBox3.Text));
            }
            else
            {
                MessageBox.Show("Məbləğ düzgün daxil edilməyib və ya valyuta kursu yanlış seçilib");
            }
        }





        private void button2_Click(object sender, EventArgs e) //Təmizlə
        {
            textBox1.Clear();
            textBox2.Clear();
            label1.Text = null;
            textBox3.Text = null;
            pictureBox1.BackgroundImage = base.BackgroundImage;   
            datagetirildi = false;
            comboBox1.SelectedIndex = -1;
            datagetirildi = true;


        }





        private void button4_Click(object sender, EventArgs e) //Dəyişdir
        {



         
            string commandText = "UPDATE Table1 SET KURS = ? WHERE ID =  ? ";

            using (OleDbConnection connection = new OleDbConnection(connectString))
            {
                OleDbCommand command = new OleDbCommand(commandText, connection);
                command.Parameters.AddWithValue("@KURS", textBox3.Text);
                command.Parameters.AddWithValue("@ID", secilmishID);

                try
                {
                    command.Connection.Open();
                    int response = command.ExecuteNonQuery();
                    MessageBox.Show(textBox3.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: {0}" + ex.Message);
                    MessageBox.Show(textBox3.Text);
                }

                //Dəyişmə prosesindən sonra Datamızı yenidən gətiririk yenilənmə üçün ( datagetirildi hemin prosesdə false olmalıdır comboBox1_SelectedValueChanged giclemesin deye)
                datagetirildi = false;
                datagetir();
                datagetirildi = true;
                textBox3.Text = null;

            }


        }





        private void button5_Click(object sender, EventArgs e) //Sil
        {




   
            string commandText = "DELETE FROM Table1 WHERE ID =  ? ";

            using (OleDbConnection connection = new OleDbConnection(connectString))
            {
                OleDbCommand command = new OleDbCommand(commandText, connection);
                command.Parameters.AddWithValue("@ID", secilmishID);

                try
                {
                    command.Connection.Open();
                    int response = command.ExecuteNonQuery();
                    MessageBox.Show(secilmishID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: {0}" + ex.Message);
                    MessageBox.Show(secilmishID);
                }


            }

            //Silmə prosesindən sonra Datamızı yenidən gətiririk yenilənmə üçün ( datagetirildi hemin prosesdə false olmalıdır comboBox1_SelectedValueChanged giclemesin deye)
            datagetirildi = false;
            datagetir();
            datagetirildi = true;
            textBox3.Text = null;

        }






        private void button3_Click(object sender, EventArgs e)  // Yeni Valyuta
        {
            Form2 f = new Form2();
            f.Owner = this;
          //  f.Show();
            f.ShowDialog(); // any code after this will be executed after form2_closed


            //Valyuta əlavəetmə prosesindən sonra Datamızı yenidən gətiririk yenilənmə üçün ( datagetirildi hemin prosesdə false olmalıdır comboBox1_SelectedValueChanged giclemesin deye)
            datagetirildi = false;
            datagetir();
            datagetirildi = true;
            textBox3.Text = null;

        }



        

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            myConnection.Close();
        }

      











    }
}
