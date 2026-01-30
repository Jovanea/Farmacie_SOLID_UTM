using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Farmacie_SOLID_UTM.Interfaces;
using Farmacie_SOLID_UTM.Models;

namespace Farmacie_SOLID_UTM
{
    public partial class Form1 : Form
    {
        // 1. Adaug o variabilă privată pentru interfață
        private readonly IStocare _stocare;

        // 2. Modific constructorul (metoda cu același nume ca și clasa)
        public Form1(IStocare stocare)
        {
            InitializeComponent();
            _stocare = stocare; // Aici "injectăm" dependența
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnAdauga_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Colectăm datele introduse în interfață
                string nume = txtNume.Text;
                decimal pret = decimal.Parse(txtPret.Text); // Transformăm textul în număr
                string producator = txtProducator.Text;

                // 2. Creăm un obiect nou folosind clasa Medicament din Models
                Medicament nou = new Medicament(nume, pret, producator);

                // 3. Adăugăm datele obiectului direct în tabel (DataGridView)
                // Folosim numele tabelului, care în Properties apare ca dataGridView1
                dataGridView1.Rows.Add(nou.Nume, nou.Pret, nou.Producator);

                // 4. Curățăm căsuțele pentru a fi gata de o nouă introducere
                txtNume.Clear();
                txtPret.Clear();
                txtProducator.Clear();

                txtNume.Focus(); // Punem cursorul înapoi pe prima căsuță
            }
            catch (Exception ex)
            {
                // Afișăm o eroare dacă, de exemplu, am scris litere în câmpul de preț
                MessageBox.Show("Eroare la adăugare: " + ex.Message, "Atenție!");
            }

        }

        private void txtNume_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
