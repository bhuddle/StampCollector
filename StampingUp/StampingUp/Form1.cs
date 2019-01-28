using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace StampingUp
{
    public partial class Form1 : Form
    {
        MySqlConnection conn = new MySqlConnection("server=localhost;user id=newuser;database=stampcollection");
        MySqlDataReader rdr;
        MySqlCommand cmd;
        String Query;
        public Form1()
        {
            InitializeComponent();
        }

        //add button
        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            try
            {
                var today = DateTime.Now.ToString("MM/dd/yyyy");
                Query = "insert into stampcollection.stamps (name,date) values ('" + textBox1.Text.ToUpper() + "', '" + today + "')";
                cmd = new MySqlCommand(Query, conn);
                rdr = cmd.ExecuteReader();
                MessageBox.Show("Entered value successfully!");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("'" + textBox1.Text + "' might already exist!");
            }
            conn.Close();
            conn.Open();
            Query = "select * from stampcollection.stamps";
            cmd = new MySqlCommand(Query, conn);
            int rows = cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
            ActiveControl = textBox1;
            textBox1.SelectionStart = 0;
            textBox1.SelectionLength = textBox1.Text.Length;
        }

        // delete button
        private void button2_Click(object sender, EventArgs e)
        {
            conn.Open();
            DialogResult diagres = MessageBox.Show("Confirm delete?", "Remove Item" ,MessageBoxButtons.YesNo);
            if (diagres == DialogResult.Yes) {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    DataGridViewRow delrow = dataGridView1.Rows[i];
                    if (delrow.Selected == true)
                    {
                        try
                        {
                            Query = "delete from stampcollection.stamps where name = '" + delrow.Cells[0].Value.ToString() + "'";
                            Debug.WriteLine(Query);
                            cmd = new MySqlCommand(Query, conn);
                            cmd.ExecuteNonQuery();
                            dataGridView1.Rows.RemoveAt(i);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }
                foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
                {
                    if (cell.Selected && dataGridView1.Columns[cell.ColumnIndex].Name.ToString() == "name")
                    {
                        try
                        {
                            Debug.WriteLine(cell.Value.ToString());
                            Debug.WriteLine(dataGridView1.Columns[cell.ColumnIndex].Name.ToString());
                            Query = "delete from stampcollection.stamps where name = '" + cell.Value.ToString() + "'";
                            cmd = new MySqlCommand(Query, conn);
                            cmd.ExecuteNonQuery();
                            dataGridView1.Rows.RemoveAt(cell.RowIndex);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }
            }
            conn.Close();

            ActiveControl = textBox1;
            textBox1.SelectionStart = 0;
            textBox1.SelectionLength = textBox1.Text.Length;
        }

        // display all
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                Query = "select * from stampcollection.stamps";
                cmd = new MySqlCommand(Query, conn);
                int rows = cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                conn.Close();
                ActiveControl = textBox1;
                textBox1.SelectionStart = 0;
                textBox1.SelectionLength = textBox1.Text.Length;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("'" + textBox1.Text + "', might already exist or due to: " + ex.Message);
            }
            finally
            {
                conn.Dispose();
            }
        }

        private void Search_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                Query = "select * from stampcollection.stamps where name like '%" + textBox1.Text + "%'";
                cmd = new MySqlCommand(Query, conn);
                int rows = cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                conn.Close();
                ActiveControl = textBox1;
                textBox1.SelectionStart = 0;
                textBox1.SelectionLength = textBox1.Text.Length;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("'" + textBox1.Text + "', might already exist or due to: " + ex.Message);
            }
            finally
            {
                conn.Dispose();
            }
        }
    }
}
