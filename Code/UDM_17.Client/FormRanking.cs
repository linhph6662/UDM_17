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
            dgvRanking.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Rank", DataPropertyName = "Rank", Width = 60 });
            dgvRanking.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "UserId", DataPropertyName = "UserId", Width = 120 });
            dgvRanking.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Username", DataPropertyName = "Username", Width = 100 });
            dgvRanking.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "DisplayName", DataPropertyName = "DisplayName", Width = 120 });
            dgvRanking.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Score", DataPropertyName = "Score", Width = 80 });
        }

        public void LoadRows(List<RankingRow> rows)
        {
            dgvRanking.DataSource = rows;
            lblFooter.Text = $"Top {rows.Count} người chơi";
        }
    }
}
