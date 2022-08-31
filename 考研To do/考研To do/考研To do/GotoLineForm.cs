using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 考研ToDo
{
    public partial class GotoLineForm : Form
    {
        static ToolTip toolTip = new ToolTip();
        MainForm mainForm;
        public GotoLineForm()
        {
            InitializeComponent();
        }
        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            toolTip.Hide(textBox1);
            toolTip.IsBalloon = true;
            toolTip.ShowAlways = true;
            toolTip.UseAnimation = true;
            toolTip.UseFading = true;
            toolTip.ToolTipIcon = ToolTipIcon.Error;
            toolTip.ToolTipTitle = "不能接受的字符";
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != 8 && e.KeyChar != 13)
            {
                e.Handled = true;
                toolTip.Show("你只能在此处键入数字。",textBox1,-10+textBox1.TextLength*10,-55);
                System.Media.SystemSounds.Beep.Play();
            }
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            int pos = 0;
            RichTextBox rb = mainForm.richTextBox1;
            string[] str = rb.Text.Split('\r', '\n');
            int target = Int32.Parse(textBox1.Text);
            if (target > 0 && target <= str.Length)
            {
                for (int i = 1; i < target; i++)
                    pos += str[i - 1].Length + 1;
                mainForm.richTextBox1.SelectionStart = pos;
            }
            else
            {
                MessageBox.Show("行数超过了总行数", "记事本 - 跳行", MessageBoxButtons.OK);
                textBox1.Text = str.Length.ToString();
            }
        }

        private void GotoLineForm_Load(object sender, EventArgs e)
        {
            mainForm = (MainForm)this.Owner; 
            textBox1.Text = (mainForm.richTextBox1.GetLineFromCharIndex(mainForm.richTextBox1.SelectionStart)+1).ToString();
        }
    }
}
