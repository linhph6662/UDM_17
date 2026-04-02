using System;
using System.Threading;
using System.Windows.Forms;

namespace co_caro
{
 
    public partial class FormServer : Form
    {
        ServerManager server = new ServerManager();

        public FormServer()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            new System.Threading.Thread(() =>
            {
                server.Start(8888);
            }).Start();

            MessageBox.Show("Server đang chạy!");
        }
        private void FormSever_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Thread(() => server.Start(8888)).Start();
            MessageBox.Show("Server đang chạy!");
        }
    }
}