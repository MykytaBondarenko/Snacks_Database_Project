using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Sweet_Bondarenko
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        DataSet ds;
        DataView dv1, dv2;
        public static string nameXMLfile = "Sets.xml";
        public static int currentRow = 0;
        bool isChangesSaved = true;
        string[] nazvaPolia = new string[] { "CodeSet", "SetName", "Weiqht", "Price", "Amount" };
        frmInfo frmInfo = new frmInfo();

        void LoadXmlFile()
        {
            ds = new DataSet();
            FileStream fsReadXml = new FileStream(nameXMLfile, FileMode.Open);
            ds.ReadXml(fsReadXml);
            fsReadXml.Close();
            dv1 = new DataView(ds.Tables[0]);
            dataGridView1.DataSource = dv1;
            string s = dataGridView1.Rows[0].Cells[0].Value.ToString();
            dv2 = new DataView(ds.Tables[1]);
            dv2.RowFilter = "CodeSet = '" + s + "'";
            dataGridView2.DataSource = dv2;

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Назва набору";
            dataGridView1.Columns[2].HeaderText = "Вага набору";
            dataGridView1.Columns[3].HeaderText = "Опис набору";
            dataGridView1.Columns[4].HeaderText = "Ціна набору";
            dataGridView1.Columns[5].HeaderText = "Кількість";
            dataGridView1.Columns[1].Width = 120;
            dataGridView1.Columns[2].Width = 50;
            dataGridView1.Columns[3].Width = dataGridView1.Width - 420;
            dataGridView1.Columns[4].Width = 50;
            dataGridView1.Columns[5].Width = 70;
            dataGridView1.Columns[6].Width = 70;
            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[7].Visible = false;

            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            currentRow = 0;
            isChangesSaved = true;
        }

        void SaveXmlFile()
        {
            FileStream fsWriteXml = new FileStream(nameXMLfile, FileMode.Create);
            ds.WriteXml(fsWriteXml);
            fsWriteXml.Close();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Filter = "";
            openFileDialog1.FileName = "Sets.xml";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                nameXMLfile = openFileDialog1.FileName;
                LoadXmlFile();
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            string s;
            int i = e.RowIndex;
            currentRow = i;
            if (!dataGridView1.Rows[i].Cells[1].Value.Equals(DBNull.Value))
            {
                lblSetName.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
                //демонстрація вибраного запису

                string sPict = dataGridView1.Rows[i].Cells[6].Value.ToString();
                if (sPict != "Нет" && sPict != "")
                    pctImage.Load("photo\\" + sPict);
                else
                    pctImage.Load("photo\\some.jpg");
                s = dataGridView1.Rows[i].Cells[0].Value.ToString();
                dv2 = new DataView(ds.Tables[1]);
                dv2.RowFilter = "CodeSet = '" + s + "'";
                dataGridView2.DataSource = dv2;
                //txtCode.Visible = false;
                //lblCode.Visible = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                dv1.Sort = nazvaPolia[comboBox1.SelectedIndex];
                if (radioButton2.Checked) dv1.Sort += " DESC";
                groupBox3.Enabled = true;
            }
            else
            {
                groupBox3.Enabled = false;
                comboBox2.Text = "";
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "") dv1.Sort = nazvaPolia[comboBox1.SelectedIndex] + " DESC";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dv1.RowFilter = "SetName Like '%" + textNazv.Text + "%'";
            if (checkBox2.Checked)
            {
                if (txtFrom.Text == "") txtFrom.Text = "0";
                if (txtTo.Text == "") txtTo.Text = "50";
                string filt = dv1.RowFilter + " And Price >= " + Convert.ToDouble(txtFrom.Text) + " And Price <= " + Convert.ToDouble(txtTo.Text);
                dv1.RowFilter = filt;
            }
            if(checkBox3.Checked)
            {
                if (cmbWeight.Text != "") dv1.RowFilter += " And Weiqht = " + cmbWeight.Text;
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string s = Microsoft.VisualBasic.Interaction.InputBox("Введіть код набору");
            if (s != "")
            {
                string strSort = dv1.Sort;
                dv1.Sort = "CodeSet"; // сортуємо по полю, в якому буде відбуватись пошук;
                int index = dv1.Find(s);
                if (index != -1)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[index].Selected = true;
                }
                else
                {
                    dv1.Sort = strSort;
                    MessageBox.Show("Такого набору немає!");
                }
            }
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            isChangesSaved = false;
            dv1.AddNew();
            txtCode.Visible = true;
            lblCode.Visible = true;
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            string s;
            if (!dataGridView1.Rows[currentRow].Cells[1].Value.Equals(DBNull.Value))
                s = dataGridView1.Rows[currentRow].Cells[1].Value.ToString();
            else s = "Без назви";
            if (MessageBox.Show("Ви дійсно бажаєте видалити набір " + s + "?", "Видалення даних", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                isChangesSaved = false;
                dv1.Delete(currentRow);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isChangesSaved)
            {
                DialogResult result;
                result = MessageBox.Show("Зберегти зміни?", "Збереження", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Yes)
                {
                    SaveXmlFile();
                    isChangesSaved = true;
                }
                else if (result == DialogResult.No)
                {
                    isChangesSaved = true;
                }
                else
                {
                    e.Cancel = true; // скасування закриття форми
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmInfo.StartPosition = FormStartPosition.CenterScreen;
            frmInfo.ShowDialog();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            nameXMLfile = "Sets.xml";
            LoadXmlFile();
        }
    }
}
