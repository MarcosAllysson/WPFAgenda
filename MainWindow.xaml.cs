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
using Microsoft.EntityFrameworkCore;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ListContacts();
            ChangeButtons(1);
        }

        private void ChangeButtons(int option)
        {
            buttonInsert.IsEnabled = false;
            buttonUpdate.IsEnabled = false;
            buttonRemove.IsEnabled = false;
            buttonFind.IsEnabled = false;
            buttonSave.IsEnabled = false;

            // Initial options
            if (option == 1)
            {
                buttonInsert.IsEnabled = true;
                buttonFind.IsEnabled = true;
            }

            // Insert
            else if (option == 2)
            {
                buttonSave.IsEnabled = true;
            }

            // Update and Remove
            else if (option == 3)
            {
                buttonUpdate.IsEnabled = true;
                buttonRemove.IsEnabled = true;
            }
        }

        private void ListContacts()
        {
            using (WpfAgendaDbContext _context = new WpfAgendaDbContext())
            {
                DataGridItem.ItemsSource = _context.Contacts
                    .AsNoTracking()
                    .ToList();

                int contactsQuantity = _context.Contacts.Count();
                labelContactList.Content = $"Contact List ({contactsQuantity})";
            }
        }

        private void buttonInsert_Click(object sender, RoutedEventArgs e)
        {
            this.operation = OperationTag.INSERT;
            ChangeButtons(2);
            CleanFormFields();
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            this.operation = OperationTag.UPDATE;
            ChangeButtons(2);
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            using (WpfAgendaDbContext _context = new WpfAgendaDbContext())
            {
                int userId = Convert.ToInt32(textBoxId.Text);

                Contact contact = _context.Contacts.FirstOrDefault(x => x.Id == userId);

                if (contact != null)
                {
                    _context.Contacts.Remove(contact);
                    _context.SaveChanges();
                }
            }

            CleanFormFields();
            ListContacts();
            ChangeButtons(1);
        }

        private void buttonFind_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(textBoxName.Text))
                {
                    using (WpfAgendaDbContext _context = new WpfAgendaDbContext())
                    {
                        var response = _context.Contacts
                            .AsNoTracking()
                            .Where(x => x.Name.ToLower().Contains(textBoxName.Text.ToLower()))
                            .ToList();

                        DataGridItem.ItemsSource = response;
                    }
                }
                else if (!string.IsNullOrEmpty(textBoxEmail.Text))
                {
                    using (WpfAgendaDbContext _context = new WpfAgendaDbContext())
                    {
                        var response = _context.Contacts
                            .AsNoTracking()
                            .Where(x => x.Email.ToLower().Contains(textBoxEmail.Text.ToLower()))
                            .ToList();

                        DataGridItem.ItemsSource = response;
                    }
                }
                else if (!string.IsNullOrEmpty(textBoxPhone.Text))
                {
                    using (WpfAgendaDbContext _context = new WpfAgendaDbContext())
                    {
                        var response = _context.Contacts
                            .AsNoTracking()
                            .Where(x => x.Phone.ToLower().Contains(textBoxPhone.Text.ToLower()))
                            .ToList();

                        DataGridItem.ItemsSource = response;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userName = textBoxName.Text.Trim();
                string userEmail = textBoxEmail.Text.Trim();
                string userPhone = textBoxPhone.Text.Trim();

                if (this.operation == OperationTag.INSERT)
                {
                    Contact contact = new Contact(userName, userEmail, userPhone);

                    using (WpfAgendaDbContext _context = new WpfAgendaDbContext())
                    {
                        _context.Contacts.Add(contact);
                        _context.SaveChanges();
                    }

                }
                else if(this.operation == OperationTag.UPDATE)
                {
                    int userId = Convert.ToInt32(textBoxId.Text);

                    using (WpfAgendaDbContext _context = new WpfAgendaDbContext())
                    {
                        Contact contact = _context.Contacts.FirstOrDefault(x => x.Id == userId);

                        if(contact != null)
                        {
                            contact.UpdateContact(userName, userEmail, userPhone);
                            _context.SaveChanges();
                        }
                    }
                }

                CleanFormFields();
                ListContacts();
                ChangeButtons(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DataGridItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGridItem.SelectedIndex != -1)
            {
                ChangeButtons(3);

                //Contact contact = (Contact)DataGridItem.SelectedItem;
                Contact contact = (Contact)DataGridItem.Items[DataGridItem.SelectedIndex];
                
                if (contact != null)
                {
                    textBoxId.Text = contact.Id.ToString();
                    textBoxEmail.Text = contact.Email;
                    textBoxPhone.Text = contact.Phone;
                    textBoxName.Text = contact.Name;
                }
            }
        }

        private void CleanFormFields()
        {
            textBoxId.Clear();
            textBoxName.Clear();
            textBoxEmail.Clear();
            textBoxPhone.Clear();
        }
    }
}