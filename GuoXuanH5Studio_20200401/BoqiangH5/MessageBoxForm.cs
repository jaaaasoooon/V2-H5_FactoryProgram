using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoqiangH5
{
    public partial class MessageBoxForm : Form
    {
        int counter = 0;
        int second = 0;
        Timer timer;
        public MessageBoxForm(string text, string title, int milliseconds)
        {
            InitializeComponent();
            second = milliseconds / 1000;
            label1.Text = text;
            this.Text = title;
            counter = 0;
            btnOK.Text = string.Format("确定({0})", second - counter);
            timer = new Timer();
            timer.Enabled = true;
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();
            counter++;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // 如果没有到达指定的时间限制
            if (this.counter <= this.second)
            {
                // 刷新按钮的文本
                this.btnOK.Text = string.Format("确定({0})", this.second - this.counter);
                this.Refresh();
                // 计数器自增
                this.counter++;
            }
            // 如果到达时间限制
            else
            {
                // 关闭timer
                this.timer.Enabled = false;
                this.timer.Stop();
                // 关闭对话框
                this.Close();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static void Show(string msg, string caption, int milliseconds)
        {
            MessageBoxForm form = new MessageBoxForm( msg,caption, milliseconds);
            form.ShowDialog();
        }
    }
}
