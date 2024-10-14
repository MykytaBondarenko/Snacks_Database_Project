using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sweet_Bondarenko
{
    public partial class frmInfo : Form
    {
        public frmInfo()
        {
            InitializeComponent();
        }

        DataSet ds;
        DataView dv1, dv2;
        string nameXMLfile = "sets.xml";
        int currentRow = 0;

        void FillTextBox()
        {
            int columnsCount = dv2.Table.Columns.Count;
            string[] sTextBox = new string[columnsCount];
            int i = currentRow;
            txtCurrent.Text = currentRow.ToString();

            for (int j = 0; j < columnsCount; j++)
            {
                if (!dv2.Table.Rows[i].ItemArray[j].Equals(DBNull.Value))
                    sTextBox[j] = dv2.Table.Rows[i].ItemArray[j].ToString();
                else
                    sTextBox[j] = "";
            }

            cmbCode.Text = sTextBox[0];
            txtName.Text = sTextBox[1];
            txtChoco.Text = sTextBox[2];
            txtNut.Text = sTextBox[3];
            txtFilling.Text = sTextBox[4];
            txtDescription.Text = sTextBox[5];
            txtPrice.Text = sTextBox[6];
            txtCodSet.Text = sTextBox[7];
            txtPict.Text = sTextBox[8];

            if (sTextBox[8] != "Нет" && sTextBox[8] != "")
                pctPhoto.Load("Photo\\" + sTextBox[8]);
            else
                pctPhoto.Load("Photo\\some.jpg");

            string s = sTextBox[7];
            int index = dv1.Find(s);
            if (index != -1)
            {
                dataGridView1.ClearSelection();
                dataGridView1.Rows[index].Selected = true;
                txtSetNazva.Text = dataGridView1.Rows[index].Cells[1].Value.ToString();
            }
        }

        private void cmbCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentRow = cmbCode.SelectedIndex;
            FillTextBox();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (currentRow < dv1.Count - 1)
            {
                currentRow++;
                FillTextBox();
            }
        }

        private void btPrev_Click(object sender, EventArgs e)
        {
            if (currentRow > 0)
            {
                currentRow--;
                FillTextBox();
            }
        }

        private void btLast_Click(object sender, EventArgs e)
        {
            currentRow = dv1.Count - 1;
            FillTextBox();
        }

        private void btFirst_Click(object sender, EventArgs e)
        {
            currentRow = 0;
            FillTextBox();
        }

        private void frmInfo_Load(object sender, EventArgs e)
        {
            nameXMLfile = frmMain.nameXMLfile;
            currentRow = frmMain.currentRow;
            ds = new DataSet();
            System.IO.FileStream fsReadXml = new System.IO.FileStream(nameXMLfile, System.IO.FileMode.Open);
            ds.ReadXml(fsReadXml);

            fsReadXml.Close();
            dv1 = new DataView(ds.Tables[0]);
            dataGridView1.DataSource = dv1;
            dv1.Sort = "CodeSet";
            dv2 = new DataView(ds.Tables[1]);
            dv2.Sort = "Code";
            dataGridView2.DataSource = dv2;
            FillTextBox();
        }
    }
}
