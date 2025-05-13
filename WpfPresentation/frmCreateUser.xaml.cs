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
    /// <summary>
    /// Interaction logic for frmCreateUser.xaml
    /// </summary>
    public partial class frmCreateUser : Window
    {
        private User _user;
        private IUserManager _userManager;
        private bool _isNewUser;
        private UserRoleManager _userRole = new UserRoleManager();

    public frmCreateUser(User user,
            UserManager userManager, bool isNew = false)
        {
            this._user = user;
            this._userManager = userManager;
            this._isNewUser = isNew;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
             var userManager = new UserManager();
             List<User> userList = userManager.SelectAllUsers();

            foreach (User user in userList)
            {
                if (user.Email == txtEmail.Text)
                {
                    MessageBox.Show("Email Account already exists.");
                    return;
                }
                if (user.PhoneNumber == txtPhoneNumber.Text)
                {
                    MessageBox.Show("Phone Account already exists.");
                    return;
                }

            }

            if (txtFirstName.Text.Length == 0)
            {
                MessageBox.Show("You have to enter a first name.");
                return;
            }

            if (txtLastName.Text.Length == 0)
            {
                MessageBox.Show("You have to enter a last name.");
                return;
            }

            string regExPhone = @"^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$";
            string regExEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(txtPhoneNumber.Text, regExPhone))
            {
                MessageBox.Show("Invalid Phone number.");
                return;
            }
            if (!Regex.IsMatch(txtEmail.Text, regExEmail))
            {
                MessageBox.Show("Invalid Email.");
                return;
            }

            string givenName = txtFirstName.Text;
            string familyName = txtLastName.Text;
            string phoneNumber = txtPhoneNumber.Text;
            string email = txtEmail.Text;
            bool approvedApplication = false;
            try
            {
                int result = (userManager.InsertUser(givenName, familyName, phoneNumber, email));
                if(result > 0)
                 {
                    this.DialogResult = true;
                    _userRole.InsertUserRole("User", result, "Basic user with no special access.");
                }
                else
                {
                    MessageBox.Show("Create Account failed.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while creating the account: {ex.Message}");
            }

        }
    }
}
