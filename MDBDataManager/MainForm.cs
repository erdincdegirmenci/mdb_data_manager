using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace MDBDataManager
{
    public partial class MainForm : Form
    {
        private string connectionString;
        private List<string> columnNames = new List<string>();
        private string currentSelectedTable = string.Empty;

        public MainForm()
        {
            string iconPath = Path.Combine(Directory.GetCurrentDirectory(), "mdbaddericon.ico");
            if (File.Exists(iconPath))
            {
                this.Icon = new Icon(iconPath);
            }
            InitializeComponent();
            dataGridViewColumns.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewColumns.MultiSelect = false;
        }
        private void chkShowAll_CheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentSelectedTable))
            {
                LoadTableColumns(currentSelectedTable);
            }
        }
        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Access Database (*.mdb)|*.mdb";
                openFileDialog.Title = "Veritabanı Dosyasını Seçin";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + openFileDialog.FileName;
                    lblFileName.Text = "Seçilen Dosya: " + openFileDialog.FileName;
                    LoadTableNames();
                }
                else
                {
                    MessageBox.Show("Geçerli bir dosya seçmediniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void LoadTableNames()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    DataTable schema = connection.GetSchema("Tables");

                    comboBoxTables.Items.Clear();

                    foreach (DataRow row in schema.Rows)
                    {
                        string tableName = row["TABLE_NAME"].ToString();
                        string tableType = row["TABLE_TYPE"].ToString();

                        if (tableType == "TABLE" && !tableName.StartsWith("MSys"))
                        {
                            comboBoxTables.Items.Add(tableName);
                        }
                    }

                    if (comboBoxTables.Items.Count > 0)
                    {
                        comboBoxTables.SelectedIndex = 0;
                    }
                    else
                    {
                        MessageBox.Show("Veritabanında tablo bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tablolar yüklenirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void showDetailsMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedCell != null)
            {
                var cellValue = selectedCell.Value?.ToString();
                if (cellValue != null)
                {
                    using (var detailsForm = new DetailsPopupForm(cellValue))
                    {
                        if (detailsForm.ShowDialog() == DialogResult.OK)
                        {
                            selectedCell.Value = detailsForm.CellValue;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Bu hücrede değer yok.");
                }
            }
            else
            {
                MessageBox.Show("Geçerli bir hücre seçilmedi.");
            }
        }
        private void dataGridViewColumns_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hitTest = dataGridViewColumns.HitTest(e.X, e.Y);

                if (hitTest.RowIndex >= 0 && hitTest.ColumnIndex >= 0)
                {
                    dataGridViewColumns.ClearSelection();

                    dataGridViewColumns.Rows[hitTest.RowIndex].Cells[hitTest.ColumnIndex].Selected = true;

                    contextMenuStrip.Show(dataGridViewColumns, new Point(e.X, e.Y));

                    selectedCell = dataGridViewColumns.Rows[hitTest.RowIndex].Cells[hitTest.ColumnIndex];
                }
            }
        }
        private void dataGridViewColumns_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var column = dataGridViewColumns.Columns[e.ColumnIndex];
            var cellValue = e.FormattedValue.ToString();

            if (dataGridViewColumns.DataSource is DataTable dt)
            {
                var expectedType = dt.Columns[e.ColumnIndex].DataType;

                try
                {
                    if (expectedType == typeof(int))
                        int.Parse(cellValue);
                    else if (expectedType == typeof(double))
                        double.Parse(cellValue);
                    else if (expectedType == typeof(DateTime))
                        DateTime.Parse(cellValue);
                }
                catch
                {
                    string expectedTypeName = "";

                    if (expectedType == typeof(int)) expectedTypeName = "Int";
                    else if (expectedType == typeof(double)) expectedTypeName = "Double";
                    else if (expectedType == typeof(DateTime)) expectedTypeName = "DateTime";
                    else expectedTypeName = expectedType.Name;

                    MessageBox.Show($"Hatalı veri girişi! {expectedTypeName} tipinde değer girilmelidir.", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
            }
        }
        private void comboBoxTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Text = String.Empty;
            currentSelectedTable = comboBoxTables.SelectedItem.ToString();
            LoadTableColumns(currentSelectedTable);
        }
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                LoadTableNames();
                return;
            }

            string selectedTable = comboBoxTables.SelectedItem.ToString();

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    string getColumnsQuery = $"SELECT TOP 1 * FROM {selectedTable}";
                    OleDbDataAdapter columnAdapter = new OleDbDataAdapter(getColumnsQuery, connection);
                    DataTable columnData = new DataTable();
                    columnAdapter.Fill(columnData);

                    if (columnData.Columns.Count > 0)
                    {
                        string searchQuery = $"SELECT * FROM {selectedTable} WHERE ";

                        List<string> searchConditions = new List<string>();

                        foreach (DataColumn col in columnData.Columns)
                        {
                            searchConditions.Add($"{col.ColumnName} LIKE '%{searchTerm}%'");
                        }

                        searchQuery += string.Join(" OR ", searchConditions);

                        OleDbDataAdapter adapter = new OleDbDataAdapter(searchQuery, connection);
                        DataTable data = new DataTable();
                        adapter.Fill(data);

                        columnNames.Clear();
                        foreach (DataColumn col in data.Columns)
                        {
                            columnNames.Add(col.ColumnName);
                        }

                        dataGridViewColumns.DataSource = data;
                    }
                    else
                    {
                        MessageBox.Show("Tabloda kolon bulunamadı.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Arama sırasında hata oluştu: " + ex.Message);
            }
        }
        private void LoadTableColumns(string tableName)
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    string getColumnsQuery = $"SELECT TOP 1 * FROM {tableName}";
                    OleDbDataAdapter columnAdapter = new OleDbDataAdapter(getColumnsQuery, connection);
                    DataTable columnData = new DataTable();
                    columnAdapter.Fill(columnData);

                    if (columnData.Columns.Count > 0)
                    {
                        string firstColumn = columnData.Columns[0].ColumnName;

                        string query = $"SELECT * FROM {tableName}";

                        OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
                        DataTable data = new DataTable();
                        adapter.Fill(data);

                        columnNames.Clear();

                        foreach (DataColumn col in data.Columns)
                        {
                            columnNames.Add(col.ColumnName);
                        }

                        if (chkShowAll.Checked && data.Rows.Count > 0)
                        {
                            dataGridViewColumns.DataSource = data;
                        }
                        else
                        {
                            DataTable lastRowTable = data.Clone();
                            lastRowTable.ImportRow(data.Rows[data.Rows.Count - 1]);

                            dataGridViewColumns.DataSource = lastRowTable;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tabloda kolon bulunamadı.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kolonlar yüklenirken hata oluştu: " + ex.Message);
            }
        }
        private void btnAddRecord_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxTables.SelectedItem == null || dataGridViewColumns.Rows.Count == 0)
                {
                    MessageBox.Show("Lütfen bir tablo seçin ve veri girin.");
                    return;
                }

                string selectedTable = comboBoxTables.SelectedItem.ToString();
                DataGridViewRow row = dataGridViewColumns.CurrentRow;

                string columns = string.Join(", ", columnNames);
                List<string> values = new List<string>();
                List<string> updateValues = new List<string>();
                string firstColumnValue = row.Cells[columnNames[0]].Value?.ToString() ?? "";

                foreach (string col in columnNames)
                {
                    var cellValue = row.Cells[col].Value?.ToString() ?? "";
                    values.Add($"'{cellValue.Replace("'", "''")}'");


                    if (col != columnNames[0])
                    {
                        updateValues.Add($"{col} = '{cellValue.Replace("'", "''")}'");
                    }
                }

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string checkQuery = $"SELECT COUNT(*) FROM {selectedTable} WHERE {columnNames[0]} = {firstColumnValue}";
                    OleDbCommand checkCommand = new OleDbCommand(checkQuery, connection);
                    object result = checkCommand.ExecuteScalar();
                    int existingCount = Convert.ToInt32(result);

                    if (existingCount > 0)
                    {
                        var dialogResult = MessageBox.Show(
                            $"#{firstColumnValue} numaralı kayıt zaten mevcut. Güncellemek istiyor musunuz?",
                            "Güncelleme Onayı",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question
                        );

                        if (dialogResult == DialogResult.Yes)
                        {
                            string updateQuery = $"UPDATE {selectedTable} SET {string.Join(", ", updateValues)} WHERE {columnNames[0]} = {firstColumnValue}";
                            OleDbCommand updateCommand = new OleDbCommand(updateQuery, connection);
                            int updatedRows = updateCommand.ExecuteNonQuery();

                            if (updatedRows > 0)
                            {
                                MessageBox.Show("Kayıt başarıyla güncellendi!");
                            }
                        }
                    }
                    else
                    {
                        InsertNewRecord(connection, selectedTable, columns, values);
                    }
                }

                LoadTableColumns(selectedTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayıt sırasında hata oluştu: " + ex.Message);
            }
        }
        private void InsertNewRecord(OleDbConnection connection, string selectedTable, string columns, List<string> values)
        {
            try
            {
                string insertQuery = $"INSERT INTO {selectedTable} ({columns}) VALUES ({string.Join(", ", values)})";
                OleDbCommand insertCommand = new OleDbCommand(insertQuery, connection);
                insertCommand.ExecuteNonQuery();
                MessageBox.Show("Kayıt başarıyla eklendi!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayıt eklenirken hata oluştu: " + ex.Message);
            }
        }
        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxTables.SelectedItem == null || dataGridViewColumns.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Lütfen silmek için bir satır seçin.");
                    return;
                }

                string selectedTable = comboBoxTables.SelectedItem.ToString();
                DataGridViewRow row = dataGridViewColumns.SelectedRows[0];

                string keyColumn = columnNames[0];
                var keyValue = row.Cells[keyColumn].Value?.ToString();

                if (string.IsNullOrEmpty(keyValue))
                {
                    MessageBox.Show("Anahtar değer alınamadı.");
                    return;
                }

                var dialogResult = MessageBox.Show(
                        $"#{keyValue} numaralı kayıt silinecek. Onaylıyor musunuz?",
                        "Güncelleme Onayı",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                );

                if (dialogResult == DialogResult.Yes)
                {
                    string deleteQuery = $"DELETE FROM {selectedTable} WHERE {keyColumn} = {keyValue}";
                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        connection.Open();
                        OleDbCommand command = new OleDbCommand(deleteQuery, connection);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Kayıt başarıyla silindi!");
                    LoadTableColumns(selectedTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayıt silinirken hata oluştu: " + ex.Message);
            }
        }
    }
}
