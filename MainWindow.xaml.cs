﻿using System.Text;
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

            //
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
                labelContactList.Content += $"({contactsQuantity})";
            }
        }

        private void buttonInsert_Click(object sender, RoutedEventArgs e)
        {
            this.operation = OperationTag.INSERT;
            ChangeButtons(2);
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {

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
                ListContacts();
                ChangeButtons(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CleanFormFields()
        {
            textBoxName.Clear();
            textBoxEmail.Clear();
            textBoxPhone.Clear();
        }
    }
}