using System;
using System.Drawing;
using System.Windows.Forms;

namespace MDBRecordAdder
{
    public partial class DetailsPopupForm : Form
    {
        public string CellValue { get; set; }

        private RichTextBox txtDetailValue;
        private Button btnSave;
        public DetailsPopupForm(string currentValue)
        {
            InitializeComponent();
            CellValue = currentValue;
            txtDetailValue.Text = currentValue;
        }


        private void InitializeComponent()
        {
            this.txtDetailValue = new System.Windows.Forms.RichTextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtDetailValue
            // 
            this.txtDetailValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtDetailValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDetailValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtDetailValue.Location = new System.Drawing.Point(12, 12);
            this.txtDetailValue.Name = "txtDetailValue";
            this.txtDetailValue.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtDetailValue.Size = new System.Drawing.Size(499, 204);
            this.txtDetailValue.TabIndex = 0;
            this.txtDetailValue.Text = "";
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(12, 231);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(499, 35);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Kaydet";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += btnSave_Click;
            // 
            // DetailsPopupForm
            // 
            this.ClientSize = new System.Drawing.Size(546, 283);
            this.Controls.Add(this.txtDetailValue);
            this.Controls.Add(this.btnSave);
            this.Name = "DetailsPopupForm";
            this.Text = "Detayları Düzenle";
            this.ResumeLayout(false);

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            CellValue = txtDetailValue.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
