using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockXpertise
{
    public class UserConnected
    {
        private int id_employee { get; set; }
        private string nom { get; set; }
        private string prenom { get; set; }
        private string mot_de_passe { get; set; }
        private string mail { get; set; }
        private string role { get; set; }

        public UserConnected(int id_employee, string nom, string prenom, string mot_de_passe, string mail, string role)
        {
            this.id_employee = id_employee;
            this.nom = nom;
            this.prenom = prenom;
            this.mot_de_passe = mot_de_passe;
            this.mail = mail;
            this.role = role;
        }

        public int GetIdEmployee()
        {
            return this.id_employee;
        }

        public string GetNom()
        {
            return this.nom;
        }

        public string GetPrenom()
        {
              return this.prenom;
        }

        public string GetMot_de_passe()
        {
            return this.mot_de_passe;
        }

        public string GetMail()
        {
              return this.mail;
        }

        public string GetRole()
        {
              return this.role;
        }

        public void SetIdEmployee(int id_employee)
        {
            this.id_employee = id_employee;
        }

        public void SetNom(string nom)
        {
            this.nom = nom;
        }

        public void SetPrenom(string prenom)
        {
            this.prenom = prenom;
        }

        public void SetMot_de_passe(string mot_de_passe)
        {
            this.mot_de_passe = mot_de_passe;
        }

        public void SetMail(string mail)
        {
            this.mail = mail;
        }

        public void SetRole(string role)
        {
            this.role = role;
        }

    }
}
