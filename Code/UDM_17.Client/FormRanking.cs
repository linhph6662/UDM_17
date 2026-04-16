using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UDM_17.Client
{
    public partial class FormRanking : Form
    {
        public FormRanking()
        {
            InitializeComponent();
            BuildColumns();
        }

        private void BuildColumns()
        {
            dgvRanking.AutoGenerateColumns = false;
            dgvRanking.Columns.Clear();
            dgvRanking.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Rank",
                DataPropertyName = "Rank",
                Width = 60,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });
            dgvRanking.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Username",
                DataPropertyName = "Username",
                FillWeight = 42,
                MinimumWidth = 180,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvRanking.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "DisplayName",
                DataPropertyName = "DisplayName",
                FillWeight = 42,
                MinimumWidth = 180,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvRanking.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Score",
                DataPropertyName = "Score",
                Width = 90,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });
        }

        public void LoadRows(List<RankingRow> rows)
        {
            dgvRanking.DataSource = rows;
            lblFooter.Text = $"Top {rows.Count} người chơi";
        }

        public void ShowLoading()
        {
            dgvRanking.DataSource = new List<RankingRow>();
            lblFooter.Text = "Dang tai bang xep hang...";
        }
    }
}
