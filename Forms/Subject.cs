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

        private SubjectManager subjectManager = new SubjectManager();

        ConnectionString connect = new ConnectionString();

        private void LoadSubjects()
        {
            using (SqlConnection con = new SqlConnection(connect.connection))
            {
                SqlCommand cmd = new SqlCommand("GetSubjectsWithDepartment", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvSubjects.DataSource = dt;

                dgvSubjects.Columns["subjectId"].Visible = false;
                dgvSubjects.Columns["ProgramID"].Visible = false;
                dgvSubjects.Columns["DepartmentID"].Visible = false;
            }
        }
        private void Subject_Load_1(object sender, EventArgs e)
        {
            LoadComboBoxes();
            LoadSubjects();

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
        private void LoadComboBoxes()
        {
            using (SqlConnection con = new SqlConnection(connect.connection))
            {
                SqlCommand cmd = new SqlCommand("GetSubjectsWithDepartment", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                DataTable deptTable = dt.DefaultView.ToTable(
                    true, 
                    "DepartmentID",
                    "DepartmentName"
                );

                cmbDepartment.DataSource = deptTable;
                cmbDepartment.DisplayMember = "DepartmentName"; 
                cmbDepartment.ValueMember = "DepartmentID";     
                cmbDepartment.SelectedIndex = -1;

                DataTable progTable = dt.DefaultView.ToTable(
                    true, 
                    "ProgramID",
                    "ProgramName"
                );

                cmbProgram.DataSource = progTable;
                cmbProgram.DisplayMember = "ProgramName"; 
                cmbProgram.ValueMember = "ProgramID";    
                cmbProgram.SelectedIndex = -1;
            }
        }
        private void btnAddSubject_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtCode.Text) || string.IsNullOrWhiteSpace(txtLectureUnits.Text) 
                || string.IsNullOrWhiteSpace(txtLaboratoryUnits.Text))
            {
                MessageBox.Show("Please fill up all the form", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            string code = txtCode.Text; 
            string title = txtTitle.Text;
            int departmentId = Convert.ToInt32(cmbDepartment.SelectedValue);
            int programID = Convert.ToInt32(cmbProgram.SelectedValue);
            string lectureUnits = txtLectureUnits.Text;
            string laboratoryUnits = txtLaboratoryUnits.Text;

            DataClasses1DataContext db =    new DataClasses1DataContext();
        }
        private void btnEditSubject_Click_1(object sender, EventArgs e)
        {
            if (dgvSubjects.CurrentRow == null) return;

            int id = Convert.ToInt32(dgvSubjects.CurrentRow.Cells["subjectId"].Value);

            if (!int.TryParse(txtCode.Text.Trim(), out int code))
            {
                MessageBox.Show("Code must be a number.");
                return;
            }

            string title = txtTitle.Text.Trim();
  
            string department = cmbDepartment.Text.Trim();

            string program = cmbProgram.Text.Trim();
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
                LoadSubjects();
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