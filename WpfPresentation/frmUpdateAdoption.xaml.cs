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
    public partial class frmUpdateAdoption : Window
    {
        public Adoption selectedAdoption { get; set; }

        // Constructor accepts the selected application
        public frmUpdateAdoption(Adoption adoption)
        {
            InitializeComponent();
            selectedAdoption = adoption;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var adoptionManager = new AdoptionManager();

            cmbStatus.Text = selectedAdoption.Status;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            var adoptionManager = new AdoptionManager();
            try
            {
                if (adoptionManager.UpdateAdoption(selectedAdoption.AdoptionID, "In Progress", cmbStatus.Text) > 0)
                {
                    MessageBox.Show("Adoption Status Updated!");
                    this.Close();
                    return;
                }
                else
                {
                    MessageBox.Show("Update Failed");
                    this.Close();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
    }

}
