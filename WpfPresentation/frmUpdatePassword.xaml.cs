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

namespace WpfPresentation
{
    /// <summary>
    /// Interaction logic for frmUpdatePassword.xaml
    /// </summary>
    public partial class frmUpdatePassword : Window
    {
        private User _user;
        private IUserManager _userManager;
        private bool _isNewUser;

        public frmUpdatePassword(User user,
            UserManager userManager, bool isNew = false)
        {
            this._user = user;
            this._userManager = userManager;
            this._isNewUser = isNew;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_isNewUser)
            {
                tbkMessage.Text = _user.GivenName +
                    " as a new user, you must "
                    + tbkMessage.Text;
                txtEmail.Text = _user.Email;
                pwdOldPassword.Password = "newuser";
                txtEmail.IsEnabled = false;
                pwdOldPassword.IsEnabled = false;
            }
            else
            {
                tbkMessage.Text = "You may use this dialog to "
                    + tbkMessage.Text;
            }
            txtEmail.Focus();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text.Length < 7 || txtEmail.Text.Length > 100)
            {
                MessageBox.Show("Invalid Email.");
                txtEmail.Focus();
                txtEmail.SelectAll();
                return;
            }

            if (pwdOldPassword.Password.Length < 6)
            {
                MessageBox.Show("Invalid Current Password");
                pwdOldPassword.Focus();
                pwdOldPassword.SelectAll();
                return;
            }

            if (pwdNewPassword.Password.Length < 6 || pwdNewPassword.Password == pwdOldPassword.Password)
            {
                MessageBox.Show("Invalid New Password");
                pwdNewPassword.Focus();
                pwdNewPassword.SelectAll();
                return;
            }

            if (string.Compare(pwdNewPassword.Password,
                pwdRetypePassword.Password) != 0)
            {
                MessageBox.Show("New Password and Retyped Password must match.");
                pwdRetypePassword.Password = "";
                pwdNewPassword.Focus();
                pwdNewPassword.SelectAll();
                return;
            }


            string oldPassword = pwdOldPassword.Password;
            string newPassword = pwdNewPassword.Password;
            string username = txtEmail.Text;
            try
            {
                if (_userManager.UpdatePassword(username, oldPassword, newPassword))
                {
                    MessageBox.Show("Password Updated");
                    this.DialogResult = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
