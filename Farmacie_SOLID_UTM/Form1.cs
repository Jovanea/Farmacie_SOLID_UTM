using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Farmacie_SOLID_UTM.Interfaces;
using Farmacie_SOLID_UTM.Models;
using Farmacie_SOLID_UTM.Factories;
using Farmacie_SOLID_UTM.Builders;
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
        
        // --- Componente UI ---
        private TabControl tabControl;
        private TabPage tabVanzari;
        private TabPage tabAprovizionare;
        private TabPage tabRapoarte;
        
        // UI: Vanzari
        private ComboBox cmbProduseVanzare;
        private ComboBox cmbStrategieDiscount;
        private ListBox lstCos;
        private Button btnAdaugaCos;
        private Button btnUndoCos;
        private Button btnFinalizeazaVanzare;
        private Label lblTotal;

        // UI: Aprovizionare
        private DataGridView dgvStoc;
        private Button btnCereAprobareBuget;
        private Button btnTrusaCopii;
        private Button btnTrusaAdulti;
        private Label lblStareComanda;
        private Button btnAvansStareComanda;
        
        // UI: Rapoarte
        private Button btnRaportPdf;
        private Button btnRaportCsv;
        private Button btnExportXml;

        // --- Instante Patternuri ---
        private CosOriginator _cos;
        private IstoricCosCaretaker _istoric;
        private CentralaFarmacie _mediator;
        private DepartamentVanzari _vanzari;
        private DepartamentDepozit _depozit;
        private CasaDeMarcat _casaMarcat;
        private ComandaAprovizionare _comandaCurenta;
        private decimal _totalCos = 0;

        public Form1(IStocare stocare)
        {
            InitializeComponent();
            _stocare = stocare;
            
            // Eliminam vechiul UI (designerul vechi plin de butoane "Test")
            this.Controls.Clear();
            
            // Instantiere arhitectura pentru comunicare Mediator
            _mediator = new CentralaFarmacie();
            _vanzari = new DepartamentVanzari();
            _depozit = new DepartamentDepozit();
            _mediator.SeteazaVanzari(_vanzari);
            _mediator.SeteazaDepozit(_depozit);

            // Memento + Command pentru Casa de marcat
            _cos = new CosOriginator();
            _istoric = new IstoricCosCaretaker(_cos);
            _casaMarcat = new CasaDeMarcat();
            
            // Initializare date stoc fictiv
            PopuleazaStocInitial();

            // Construire Interfata Generala (Functional)
            ConstruiesteInterfataVizuala();
        }

        private void PopuleazaStocInitial()
        {
            StocManager.Instance.AdaugaProdus(new Medicament("Nurofen Răceală", 35.5m, "Reckitt Benckiser"));
            StocManager.Instance.AdaugaProdus(new Medicament("Aspirină Cardio", 15.0m, "Bayer"));
            StocManager.Instance.AdaugaProdus(new EchipamentMedical("Termometru Digital", 45.0m, "Dispozitiv Masurare"));
            StocManager.Instance.AdaugaProdus(new BandajElastic());
        }

        private void ConstruiesteInterfataVizuala()
        {
            this.Text = "Farmacie Inteligentă - Panou Principal";
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 244, 248);

            // Header
            Panel header = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(41, 128, 185) };
            Label title = new Label { Text = "Sistem Gestiune Farmacie", ForeColor = Color.White, Font = new Font("Segoe UI", 16, FontStyle.Bold), AutoSize = true, Location = new Point(20, 15) };
            header.Controls.Add(title);
            this.Controls.Add(header);

            tabControl = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 11) };
            this.Controls.Add(tabControl);
            tabControl.BringToFront();

            // Tab-urile
            tabVanzari = new TabPage("🛒 Casă de Marcat (Vânzări)");
            tabAprovizionare = new TabPage("📦 Depozit & Aprovizionare");
            tabRapoarte = new TabPage("📊 Rapoarte & Export");

            tabControl.TabPages.Add(tabVanzari);
            tabControl.TabPages.Add(tabAprovizionare);
            tabControl.TabPages.Add(tabRapoarte);

            CreareTabVanzari();
            CreareTabAprovizionare();
            CreareTabRapoarte();
            
            RefreshComboBoxVanzari();
            RefreshDataGridView();
        }

        // ==========================================
        // TAB 1: VANZARI (Foloseste Memento, Command, Strategy, Mediator)
        // ==========================================
        private void CreareTabVanzari()
        {
            tabVanzari.BackColor = Color.White;

            Label l1 = new Label { Text = "Selectați Produsul:", AutoSize = true, Location = new Point(30, 30) };
            cmbProduseVanzare = new ComboBox { Location = new Point(30, 60), Size = new Size(250, 30), DropDownStyle = ComboBoxStyle.DropDownList };
            
            btnAdaugaCos = new Button { Text = "➕ Adaugă Produs", Location = new Point(300, 58), Size = new Size(150, 35), BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnAdaugaCos.Click += BtnAdaugaCos_Click;

            btnUndoCos = new Button { Text = "⏪ Undo", Location = new Point(470, 58), Size = new Size(100, 35), BackColor = Color.FromArgb(231, 76, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnUndoCos.Click += BtnUndoCos_Click;

            lstCos = new ListBox { Location = new Point(30, 120), Size = new Size(540, 250) };

            Label l2 = new Label { Text = "Strategie Discount:", AutoSize = true, Location = new Point(620, 120) };
            cmbStrategieDiscount = new ComboBox { Location = new Point(620, 150), Size = new Size(200, 30), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbStrategieDiscount.Items.AddRange(new string[] { "Fără Discount", "Fidelitate (10%)", "Pensionar (20%)" });
            cmbStrategieDiscount.SelectedIndex = 0;
            cmbStrategieDiscount.SelectedIndexChanged += UpdateTotal;

            lblTotal = new Label { Text = "Total: 0.00 MDL", Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.FromArgb(41, 128, 185), AutoSize = true, Location = new Point(620, 220) };

            btnFinalizeazaVanzare = new Button { Text = "💵 Finalizează Bon", Location = new Point(620, 280), Size = new Size(200, 50), BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            btnFinalizeazaVanzare.Click += BtnFinalizeazaVanzare_Click;

            tabVanzari.Controls.Add(l1);
            tabVanzari.Controls.Add(cmbProduseVanzare);
            tabVanzari.Controls.Add(btnAdaugaCos);
            tabVanzari.Controls.Add(btnUndoCos);
            tabVanzari.Controls.Add(lstCos);
            tabVanzari.Controls.Add(l2);
            tabVanzari.Controls.Add(cmbStrategieDiscount);
            tabVanzari.Controls.Add(lblTotal);
            tabVanzari.Controls.Add(btnFinalizeazaVanzare);
        }

        private void BtnAdaugaCos_Click(object sender, EventArgs e)
        {
            if (cmbProduseVanzare.SelectedItem == null) return;
            string sel = cmbProduseVanzare.SelectedItem.ToString();
            string numeProdus = sel.Split('-')[0].Trim();
            decimal pret = decimal.Parse(sel.Split('-')[1].Replace("MDL", "").Trim());

            // MEMENTO PATTERN: Salvam starea curenta INAINTE de a adauga noul produs
            _istoric.SalveazaStarea(); 
            
            // COMMAND PATTERN: Simulam executia unei comenzi
            var sistemGest = new SistemGestiune();
            var cmd = new ComandaVanzare(sistemGest, numeProdus, 1);
            _casaMarcat.SetCommand(cmd);
            _casaMarcat.ExecuteCommand();

            _cos.AdaugaProdus(numeProdus);
            _totalCos += pret;
            
            UpdateCosUI();
        }

        private void BtnUndoCos_Click(object sender, EventArgs e)
        {
            // MEMENTO PATTERN: Anulam ultima adaugare
            _istoric.Undo();
            
            // Re-calculam totalul (simplificat ptr demonstratie)
            _totalCos = 0;
            var produseDinStoc = StocManager.Instance.GetProduse();
            foreach (var pCos in _cos.AfiseazaContinut().Split('\n'))
            {
                if(string.IsNullOrWhiteSpace(pCos)) continue;
                var match = produseDinStoc.FirstOrDefault(x => pCos.Contains(x.Nume));
                if(match != null) _totalCos += match.Pret;
            }

            UpdateCosUI();
            MessageBox.Show("[Memento Pattern]\nStarea coșului a fost restaurată la pasul anterior!", "Undo Efectuat");
        }

        private void UpdateCosUI()
        {
            lstCos.Items.Clear();
            var items = _cos.AfiseazaContinut().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in items) lstCos.Items.Add(item);
            UpdateTotal(null, null);
        }

        private void UpdateTotal(object sender, EventArgs e)
        {
            // STRATEGY PATTERN
            var calc = new CalculatorPretFinal();
            if (cmbStrategieDiscount.SelectedIndex == 1) calc.SetStrategie(new DiscountFidelitate());
            else if (cmbStrategieDiscount.SelectedIndex == 2) calc.SetStrategie(new DiscountPensionar());
            else calc.SetStrategie(new FaraDiscount());

            decimal final = calc.CalculeazaPretul(_totalCos);
            lblTotal.Text = $"Total: {final:F2} MDL";
        }

        private void BtnFinalizeazaVanzare_Click(object sender, EventArgs e)
        {
            if (lstCos.Items.Count == 0) return;

            // MEDIATOR PATTERN
            // DepartamentVanzari informează sistemul (Mediatorul) că s-a vândut ceva
            // Iar Mediatorul (CentralaFarmacie) se va duce automat in Departamentul Depozit sa faca ajustarile.
            _vanzari.EfectueazaVanzare();

            MessageBox.Show($"Vânzare finalizată cu succes!\n\n[Mediator Pattern] a asigurat scăderea stocului în depozit (fără ca Vanzarile să comunice direct cu Depozitul).\n[Strategy Pattern] a aplicat discountul.", "Bon Fiscal");
            
            _cos = new CosOriginator(); // Golim cosul
            _istoric = new IstoricCosCaretaker(_cos);
            _totalCos = 0;
            UpdateCosUI();
        }

        // ==========================================
        // TAB 2: APROVIZIONARE (Foloseste Chain of Resp, State, Factory)
        // ==========================================
        private void CreareTabAprovizionare()
        {
            tabAprovizionare.BackColor = Color.White;

            dgvStoc = new DataGridView { Location = new Point(30, 30), Size = new Size(500, 350), ReadOnly = true, BackgroundColor = Color.White, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            
            Label l1 = new Label { Text = "Acțiuni Creare (Abstract Factory):", AutoSize = true, Location = new Point(570, 30), Font = new Font("Segoe UI", 11, FontStyle.Bold) };
            
            btnTrusaAdulti = new Button { Text = "Crează Trusă Adulți", Location = new Point(570, 60), Size = new Size(200, 40), BackColor = Color.LightGray, FlatStyle = FlatStyle.Flat };
            btnTrusaAdulti.Click += (s, e) => { AdaugaTrusa(new TrusaAdultiFactory()); };
            
            btnTrusaCopii = new Button { Text = "Crează Trusă Copii", Location = new Point(570, 110), Size = new Size(200, 40), BackColor = Color.LightGray, FlatStyle = FlatStyle.Flat };
            btnTrusaCopii.Click += (s, e) => { AdaugaTrusa(new TrusaCopiiFactory()); };

            Label l2 = new Label { Text = "Livrări și Aprobări:", AutoSize = true, Location = new Point(570, 180), Font = new Font("Segoe UI", 11, FontStyle.Bold) };

            btnCereAprobareBuget = new Button { Text = "Cere Aprobare Comandă\n(Chain of Responsibility)", Location = new Point(570, 210), Size = new Size(250, 50), BackColor = Color.FromArgb(243, 156, 18), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnCereAprobareBuget.Click += BtnCereAprobareBuget_Click;

            lblStareComanda = new Label { Text = "Stare Comandă Curentă: INEXISTENTĂ", Location = new Point(570, 280), Size = new Size(350, 30), Font = new Font("Segoe UI", 10, FontStyle.Italic) };

            btnAvansStareComanda = new Button { Text = "Procesează Starea Comenzii (State)", Location = new Point(570, 320), Size = new Size(250, 40), BackColor = Color.DarkSlateBlue, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Enabled = false };
            btnAvansStareComanda.Click += BtnAvansStareComanda_Click;

            tabAprovizionare.Controls.Add(dgvStoc);
            tabAprovizionare.Controls.Add(l1);
            tabAprovizionare.Controls.Add(btnTrusaAdulti);
            tabAprovizionare.Controls.Add(btnTrusaCopii);
            tabAprovizionare.Controls.Add(l2);
            tabAprovizionare.Controls.Add(btnCereAprobareBuget);
            tabAprovizionare.Controls.Add(lblStareComanda);
            tabAprovizionare.Controls.Add(btnAvansStareComanda);
        }

        private void AdaugaTrusa(ITrusaFactory factory)
        {
            var med = factory.CreareMedicamentDurere();
            var bandaj = factory.CreareBandaj();
            StocManager.Instance.AdaugaProdus(med);
            StocManager.Instance.AdaugaProdus(bandaj);
            RefreshDataGridView();
            RefreshComboBoxVanzari();
            MessageBox.Show("[Abstract Factory]\nTrusa (Medicament + Bandaj) a fost asamblată și adăugată în inventar!", "Succes");
        }

        private void BtnCereAprobareBuget_Click(object sender, EventArgs e)
        {
            // CHAIN OF RESPONSIBILITY PATTERN
            var farmacist = new FarmacistHandler();
            var manager = new ManagerHandler();
            var director = new DirectorHandler();
            
            // Setam lantul: Farmacist -> Manager -> Director
            farmacist.SetNext(manager).SetNext(director);
            
            // Cerem o reaprovizionare gigant de 20% discount (doar Directorul poate)
            farmacist.GestioneazaCererea(20);

            MessageBox.Show("[Chain of Responsibility]\nCererea ta de reducere bugetară (20%) a fost trecută automat prin lanțul ierarhic.\nFarmacistul nu a putut. Managerul nu a putut. Directorul a aprobat-o!", "Aprobare Reușită");

            // Initiem crearea comenzii (State)
            _comandaCurenta = new ComandaAprovizionare(new StareNoua());
            lblStareComanda.Text = "Stare Comandă Curentă: NOUĂ";
            btnAvansStareComanda.Enabled = true;
        }

        private void BtnAvansStareComanda_Click(object sender, EventArgs e)
        {
            // STATE PATTERN
            if (lblStareComanda.Text.Contains("NOUĂ"))
            {
                _comandaCurenta.Proceseaza(); // Muta starea din interior
                lblStareComanda.Text = "Stare Comandă Curentă: ÎN PROCESARE";
                MessageBox.Show("[State Pattern]\nComanda se comportă acum ca fiind 'În Procesare'.", "Tranziție");
            }
            else if (lblStareComanda.Text.Contains("ÎN PROCESARE"))
            {
                _comandaCurenta.Livreaza(); // Muta starea
                lblStareComanda.Text = "Stare Comandă Curentă: LIVRATĂ";
                btnAvansStareComanda.Enabled = false; // Gata drumul
                MessageBox.Show("[State Pattern]\nComanda a sosit la farmacie! Stare finală atinsă.", "Tranziție");
            }
        }

        private void RefreshDataGridView()
        {
            dgvStoc.DataSource = null;
            dgvStoc.DataSource = StocManager.Instance.GetProduse();
        }

        private void RefreshComboBoxVanzari()
        {
            cmbProduseVanzare.Items.Clear();
            foreach (var p in StocManager.Instance.GetProduse())
            {
                cmbProduseVanzare.Items.Add($"{p.Nume} - {p.Pret} MDL");
            }
            if (cmbProduseVanzare.Items.Count > 0) cmbProduseVanzare.SelectedIndex = 0;
        }

        // ==========================================
        // TAB 3: RAPOARTE (Foloseste Template Method, Visitor)
        // ==========================================
        private void CreareTabRapoarte()
        {
            tabRapoarte.BackColor = Color.White;

            Label l1 = new Label { Text = "1. Generare Documente Contabile (Template Method)", Font = new Font("Segoe UI", 12, FontStyle.Bold), AutoSize = true, Location = new Point(50, 40) };
            
            btnRaportPdf = new Button { Text = "Generare Raport PDF (Zilnic)", Location = new Point(50, 80), Size = new Size(250, 45), BackColor = Color.Firebrick, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnRaportPdf.Click += (s, e) => {
                RaportTemplate raport = new RaportZilnicVanzari();
                raport.GenereazaRaport();
                MessageBox.Show("[Template Method]\nS-au executat pașii fixați (Culegere -> Formatare PDF -> Printare) garantând structura!", "Raport Generat");
            };

            btnRaportCsv = new Button { Text = "Generare Raport CSV (Stoc)", Location = new Point(320, 80), Size = new Size(250, 45), BackColor = Color.SeaGreen, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnRaportCsv.Click += (s, e) => {
                RaportTemplate raport = new RaportStocCritic();
                raport.GenereazaRaport();
                MessageBox.Show("[Template Method]\nS-au executat pașii fixați (Culegere -> Formatare CSV -> Printare) garantând structura!", "Raport Generat");
            };

            Label l2 = new Label { Text = "2. Integrare cu Sisteme Externe XML (Visitor Pattern)", Font = new Font("Segoe UI", 12, FontStyle.Bold), AutoSize = true, Location = new Point(50, 180) };
            
            btnExportXml = new Button { Text = "Extrage XML din Rețete și Facturi", Location = new Point(50, 220), Size = new Size(520, 45), BackColor = Color.MediumOrchid, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 11, FontStyle.Bold) };
            btnExportXml.Click += BtnExportXml_Click;

            tabRapoarte.Controls.Add(l1);
            tabRapoarte.Controls.Add(btnRaportPdf);
            tabRapoarte.Controls.Add(btnRaportCsv);
            tabRapoarte.Controls.Add(l2);
            tabRapoarte.Controls.Add(btnExportXml);
        }

        private void BtnExportXml_Click(object sender, EventArgs e)
        {
            // VISITOR PATTERN
            var reteta = new RetetaCompensata { NumePacient = "Vasile Popa", Diagnostic = "Gripă" };
            var factura = new FacturaFirma { NumeFirma = "DepozitFarm SRL", TotalDePlata = 14500m };
            
            var visitor = new ExportXmlVisitor();
            
            // "Vizitatorul" intra curat in clase si le trage datele in afara fara sa le altereze codul
            reteta.Accept(visitor);
            factura.Accept(visitor);

            string demoXml = "<?xml version=\"1.0\"?>\n<Export>\n  <Reteta Pacient=\"Vasile Popa\" Diagnostic=\"Gripă\"/>\n  <Factura Firma=\"DepozitFarm SRL\" Total=\"14500\"/>\n</Export>";
            MessageBox.Show($"[Visitor Pattern]\nVizitatorul a extras datele cu succes fără să modifice clasele inițiale.\n\nRezultat generat:\n{demoXml}", "Export XML Finalizat");
        }
    }
}
