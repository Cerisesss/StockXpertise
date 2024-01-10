﻿using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StockXpertise.User
{
    /// <summary>
    /// Logique d'interaction pour Modify_User.xaml
    /// </summary>
    public partial class Modify_User : Page
    {
        int id;
        string nom;
        string prenom;
        string password;
        string mail;
        string role;

        public Modify_User()
        {
            InitializeComponent();

            //affectation aux labels les données recuperés de la ligne selectionnée dans le datagrid avec les variables "globales"
            labelNom.Content = Application.Current.Properties["NomDataGrid"].ToString();
            labelPrenom.Content = Application.Current.Properties["PrenomDataGrid"].ToString();
            labelMdp.Content = Application.Current.Properties["MdpDataGrid"].ToString();
            labelMail.Content = Application.Current.Properties["MailDataGrid"].ToString();
            labelRole.Content = Application.Current.Properties["RoleDataGrid"].ToString();

            //affectation aux variables ci dessus les données recuperés de la ligne selectionnée
            id = Convert.ToInt32(Application.Current.Properties["IdDataGrid"].ToString());
            nom = Application.Current.Properties["NomDataGrid"].ToString();
            prenom = Application.Current.Properties["PrenomDataGrid"].ToString();
            password = Application.Current.Properties["MdpDataGrid"].ToString();
            password = Application.Current.Properties["MdpDataGrid"].ToString();
            mail = Application.Current.Properties["MailDataGrid"].ToString();
            role = Application.Current.Properties["RoleDataGrid"].ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //conditions pour verifier si les champs sont vides et si les données sont differentes de celles de la ligne selectionnée
            //si c'est le cas alors on affecte les nouvelles données aux variables
            if ((string)labelNom.Content != nomTextBox.Text && !string.IsNullOrEmpty(nomTextBox.Text))
            {
                nom = nomTextBox.Text;
            }

            if ((string)labelPrenom.Content != prenomTextBox.Text && !string.IsNullOrEmpty(prenomTextBox.Text))
            {
                prenom = prenomTextBox.Text;
            }

            if ((string)labelMdp.Content != mdpTextBox.Text && !string.IsNullOrEmpty(mdpTextBox.Text))
            {
                password = mdpTextBox.Text;
            }

            if ((string)labelMail.Content != mailTextBox.Text && !string.IsNullOrEmpty(mailTextBox.Text))
            {
                mail = mailTextBox.Text;
            }

            if (radioAdmin.IsChecked == true)
            {
                role = radioAdmin.Content.ToString();
            }
            else if (radioPersonnel.IsChecked == true)
            {
                role = radioPersonnel.Content.ToString();
            }
            else if (radioCaissier.IsChecked == true)
            {
                role = radioCaissier.Content.ToString();
            }

            //requete pour modifier les données de la ligne selectionnée
            Query_User query_modify = new Query_User(id, nom, prenom, password, mail, role);
            query_modify.Update_User();

            //redirection vers la page User.xaml
            User user = new User();
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = user;
            }

            //gridModifyUser.Visibility = Visibility.Collapsed;
            //modifyUser.Navigate(new Uri("/User/User.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //redirection vers la page User.xaml
            User user = new User();
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = user;
            }

            //gridModifyUser.Visibility = Visibility.Collapsed;
            //modifyUser.Navigate(new Uri("/User/User.xaml", UriKind.RelativeOrAbsolute));
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Supprimer(object sender, RoutedEventArgs e)
        {
            //requete pour supprimer l'utilisateur selectionné
            Query_User query_delete = new Query_User(id, nom, prenom, password, mail, role);
            query_delete.Delete_User();

            //redirection vers la page User.xaml
            User user = new User();
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = user;
            }
        }
    }
}