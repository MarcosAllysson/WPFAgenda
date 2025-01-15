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
using WPFAgenda.Models;

namespace WPFAgenda
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string operation;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userName = textBoxName.Text;
                string userEmail = textBoxEmail.Text;
                string userPhone = textBoxPhone.Text;

                Contact contact = new Contact(userName, userEmail, userPhone);

                if (this.operation == OperationTag.INSERT)
                {
                    using (WpfAgendaDbContext _context = new WpfAgendaDbContext())
                    {
                        _context.Contacts.Add(contact);
                        _context.SaveChanges();
                    }
                }
                else if(this.operation == OperationTag.UPDATE)
                {

                }
                else if (this.operation == OperationTag.REMOVE)
                {

                }
                else if (this.operation == OperationTag.FIND)
                {

                }

                CleanFormFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CleanFormFields()
        {
            textBoxName.Text = string.Empty;
            textBoxEmail.Text = string.Empty;
            textBoxPhone.Text = string.Empty;
        }

        private void buttonInsert_Click(object sender, RoutedEventArgs e)
        {
            this.operation = OperationTag.INSERT;
        }
    }
}