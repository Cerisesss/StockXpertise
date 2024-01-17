using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockXpertise
{
    public class Fournisseur
    {
        int id;
        string nom;
        string prenom;
        int num;
        string mail;
        string adresse;

        public Fournisseur(string nom, string prenom) : this(0, nom, prenom, 0, "", "")
        {
        }

        public Fournisseur(int id, string nom, string prenom, int num, string mail, string adresse)
        {
            this.id = id;
            this.nom = nom;
            this.prenom = prenom;
            this.num = num;
            this.mail = mail;
            this.adresse = adresse;
        }

        public int getId() { return id; }
        public string getNom() { return nom; }
        public string getPrenom() {  return prenom; }
        public int getNum() { return num; }
        public string getMail() { return mail; }
        public string getAdresse() {  return adresse; }

        public override string ToString()
        {
            return $"{nom} {prenom}";
        }

    }
}
