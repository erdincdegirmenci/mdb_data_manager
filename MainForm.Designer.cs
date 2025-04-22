using System.Drawing;
using System;
using System.Windows.Forms;

namespace MDBRecordAdder
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.contextMenuStrip = new ContextMenuStrip();
            this.showDetailsMenuItem = new ToolStripMenuItem("Detayları Göster");
            this.txtSearch = new TextBox();
            this.btnSearch = new Button();
            this.comboBoxTables = new ComboBox();
            this.dataGridViewColumns = new DataGridView();
            this.btnAddRecord = new Button();
            this.btnDeleteRecord = new Button();
            this.lblFileName = new Label();
            this.btnSelectFile = new Button();

            // ContextMenu Setup
            this.contextMenuStrip.Items.Add(showDetailsMenuItem);
            showDetailsMenuItem.Click += showDetailsMenuItem_Click;
            dataGridViewColumns.MouseClick += dataGridViewColumns_MouseClick;

            // Dosya seçme butonu
            this.btnSelectFile.Location = new Point(12, 40);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new Size(725, 40);
            this.btnSelectFile.TabIndex = 0;
            this.btnSelectFile.Text = "Dosya Seç";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new EventHandler(this.btnSelectFile_Click);


            // Label for File Name Display
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new Point(175, 15);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new Size(0, 20);
            this.lblFileName.TabIndex = 5;
            this.lblFileName.Font = new Font("Segoe UI", 10, FontStyle.Italic);
            this.lblFileName.ForeColor = Color.FromArgb(0, 120, 212);

            // TextBox for Search
            this.txtSearch.Location = new Point(12, 120);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new Size(570, 30);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.Font = new Font("Segoe UI", 12);
            this.txtSearch.BackColor = Color.FromArgb(245, 245, 245);
            this.txtSearch.BorderStyle = BorderStyle.FixedSingle;
            this.txtSearch.KeyDown += new KeyEventHandler(txtSearch_KeyDown);

            // Search Button
            this.btnSearch.Location = new Point(585, 115);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new Size(150, 40);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Ara";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.BackColor = Color.FromArgb(0, 120, 212); // Mavi ton
            this.btnSearch.ForeColor = Color.White;
            this.btnSearch.FlatStyle = FlatStyle.Flat;
            this.btnSearch.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.btnSearch.Cursor = Cursors.Hand;
            this.btnSearch.Click += btnSearch_Click;

            // ComboBox for Tables
            this.comboBoxTables.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxTables.FormattingEnabled = true;
            this.comboBoxTables.Location = new Point(12, 85);
            this.comboBoxTables.Name = "comboBoxTables";
            this.comboBoxTables.Size = new Size(725, 25);
            this.comboBoxTables.TabIndex = 0;
            this.comboBoxTables.SelectedIndexChanged += comboBoxTables_SelectedIndexChanged;
            this.comboBoxTables.Font = new Font("Segoe UI", 10);
            this.comboBoxTables.BackColor = Color.FromArgb(245, 245, 245);
            this.comboBoxTables.FlatStyle = FlatStyle.Flat;

            // DataGridView for Columns
            this.dataGridViewColumns.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewColumns.Location = new Point(12, 160);
            this.dataGridViewColumns.Name = "dataGridViewColumns";
            this.dataGridViewColumns.RowHeadersWidth = 43;
            this.dataGridViewColumns.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewColumns.Size = new Size(725, 125);
            this.dataGridViewColumns.TabIndex = 1;
            this.dataGridViewColumns.BackgroundColor = Color.White;
            this.dataGridViewColumns.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            this.dataGridViewColumns.DefaultCellStyle.Font = new Font("Segoe UI", 8);
            this.dataGridViewColumns.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            this.dataGridViewColumns.CellValidating += dataGridViewColumns_CellValidating;


            // Add Record Button
            this.btnAddRecord.Location = new Point(12, 300);
            this.btnAddRecord.Name = "btnAddRecord";
            this.btnAddRecord.Size = new Size(350, 40);
            this.btnAddRecord.TabIndex = 2;
            this.btnAddRecord.Text = "Add Record";
            this.btnAddRecord.UseVisualStyleBackColor = true;
            this.btnAddRecord.BackColor = Color.FromArgb(0, 178, 148); // Green
            this.btnAddRecord.ForeColor = Color.White;
            this.btnAddRecord.FlatStyle = FlatStyle.Flat;
            this.btnAddRecord.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.btnAddRecord.Cursor = Cursors.Hand;
            this.btnAddRecord.Click += btnAddRecord_Click;

            // Delete Record Button
            this.btnDeleteRecord.Location = new Point(388, 300);
            this.btnDeleteRecord.Name = "btnDeleteRecord";
            this.btnDeleteRecord.Size = new Size(350, 40);
            this.btnDeleteRecord.TabIndex = 4;
            this.btnDeleteRecord.Text = "Delete Record";
            this.btnDeleteRecord.UseVisualStyleBackColor = true;
            this.btnDeleteRecord.BackColor = Color.FromArgb(232, 17, 35); // Red
            this.btnDeleteRecord.ForeColor = Color.White;
            this.btnDeleteRecord.FlatStyle = FlatStyle.Flat;
            this.btnDeleteRecord.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.btnDeleteRecord.Cursor = Cursors.Hand;
            this.btnDeleteRecord.Click += btnDeleteRecord_Click;

            // Main Form Setup
            this.ClientSize = new Size(750, 360);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.btnAddRecord);
            this.Controls.Add(this.btnDeleteRecord);
            this.Controls.Add(this.dataGridViewColumns);
            this.Controls.Add(this.comboBoxTables);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearch);
            this.Name = "MainForm";
            this.Text = "MDB RECORD ADDER";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewColumns)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem showDetailsMenuItem;
        private TextBox txtSearch;
        private Button btnSearch;
        private ComboBox comboBoxTables;
        private DataGridView dataGridViewColumns;
        private Button btnAddRecord;
        private Button btnDeleteRecord;
        private Button btnSelectFile;
        private Label lblFileName;
        private DataGridViewCell selectedCell = null;
    }
}
