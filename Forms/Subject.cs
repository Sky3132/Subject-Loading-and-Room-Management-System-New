using __Subject_Loading_and_Room_Assignment_Monitoring_System.Forms;
using __Subject_Loading_and_Room_Assignment_Monitoring_System.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace __Subject_Loading_and_Room_Assignment_Monitoring_System
{
    public partial class Subject : Form
    {
        public Subject()
        {
            InitializeComponent();
        }

        string connectionString = "Data Source=DESKTOP-RFR1DK9;Initial Catalog=Schooldb;Integrated Security=True;";
        private SubjectManager subjectManager = new SubjectManager();

        private void LoadingSubject()
        {
            dgvSubjects.DataSource = subjectManager.GetAllSubjects();
        }
        private void Subject_Load_1(object sender, EventArgs e)
        {
            LoadingSubject();
            dgvSubjects.Columns["subjectId"].Visible = false;
        }

        private void dgvSubjects_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSubjects.Rows[e.RowIndex];
                txtCode.Text = row.Cells["Code"].Value.ToString();
                txtTitle.Text = row.Cells["Title"].Value.ToString();
            }
        }
        private void btnAddSubject_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtCode.Text.Trim(), out int code))
            {
                MessageBox.Show("Code must be a number.");
                return;
            }

            string title = txtTitle.Text.Trim();
            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Please enter Title.");
                return;
            }

            if (!int.TryParse(txtUnits.Text.Trim(), out int units))
            {
                MessageBox.Show("Units must be a number.");
                return;
            }

            string department = txtDepartment.Text.Trim();
            string program = txtProgram.Text.Trim();

            bool added = subjectManager.AddSubject(code, title, units, department, program);

            if (added)
            {
                MessageBox.Show("Subject added successfully!");
                LoadingSubject();
            }
            else
            {
                MessageBox.Show("This Code already exists!");
            }
        }

        private void btnEditSubject_Click(object sender, EventArgs e)
        {
            if (dgvSubjects.CurrentRow == null) return;

            int id = Convert.ToInt32(dgvSubjects.CurrentRow.Cells["subjectId"].Value);

            if (!int.TryParse(txtCode.Text.Trim(), out int code))
            {
                MessageBox.Show("Code must be a number.");
                return;
            }

            string title = txtTitle.Text.Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Please enter Title.");
                return;
            }

            if (!int.TryParse(txtUnits.Text.Trim(), out int units))
            {
                MessageBox.Show("Units must be a number.");
                return;
            }

            string department = txtDepartment.Text.Trim();
            if (string.IsNullOrWhiteSpace(department))
            {
                MessageBox.Show("Please enter Department.");
                return;
            }

            string program = txtProgram.Text.Trim();
            if (string.IsNullOrWhiteSpace(program))
            {
                MessageBox.Show("Please enter Program.");
                return;
            }

            bool updated = subjectManager.UpdateSubject(id, code, title, units, department, program);

            if (updated)
            {
                MessageBox.Show("Subject updated successfully!");
                LoadingSubject();
            }
            else
            {
                MessageBox.Show("This Code already exists!");
            }
        }

        private void btnDeleteSubject_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgvSubjects.CurrentRow.Cells["subjectId"].Value);

            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this subject?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.No)
            {
                return;
            }

            bool deleted = subjectManager.DeleteSubject(id);

            if (deleted)
            {
                MessageBox.Show("Deleted Successfully");
                LoadingSubject();
            }
        }
        
        private void btnSearchSubject_Click(object sender, EventArgs e)
        {
            bool foundMatch = false;
            string searchValue = txtSearch.Text.Trim().ToLower();

            foreach (DataGridViewRow row in dgvSubjects.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.White;
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchValue))
                        {
                            cell.Style.BackColor = Color.Yellow; 
                            foundMatch = true;
                        }
                    }
                }

            }
            if (!foundMatch)
            {
                MessageBox.Show("No match record found.", "Not found", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void txtSearch_KeyUp_1(object sender, KeyEventArgs e)
        {

            string searchValue = txtSearch.Text.Trim().ToLower();

            foreach (DataGridViewRow row in dgvSubjects.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.White;
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchValue))
                        {
                            cell.Style.BackColor = Color.Yellow; 
                        }
                    }
                }

            }
        }

        private void lblSubjectOffering_Click(object sender, EventArgs e)
        {    
            SubjectOffering subject = new SubjectOffering();
            subject.Show();
            this.Hide();
        }
    }   
}