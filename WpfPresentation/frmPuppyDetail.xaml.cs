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
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Diagnostics;
using Azure.Core;
using System.Windows.Navigation;

namespace WpfPresentation
{
    /// <summary>
    /// Interaction logic for frmPuppyDetail.xaml
    /// </summary>
    public partial class frmPuppyDetail : Window
    {
        private List<Puppy> _puppies = new List<Puppy>();
        private IPuppyManager puppyManager = new PuppyManager();
        private IMedicalRecordManager medicalRecordManager = new MedicalRecordManager();
        private IAdoptionManager adoptionManager = new AdoptionManager();
        private IApplicationManager applicationManager = new ApplicationManager();
        private IUserManager userManager = new UserManager();

        private string _litterID;
        private int _number;
        private UserVM _user;
        public frmPuppyDetail(string litterID, int number, UserVM user)
        {
            this._litterID = litterID;
            this._number = number;
            this._user = user;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _puppies = puppyManager.SelectPuppiesByLitterID(_litterID);
            Puppy puppy = _puppies[_number];
            MedicalRecord medRecord = medicalRecordManager.SelectMedicalRecordByMedicalRecordID(puppy.MedicalRecordID);
            List<DataDomain.Application> applications = new List<DataDomain.Application>();
            try
            {
                applications = applicationManager.SelectApplicationsByUserID(_user.UserID);
            }
            catch
            {
                
            }
            List<Adoption> adoptions = adoptionManager.GetAllAdoptions();

            string adopted = "";

            if (puppy.Adopted == true)
            {
                adopted = "Sold";
                btnPupAdopt.Visibility = Visibility.Hidden;
            }
            else 
            {
                adopted = "Available";

                if (_user.Roles.Contains("adopter") || _user.Roles.Contains("Adopter"))
                {
                    // checks for approved applications
                    var approvedApplications = applications.Where(app => app.Status == true).ToList();

                    // gets all applicationIDs in adoptions
                    var adoptedApplicationIDs = adoptions.Select(adopt => adopt.ApplicationID).ToList();
                    bool canAdopt = false;

                    // Sees if the user has any unused applications
                    foreach (var app in approvedApplications)
                    {
                        if (!adoptedApplicationIDs.Contains(app.ApplicationID))
                        {
                            canAdopt = true;
                            break;
                        }
                    }
                    if (canAdopt == true)
                    {
                        btnPupAdopt.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        btnPupAdopt.Visibility = Visibility.Hidden;
                    }
                }
                else
                {
                    btnPupAdopt.Visibility = Visibility.Hidden;
                }
            }

            lblPupAvail.Content = "Availability: " + adopted;
            lblPupBreed.Content = puppy.BreedID;
            lblPupMicro.Content = "Microchip: " + puppy.Microchip;
            lblPupPrice.Content = "Price: $" + puppy.Price;
            lblPupTreatment.Content = "Treatments: " + medRecord.Treatment;
            lblPupWeight.Content = "Weight: " + medRecord.Weight + " lb";

            Uri pupImageUri = new Uri("Photos/" + puppy.Image, UriKind.Relative);

            BitmapImage pupImg = new BitmapImage();
            pupImg.BeginInit();
            pupImg.UriSource = pupImageUri;
            pupImg.CacheOption = BitmapCacheOption.OnLoad;
            pupImg.EndInit();
            imgPup.Source = pupImg;

        }

        private void btnPupAdopt_Click(object sender, RoutedEventArgs e)
        {
            var puppyManager = new PuppyManager();
            _puppies = puppyManager.SelectPuppiesByLitterID(_litterID);
            Puppy puppy = _puppies[_number];
            List<Adoption> adoptions = adoptionManager.GetAllAdoptions();
            List<DataDomain.Application> applications = applicationManager.SelectApplicationsByUserID(_user.UserID);
            DataDomain.Application application = applicationManager.GetAllApplications().FirstOrDefault();
           
            // Get the user's approved applications
            var approvedApplications = applications.Where(app => app.Status == true).ToList();

            // Get IDs of already used applications
            var adoptedApplicationIDs = adoptions.Select(adopt => adopt.ApplicationID).ToList();

            // Find the first unused approved application
            var unusedApp = approvedApplications.FirstOrDefault(app => !adoptedApplicationIDs.Contains(app.ApplicationID));

            try
            {
                int result = adoptionManager.InsertAdoption(unusedApp.ApplicationID, puppy.PuppyID, _user.UserID, "In Progress");
                if (result > 0)
                {
                    puppyManager.UpdatePuppy(puppy.PuppyID, false, true);
                    MessageBox.Show("Puppy successfully adopted!");
                    this.Close();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                this.Close();
            }

        }
    }
}
