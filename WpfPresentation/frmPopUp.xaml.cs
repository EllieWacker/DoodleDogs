using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LogicLayer;
using DataDomain;
using System.Text.RegularExpressions;

namespace WpfPresentation
{
    public partial class frmPopUp : Window
    {
        public DataDomain.Application selectedApplication { get; set; }
  

        // Constructor accepts the selected application
        public frmPopUp(DataDomain.Application application)
        {
            InitializeComponent();
            selectedApplication = application;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            string result = "approved";
            var userManager = new UserManager();
            List<User> userList = userManager.SelectAllUsers();

            // first or default finds the first element that satisfies the condition.
            var selectedUser = userList.FirstOrDefault(user => user.UserID == selectedApplication.UserID);
            if (selectedApplication.Status == false)
            {
                result = "not approved. Would you like to approve it?";
                btnApprove.IsEnabled = true;
            }
            if (selectedApplication != null)
            {
                txtApplicationDetails.Text = "You selected Application Number: " + selectedApplication.ApplicationID + ". It is " + result;
            }
        }


        private void ApproveButton_Click(object sender, RoutedEventArgs e)
        {
            var userManager = new UserManager();
            var userRoleManager = new UserRoleManager();
            var applicationManager = new ApplicationManager();
            List<User> userList = userManager.SelectAllUsers();
            List<string> userRoleList = userManager.GetRolesForUser(selectedApplication.UserID);
            var selectedUser = userList.FirstOrDefault(user => user.UserID == selectedApplication.UserID);
            try
            {
                if (applicationManager.UpdateApplicationStatus(selectedApplication.ApplicationID, false, true) == 1)
                {
                    MessageBox.Show("Application Approved!");
                    try
                    {
                        if (userRoleList.Contains("Adopter"))
                        {
                            return;
                        }
                        else if(userRoleList.Contains("User"))
                        {
                            if(userRoleManager.UpdateUserRole(selectedUser.UserID, "User", "Adopter") == 1)
                            {
                                MessageBox.Show("User is now an adopter!");
                            }
                            else
                            {
                                MessageBox.Show("Role Update Failed");
                            } 
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DenyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
