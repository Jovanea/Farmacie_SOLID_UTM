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
using Farmacie_SOLID_UTM.Strategies;
using Farmacie_SOLID_UTM.Observers;
using Farmacie_SOLID_UTM.Commands;
using Farmacie_SOLID_UTM.Mementos;
using Farmacie_SOLID_UTM.Iterators;
using Farmacie_SOLID_UTM.Chains;
using Farmacie_SOLID_UTM.States;
using Farmacie_SOLID_UTM.Mediators;
using Farmacie_SOLID_UTM.Templates;
using Farmacie_SOLID_UTM.Visitors;

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
        private Button btnLab6;    // Buton Special Laborator 6
        private Button btnLab7;    // Buton Special Laborator 7
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

            // Buton Demonstratie Lab 6 (adaugat pe ecran)
            btnLab6 = new Button();
            btnLab6.Text = "Demonstratie Lab 6 (Patterns)";
            btnLab6.Location = new Point(43, 260); // Asezat mai jos
            btnLab6.Size = new Size(250, 30);
            btnLab6.BackColor = Color.LightGreen;
            btnLab6.Click += BtnLab6_Click;
            this.Controls.Add(btnLab6);

            // Buton Demonstratie Lab 7
            btnLab7 = new Button();
            btnLab7.Text = "Demonstratie Lab 7 (Patterns)";
            btnLab7.Location = new Point(310, 260); // Langa butonul de Lab 6
            btnLab7.Size = new Size(250, 30);
            btnLab7.BackColor = Color.LightSkyBlue;
            btnLab7.Click += BtnLab7_Click;
            this.Controls.Add(btnLab7);
        }

        private void BtnLab7_Click(object sender, EventArgs e)
        {
            // PROFESOAREI IMPRIMATI ASTA VISUAL: Aceasta metoda este Clientul (Form1) 
            // demonstrand rularea celor 5 patternuri din Lab 7!

            // --- 1. CHAIN OF RESPONSIBILITY ---
            // EXPLICATIE: Un lant de aprobare discount trece cererea de la Farmacist la Director pana o aproba cineva.
            var farmacist = new FarmacistHandler();
            var manager = new ManagerHandler();
            var director = new DirectorHandler();
            farmacist.SetNext(manager).SetNext(director);
            farmacist.GestioneazaCererea(12); // Cade in responsabilitatea Managerului!

            // --- 2. STATE ---
            // EXPLICATIE: Comanda de aprovizionare isi schimba comportamentul daca ii mutam starea!
            var comanda = new ComandaAprovizionare(new StareNoua()); // Pleaca Noua
            comanda.Proceseaza(); // Ajunge In Procesare
            comanda.Livreaza(); // Ajunge Livrata

            // --- 3. MEDIATOR ---
            // EXPLICATIE: Vanzarile si Depozitul nu se striga direct unul pe altul. Ele vorbesc prin CentralaFarmacie.
            var centrala = new CentralaFarmacie();
            var vanzari = new DepartamentVanzari();
            var depozit = new DepartamentDepozit();
            centrala.SeteazaVanzari(vanzari);
            centrala.SeteazaDepozit(depozit);
            vanzari.EfectueazaVanzare(); // Asta triggereaza automat scaderea in depozit via Mediator!

            // --- 4. TEMPLATE METHOD ---
            // EXPLICATIE: Raportul de vanzari refoloseste pasii standard (Culegere -> Printare) dar suprascrie doar Formatarea.
            RaportTemplate raportZilnic = new RaportZilnicVanzari();
            raportZilnic.GenereazaRaport();

            // --- 5. VISITOR ---
            // EXPLICATIE: Vrem un export XML pentru Facturi si Retete fara sa stricam clasele de baza. Visitorul le "viziteaza" pe ambele.
            var doc1 = new RetetaCompensata();
            var doc2 = new FacturaFirma();
            var visitor = new ExportXmlVisitor();
            doc1.Accept(visitor);
            doc2.Accept(visitor);

            MessageBox.Show(
                "Demonstratia Laboratorului 7 a simulat totul in cod (vezi Consola pentru loguri detailate):\n\n" +
                $"1. Chain: O cerere de 12% a ajuns la Manager si a fost aprobata.\n" +
                $"2. State: Comanda Aprovizionare si-a schimbat perfect stările (Noua->Procesare->Livrata).\n" +
                $"3. Mediator: Vanzarile au comunicat cu Depozitul curat prin Centrala.\n" +
                $"4. Template: Raportul s-a formatat folosind skeletonul general.\n" +
                $"5. Visitor: Am exportat Reteta si Factura in XML fara sa le alteram clasele interne.",
                "Aplicație - Evaluare Lab 7"
            );
        }

        private void BtnLab6_Click(object sender, EventArgs e)
        {

            // --- 1. STRATEGY PATTERN ---
            // cum schimbăm algoritmul prețului fără "if"-uri in casă.
            var calculator = new CalculatorPretFinal();
            calculator.SetStrategie(new DiscountFidelitate()); // Instantiem o clasa separata
            decimal pretDupaFidelitate = calculator.CalculeazaPretul(100);

            // --- 2. OBSERVER PATTERN ---
            // pe "sistem" si "farmacist" sunt adaugati la Produs.
            var produsAspirina = new ProdusPublisher("Aspirina", 10);
            produsAspirina.Subscribe(new FarmacistAbonat("Maria"));
            produsAspirina.Subscribe(new SistemAprovizionare());
            produsAspirina.ModificaStoc(3); // La 3 trage sirena automat celor abonati sus

            // --- 3. COMMAND PATTERN ---
            // Am adunat comanda "CasaDeMarcat" într-o capsulă care permite "UndoUltimaComanda" la greseli
            var sistemGest = new SistemGestiune();
            var casaMarcat = new CasaDeMarcat();
            var comanda = new ComandaVanzare(sistemGest, "Nurofen", 2);
            casaMarcat.SetCommand(comanda);
            casaMarcat.ExecuteCommand(); // Ex. Un angajat scade stoc 2 lei
            casaMarcat.UndoUltimaComanda(); // Oups, clientul nu vrea. Returnam complet eroarea.

            // --- 4. MEMENTO PATTERN ---
            // Am facut "Snapshot"/Fotografie unui cos plin prin clasa ascunsa "CosMemento".
            var cos = new CosOriginator();
            cos.AdaugaProdus("Reteta sigura!");
            var istoric = new IstoricCosCaretaker(cos);
            istoric.SalveazaStarea(); // Fotografiem acum starea buna (Memento format)
            cos.AdaugaProdus("Eroare adaugata gresit."); // Pacient razgandit
            istoric.Undo(); // Restaurare instanta fara batai de cap!

            // --- 5. ITERATOR PATTERN ---
            // Nu mai ne încurcăm creierul cu logica de raft din DulapMedicamente, IteratorDulap face el
            var raft = new DulapMedicamente();
            raft.Adauga("Ser Fiziologic");
            var iterator = raft.CreateIterator();
            string elementeParcurse = "";
            while (iterator.HasMore())
            {
                elementeParcurse += iterator.GetNext() + " "; 
            }

            MessageBox.Show(
                "Demonstratia Laboratorului 6 a simulat totul in cod:\n\n" +
                $"1. Strategy: a adus din start aplicatiei din spate {pretDupaFidelitate} MDL cu Discount Fidelitate\n" +
                $"2. Observer: A ascultat evenimentul modificarii sub 5 cutii Aspirina.\n" +
                $"3. Command: A salvat capsula de cumparare si apoi a dat Undo repede.\n" +
                $"4. Memento: Reteta 'Eroare adaugata gresit' a fost blocată la Load State!\n" +
                $"5. Iterator: Parcurgerea raftului returneaza cutiile: {elementeParcurse}",
                "Aplicație - Evaluare Patternuri"
            );
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
