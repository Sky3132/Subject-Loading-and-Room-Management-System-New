using __Subject_Loading_and_Room_Assignment_Monitoring_System.Managers;
using __Subject_Loading_and_Room_Assignment_Monitoring_System.Models;
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

namespace __Subject_Loading_and_Room_Assignment_Monitoring_System
{
    public partial class Login : Form
    {
        private UserManager userManager = new UserManager();

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            User user = new User(); 
            user.Username = txtUsername.Text;
            user.Password = txtPassword.Text;

            bool success = userManager.Login(user.Username, user.Password);

            if (success)
            {
                Main mn = new Main();
                mn.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid Username or Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}
