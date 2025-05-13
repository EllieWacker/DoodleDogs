using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataDomain;
using LogicLayer;
using Microsoft.Win32;

namespace WpfPresentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UserVM _accessToken = null;
        private TabItem _storedApplicationTab;
        private TabItem _storedAdminTab;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtEmail.Focus();
            btnLoginLogout.IsDefault = true;
            hideAllTabs();
        }

        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Create new user account
        private void mnuNewUser_Click(object sender, RoutedEventArgs e)
        {
            var newUserWindow = new frmCreateUser(_accessToken, new UserManager());
            bool? result = newUserWindow.ShowDialog();
            if (result == true)
            {
                MessageBox.Show("User Created. Your new password is newuser.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("User was not created!", "Update Failed", MessageBoxButton.OK,
                       MessageBoxImage.Exclamation);
            }
        }

        // log in/out open program stuff

        private void btnLoginLogout_Click(object sender, RoutedEventArgs e)
        {
            var userManager = new UserManager();
            var applicationManager = new ApplicationManager();
            string email = txtEmail.Text;
            string password = pwdPassword.Password;
            UserVM userVM = null;

            if (btnLoginLogout.Content.ToString() == "Log Out")
            {
                logoutUser();
                return;
            }
            if (email.Length < 7)
            {
                MessageBox.Show("Invalid");
                txtEmail.Focus();
                txtEmail.SelectAll();
                return;
            }
            if (password.Length < 7)
            {
                MessageBox.Show("Invalid Password");
                pwdPassword.Focus();
                pwdPassword.SelectAll();
                return;
            }
            try
            {
                _accessToken = userManager.LoginUser(email, password);
                AdoptionManager adoptionManager = new AdoptionManager();
                List<Adoption> adoptions = adoptionManager.GetAllAdoptions();
                string roles = "";
                string message = "";
                for (int i = 0; i < _accessToken.Roles.Count; i++)
                {
                    roles += _accessToken.Roles[i];
                    if (i < _accessToken.Roles.Count - 1)
                    {
                        roles += ", ";
                    }
                    else if (i == _accessToken.Roles.Count - 2)
                    {
                        roles += ", and ";
                    }
                    else if (i == _accessToken.Roles.Count - 1)
                    {
                        roles += ".";
                    }
                }
                message = "Welcome, " + _accessToken.GivenName +
                    ". You are logged in as " + roles;
                txbGreeting.Text = message;

                statMessage.Content = "Logged in. Please log out before you leave.";

                btnLoginLogout.Content = "Log Out";
                txtEmail.Text = "";
                pwdPassword.Password = "";
                txtEmail.IsEnabled = false;
                pwdPassword.IsEnabled = false;
                txtEmail.Visibility = Visibility.Hidden;
                pwdPassword.Visibility = Visibility.Hidden;
                lblEmail.Visibility = Visibility.Hidden;
                lblPassword.Visibility = Visibility.Hidden;
                btnLoginLogout.IsDefault = false;

                if (password == "newuser")
                {
                    var updatePassword = new frmUpdatePassword(_accessToken, userManager, isNew: true);
                    if (updatePassword.ShowDialog() == false)
                    {
                        logoutUser();
                        MessageBox.Show("You must change your password to continue");
                        return;
                    }
                }
                showUserTabs();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "\n\n" + ex.InnerException.Message;
                }
                MessageBox.Show(message, "Login Failed");
            }

            txtEmail.Text = "";
            pwdPassword.Password = "";
            txtEmail.Focus();
        }

        private void hideAllTabs()
        {
            tabContainer.Visibility = Visibility.Hidden;
            foreach (var item in tabSetMain.Items)
            {
                if (item is TabItem tabItem)
                {
                    tabItem.Visibility = Visibility.Collapsed; 
                }
            }
        }

        private void showAllTabs()
        {
            tabContainer.Visibility = Visibility.Visible;
            foreach (var item in tabSetMain.Items)
            {
                if (item is TabItem tabItem)
                {
                    tabItem.Visibility = Visibility.Visible;  
                }
            }
        }

        private void showUserTabs()
        {
            
            showAllTabs();
            if (_accessToken.Roles.Contains("admin", StringComparer.OrdinalIgnoreCase))
            {
                _storedApplicationTab = tabApplication;
                tabSetMain.Items.Remove(tabApplication);
                if (_storedAdminTab != null && !tabSetMain.Items.Contains(_storedAdminTab))
                {
                    tabSetMain.Items.Add(_storedAdminTab);
                }
            }
            else
            {
                if (_storedApplicationTab != null && !tabSetMain.Items.Contains(_storedApplicationTab))
                {
                    tabSetMain.Items.Add(_storedApplicationTab);
                }
                _storedAdminTab = tabAdmin;
                tabSetMain.Items.Remove(tabAdmin);
            }

            tabSetMain.SelectedItem = tabHome;
        }

        private void logoutUser()
        {
            _accessToken = null;

            statMessage.Content = "You are not logged in. Please log in to continue.";

            btnLoginLogout.Content = "Log In";
            txtEmail.Text = "";
            pwdPassword.Password = "";
            txtEmail.IsEnabled = true;
            pwdPassword.IsEnabled = true;
            txtEmail.Visibility = Visibility.Visible;
            pwdPassword.Visibility = Visibility.Visible;
            lblEmail.Visibility = Visibility.Visible;
            lblPassword.Visibility = Visibility.Visible;
            
            hideAllTabs();

            txtEmail.Focus();
            btnLoginLogout.IsDefault = true;
            txbGreeting.Text = "You are not logged in.";
            return;


        }

        private void mnuUpdatePassword_Click(object sender, RoutedEventArgs e)
        {
            if (_accessToken == null)
            {
                MessageBox.Show("You must be logged in to change your password");
                return;
            }

            var updateWindow = new frmUpdatePassword(_accessToken, new UserManager());
            bool? result = updateWindow.ShowDialog();
            if (result == true)
            {
                MessageBox.Show("Password Updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Password was not updated!", "Update Failed", MessageBoxButton.OK,
                       MessageBoxImage.Exclamation);
            }
        }


        //Father dog
        FatherDog _fatherDog = null;
        private void btnParHarold_Click(object sender, RoutedEventArgs e)
        {
            txbBrdAbout.Visibility = Visibility.Visible;
            string _fatherDogID = "Harold";
            var fatherDogManager = new FatherDogManager();
            _fatherDog = fatherDogManager.SelectFatherDogByFatherDogID(_fatherDogID);
            if (_fatherDog != null)
            {
                txbParAbout.Text = _fatherDog.Description;
                lblParImage.Content = "About " + _fatherDog.FatherDogID;
                lblParList1.Content = "Personality: " +  _fatherDog.Personality;
                lblParList2.Content = "Energy Level: " + _fatherDog.EnergyLevel;
                lblParList3.Content = "Barking Level: " +_fatherDog.BarkingLevel;
                lblParList4.Content = "Trainability: " + _fatherDog.Trainability;

                Uri imageUri = new Uri("Photos/" + _fatherDog.Image, UriKind.Relative);

                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = imageUri;
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.EndInit();
                imgParImage.Source = img;
            }

        }



        // Mother Dog
        MotherDog _motherDog = null;

        private void btnParClemmy_Click(object sender, RoutedEventArgs e)
        {
            txbBrdAbout.Visibility = Visibility.Visible;
            string _motherDogID = "Clemmy";
            var motherDogManager = new MotherDogManager();
            _motherDog = motherDogManager.SelectMotherDogByMotherDogID(_motherDogID);
            if (_motherDog != null)
            {
                txbParAbout.Text = _motherDog.Description;
                lblParImage.Content = "About " + _motherDog.MotherDogID;
                lblParList1.Content = "Personality: " + _motherDog.Personality;
                lblParList2.Content = "Energy Level: " + _motherDog.EnergyLevel;
                lblParList3.Content = "Barking Level: " + _motherDog.BarkingLevel;
                lblParList4.Content = "Trainability: " + _motherDog.Trainability;

                Uri imageUri = new Uri("Photos/" + _motherDog.Image, UriKind.Relative);

                BitmapImage img = new BitmapImage();
                // The BeginInit() makes the page wait until the image loads to show up
                img.BeginInit();
                img.UriSource = imageUri;
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.EndInit();
                imgParImage.Source = img;
            }

        }

        private void btnParRosie_Click(object sender, RoutedEventArgs e)
        {
            txbBrdAbout.Visibility = Visibility.Visible;
            string _motherDogID = "Rosie";
            var motherDogManager = new MotherDogManager();
            _motherDog = motherDogManager.SelectMotherDogByMotherDogID(_motherDogID);
            if (_motherDog != null)
            {
                txbParAbout.Text = _motherDog.Description;
                lblParImage.Content = "About " + _motherDog.MotherDogID;
                lblParList1.Content = "Personality: " + _motherDog.Personality;
                lblParList2.Content = "Energy Level: " + _motherDog.EnergyLevel;
                lblParList3.Content = "Barking Level: " + _motherDog.BarkingLevel;
                lblParList4.Content = "Trainability: " + _motherDog.Trainability;

                Uri imageUri = new Uri("Photos/" + _motherDog.Image, UriKind.Relative);

                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = imageUri;
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.EndInit();
                imgParImage.Source = img;
            }

        }

        private void btnParMaya_Click(object sender, RoutedEventArgs e)
        {
            txbBrdAbout.Visibility = Visibility.Visible;
            string _motherDogID = "Mya";
            var motherDogManager = new MotherDogManager();
            _motherDog = motherDogManager.SelectMotherDogByMotherDogID(_motherDogID);
            if (_motherDog != null)
            {
                txbParAbout.Text = _motherDog.Description;
                lblParImage.Content = "About " + _motherDog.MotherDogID;
                lblParList1.Content = "Personality: " + _motherDog.Personality;
                lblParList2.Content = "Energy Level: " + _motherDog.EnergyLevel;
                lblParList3.Content = "Barking Level: " + _motherDog.BarkingLevel;
                lblParList4.Content = "Trainability: " + _motherDog.Trainability;

                Uri imageUri = new Uri("Photos/" + _motherDog.Image, UriKind.Relative);

                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = imageUri;
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.EndInit();
                imgParImage.Source = img;
            }

        }


        private void tabParentDogs_Loaded(object sender, RoutedEventArgs e)
        {
            btnParClemmy_Click(sender, e);
        }

        // breed stuff

        Breed _breed = null;

        private void btnBrdAussie_Click(object sender, RoutedEventArgs e)
        {
            txbBrdAbout.Visibility = Visibility.Visible;
            string _breedID = "Mini Aussiedoodle";
            var breedManager = new BreedManager();
            _breed = breedManager.SelectBreedByBreedID(_breedID);
            if (_breed != null)
            {
                txbBrdAbout.Text = _breed.Description;
                lblBrdImage.Content = "About " + _breedID + "s";
                lblBrdList1.Content = "Hypoallergenic: " + _breed.Hypoallergenic;
                lblBrdList2.Content = "Size: " + _breed.Size;
                lblBrdList3.Content = "LifeExpectancy: " + _breed.LifeExpectancy;
                lblBrdList4.Content = "Good with Other Dogs: " + _breed.GoodDogs;
                lblBrdList5.Content = "Good with Children: " + _breed.GoodKids;

                Uri imageUri = new Uri("Photos/" + _breed.Image, UriKind.Relative);

                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = imageUri;
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.EndInit();
                imgBrdImage.Source = img;
            }

        }

        private void btnBrdGolden_Click(object sender, RoutedEventArgs e)
        {
            txbBrdAbout.Visibility = Visibility.Visible;
            string _breedID = "Mini Goldendoodle";
            var breedManager = new BreedManager();
            _breed = breedManager.SelectBreedByBreedID(_breedID);
            if (_breed != null)
            {
                txbBrdAbout.Text = _breed.Description;
                lblBrdImage.Content = "About " + _breedID + "s";
                lblBrdList1.Content = "Hypoallergenic: " + _breed.Hypoallergenic;
                lblBrdList2.Content = "Size: " + _breed.Size;
                lblBrdList3.Content = "LifeExpectancy: " + _breed.LifeExpectancy;
                lblBrdList4.Content = "Good with Other Dogs: " + _breed.GoodDogs;
                lblBrdList5.Content = "Good with Children: " + _breed.GoodKids;

                Uri imageUri = new Uri("Photos/" + _breed.Image, UriKind.Relative);

                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = imageUri;
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.EndInit();
                imgBrdImage.Source = img;
            }

        }

        private void btnBrdCockapoo_Click(object sender, RoutedEventArgs e)
        {
            txbBrdAbout.Visibility = Visibility.Visible;
            string _breedID = "Cockapoo";
            var breedManager = new BreedManager();
            _breed = breedManager.SelectBreedByBreedID(_breedID);
            if (_breed != null)
            {
                txbBrdAbout.Text = _breed.Description;
                lblBrdImage.Content = "About " + _breedID + "s";
                lblBrdList1.Content = "Hypoallergenic: " + _breed.Hypoallergenic;
                lblBrdList2.Content = "Size: " + _breed.Size;
                lblBrdList3.Content = "LifeExpectancy: " + _breed.LifeExpectancy;
                lblBrdList4.Content = "Good with Other Dogs: " + _breed.GoodDogs;
                lblBrdList5.Content = "Good with Children: " + _breed.GoodKids;

                Uri imageUri = new Uri("Photos/" + _breed.Image, UriKind.Relative);

                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = imageUri;
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.EndInit();
                imgBrdImage.Source = img;
            }

        }

        private void tabBreeds_Loaded(object sender, RoutedEventArgs e)
        {
            btnBrdAussie_Click(sender, e);
        }


        // Testimonials
       Testimonial _test = null;
        private void btnTestAussie_Click(object sender, RoutedEventArgs e)
        {
            txbBrdAbout.Visibility = Visibility.Visible;
            string _testimonialID = "Wacker Family Testimonial";
            var testimonialManager = new TestimonialManager();
            _test = testimonialManager.SelectTestimonialByTestimonialID(_testimonialID);
            if (_test != null)
            {
                txbTestImage.Text = _test.TestimonialID;
                txbTestAbout.Text = _test.Details;

                Uri imageUri = new Uri("Photos/" + _test.Image, UriKind.Relative);

                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = imageUri;
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.EndInit();
                imgTestImage.Source = img;
            }

        }
        private void btnTestGolden_Click(object sender, RoutedEventArgs e)
        {
            txbBrdAbout.Visibility = Visibility.Visible;
            string _testimonialID = "Meyer Family Testimonial";
            var testimonialManager = new TestimonialManager();
            _test = testimonialManager.SelectTestimonialByTestimonialID(_testimonialID);
            if (_test != null)
            {
                txbTestImage.Text = _test.TestimonialID;
                txbTestAbout.Text = _test.Details;

                Uri imageUri = new Uri("Photos/" + _test.Image, UriKind.Relative);

                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = imageUri;
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.EndInit();
                imgTestImage.Source = img;
            }

        }

        private void btnTestCockapoo_Click(object sender, RoutedEventArgs e)
        {
            txbBrdAbout.Visibility = Visibility.Visible;
            string _testimonialID = "Sanner Family Testimonial";
            var testimonialManager = new TestimonialManager();
            _test = testimonialManager.SelectTestimonialByTestimonialID(_testimonialID);
            if (_test != null)
            {
                txbTestImage.Text = _test.TestimonialID;
                txbTestAbout.Text = _test.Details;

                Uri imageUri = new Uri("Photos/" + _test.Image, UriKind.Relative);

                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = imageUri;
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.EndInit();
                imgTestImage.Source = img;
            }

        }

        private void tabTestimonials_Loaded(object sender, RoutedEventArgs e)
        {
            btnTestAussie_Click(sender, e);
        }


        // Litters
        Litter _lit = null;
        MotherDog _mDog = null;
        FatherDog _fDog = null;
        private void btnLitAussie_Click(object sender, RoutedEventArgs e)
        {
            txbLitAbout.Visibility = Visibility.Visible;
            string _litterID = "AussieLit2";
            string _motherDogID = "Clemmy";
            string _fatherDogID = "Harold";

            var litterManager = new LitterManager();
            var motherDogManager = new MotherDogManager();
            var fatherDogManager = new FatherDogManager();

            _lit = litterManager.SelectLitterByLitterID(_litterID);
            _mDog = motherDogManager.SelectMotherDogByMotherDogID(_motherDogID);
            _fDog = fatherDogManager.SelectFatherDogByFatherDogID(_fatherDogID);
            if (_lit != null && _mDog != null && _fDog != null)
            {
               txbLitName.Text = _mDog.MotherDogID + " and " + _fDog.FatherDogID + "'s " + "Aussiedoodle litter";
                txbLitAbout.Text = "This aussie litter of " + _lit.NumberPuppies + " puppies was born on " + _lit.DateOfBirth.ToString("MM-dd-yyyy") + " and they will go home on " + _lit.GoHomeDate.ToString("MM-dd-yyyy");

                Uri litterImageUri = new Uri("Photos/" + _lit.Image, UriKind.Relative);

                BitmapImage litterImg = new BitmapImage();
                litterImg.BeginInit();
                litterImg.UriSource = litterImageUri;
                litterImg.CacheOption = BitmapCacheOption.OnLoad;
                litterImg.EndInit();
                imgLitAll.Source = litterImg;

                Uri momImageUri = new Uri("Photos/" + _mDog.Image, UriKind.Relative);

                BitmapImage momImg = new BitmapImage();
                momImg.BeginInit();
                momImg.UriSource = momImageUri;
                momImg.CacheOption = BitmapCacheOption.OnLoad;
                momImg.EndInit();
                imgLitMom.Source = momImg;

                Uri dadImageUri = new Uri("Photos/" + _fDog.Image, UriKind.Relative);

                BitmapImage dadImg = new BitmapImage();
                dadImg.BeginInit();
                dadImg.UriSource = dadImageUri;
                dadImg.CacheOption = BitmapCacheOption.OnLoad;
                dadImg.EndInit();
                imgLitDad.Source = dadImg;
            }

        }

        private void btnLitGolden_Click(object sender, RoutedEventArgs e)
        {
            txbLitAbout.Visibility = Visibility.Visible;
            string _litterID = "GoldenLit2";
            string _motherDogID = "Mya";
            string _fatherDogID = "Harold";

            var litterManager = new LitterManager();
            var motherDogManager = new MotherDogManager();
            var fatherDogManager = new FatherDogManager();

            _lit = litterManager.SelectLitterByLitterID(_litterID);
            _mDog = motherDogManager.SelectMotherDogByMotherDogID(_motherDogID);
            _fDog = fatherDogManager.SelectFatherDogByFatherDogID(_fatherDogID);
            if (_lit != null && _mDog != null && _fDog != null)
            {
                txbLitName.Text = _mDog.MotherDogID + " and " + _fDog.FatherDogID + "'s " + "Goldendoodle litter";
                txbLitAbout.Text = "This aussie litter of " + _lit.NumberPuppies + " puppies was born on " + _lit.DateOfBirth.ToString("MM-dd-yyyy") + " and they will go home on " + _lit.GoHomeDate.ToString("MM-dd-yyyy");

                Uri litterImageUri = new Uri("Photos/" + _lit.Image, UriKind.Relative);

                BitmapImage litterImg = new BitmapImage();
                litterImg.BeginInit();
                litterImg.UriSource = litterImageUri;
                litterImg.CacheOption = BitmapCacheOption.OnLoad;
                litterImg.EndInit();
                imgLitAll.Source = litterImg;

                Uri momImageUri = new Uri("Photos/" + _mDog.Image, UriKind.Relative);

                BitmapImage momImg = new BitmapImage();
                momImg.BeginInit();
                momImg.UriSource = momImageUri;
                momImg.CacheOption = BitmapCacheOption.OnLoad;
                momImg.EndInit();
                imgLitMom.Source = momImg;

                Uri dadImageUri = new Uri("Photos/" + _fDog.Image, UriKind.Relative);

                BitmapImage dadImg = new BitmapImage();
                dadImg.BeginInit();
                dadImg.UriSource = dadImageUri;
                dadImg.CacheOption = BitmapCacheOption.OnLoad;
                dadImg.EndInit();
                imgLitDad.Source = dadImg;
            }
        }

        private void btnLitCockapoo_Click(object sender, RoutedEventArgs e)
        {
            txbLitAbout.Visibility = Visibility.Visible;
            string _litterID = "CockerLit2";
            string _motherDogID = "Rosie";
            string _fatherDogID = "Harold";

            var litterManager = new LitterManager();
            var motherDogManager = new MotherDogManager();
            var fatherDogManager = new FatherDogManager();

            _lit = litterManager.SelectLitterByLitterID(_litterID);
            _mDog = motherDogManager.SelectMotherDogByMotherDogID(_motherDogID);
            _fDog = fatherDogManager.SelectFatherDogByFatherDogID(_fatherDogID);
            if (_lit != null && _mDog != null && _fDog != null)
            {
                txbLitName.Text = _mDog.MotherDogID + " and " + _fDog.FatherDogID + "'s " + "Cockapoo litter";
                txbLitAbout.Text = "This aussie litter of " + _lit.NumberPuppies + " puppies was born on " + _lit.DateOfBirth.ToString("MM-dd-yyyy") + " and they will go home on " + _lit.GoHomeDate.ToString("MM-dd-yyyy");

                Uri litterImageUri = new Uri("Photos/" + _lit.Image, UriKind.Relative);

                BitmapImage litterImg = new BitmapImage();
                litterImg.BeginInit();
                litterImg.UriSource = litterImageUri;
                litterImg.CacheOption = BitmapCacheOption.OnLoad;
                litterImg.EndInit();
                imgLitAll.Source = litterImg;

                Uri momImageUri = new Uri("Photos/" + _mDog.Image, UriKind.Relative);

                BitmapImage momImg = new BitmapImage();
                momImg.BeginInit();
                momImg.UriSource = momImageUri;
                momImg.CacheOption = BitmapCacheOption.OnLoad;
                momImg.EndInit();
                imgLitMom.Source = momImg;

                Uri dadImageUri = new Uri("Photos/" + _fDog.Image, UriKind.Relative);

                BitmapImage dadImg = new BitmapImage();
                dadImg.BeginInit();
                dadImg.UriSource = dadImageUri;
                dadImg.CacheOption = BitmapCacheOption.OnLoad;
                dadImg.EndInit();
                imgLitDad.Source = dadImg;
            }
        }

        private void tabLitters_Loaded(object sender, RoutedEventArgs e)
        {
            btnLitAussie_Click(sender, e);
        }



        //Puppies

        List<Puppy> _puppies = null;
        string _litterID = "";
        private void btnPupAussie_Click(object sender, RoutedEventArgs e)
        {
            btnPup5.Visibility = Visibility.Hidden;
            txbPup5.Visibility = Visibility.Hidden;
            txbPupGen5.Visibility = Visibility.Hidden;
            imgPup5.Visibility = Visibility.Hidden;

            _litterID = "AussieLit2";
            var puppyManager = new PuppyManager();
            _puppies = puppyManager.SelectPuppiesByLitterID(_litterID);
            lblPupName.Content = "Harold and Mya's Aussiedoodle Puppies";

            if (_puppies != null)
            {
                int index = 0;
                foreach (Puppy pup in _puppies) 
                { 
                    string adopted = "";
                    if(pup.Adopted == true)
                    {
                        adopted = "Sold";
                    }
                    else if (pup.Adopted == false)
                    {
                         adopted = "Available";
                    }
                    // I found the this.FindName code online after looking up how to add numbers to my label names
                    TextBlock txbPupGen = this.FindName($"txbPupGen{index + 1}") as TextBlock;
                    txbPupGen.Text = pup.Gender;

                    TextBlock txbPup = this.FindName($"txbPup{index + 1}") as TextBlock;
                    txbPup.Text = adopted;


                    Image imgPup = this.FindName($"imgPup{index + 1}") as Image;
                    Uri imageUri = new Uri("Photos/" + pup.Image, UriKind.Relative);

                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = imageUri;
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.EndInit();
                    imgPup.Source = img;

                    index++;
                }
            }
        }

        private void btnPupGolden_Click(object sender, RoutedEventArgs e)
        {
            txbPup5.Visibility = Visibility.Visible;
            txbPupGen5.Visibility = Visibility.Visible;
            imgPup5.Visibility = Visibility.Visible;
            btnPup5.Visibility = Visibility.Visible;

            _litterID = "GoldenLit2";
            var puppyManager = new PuppyManager();
            _puppies = puppyManager.SelectPuppiesByLitterID(_litterID);
            lblPupName.Content = "Harold and Rosie's Goldendoodle Puppies";

            if (_puppies != null)
            {
                int index = 0;
                foreach (Puppy pup in _puppies)
                {
                    string adopted = "";
                    if (pup.Adopted == true)
                    {
                        adopted = "Sold";
                    }
                    else if (pup.Adopted == false)
                    {
                        adopted = "Available";
                    }
                    // I found the this.FindName code online after looking up how to add numbers to my label names
                    TextBlock txbPupGen = this.FindName($"txbPupGen{index + 1}") as TextBlock;
                    txbPupGen.Text = pup.Gender;

                    TextBlock txbPup = this.FindName($"txbPup{index + 1}") as TextBlock;
                    txbPup.Text = adopted;


                    Image imgPup = this.FindName($"imgPup{index + 1}") as Image;
                    Uri imageUri = new Uri("Photos/" + pup.Image, UriKind.Relative);

                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = imageUri;
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.EndInit();
                    imgPup.Source = img;

                    index++;
                }
            }
        }

        private void btnPupCockapoo_Click(object sender, RoutedEventArgs e)
        {
            txbPup5.Visibility = Visibility.Visible;
            txbPupGen5.Visibility = Visibility.Visible;
            imgPup5.Visibility = Visibility.Visible;
            btnPup5.Visibility = Visibility.Visible;
            _litterID = "CockerLit2";
            var puppyManager = new PuppyManager();
            _puppies = puppyManager.SelectPuppiesByLitterID(_litterID);
            lblPupName.Content = "Harold and Clemmy's Cockapoo Puppies";

            if (_puppies != null)
            {
                int index = 0;
                foreach (Puppy pup in _puppies)
                {
                    string adopted = "";
                    if (pup.Adopted == true)
                    {
                        adopted = "Sold";
                    }
                    else if (pup.Adopted == false)
                    {
                        adopted = "Available";
                    }
                    // I found the this.FindName code online after looking up how to add numbers to my label names
                    TextBlock txbPupGen = this.FindName($"txbPupGen{index + 1}") as TextBlock;
                    txbPupGen.Text = pup.Gender;

                    TextBlock txbPup = this.FindName($"txbPup{index + 1}") as TextBlock;
                    txbPup.Text = adopted;


                    Image imgPup = this.FindName($"imgPup{index + 1}") as Image;
                    Uri imageUri = new Uri("Photos/" + pup.Image, UriKind.Relative);

                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = imageUri;
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.EndInit();
                    imgPup.Source = img;

                    index++;
                }
            }
        }
        private void tabPuppies_Loaded(object sender, RoutedEventArgs e)
        {
            btnPupAussie_Click(sender, e);
        }

        private void btnPup1_Click(object sender, RoutedEventArgs e)
        {
            int number = 0;
            var detailWindow = new frmPuppyDetail(_litterID, number, _accessToken);
            detailWindow.Show();
        }

        private void btnPup2_Click(object sender, RoutedEventArgs e)
        {
            int number = 1;
            var detailWindow = new frmPuppyDetail(_litterID, number, _accessToken);
            detailWindow.Show();
        }

        private void btnPup3_Click(object sender, RoutedEventArgs e)
        {
            int number = 2;
            var detailWindow = new frmPuppyDetail(_litterID, number, _accessToken);
            detailWindow.Show();
        }

        private void btnPup4_Click(object sender, RoutedEventArgs e)
        {
            int number = 3;
            var detailWindow = new frmPuppyDetail(_litterID, number, _accessToken);
            detailWindow.Show();

        }

        private void btnPup5_Click(object sender, RoutedEventArgs e)
        {
            int number = 4;
            var detailWindow = new frmPuppyDetail(_litterID, number, _accessToken);
            detailWindow.Show();

        }

        //Application
        DataDomain.Application _app = null;
        bool completed = false;
        private void btnAppSubmit_Click(object sender, RoutedEventArgs e)
        {
            var userManager = new UserManager();
            var applicationManager = new ApplicationManager();

            if (txtAppFullName.Text.Length < 1 || txtEmail.Text.Length > 50)
            {
                MessageBox.Show("Invalid Name.");
                return;
            }
            if (!txtAppFullName.Text.ToLower().Contains(_accessToken.FamilyName.ToLower()))
                {
                MessageBox.Show("Last name must match user last name.");
                return;
            }

            if (txtAppFullName.Text.Contains("Admin") || txtAppFullName.Text.Contains("System"))
            {
                MessageBox.Show("Admin can not fill out the Puppy Application.");
                return;
            }
            if (txtAppAge.Text.Length < 1)
            {
                MessageBox.Show("You must enter an age.");
                return;
            }

            if (Convert.ToInt32(txtAppAge.Text) < 18)
            {
                MessageBox.Show("Age is too young.");
                return;
            }


            if (cmbAppRenting.Text.Equals("Yes"))
            {
                cmbAppRenting.Text = "True";
            }
            else if (cmbAppRenting.Text.Equals("No"))
            {
                cmbAppRenting.Text = "False";
            }
            if (cmbAppYard.Text.Equals("Yes"))
            {
                cmbAppYard.Text = "True";
            }
            else if (cmbAppYard.Text.Equals("No"))
            {
                cmbAppYard.Text = "False";
            }

            try
            {
                int result = applicationManager.InsertApplication(_accessToken.UserID, txtAppFullName.Text, Convert.ToInt32(txtAppAge.Text), Convert.ToBoolean(cmbAppRenting.Text), Convert.ToBoolean(cmbAppYard.Text), cmbAppBreed.Text, cmbAppGender.Text, cmbAppCommunication.Text, false, txtAppComment.Text);
                if (result > 0)
                {
                    MessageBox.Show("Application Sent!");
                    completed = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //source: https://stackoverflow.com/questions/1268552/how-do-i-get-a-textbox-to-only-accept-numeric-input-in-wpf
        private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            // Use SelectionStart property to find the caret position.
            // Insert the previewed text into the existing text in the textbox.
            var fullText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

            double val;
            // If parsing is successful, set Handled to false
            e.Handled = !double.TryParse(fullText, out val);
        }


        //Admin
        private void btnAdmApplication_Click(object sender, RoutedEventArgs e)
        {
            grdUsers.Visibility = Visibility.Hidden;
            grdAdoptions.Visibility = Visibility.Hidden;
            grdApplications.Visibility = Visibility.Visible;
            grdTestimonials.Visibility = Visibility.Hidden;
            grdPuppies.Visibility = Visibility.Hidden;
            grdLitters.Visibility = Visibility.Hidden;
            List<DataDomain.Application> applications;
            try
            {
                applications = _applicationManager.GetAllApplications();
                grdApplications.ItemsSource = applications;
            }
            catch (Exception ex)
            {
                string message = ex.InnerException == null ?
                    ex.Message : ex.Message + "\n\n" + ex.InnerException.Message;
                MessageBox.Show(message);
            }
        }
        private void btnAdmUser_Click(object sender, RoutedEventArgs e)
        {
            grdUsers.Visibility = Visibility.Visible;
            grdApplications.Visibility = Visibility.Hidden;
            grdAdoptions.Visibility = Visibility.Hidden;
            grdTestimonials.Visibility = Visibility.Hidden;
            grdPuppies.Visibility = Visibility.Hidden;
            grdLitters.Visibility = Visibility.Hidden;
            List<User> users;
            try
            {
                users = _userManager.SelectAllUsers();
                grdUsers.ItemsSource = users;
            }
            catch (Exception ex)
            {
                string message = ex.InnerException == null ?
                    ex.Message : ex.Message + "\n\n" + ex.InnerException.Message;
                MessageBox.Show(message);
            }
        }


        private void btnAdmAdoption_Click(object sender, RoutedEventArgs e)
        {
            grdAdoptions.Visibility = Visibility.Visible;
            grdApplications.Visibility = Visibility.Hidden;
            grdUsers.Visibility = Visibility.Hidden;
            grdTestimonials.Visibility = Visibility.Hidden;
            List<Adoption> adoptions;
            try
            {
                adoptions = _adoptionManager.GetAllAdoptions();
                grdAdoptions.ItemsSource = adoptions;
            }
            catch (Exception ex)
            {
                string message = ex.InnerException == null ?
                    ex.Message : ex.Message + "\n\n" + ex.InnerException.Message;
                MessageBox.Show(message);
            }
        }
        private void btnAdmTestimonial_Click(object sender, RoutedEventArgs e)
        {
            grdAdoptions.Visibility = Visibility.Hidden;
            grdApplications.Visibility = Visibility.Hidden;
            grdUsers.Visibility = Visibility.Hidden;
            grdTestimonials.Visibility = Visibility.Visible;
            grdPuppies.Visibility = Visibility.Hidden;
            grdLitters.Visibility = Visibility.Hidden;
            List<Testimonial> testimonials;
            try
            {
                testimonials = _testimonialManager.GetAllTestimonials();
                grdTestimonials.ItemsSource = testimonials;
            }
            catch (Exception ex)
            {
                string message = ex.InnerException == null ?
                    ex.Message : ex.Message + "\n\n" + ex.InnerException.Message;
                MessageBox.Show(message);
            }
        }



        private void btnAdmLitter_Click(object sender, RoutedEventArgs e)
        {
            grdAdoptions.Visibility = Visibility.Hidden;
            grdApplications.Visibility = Visibility.Hidden;
            grdUsers.Visibility = Visibility.Hidden;
            grdPuppies.Visibility = Visibility.Hidden;
            grdLitters.Visibility = Visibility.Visible;
            List<Litter> litters;
            try
            {
                litters = _litterManager.GetAllLitters();
                grdLitters.ItemsSource = litters;
            }
            catch (Exception ex)
            {
                string message = ex.InnerException == null ?
                    ex.Message : ex.Message + "\n\n" + ex.InnerException.Message;
                MessageBox.Show(message);
            }
        }

        private void btnAdmPuppy_Click(object sender, RoutedEventArgs e)
        {
            grdAdoptions.Visibility = Visibility.Hidden;
            grdApplications.Visibility = Visibility.Hidden;
            grdUsers.Visibility = Visibility.Hidden;
            grdPuppies.Visibility = Visibility.Visible;
            grdLitters.Visibility = Visibility.Hidden ;
            List<Puppy> puppies;
            try
            {
               puppies = _puppyManager.GetAllPuppies();
                grdPuppies.ItemsSource = puppies;
            }
            catch (Exception ex)
            {
                string message = ex.InnerException == null ?
                    ex.Message : ex.Message + "\n\n" + ex.InnerException.Message;
                MessageBox.Show(message);
            }

        }

        ApplicationManager _applicationManager = new ApplicationManager();
        UserManager _userManager = new UserManager();
        AdoptionManager _adoptionManager = new AdoptionManager();
        TestimonialManager _testimonialManager = new TestimonialManager();
        LitterManager _litterManager = new LitterManager();
        PuppyManager _puppyManager = new PuppyManager();


        private void tabAdmin_Loaded(object sender, RoutedEventArgs e)
        {
            btnAdmApplication_Click(sender, e);
        }
        private void grdApplications_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            var userManager = new UserManager();
            List<User> userList = userManager.SelectAllUsers();
            var selectedApplication = grdApplications.SelectedItem as DataDomain.Application;
            var selectedUser = userList.FirstOrDefault(user => user.UserID == selectedApplication.UserID);
            if(selectedApplication.Status == false)
            {
                var popUpWindow = new frmPopUp(selectedApplication);  
                popUpWindow.ShowDialog(); 
            }
            else if (selectedApplication != null)
            {
                MessageBox.Show("You selected Application Number: " + selectedApplication.ApplicationID + ". It is approved.");
            }
            return;
        }

        private void grdUsers_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            var selectedUser = grdUsers.SelectedItem as User;
            if (selectedUser != null)
            {
                MessageBox.Show("You selected User Number: " + selectedUser.UserID);
            }
            return;
        }

        private void grdAdoptions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var adoptionManager = new AdoptionManager();
            var selectedAdoption = grdAdoptions.SelectedItem as Adoption;
            if (selectedAdoption.Status != "Adopted")
            {
                var popUpWindow = new frmUpdateAdoption(selectedAdoption);
                popUpWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("You selected Adoption Number: " + selectedAdoption.AdoptionID);
            }
            return;
        }

        private void grdTestimonials_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var testimonialManager = new TestimonialManager();
            var selectedTestimonial = grdTestimonials.SelectedItem as Testimonial;
            if (selectedTestimonial != null)
            {
                MessageBox.Show("You selected Testimonial: " + selectedTestimonial.TestimonialID);
            }
            return;

        }

        private void grdPuppies_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var puppyManager = new PuppyManager();
            var selectedPuppy = grdPuppies.SelectedItem as Puppy;
            if (selectedPuppy != null)
            {
                MessageBox.Show("You selected Puppy: " + selectedPuppy.PuppyID);
            }
            return;
        }

        private void grdLitters_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var litterManager = new LitterManager();
            var selectedLitter = grdLitters.SelectedItem as Litter;
            if (selectedLitter != null)
            {
                MessageBox.Show("You selected Litter: " + selectedLitter.LitterID);
            }
            return;
        }
    }

}