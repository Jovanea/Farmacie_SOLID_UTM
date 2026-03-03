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
using Farmacie_SOLID_UTM.Factories;
using Farmacie_SOLID_UTM.Builders;
using Farmacie_SOLID_UTM.Director;
using Farmacie_SOLID_UTM.Services;

namespace Farmacie_SOLID_UTM
{
    public partial class Form1 : Form
    {
        private readonly IStocare _stocare;
        
        // Controale noi pentru Pattern-uri
        private ComboBox cmbTipProdus;
        private Button btnTrusaAdulti;
        private Button btnTrusaCopii;
        private Button btnBuilder; // Builder Pattern (Director)
        private Button btnClone;   // Prototype Pattern
        private Button btnTest;    // Unit Tests
        private Label lblTipProdus;

        public Form1(IStocare stocare)
        {
            InitializeComponent();
            _stocare = stocare;
            
            InitializeCustomControls();
        }

        private void InitializeCustomControls()
        {
            // Label pentru ComboBox
            lblTipProdus = new Label();
            lblTipProdus.Text = "Tip Produs:";
            lblTipProdus.Location = new Point(45, 180);
            lblTipProdus.AutoSize = true;
            this.Controls.Add(lblTipProdus);

            // ComboBox pentru Factory Method
            cmbTipProdus = new ComboBox();
            cmbTipProdus.Location = new Point(127, 177);
            cmbTipProdus.Items.AddRange(new object[] { "Medicament", "Echipament Medical" });
            cmbTipProdus.SelectedIndex = 0; // Default Medicament
            cmbTipProdus.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(cmbTipProdus);

            // Buton Trusa Adulti (Abstract Factory)
            btnTrusaAdulti = new Button();
            btnTrusaAdulti.Text = "Creeaza Trusa Adulti";
            btnTrusaAdulti.Location = new Point(43, 220);
            btnTrusaAdulti.Size = new Size(150, 30);
            btnTrusaAdulti.Click += BtnTrusaAdulti_Click;
            this.Controls.Add(btnTrusaAdulti);

            // Buton Trusa Copii (Abstract Factory)
            btnTrusaCopii = new Button();
            btnTrusaCopii.Text = "Creeaza Trusa Copii";
            btnTrusaCopii.Location = new Point(200, 220); // Put it next to the other one
            btnTrusaCopii.Size = new Size(150, 30);
            btnTrusaCopii.Click += BtnTrusaCopii_Click;
            btnTrusaCopii.Click += BtnTrusaCopii_Click;
            this.Controls.Add(btnTrusaCopii);

            // Buton Builder Pattern (Trusa Personalizata)
            btnBuilder = new Button();
            btnBuilder.Text = "Trusa Personalizata (Builder)";
            btnBuilder.Location = new Point(360, 220); 
            btnBuilder.Size = new Size(180, 30);
            btnBuilder.Click += BtnBuilder_Click;
            this.Controls.Add(btnBuilder);

            // Buton Prototype Pattern (Cloneaza Selectia)
            btnClone = new Button();
            btnClone.Text = "Cloneaza Produs (Prototype)";
            btnClone.Location = new Point(550, 220);
            btnClone.Size = new Size(180, 30);
            btnClone.Click += BtnClone_Click;
            this.Controls.Add(btnClone);

            // Buton Testare
            btnTest = new Button();
            btnTest.Text = "Ruleaza Teste Unitare";
            btnTest.Location = new Point(740, 220); // Far right
            btnTest.Size = new Size(150, 30);
            btnTest.Click += (s, e) => Farmacie_SOLID_UTM.Tests.UnitTests.RuleazaToate();
            this.Controls.Add(btnTest);
        }

        private void BtnTrusaAdulti_Click(object sender, EventArgs e)
        {
            try
            {
                // Abstract Factory: Trusa Adulti
                ITrusaFactory factory = new TrusaAdultiFactory();
                MedicamentDurere med = factory.CreareMedicamentDurere();
                Bandaj bandaj = factory.CreareBandaj();

                AdaugaInGrid(med);
                AdaugaInGrid(bandaj);

                MessageBox.Show("Trusa pentru Adulti a fost creata!", "Succes");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Eroare: " + ex.Message);
            }
        }

        private void BtnTrusaCopii_Click(object sender, EventArgs e)
        {
            try
            {
                // Abstract Factory: Trusa Copii
                ITrusaFactory factory = new TrusaCopiiFactory();
                MedicamentDurere med = factory.CreareMedicamentDurere();
                Bandaj bandaj = factory.CreareBandaj();

                AdaugaInGrid(med);
                AdaugaInGrid(bandaj);

                MessageBox.Show("Trusa pentru Copii a fost creata!", "Succes");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare: " + ex.Message);
            }
        }

        // Builder Pattern EventHandler (Director Implementation)
        private void BtnBuilder_Click(object sender, EventArgs e)
        {
            try
            {
                // Varianta cu Director (Cerința Lab 3)
                TrusaBuilder builder = new TrusaBuilder();
                TrusaDirector director = new TrusaDirector(builder);

                // Construim o trusă standard folosind Directorul
                TrusaMedicala trusa = director.ConstructTrusaVacanta();
                
                MessageBox.Show(trusa.ListeazaContinut(), "Trusa Vacanță (via Director)");

                // Putem demonstra și cealaltă rețetă dacă e nevoie
                // TrusaMedicala trusaAuto = director.ConstructTrusaAuto();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare Builder: " + ex.Message);
            }
        }

        // Prototype Pattern EventHandler
        private void BtnClone_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecteaza un rand pentru a clona!");
                return;
            }

            try
            {
                // Luam ultimul produs adaugat in StocManager ca demo (sau ar trebui sa mapam grid-ul la obiecte)
                var produse = StocManager.Instance.GetProduse();
                if (produse.Count == 0) return;

                Produs original = produse.Last(); 
                Produs clona = original.Cloneaza(); // Deep/Shallow Copy

                AdaugaInGrid(clona);
                MessageBox.Show($"Produs clonat cu succes!\nOriginal: {original.Nume}\nClona: {clona.Nume}", "Prototype Pattern");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare Prototype: " + ex.Message);
            }
        }

        private void AdaugaInGrid(Produs p)
        {
            // Helper pentru adaugare
            if (p is Medicament m)
            {
                dataGridView1.Rows.Add(m.Nume, m.Pret, m.Producator);
            }
            else if (p is EchipamentMedical em)
            {
                dataGridView1.Rows.Add(em.Nume, em.Pret, em.TipEchipament);
            }
            // Singleton Pattern: Adaugam in stocul global
            StocManager.Instance.AdaugaProdus(p);
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
                string nume = txtNume.Text;
                if(string.IsNullOrWhiteSpace(nume)) throw new Exception("Numele este obligatoriu.");
                
                decimal pret = decimal.Parse(txtPret.Text);
                string extra = txtProducator.Text; // Producator sau Tip Echipament

                // Factory Method
                ProdusFactory factory;
                string tipSelectat = cmbTipProdus.SelectedItem.ToString();

                if (tipSelectat == "Medicament")
                {
                    factory = new MedicamentFactory();
                }
                else
                {
                    factory = new EchipamentFactory();
                }

                // Polimorfism: Nu stim exact ce clasa e, dar stim ca e Produs
                Produs nou = factory.CreazaProdus(nume, pret, extra);

                // Adaugare in Grid
                AdaugaInGrid(nou);

                // Curatare UI
                txtNume.Clear();
                txtPret.Clear();
                txtProducator.Clear();
                txtNume.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la adăugare: " + ex.Message, "Atenție!");
            }
        }

        private void txtNume_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
