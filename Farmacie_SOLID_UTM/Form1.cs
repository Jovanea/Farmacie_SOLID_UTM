using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
using Farmacie_SOLID_UTM.Decorators;

namespace Farmacie_SOLID_UTM
{
    public partial class Form1 : Form
    {
        private readonly IStocare _stocare;

        // UI
        private TabControl tabControl;
        private TabPage tabPOS, tabGestiune, tabAprovizionare, tabRapoarte, tabSistem;
        private RichTextBox rtbLog;
        private Label lblStatusBar;

        // Tab 1 - POS
        private ComboBox cmbProduse;
        private NumericUpDown nudCantitate;
        private ListBox lstCos;
        private Label lblTotal;
        private ComboBox cmbDiscount;
        private CheckBox chkAmbalaj;
        private Button btnAdaugaCos, btnUndoCos, btnFinalizeaza;

        // Tab 2 - Gestiune
        private DataGridView dgvProduse;
        private TextBox txtNumeProdus, txtPretProdus;
        private ComboBox cmbTipProdus;

        // Tab 3 - Aprovizionare
        private DataGridView dgvComenzi;
        private Label lblStareComanda;
        private Button btnAvansStare;
        private ComboBox cmbTipTrusa;

        // Instante patternuri
        private CosOriginator _cos;
        private IstoricCosCaretaker _istoric;
        private CentralaFarmacie _mediator;
        private DepartamentVanzari _vanzari;
        private DepartamentDepozit _depozit;
        private CasaDeMarcat _casaMarcat;
        private ComandaAprovizionare _comandaCurenta;
        private ProdusPublisher _produsObservat;
        private decimal _totalCos = 0;
        private List<string[]> _comenzi = new List<string[]>();

        // Culori tema
        private static readonly Color ALBASTRU   = Color.FromArgb(26,  82,  118);
        private static readonly Color VERDE      = Color.FromArgb(39,  174,  96);
        private static readonly Color PORTOCALIU = Color.FromArgb(230, 126,  34);
        private static readonly Color ROZ        = Color.FromArgb(142,  68, 173);
        private static readonly Color GRI_DARK   = Color.FromArgb(44,   62,  80);
        private static readonly Color ROSU       = Color.FromArgb(192,  57,  43);
        private static readonly Color BG         = Color.FromArgb(245, 247, 250);

        public Form1(IStocare stocare)
        {
            InitializeComponent();
            _stocare = stocare;
            this.Controls.Clear();

            _mediator = new CentralaFarmacie();
            _vanzari  = new DepartamentVanzari();
            _depozit  = new DepartamentDepozit();
            _mediator.SeteazaVanzari(_vanzari);
            _mediator.SeteazaDepozit(_depozit);

            _cos       = new CosOriginator();
            _istoric   = new IstoricCosCaretaker(_cos);
            _casaMarcat = new CasaDeMarcat();
            _produsObservat = new ProdusPublisher("Aspirina Cardio 100mg", 10);

            // Observer: aboneaza automat sistemul de aprovizionare
            _produsObservat.Subscribe(new SistemAprovizionare());

            PopuleazaStoc();
            ConstruiesteUI();
        }

        private void PopuleazaStoc()
        {
            var s = StocManager.Instance;
            s.AdaugaProdus(new Medicament("Nurofen Raceala 200mg", 35.5m,  "Reckitt") { Cantitate = 20 });
            s.AdaugaProdus(new Medicament("Aspirina Cardio 100mg", 15.0m,  "Bayer")   { Cantitate = 15 });
            s.AdaugaProdus(new Medicament("Paracetamol 500mg",      8.5m,  "Terapia") { Cantitate = 30 });
            s.AdaugaProdus(new Medicament("Ibuprofen 400mg",        12.0m, "BioFarm") { Cantitate = 25 });
            s.AdaugaProdus(new EchipamentMedical("Termometru Digital", 45.0m, "Masurare") { Cantitate = 8 });
            s.AdaugaProdus(new EchipamentMedical("Tensiometru",       110.0m, "Masurare") { Cantitate = 5 });
            s.AdaugaProdus(new BandajElastic() { Cantitate = 50 });
        }

        // =====================================================================
        //  CONSTRUCTIE INTERFATA
        // =====================================================================
        private void ConstruiesteUI()
        {
            this.Text = "FarmSys — Sistem de Gestiune Farmacie";
            this.Size = new Size(1150, 750);
            this.MinimumSize = new Size(1050, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = BG;
            this.Font = new Font("Segoe UI", 9f);

            // ── Header ──────────────────────────────────────────
            Panel header = new Panel { Dock = DockStyle.Top, Height = 52, BackColor = ALBASTRU };
            var lblApp = new Label { Text = "  FarmSys  |  Sistem Integrat de Gestiune Farmacie", ForeColor = Color.White, Font = new Font("Segoe UI", 13, FontStyle.Bold), AutoSize = true, Location = new Point(12, 13) };
            var lblData = new Label { Text = DateTime.Now.ToString("dd MMMM yyyy"), ForeColor = Color.FromArgb(174, 214, 241), Font = new Font("Segoe UI", 10), AutoSize = true, Location = new Point(870, 16) };
            header.Controls.Add(lblApp);
            header.Controls.Add(lblData);
            this.Controls.Add(header);

            // ── Status bar ──────────────────────────────────────
            Panel statusBar = new Panel { Dock = DockStyle.Bottom, Height = 26, BackColor = GRI_DARK };
            lblStatusBar = new Label { Text = "  Gata.", ForeColor = Color.FromArgb(149, 165, 166), Font = new Font("Segoe UI", 8.5f), AutoSize = true, Location = new Point(4, 5) };
            statusBar.Controls.Add(lblStatusBar);
            this.Controls.Add(statusBar);

            // ── Log panel (dreapta) ──────────────────────────────
            Panel logPanel = new Panel { Dock = DockStyle.Right, Width = 265, BackColor = Color.FromArgb(30, 39, 46) };
            var lblLog = new Label { Text = "  Jurnal Sistem", ForeColor = Color.FromArgb(100, 120, 130), Font = new Font("Segoe UI", 9, FontStyle.Bold), Dock = DockStyle.Top, Height = 26, TextAlign = ContentAlignment.MiddleLeft };
            rtbLog = new RichTextBox { Dock = DockStyle.Fill, BackColor = Color.FromArgb(30, 39, 46), ForeColor = Color.FromArgb(180, 200, 220), Font = new Font("Consolas", 8f), ReadOnly = true, BorderStyle = BorderStyle.None, ScrollBars = RichTextBoxScrollBars.Vertical };
            logPanel.Controls.Add(rtbLog);
            logPanel.Controls.Add(lblLog);
            this.Controls.Add(logPanel);

            // ── Tab control ──────────────────────────────────────
            tabControl = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10f), Padding = new Point(16, 6) };
            this.Controls.Add(tabControl);
            tabControl.BringToFront();

            tabPOS          = new TabPage("  Casa de Marcat");
            tabGestiune     = new TabPage("  Gestiune Produse");
            tabAprovizionare= new TabPage("  Aprovizionare");
            tabRapoarte     = new TabPage("  Rapoarte");
            tabSistem       = new TabPage("  Sistem & Acces");

            tabControl.TabPages.AddRange(new TabPage[] { tabPOS, tabGestiune, tabAprovizionare, tabRapoarte, tabSistem });

            BuildTabPOS();
            BuildTabGestiune();
            BuildTabAprovizionare();
            BuildTabRapoarte();
            BuildTabSistem();

            RefreshProduseCmb();
            RefreshGridProduse();

            Status("Aplicatia a pornit. Stoc incarcat: " + StocManager.Instance.GetTotalProduse() + " produse.");
            Log("Sistem pornit", "Singleton");
            Log("StocManager instanta ID: #" + StocManager.Instance.GetHashCode(), "Singleton");
        }

        // =====================================================================
        //  TAB 1 — CASA DE MARCAT
        //  Patterns: Command, Memento, Strategy, Mediator, Decorator, Facade
        // =====================================================================
        private void BuildTabPOS()
        {
            tabPOS.BackColor = BG;

            // ── Stanga: selectie produse + cos ──────────────────
            Panel pLeft = new Panel { Location = new Point(10, 10), Size = new Size(480, 580), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            AddSectionTitle(pLeft, "Bon Curent", ALBASTRU, 0);

            new Label { Text = "Produs:", AutoSize = true, Location = new Point(12, 38) }.Let(pLeft.Controls.Add);
            cmbProduse = new ComboBox { Location = new Point(12, 58), Size = new Size(290, 26), DropDownStyle = ComboBoxStyle.DropDownList };
            pLeft.Controls.Add(cmbProduse);

            new Label { Text = "Cant.:", AutoSize = true, Location = new Point(312, 38) }.Let(pLeft.Controls.Add);
            nudCantitate = new NumericUpDown { Location = new Point(312, 58), Size = new Size(60, 26), Minimum = 1, Maximum = 99, Value = 1 };
            pLeft.Controls.Add(nudCantitate);

            btnAdaugaCos = Buton("Adauga", VERDE, 384, 56, 82, 28);
            btnAdaugaCos.Click += BtnAdaugaCos_Click;
            pLeft.Controls.Add(btnAdaugaCos);

            new Label { Text = "Produse adaugate:", AutoSize = true, Location = new Point(12, 96), ForeColor = Color.Gray }.Let(pLeft.Controls.Add);
            lstCos = new ListBox { Location = new Point(12, 116), Size = new Size(452, 300), Font = new Font("Segoe UI", 10f), BorderStyle = BorderStyle.FixedSingle };
            pLeft.Controls.Add(lstCos);

            btnUndoCos = Buton("Anuleaza ultima", ROSU, 12, 426, 150, 32);
            btnUndoCos.Click += BtnUndoCos_Click;
            pLeft.Controls.Add(btnUndoCos);

            tabPOS.Controls.Add(pLeft);

            // ── Dreapta: discount, total, finalizare ────────────
            Panel pRight = new Panel { Location = new Point(502, 10), Size = new Size(310, 580), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            AddSectionTitle(pRight, "Calcul si Plata", VERDE, 0);

            new Label { Text = "Tip client:", AutoSize = true, Location = new Point(12, 38) }.Let(pRight.Controls.Add);
            cmbDiscount = new ComboBox { Location = new Point(12, 58), Size = new Size(280, 26), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbDiscount.Items.AddRange(new[] { "Client obisnuit", "Card Fidelitate  -10%", "Pensionar  -20%" });
            cmbDiscount.SelectedIndex = 0;
            cmbDiscount.SelectedIndexChanged += (s, e) => RecalcTotal();
            pRight.Controls.Add(cmbDiscount);

            chkAmbalaj = new CheckBox { Text = "Ambalaj cadou  (+5 MDL)", Location = new Point(12, 98), AutoSize = true, Font = new Font("Segoe UI", 9.5f) };
            chkAmbalaj.CheckedChanged += (s, e) => RecalcTotal();
            pRight.Controls.Add(chkAmbalaj);

            new Label { Text = "TOTAL DE PLATA:", AutoSize = true, Location = new Point(12, 146), Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = GRI_DARK }.Let(pRight.Controls.Add);
            lblTotal = new Label { Text = "0.00 MDL", Font = new Font("Segoe UI", 26, FontStyle.Bold), ForeColor = ALBASTRU, AutoSize = true, Location = new Point(12, 168) };
            pRight.Controls.Add(lblTotal);

            btnFinalizeaza = Buton("  EMITE BON FISCAL", ALBASTRU, 12, 240, 282, 52);
            btnFinalizeaza.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnFinalizeaza.Click += BtnFinalizeaza_Click;
            pRight.Controls.Add(btnFinalizeaza);

            // Info discount tip
            var lblInfo = new Label
            {
                Text = "Reducerile se aplica automat\nin functie de tipul clientului.",
                AutoSize = true, Location = new Point(12, 308),
                ForeColor = Color.Silver, Font = new Font("Segoe UI", 8.5f, FontStyle.Italic)
            };
            pRight.Controls.Add(lblInfo);

            tabPOS.Controls.Add(pRight);
        }

        private void BtnAdaugaCos_Click(object sender, EventArgs e)
        {
            if (cmbProduse.SelectedItem == null) return;
            string sel = cmbProduse.SelectedItem.ToString();
            string numeProdus = sel.Split('|')[0].Trim();
            int cantitate = (int)nudCantitate.Value;

            var produs = StocManager.Instance.GetProduse().FirstOrDefault(p => p.Nume == numeProdus);
            if (produs == null || produs.Cantitate < cantitate)
            {
                MessageBox.Show("Stoc insuficient pentru: " + numeProdus, "Stoc Epuizat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _istoric.SalveazaStarea();                          // Memento
            var cmd = new ComandaVanzare(new SistemGestiune(), numeProdus, cantitate); // Command
            _casaMarcat.SetCommand(cmd);
            _casaMarcat.ExecuteCommand();

            for (int i = 0; i < cantitate; i++) _cos.AdaugaProdus(numeProdus);
            produs.Cantitate -= cantitate;
            _totalCos += produs.Pret * cantitate;

            RefreshCosUI();
            RefreshGridProduse();
            Log("Adaugat in bon: " + numeProdus + " x" + cantitate, "Command + Memento");
            Status("Produs adaugat: " + numeProdus);
        }

        private void BtnUndoCos_Click(object sender, EventArgs e)
        {
            if (_totalCos == 0) return;
            string[] linii = _cos.AfiseazaContinut().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string ultim = linii.Length > 0 ? linii[linii.Length - 1].Trim() : null;

            _istoric.Undo();                                    // Memento

            if (ultim != null)
            {
                var p = StocManager.Instance.GetProduse().FirstOrDefault(x => x.Nume == ultim);
                if (p != null) { p.Cantitate++; _totalCos = Math.Max(0, _totalCos - p.Pret); }
            }

            RefreshCosUI();
            RefreshGridProduse();
            Log("Anulata ultima pozitie din bon: " + ultim, "Memento + Command Undo");
            Status("Anulare efectuata.");
        }

        private void BtnFinalizeaza_Click(object sender, EventArgs e)
        {
            if (lstCos.Items.Count == 0)
            {
                MessageBox.Show("Bonul este gol.", "Atentie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Strategy: calculeaza pretul cu discount
            var calc = new CalculatorPretFinal();
            if (cmbDiscount.SelectedIndex == 1) calc.SetStrategie(new DiscountFidelitate());
            else if (cmbDiscount.SelectedIndex == 2) calc.SetStrategie(new DiscountPensionar());
            else calc.SetStrategie(new FaraDiscount());
            decimal total = calc.CalculeazaPretul(_totalCos);

            // Decorator: ambalaj cadou
            if (chkAmbalaj.Checked)
            {
                var prodDummy = new Medicament("Ambalaj", 0m, "");
                var decorat = new AmbalajCadouDecorator(prodDummy);
                total += decorat.Pret;
                Log("Ambalaj cadou aplicat +5 MDL", "Decorator");
            }

            // Facade: vanzare completa (stoc + plata + facturare)
            _vanzari.EfectueazaVanzare();                       // Mediator

            Log("Bon emis: " + total.ToString("F2") + " MDL", "Facade + Strategy + Mediator");
            Status("Bon emis cu succes: " + total.ToString("F2") + " MDL");

            MessageBox.Show(
                "BON FISCAL\n" +
                "══════════════════════\n" +
                lstCos.Items.Cast<string>().Aggregate("", (a, b) => a + b.TrimStart() + "\n") +
                "══════════════════════\n" +
                "Subtotal:  " + _totalCos.ToString("F2") + " MDL\n" +
                (cmbDiscount.SelectedIndex > 0 ? "Discount aplicat.\n" : "") +
                (chkAmbalaj.Checked ? "Ambalaj cadou: +5.00 MDL\n" : "") +
                "TOTAL:     " + total.ToString("F2") + " MDL\n" +
                "══════════════════════\nMultumim!",
                "Bon Fiscal", MessageBoxButtons.OK, MessageBoxIcon.Information);

            _cos = new CosOriginator();
            _istoric = new IstoricCosCaretaker(_cos);
            _totalCos = 0;
            chkAmbalaj.Checked = false;
            RefreshCosUI();
        }

        private void RecalcTotal()
        {
            var calc = new CalculatorPretFinal();
            if (cmbDiscount.SelectedIndex == 1) calc.SetStrategie(new DiscountFidelitate());
            else if (cmbDiscount.SelectedIndex == 2) calc.SetStrategie(new DiscountPensionar());
            else calc.SetStrategie(new FaraDiscount());
            decimal t = calc.CalculeazaPretul(_totalCos);
            if (chkAmbalaj.Checked) t += 5m;
            lblTotal.Text = t.ToString("F2") + " MDL";
        }

        private void RefreshCosUI()
        {
            lstCos.Items.Clear();
            foreach (var item in _cos.AfiseazaContinut().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
                lstCos.Items.Add("  " + item);
            RecalcTotal();
        }

        // =====================================================================
        //  TAB 2 — GESTIUNE PRODUSE
        //  Patterns: Singleton, Factory Method, Prototype, Composite, Adapter, Flyweight, Observer
        // =====================================================================
        private void BuildTabGestiune()
        {
            tabGestiune.BackColor = BG;

            // ── Grid produse ────────────────────────────────────
            Panel pGrid = new Panel { Location = new Point(10, 10), Size = new Size(530, 560), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            AddSectionTitle(pGrid, "Inventar Produse  [" + StocManager.Instance.GetTotalProduse() + " produse]", VERDE, 0);

            dgvProduse = new DataGridView
            {
                Location = new Point(8, 32), Size = new Size(512, 516),
                ReadOnly = true, BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false, BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9f), GridColor = Color.FromArgb(230, 230, 230)
            };
            dgvProduse.DefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185);
            dgvProduse.DefaultCellStyle.SelectionForeColor = Color.White;
            pGrid.Controls.Add(dgvProduse);
            tabGestiune.Controls.Add(pGrid);

            // ── Actiuni dreapta ──────────────────────────────────
            Panel pAct = new Panel { Location = new Point(552, 10), Size = new Size(280, 560), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            AddSectionTitle(pAct, "Actiuni", PORTOCALIU, 0);

            // Adauga produs nou
            new Label { Text = "Adauga produs nou:", AutoSize = true, Location = new Point(10, 38), Font = new Font("Segoe UI", 9, FontStyle.Bold) }.Let(pAct.Controls.Add);
            new Label { Text = "Denumire:", AutoSize = true, Location = new Point(10, 62) }.Let(pAct.Controls.Add);
            txtNumeProdus = new TextBox { Location = new Point(10, 80), Size = new Size(256, 24) };
            pAct.Controls.Add(txtNumeProdus);
            new Label { Text = "Pret (MDL):", AutoSize = true, Location = new Point(10, 108) }.Let(pAct.Controls.Add);
            txtPretProdus = new TextBox { Location = new Point(10, 126), Size = new Size(120, 24), Text = "0.00" };
            pAct.Controls.Add(txtPretProdus);
            new Label { Text = "Tip:", AutoSize = true, Location = new Point(144, 108) }.Let(pAct.Controls.Add);
            cmbTipProdus = new ComboBox { Location = new Point(144, 126), Size = new Size(122, 24), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTipProdus.Items.AddRange(new[] { "Medicament", "Echipament" });
            cmbTipProdus.SelectedIndex = 0;
            pAct.Controls.Add(cmbTipProdus);

            var btnAdd = Buton("Adauga Produs", VERDE, 10, 158, 256, 32);
            btnAdd.Click += BtnAddProdus_Click;
            pAct.Controls.Add(btnAdd);

            Separator(pAct, 200);

            // Duplicate
            new Label { Text = "Produs selectat:", AutoSize = true, Location = new Point(10, 210), Font = new Font("Segoe UI", 9, FontStyle.Bold) }.Let(pAct.Controls.Add);
            var btnDuplica = Buton("Duplica Produs Selectat", ALBASTRU, 10, 228, 256, 32);
            btnDuplica.Click += BtnDuplica_Click;
            pAct.Controls.Add(btnDuplica);

            Separator(pAct, 272);

            // Pachet promotional
            new Label { Text = "Pachet promotional:", AutoSize = true, Location = new Point(10, 282), Font = new Font("Segoe UI", 9, FontStyle.Bold) }.Let(pAct.Controls.Add);
            var btnPachet = Buton("Creeaza Pachet Iarna", ROZ, 10, 300, 256, 32);
            btnPachet.Click += BtnPachet_Click;
            pAct.Controls.Add(btnPachet);

            Separator(pAct, 344);

            // Import furnizor extern
            new Label { Text = "Import furnizor extern:", AutoSize = true, Location = new Point(10, 354), Font = new Font("Segoe UI", 9, FontStyle.Bold) }.Let(pAct.Controls.Add);
            var btnImport = Buton("Import Produs Extern", PORTOCALIU, 10, 372, 256, 32);
            btnImport.Click += BtnImport_Click;
            pAct.Controls.Add(btnImport);

            Separator(pAct, 416);

            // Alerta stoc
            new Label { Text = "Monitorizare stoc:", AutoSize = true, Location = new Point(10, 426), Font = new Font("Segoe UI", 9, FontStyle.Bold) }.Let(pAct.Controls.Add);
            var btnAlerta = Buton("Verifica Stoc Critic", ROSU, 10, 444, 256, 32);
            btnAlerta.Click += BtnAlerta_Click;
            pAct.Controls.Add(btnAlerta);

            // Categorii flyweight
            var btnCategorii = Buton("Incarca Categorii Produse", GRI_DARK, 10, 484, 256, 32);
            btnCategorii.Click += BtnCategorii_Click;
            pAct.Controls.Add(btnCategorii);

            tabGestiune.Controls.Add(pAct);
        }

        private void BtnAddProdus_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNumeProdus.Text)) { MessageBox.Show("Introduceti denumirea.", "Eroare"); return; }
            if (!decimal.TryParse(txtPretProdus.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal pret)) { MessageBox.Show("Pret invalid.", "Eroare"); return; }

            // Factory Method: fabrica decide tipul concret
            ProdusFactory fabrica = cmbTipProdus.SelectedIndex == 0 ? (ProdusFactory)new MedicamentFactory() : new EchipamentFactory();
            Produs p = fabrica.CreazaProdus(txtNumeProdus.Text.Trim(), pret, cmbTipProdus.SelectedItem.ToString());
            StocManager.Instance.AdaugaProdus(p);
            RefreshGridProduse();
            RefreshProduseCmb();
            txtNumeProdus.Clear();
            Log("Produs adaugat: " + p.Nume + " via " + fabrica.GetType().Name, "Factory Method");
            Status("Produs adaugat: " + p.Nume);
        }

        private void BtnDuplica_Click(object sender, EventArgs e)
        {
            if (dgvProduse.SelectedRows.Count == 0) { MessageBox.Show("Selectati un produs din lista.", "Atentie"); return; }
            string numeSel = dgvProduse.SelectedRows[0].Cells["Nume"].Value?.ToString();
            var original = StocManager.Instance.GetProduse().FirstOrDefault(p => p.Nume == numeSel);
            if (original == null) return;

            Produs clona = original.Cloneaza();    // Prototype
            clona.Cantitate = 10;
            StocManager.Instance.AdaugaProdus(clona);
            RefreshGridProduse();
            RefreshProduseCmb();
            Log("Duplicat: " + original.Nume + "  (ID original " + original.GetHashCode() + " ≠ ID clona " + clona.GetHashCode() + ")", "Prototype");
            Status("Produs duplicat cu succes.");
        }

        private void BtnPachet_Click(object sender, EventArgs e)
        {
            // Composite: pachet care aduna pretul elementelor
            var pachet = new PachetProduse("Pachet Sanatate Iarna");
            pachet.AdaugaInPachet(new Medicament("Vitamina C 1000mg", 18.0m, "Solgar"));
            pachet.AdaugaInPachet(new Medicament("Zinc 50mg", 22.0m, "Terapia"));
            pachet.AdaugaInPachet(new EchipamentMedical("Masca FFP2", 5.0m, "Protectie"));
            StocManager.Instance.AdaugaProdus(pachet);
            RefreshGridProduse();
            RefreshProduseCmb();
            Log("Pachet creat: " + pachet.Nume + " = " + pachet.Pret + " MDL (suma automata din componente)", "Composite");
            Status("Pachet promotional creat: " + pachet.Pret + " MDL");
            MessageBox.Show("Pachetul '" + pachet.Nume + "' a fost creat!\nPret total (automat): " + pachet.Pret + " MDL\n\n" + pachet.ObtineDetalii(), "Pachet Creat", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            // Adapter: traduce interfata furnizorului extern in Produs intern
            var furnizor = new FurnizorExternProdus();
            Produs adaptat = new ProdusAdapter(furnizor);
            StocManager.Instance.AdaugaProdus(adaptat);
            RefreshGridProduse();
            RefreshProduseCmb();
            Log("Importat via Adapter: " + adaptat.Nume + " (" + adaptat.Pret + " MDL)", "Adapter");
            Status("Produs importat din furnizor extern.");
            MessageBox.Show("Produsul a fost importat cu succes:\n" + adaptat.ObtineDetalii(), "Import Reusit", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnAlerta_Click(object sender, EventArgs e)
        {
            // Observer: scanează stocul real și notifică abonații pentru fiecare produs critic
            const int PRAG_CRITIC = 10;
            var produseCritice = StocManager.Instance.GetProduse()
                .FindAll(p => p.Cantitate < PRAG_CRITIC);

            if (produseCritice.Count == 0)
            {
                MessageBox.Show("Niciun produs cu stoc critic (sub " + PRAG_CRITIC + " buc).\nStocul este in parametri normali.", "Stoc OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Status("Verificare stoc: totul in ordine.");
                return;
            }

            var farmacist = new FarmacistAbonat("Maria Ionescu");
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("PRODUSE CU STOC CRITIC (sub " + PRAG_CRITIC + " buc):\n");

            foreach (var p in produseCritice)
            {
                // Observer: pentru fiecare produs critic, notifica abonații
                var publisher = new ProdusPublisher(p.Nume, p.Cantitate);
                publisher.Subscribe(farmacist);
                publisher.Subscribe(new SistemAprovizionare());
                publisher.ModificaStoc(p.Cantitate); // declanseaza notificarea
                sb.AppendLine("  !! " + p.Nume + "  —  " + p.Cantitate + " buc ramase");
            }

            sb.AppendLine("\nNotificati automat:");
            sb.AppendLine("  - Farmacist Maria Ionescu");
            sb.AppendLine("  - Sistem Aprovizionare");
            sb.AppendLine("\nComenzi catre furnizori initiate.");
            sb.AppendLine("\n(Observer: " + produseCritice.Count + " produse, abonati notificati automat\nfara ca StocManager sa stie cine sunt)");

            Log("Stoc critic: " + produseCritice.Count + " produse detectate → abonati notificati", "Observer");
            Status("Alerta stoc critic: " + produseCritice.Count + " produse sub " + PRAG_CRITIC + " buc.");
            MessageBox.Show(sb.ToString(), "Alerta Stoc Critic", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void BtnCategorii_Click(object sender, EventArgs e)
        {
            // Flyweight: categorii partajate intre mii de produse
            var fabrica = new CategorieFactory();
            var c1a = fabrica.GetCategorie("Analgezice", "Medicamente pentru durere si febra");
            var c1b = fabrica.GetCategorie("Analgezice", "Medicamente pentru durere si febra");
            fabrica.GetCategorie("Echipamente Medicale", "Dispozitive de masurare si ingrijire");
            fabrica.GetCategorie("Materiale Pansament", "Bandaje, plasturi, comprese");
            fabrica.GetCategorie("Suplimente", "Vitamine si minerale");
            bool partajate = ReferenceEquals(c1a, c1b);
            Log("Categorii incarcate: " + fabrica.NumarCategoriiCreate + " obiecte in memorie (5 cereri facute, economie RAM)", "Flyweight");
            Status("Categorii incarcate: " + fabrica.NumarCategoriiCreate + " obiecte partajate.");
            MessageBox.Show("Categorii incarcate in sistem!\n\nCereri efectuate: 5\nObiecte create efectiv in memorie: " + fabrica.NumarCategoriiCreate + "\n\nCategoria 'Analgezice' ceruta de 2 ori → aceeasi instanta: " + partajate + "\n\nLa 10.000 produse, datele categoriei se salveaza O SINGURA DATA.", "Categorii Partajate", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // =====================================================================
        //  TAB 3 — APROVIZIONARE
        //  Patterns: Abstract Factory, Builder+Director, Chain, State, Iterator
        // =====================================================================
        private void BuildTabAprovizionare()
        {
            tabAprovizionare.BackColor = BG;

            // ── Stanga: comenzi ──────────────────────────────────
            Panel pComenzi = new Panel { Location = new Point(10, 10), Size = new Size(420, 560), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            AddSectionTitle(pComenzi, "Comenzi Aprovizionare", ALBASTRU, 0);

            dgvComenzi = new DataGridView
            {
                Location = new Point(8, 32), Size = new Size(402, 300),
                ReadOnly = true, BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false, BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9f), GridColor = Color.FromArgb(230, 230, 230)
            };
            dgvComenzi.Columns.Add("Comanda", "Comanda");
            dgvComenzi.Columns.Add("Furnizor", "Furnizor");
            dgvComenzi.Columns.Add("Total", "Total MDL");
            dgvComenzi.Columns.Add("Stare", "Stare");
            pComenzi.Controls.Add(dgvComenzi);

            new Label { Text = "Stare comanda selectata:", AutoSize = true, Location = new Point(8, 342), Font = new Font("Segoe UI", 9, FontStyle.Bold) }.Let(pComenzi.Controls.Add);
            lblStareComanda = new Label { Text = "—  (nicio comanda activa)", Location = new Point(8, 362), Size = new Size(400, 22), ForeColor = Color.Gray, Font = new Font("Segoe UI", 9, FontStyle.Italic) };
            pComenzi.Controls.Add(lblStareComanda);

            btnAvansStare = Buton("Avanseaza starea comenzii", ALBASTRU, 8, 390, 260, 34);
            btnAvansStare.Enabled = false;
            btnAvansStare.Click += BtnAvansStare_Click;
            pComenzi.Controls.Add(btnAvansStare);

            // Iterator: scanare inventar
            var btnScanare = Buton("Scanare inventar complet", GRI_DARK, 8, 434, 260, 34);
            btnScanare.Click += BtnScanare_Click;
            pComenzi.Controls.Add(btnScanare);

            tabAprovizionare.Controls.Add(pComenzi);

            // ── Dreapta: actiuni aprovizionare ───────────────────
            Panel pAct = new Panel { Location = new Point(442, 10), Size = new Size(390, 560), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            AddSectionTitle(pAct, "Creare Comenzi si Truse", PORTOCALIU, 0);

            new Label { Text = "Trusa medicala:", AutoSize = true, Location = new Point(10, 38), Font = new Font("Segoe UI", 9, FontStyle.Bold) }.Let(pAct.Controls.Add);
            cmbTipTrusa = new ComboBox { Location = new Point(10, 58), Size = new Size(200, 26), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTipTrusa.Items.AddRange(new[] { "Trusa Adulti", "Trusa Copii" });
            cmbTipTrusa.SelectedIndex = 0;
            pAct.Controls.Add(cmbTipTrusa);
            var btnTrusa = Buton("Creeaza Trusa", VERDE, 220, 56, 156, 30);
            btnTrusa.Click += BtnCreareTrusa_Click;
            pAct.Controls.Add(btnTrusa);

            Separator(pAct, 100);
            new Label { Text = "Comanda personalizata:", AutoSize = true, Location = new Point(10, 110), Font = new Font("Segoe UI", 9, FontStyle.Bold) }.Let(pAct.Controls.Add);
            var btnVacanta = Buton("Trusa Vacanta (6 produse)", ALBASTRU, 10, 130, 180, 30);
            btnVacanta.Click += (s, e) => CreeazaTrusaBuilder(false);
            pAct.Controls.Add(btnVacanta);
            var btnAuto = Buton("Trusa Auto (obligatorie)", GRI_DARK, 198, 130, 178, 30);
            btnAuto.Click += (s, e) => CreeazaTrusaBuilder(true);
            pAct.Controls.Add(btnAuto);

            Separator(pAct, 174);
            new Label { Text = "Cerere reducere bugetara:", AutoSize = true, Location = new Point(10, 184), Font = new Font("Segoe UI", 9, FontStyle.Bold) }.Let(pAct.Controls.Add);
            var btnChain5  = Buton("Cerere  5% (Farmacist)", VERDE,       10, 204, 182, 30);
            var btnChain15 = Buton("Cerere 15% (Manager)",   PORTOCALIU, 200, 204, 176, 30);
            var btnChain20 = Buton("Cerere 20% (Director)",  ROSU,        10, 242, 182, 30);
            btnChain5 .Click += (s, e) => CereAprobare(5);
            btnChain15.Click += (s, e) => CereAprobare(15);
            btnChain20.Click += (s, e) => CereAprobare(20);
            pAct.Controls.AddRange(new Control[] { btnChain5, btnChain15, btnChain20 });

            tabAprovizionare.Controls.Add(pAct);
        }

        private void BtnCreareTrusa_Click(object sender, EventArgs e)
        {
            // Abstract Factory: familie compatibila de produse
            ITrusaFactory factory = cmbTipTrusa.SelectedIndex == 0 ? (ITrusaFactory)new TrusaAdultiFactory() : new TrusaCopiiFactory();
            var med    = factory.CreareMedicamentDurere();
            var bandaj = factory.CreareBandaj();
            StocManager.Instance.AdaugaProdus(med);
            StocManager.Instance.AdaugaProdus(bandaj);
            RefreshGridProduse();
            RefreshProduseCmb();
            string tip = factory is TrusaAdultiFactory ? "Adulti" : "Copii";
            AdaugaComanda("Trusa " + tip, "Fabrica Interna", (med.Pret + bandaj.Pret).ToString("F2"), "Noua");
            Log("Trusa " + tip + ": " + med.Nume + " + " + bandaj.Nume + " (familie garantat compatibila)", "Abstract Factory");
            Status("Trusa " + tip + " creata si adaugata in stoc.");
            MessageBox.Show("Trusa " + tip + " creata cu succes!\n\nComponente din aceeasi familie:\n  - " + med.ObtineDetalii() + "\n  - " + bandaj.ObtineDetalii(), "Trusa Medicala", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CreeazaTrusaBuilder(bool auto)
        {
            // Builder + Director
            var builder  = new TrusaBuilder();
            var director = new TrusaDirector(builder);
            TrusaMedicala trusa = auto ? director.ConstructTrusaAuto() : director.ConstructTrusaVacanta();
            string tip = auto ? "Auto" : "Vacanta";
            AdaugaComanda("Trusa " + tip, "Builder Director", trusa.CalculeazaPretTotal().ToString("F2"), "Noua");
            _comandaCurenta = new ComandaAprovizionare(new StareNoua());    // State
            lblStareComanda.Text = "Comanda Trusa " + tip + ": NOUA";
            lblStareComanda.ForeColor = Color.FromArgb(243, 156, 18);
            btnAvansStare.Enabled = true;
            Log("Trusa " + tip + " construita: " + trusa.CalculeazaPretTotal() + " MDL — Director a orchestrat Builder-ul pas cu pas", "Builder + Director");
            Status("Trusa " + tip + " construita: " + trusa.CalculeazaPretTotal() + " MDL");
            MessageBox.Show("Comanda creata cu Builder + Director:\n\n" + trusa.ListeazaContinut() + "\nTotal: " + trusa.CalculeazaPretTotal() + " MDL\n\nComanda se afla in starea NOUA.\nFolositi butonul 'Avanseaza starea' pentru a urmari procesul.", "Comanda Creata", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnAvansStare_Click(object sender, EventArgs e)
        {
            // State pattern: comportamentul se schimba cu starea
            if (lblStareComanda.Text.Contains("NOUA"))
            {
                _comandaCurenta.Proceseaza();
                lblStareComanda.Text = lblStareComanda.Text.Replace("NOUA", "IN PROCESARE");
                lblStareComanda.ForeColor = Color.FromArgb(41, 128, 185);
                Log("Comanda: NOUA → IN PROCESARE", "State");
                Status("Stare comanda: IN PROCESARE");
                MessageBox.Show("Comanda este acum IN PROCESARE.\n\nIn aceasta stare, anularea genereaza costuri.\nFurnizorul a fost notificat.", "Stare Actualizata", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (lblStareComanda.Text.Contains("PROCESARE"))
            {
                _comandaCurenta.Livreaza();
                lblStareComanda.Text = lblStareComanda.Text.Replace("IN PROCESARE", "LIVRATA");
                lblStareComanda.ForeColor = VERDE;
                btnAvansStare.Enabled = false;
                Log("Comanda: IN PROCESARE → LIVRATA (stare finala)", "State");
                Status("Comanda livrata cu succes.");
                MessageBox.Show("Comanda a fost LIVRATA!\n\nAnularea nu mai este posibila.\nProdusele au intrat in stoc.", "Livrat", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CereAprobare(decimal procent)
        {
            // Chain of Responsibility: cererea urca ierarhic pana e rezolvata
            var farmacist = new FarmacistHandler();
            var manager   = new ManagerHandler();
            var director  = new DirectorHandler();
            farmacist.SetNext(manager).SetNext(director);
            farmacist.GestioneazaCererea(procent);
            Log("Cerere " + procent + "% → urcata in lant pana la aprobator competent", "Chain of Responsibility");
            string aprobator = procent <= 5 ? "Farmacist" : procent <= 15 ? "Manager" : "Director General";
            Status("Reducere " + procent + "% aprobata de: " + aprobator);
            MessageBox.Show("Cerere de reducere bugetara: " + procent + "%\n\nAprobata de: " + aprobator + "\n\nCererea a urcat automat in lantul ierarhic\npana a ajuns la persoana cu autoritatea necesara.", "Cerere Aprobata", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnScanare_Click(object sender, EventArgs e)
        {
            // Iterator: parcurge colectia fara a cunoaste structura interna
            var dulap = new DulapMedicamente();
            foreach (var p in StocManager.Instance.GetProduse())
                dulap.Adauga(p.Nume + "  [" + p.Cantitate + " buc]");

            var it = dulap.CreateIterator();
            var sb = new System.Text.StringBuilder();
            int i = 1;
            while (it.HasMore()) sb.AppendLine("  " + i++ + ". " + it.GetNext());
            Log("Inventar scanat: " + (i - 1) + " pozitii via Iterator", "Iterator");
            Status("Scanare inventar completa: " + (i - 1) + " produse.");
            MessageBox.Show("RAPORT SCANARE INVENTAR\n══════════════════════\n" + sb.ToString(), "Scanare Inventar", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AdaugaComanda(string prod, string furnizor, string total, string stare)
        {
            _comenzi.Add(new[] { prod, furnizor, total, stare });
            dgvComenzi.Rows.Clear();
            foreach (var c in _comenzi) dgvComenzi.Rows.Add(c[0], c[1], c[2], c[3]);
        }

        // =====================================================================
        //  TAB 4 — RAPOARTE
        //  Patterns: Template Method, Visitor
        // =====================================================================
        private void BuildTabRapoarte()
        {
            tabRapoarte.BackColor = BG;

            Panel p = new Panel { Location = new Point(10, 10), Size = new Size(820, 560), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            AddSectionTitle(p, "Generare Rapoarte si Export", ROSU, 0);

            // Template Method
            new Label { Text = "Rapoarte periodice:", AutoSize = true, Location = new Point(12, 44), Font = new Font("Segoe UI", 10, FontStyle.Bold) }.Let(p.Controls.Add);
            new Label
            {
                Text = "Toate rapoartele respecta acelasi flux:\n  1. Culegere date  →  2. Formatare (specific)  →  3. Printare\nOrdinea este fixata de clasa de baza. Subclasele personalizeaza doar formatul.",
                AutoSize = true, Location = new Point(12, 68), ForeColor = Color.Gray, Font = new Font("Segoe UI", 9f, FontStyle.Italic)
            }.Let(p.Controls.Add);

            var btnRapVanzari = Buton("Raport Vanzari Zilnice  (CSV)", ROSU, 12, 122, 250, 42);
            btnRapVanzari.Click += (s, e) =>
            {
                string cale = new RaportZilnicVanzari().GenereazaRaport();
                Log("Raport vanzari zilnice generat (CSV)", "Template Method");
                Status("Raport vanzari generat: " + System.IO.Path.GetFileName(cale));
                MessageBox.Show("Raportul de vanzari zilnice a fost generat.\n\nPasi executati:\n  1. Culegere date din sistem\n  2. Formatare in CSV\n  3. Salvare fisier\n\nFisier salvat pe Desktop:\n" + cale, "Raport Generat", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            p.Controls.Add(btnRapVanzari);

            var btnRapStoc = Buton("Raport Stoc Critic  (.txt)", Color.FromArgb(243, 156, 18), 274, 122, 250, 42);
            btnRapStoc.Click += (s, e) =>
            {
                string cale = new RaportStocCritic().GenereazaRaport();
                Log("Raport stoc critic generat (.txt)", "Template Method");
                Status("Raport stoc critic generat: " + System.IO.Path.GetFileName(cale));
                MessageBox.Show("Raportul de stoc critic a fost generat.\n\nPasi executati:\n  1. Culegere date din sistem\n  2. Formatare cu alerte vizuale\n  3. Salvare fisier\n\nFisier salvat pe Desktop:\n" + cale, "Raport Generat", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            p.Controls.Add(btnRapStoc);

            Separator(p, 180);
            new Label { Text = "Export documente CNAS / E-Factura:", AutoSize = true, Location = new Point(12, 194), Font = new Font("Segoe UI", 10, FontStyle.Bold) }.Let(p.Controls.Add);
            new Label
            {
                Text = "Vizitatorul extrage datele din documente fara a le modifica.\nMecanismul Accept() → Visit() redirectioneaza automat la metoda corecta.",
                AutoSize = true, Location = new Point(12, 218), ForeColor = Color.Gray, Font = new Font("Segoe UI", 9f, FontStyle.Italic)
            }.Let(p.Controls.Add);

            var btnXml = Buton("Export XML Retete + Facturi", ROZ, 12, 262, 300, 42);
            btnXml.Click += BtnExportXml_Click;
            p.Controls.Add(btnXml);

            tabRapoarte.Controls.Add(p);
        }

        private void BtnExportXml_Click(object sender, EventArgs e)
        {
            // Visitor: extrage date fara a modifica clasele (Double Dispatch)
            var reteta  = new RetetaCompensata { NumePacient = "Ion Popescu", Diagnostic = "Gripa Sezoniera" };
            var factura = new FacturaFirma { NumeFirma = "DepozitFarm SRL", TotalDePlata = 14500m };
            var visitor = new ExportXmlVisitor();
            reteta.Accept(visitor);   // → visitor.Visit(RetetaCompensata)
            factura.Accept(visitor);  // → visitor.Visit(FacturaFirma)
            var (cale, xml) = visitor.Salveaza();
            Log("Export XML generat si salvat: Reteta + Factura via Double Dispatch", "Visitor");
            Status("Export XML CNAS salvat: " + System.IO.Path.GetFileName(cale));
            MessageBox.Show(
                "Export XML CNAS finalizat cu succes!\n\n" +
                "Documente exportate:\n" +
                "  ✔  Reteta compensata  —  Ion Popescu (Gripa Sezoniera)\n" +
                "  ✔  Factura firma  —  DepozitFarm SRL  (14.500 MDL)\n\n" +
                "Fisier salvat pe Desktop:\n  " + System.IO.Path.GetFileName(cale) + "\n\n" +
                "Mecanismul Visitor a apelat automat\n" +
                "metoda corecta pentru fiecare tip de document\n" +
                "fara ca clasele documentelor sa fie modificate.",
                "Export XML CNAS", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // =====================================================================
        //  TAB 5 — SISTEM & ACCES
        //  Patterns: Bridge, Proxy, Singleton (info)
        // =====================================================================
        private void BuildTabSistem()
        {
            tabSistem.BackColor = BG;

            Panel p = new Panel { Location = new Point(10, 10), Size = new Size(820, 560), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            AddSectionTitle(p, "Sistem — Control Acces", GRI_DARK, 0);

            // Singleton info
            new Label { Text = "Instanta centrala StocManager:", AutoSize = true, Location = new Point(12, 44), Font = new Font("Segoe UI", 10, FontStyle.Bold) }.Let(p.Controls.Add);
            new Label { Text = "ID instanta: #" + StocManager.Instance.GetHashCode() + "   |   Produse in stoc: " + StocManager.Instance.GetTotalProduse() + "   |   Acelasi obiect din orice ecran al aplicatiei.", AutoSize = true, Location = new Point(12, 66), ForeColor = VERDE, Font = new Font("Consolas", 9f) }.Let(p.Controls.Add);

            Separator(p, 96);

            // Bridge: notificari
            new Label { Text = "Notificari sistem:", AutoSize = true, Location = new Point(12, 110), Font = new Font("Segoe UI", 10, FontStyle.Bold) }.Let(p.Controls.Add);
            var btnSms = Buton("Trimite Alerta prin SMS", VERDE, 12, 134, 210, 36);
            var btnEmail = Buton("Trimite Alerta prin Email", ALBASTRU, 232, 134, 210, 36);
            btnSms.Click += (s, e) =>
            {
                new NotificatorUrgent(new TrimitereSms()).ExpediazaAlerta("Stoc critic la Aspirina!");
                Log("Alerta urgenta trimisa via SMS", "Bridge");
                Status("Alerta SMS trimisa.");
                MessageBox.Show("Alerta trimisa prin SMS!\n\n[! ALERTA URGENTA !] STOC CRITIC LA ASPIRINA!\n\nAcelasi Notificator poate folosi Email, SMS sau orice alta platforma\nfara sa modifice logica de alertare.", "Alerta SMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            btnEmail.Click += (s, e) =>
            {
                new Notificator(new TrimitereEmail()).ExpediazaAlerta("Raport zilnic disponibil.");
                Log("Notificare trimisa via Email", "Bridge");
                Status("Notificare Email trimisa.");
                MessageBox.Show("Notificare trimisa prin Email!\n\nRaport zilnic disponibil.\n\nSursa notificarii e aceeasi — doar platforma de livrare s-a schimbat.", "Email Trimis", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            p.Controls.AddRange(new Control[] { btnSms, btnEmail });

            Separator(p, 184);

            // Proxy: control acces
            new Label { Text = "Control acces baza de date:", AutoSize = true, Location = new Point(12, 198), Font = new Font("Segoe UI", 10, FontStyle.Bold) }.Let(p.Controls.Add);
            new Label { Text = "Proxy verifica rolul inainte de a permite operatia:", AutoSize = true, Location = new Point(12, 220), ForeColor = Color.Gray }.Let(p.Controls.Add);
            var btnManager   = Buton("Stergere ca Manager (permis)", VERDE, 12, 244, 250, 36);
            var btnFarmacist = Buton("Stergere ca Farmacist (refuzat)", ROSU, 272, 244, 250, 36);
            btnManager.Click += (s, e) =>
            {
                new ProxyBazaDate("Manager").StergeProdus("TestProdus");
                Log("Rol Manager → Proxy a permis accesul la RealBazaDate", "Proxy");
                Status("Operatie permisa: Manager.");
                MessageBox.Show("Acces PERMIS.\n\nProxy a verificat rolul 'Manager' si a permis operatia.\nRealBazaDate.StergeProdus() a fost apelata.\n\nClientul nu a stiut ca vorbea cu un intermediar.", "Acces Permis", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            btnFarmacist.Click += (s, e) =>
            {
                new ProxyBazaDate("Farmacist").StergeProdus("TestProdus");
                Log("Rol Farmacist → Proxy a BLOCAT accesul (RealBazaDate nu a fost instantiata)", "Proxy");
                Status("Operatie REFUZATA: Farmacist.");
                MessageBox.Show("Acces REFUZAT.\n\nProxy a verificat rolul 'Farmacist' — permisiuni insuficiente.\nRealBazaDate nu a fost instantiata deloc.\n\nBaza de date este complet protejata.", "Acces Refuzat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            };
            p.Controls.AddRange(new Control[] { btnManager, btnFarmacist });

         

            tabSistem.Controls.Add(p);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // =====================================================================
        //  REFRESH HELPERS
        // =====================================================================
        private void RefreshGridProduse()
        {
            dgvProduse.DataSource = null;
            dgvProduse.DataSource = StocManager.Instance.GetProduse();
        }

        private void RefreshProduseCmb()
        {
            cmbProduse.Items.Clear();
            foreach (var p in StocManager.Instance.GetProduse())
                cmbProduse.Items.Add(p.Nume + "  |  " + p.Pret.ToString("F2") + " MDL  |  stoc: " + p.Cantitate);
            if (cmbProduse.Items.Count > 0) cmbProduse.SelectedIndex = 0;
        }

        private void Log(string mesaj, string pattern)
        {
            if (rtbLog == null) return;
            rtbLog.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + mesaj + "\n             → [" + pattern + "]\n");
            rtbLog.ScrollToCaret();
        }

        private void Status(string mesaj)
        {
            if (lblStatusBar != null) lblStatusBar.Text = "  " + mesaj;
        }

        // =====================================================================
        //  UI HELPERS
        // =====================================================================
        private Button Buton(string text, Color bg, int x, int y, int w, int h)
        {
            return new Button { Text = text, Location = new Point(x, y), Size = new Size(w, h), BackColor = bg, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9f), Cursor = Cursors.Hand };
        }

        private void AddSectionTitle(Panel p, string text, Color c, int x)
        {
            var lbl = new Label { Text = "  " + text, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = c, BackColor = Color.FromArgb(245, 247, 250), Dock = DockStyle.Top, Height = 28, TextAlign = ContentAlignment.MiddleLeft };
            p.Controls.Add(lbl);
        }

        private void Separator(Panel p, int y)
        {
            var sep = new Panel { Location = new Point(8, y), Size = new Size(p.Width - 16, 1), BackColor = Color.FromArgb(220, 220, 220) };
            p.Controls.Add(sep);
        }
    }

    // Extension method pentru fluent Control.Add
    public static class ControlExt
    {
        public static T Let<T>(this T item, Action<Control> action) where T : Control
        {
            action(item);
            return item;
        }
    }
}
