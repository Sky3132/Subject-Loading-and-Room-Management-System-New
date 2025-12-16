using __Subject_Loading_and_Room_Assignment_Monitoring_System.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace __Subject_Loading_and_Room_Assignment_Monitoring_System.Forms
{
    public partial class SubjectOffering : Form
    {
        SubjectManager subjectManager = new SubjectManager();
        public int currentOfferingId = 0;
        public SubjectOffering()
        {
            InitializeComponent();
        }

        private void SubjectOffering_Load(object sender, EventArgs e)
        {
            DataTable subjects = subjectManager.GetAllSubjects();
            cmbCode.DataSource = subjects;
            cmbCode.DisplayMember = "DisplayName";
            cmbCode.ValueMember = "subjectId";  

            LoadOfferingsToDGV();
        }
        private void LoadOfferingsToDGV()
        {
            DataTable offeringsDt = subjectManager.GetAllSubjects();

            dgvSubjectOffering.DataSource = offeringsDt;

            if (dgvSubjectOffering.Columns.Contains("subjectId")) dgvSubjectOffering.Columns["subjectId"].Visible = false;
            if (dgvSubjectOffering.Columns.Contains("Code")) dgvSubjectOffering.Columns["Code"].Visible = false;
            if (dgvSubjectOffering.Columns.Contains("Title")) dgvSubjectOffering.Columns["Title"].Visible = false;

            if (dgvSubjectOffering.Columns.Contains("SubjectDisplay"))
            {
                dgvSubjectOffering.Columns["SubjectDisplay"].HeaderText = "Code - Title";
                dgvSubjectOffering.Columns["SubjectDisplay"].Visible = true;
                dgvSubjectOffering.Columns["SubjectDisplay"].DisplayIndex = 1;
            }
        }
        private void cmbSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFiltersAndReloadDGV();
        }
        private void cmbSchoolYear_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            ApplyFiltersAndReloadDGV();
        }

        private void btnAddOffering_Click_1(object sender, EventArgs e)
        {
            try
            {
                int selectedSubjectId = Convert.ToInt32(cmbCode.SelectedValue);

                string selectedSemester = cmbSemester.Text;
                string selectedSchoolYear = cmbSchoolYear.Text;

                DataClasses1DataContext db = new DataClasses1DataContext();
                db.SubjectOffering(selectedSubjectId, selectedSemester, selectedSchoolYear);
                db.SubmitChanges();
            
                LoadOfferingsToDGV();

                MessageBox.Show("Subject Offering added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding subject offering: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnEditOffering_Click(object sender, EventArgs e)
        {
            if (currentOfferingId <= 0) 
            {
                MessageBox.Show("Please select an offering to edit.", "Warning");
                return;
            }

            try
            {
                int updatedSubjectId = Convert.ToInt32(cmbCode.SelectedValue);
                string updatedSemester = cmbSemester.Text;
                string updatedSchoolYear = cmbSchoolYear.Text;

                subjectManager.UpdateOfferings(currentOfferingId, updatedSubjectId, updatedSemester, updatedSchoolYear);

                LoadOfferingsToDGV();
                MessageBox.Show("Subject Offering updated successfully!", "Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating subject offering: " + ex.Message, "Error");
            }
        }

        private void dgvSubjectOffering_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvSubjectOffering.Rows[e.RowIndex];

                currentOfferingId = Convert.ToInt32(row.Cells["offeringId"].Value);
                
                cmbCode.SelectedValue = Convert.ToInt32(row.Cells["subjectId"].Value);
                cmbSemester.Text = row.Cells["Semester"].Value.ToString();
                cmbSchoolYear.Text = row.Cells["SchoolYear"].Value.ToString();
            }
        }

        private void btnDeleteOffering_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgvSubjectOffering.CurrentRow.Cells["offeringId"].Value);

            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this subject?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.No)
            {
                return;
            }

            bool deleted = subjectManager.DeleteOfferings(id);

            if (deleted)
            {
                MessageBox.Show("Deleted Successfully");
            }
            LoadOfferingsToDGV();
        }
        private void ApplyFiltersAndReloadDGV()
        {
            string selectedSemester = cmbSemester.Text;
            string selectedSchoolYear = cmbSchoolYear.Text;

            DataTable offeringsDt;

            if (string.IsNullOrWhiteSpace(selectedSemester) && string.IsNullOrWhiteSpace(selectedSchoolYear))
            {
                offeringsDt = subjectManager.GetAllSubjects();
            }
            else
            {
                offeringsDt = subjectManager.GetOfferingsByFilter(selectedSemester, selectedSchoolYear);
            }
            dgvSubjectOffering.DataSource = offeringsDt;
        }
    }
}
