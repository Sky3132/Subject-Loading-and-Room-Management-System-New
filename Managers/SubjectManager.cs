using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace __Subject_Loading_and_Room_Assignment_Monitoring_System.Managers
{
    public class SubjectManager
    {

        ConnectionString connect = new ConnectionString();
        

        public DataTable GetAllSubjects()
        {
           
            DataTable dt = new DataTable();
            string query = "SELECT * FROM tblsubject";
     

            using (SqlConnection con = new SqlConnection(connect.connection))

            using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
            {
                con.Open();
                adapter.Fill(dt);
            }
            return dt;
        }
        public bool CodeExists(int code)
        {

            string query = "SELECT COUNT(*) FROM tblsubject WHERE Code = @Code";

            using (SqlConnection con = new SqlConnection(connect.connection))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Code", code);
                con.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        public bool AddSubject(int code, string title, int units, string department, string program)
        {
            if (CodeExists(code))
                return false; 

            string query = "INSERT INTO tblsubject (Code, Title, Units, Department, Program) VALUES (@Code, @Title, @Units, @Department, @Program)";

            using (SqlConnection con = new SqlConnection(connect.connection))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Code", code);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Units", units);
                cmd.Parameters.AddWithValue("@Department", department);
                cmd.Parameters.AddWithValue("@Program", program);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return true;
        }
        public bool UpdateSubject(int id, int code, string title, int units, string department, string program)
        {
            string queryCheck = "SELECT COUNT(*) FROM tblsubject WHERE Code = @Code AND subjectId <> @subjectId";

            using (SqlConnection con = new SqlConnection(connect.connection))
            using (SqlCommand cmdCheck = new SqlCommand(queryCheck, con))
            {
                cmdCheck.Parameters.AddWithValue("@Code", code);
                cmdCheck.Parameters.AddWithValue("@subjectId", id);
                con.Open();
                int count = (int)cmdCheck.ExecuteScalar();
                if (count > 0)
                    return false; 
            }

            string queryUpdate = "UPDATE tblsubject SET Code=@Code, Title=@Title, Units=@Units, Department=@Department, Program=@Program WHERE subjectId=@subjectId";

            using (SqlConnection con = new SqlConnection(connect.connection))
            using (SqlCommand cmdUpdate = new SqlCommand(queryUpdate, con))
            {
                cmdUpdate.Parameters.AddWithValue("@subjectId", id);
                cmdUpdate.Parameters.AddWithValue("@Code", code);
                cmdUpdate.Parameters.AddWithValue("@Title", title);
                cmdUpdate.Parameters.AddWithValue("@Units", units);
                cmdUpdate.Parameters.AddWithValue("@Department", department);
                cmdUpdate.Parameters.AddWithValue("@Program", program);

                con.Open();
                cmdUpdate.ExecuteNonQuery();
            }

            return true;
        }
        public bool DeleteSubject(int id) 
        {
            string queryDelete = "DELETE FROM tblsubject WHERE subjectId=@subjectId";

            using (SqlConnection con = new SqlConnection(connect.connection))
            using (SqlCommand cmdDelete = new SqlCommand(queryDelete, con))
            {
                cmdDelete.Parameters.AddWithValue("@subjectId", id);

                con.Open();
                cmdDelete.ExecuteNonQuery();
            }
            return true;
        }
        public DataTable GetAllSubject()
        {
            string query = "SELECT subjectId, Code, Title FROM tblsubject";

           using (SqlConnection con = new SqlConnection(connect.connection))
           using (SqlDataAdapter da = new SqlDataAdapter(query, con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                dt.Columns.Add("DisplayName", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    row["DisplayName"] = row["Code"].ToString() + " - " + row["Title"].ToString();
                }
                return dt;
            }
        }
        public DataTable AddOfferings()
        {
            DataTable dt = new DataTable();
            string spName = "GetAllSubjectOfferings";

            using (SqlConnection con = new SqlConnection(connect.connection))
            using (SqlCommand cmd = new SqlCommand(spName, con))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                adapter.Fill(dt);
            }
            dt.Columns.Add("SubjectDisplay", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                row["SubjectDisplay"] = row["Code"].ToString() + " - " + row["Title"].ToString();
            }
            return dt;
        }
        public bool UpdateOfferings(int offeringId, int subjectId, string semester, string schoolYear)
        {
            string spName = "UpdateSubjectOffering";

            using (SqlConnection con = new SqlConnection(connect.connection))
            using (SqlCommand cmd = new SqlCommand(spName, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OfferingId", offeringId);
                cmd.Parameters.AddWithValue("@SubjectId", subjectId);
                cmd.Parameters.AddWithValue("@Semester", semester);
                cmd.Parameters.AddWithValue("@SchoolYear", schoolYear);

                con.Open();

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        public bool DeleteOfferings(int offeringId)
        {
            string spName = "DeleteSubjectOffering";

            using (SqlConnection con = new SqlConnection(connect.connection))
            using (SqlCommand cmd = new SqlCommand(spName, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@offeringId", offeringId);

                con.Open();

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
public DataTable GetOfferingsByFilter(string semester, string schoolYear)
{
    DataTable dt = new DataTable();
    
    string query = @"
        SELECT 
            so.offeringId, so.subjectId, so.Semester, so.SchoolYear,
            s.Code, s.Title
        FROM 
            tblsubjectOffering so
        INNER JOIN 
            tblsubject s ON so.subjectId = s.subjectId
        WHERE 
            (@Semester = '' OR so.Semester = @Semester) 
            AND (@SchoolYear = '' OR so.SchoolYear = @SchoolYear)";

    using (SqlConnection con = new SqlConnection(connect.connection))
    using (SqlCommand cmd = new SqlCommand(query, con))
    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
    {
        cmd.Parameters.AddWithValue("@Semester", semester ?? "");
        cmd.Parameters.AddWithValue("@SchoolYear", schoolYear ?? "");
        
        con.Open();
        adapter.Fill(dt);
    }
    
    dt.Columns.Add("SubjectDisplay", typeof(string));

    foreach (DataRow row in dt.Rows)
    {
        row["SubjectDisplay"] = row["Code"].ToString() + " - " + row["Title"].ToString();
    }
    
    return dt;
}
    }
}
