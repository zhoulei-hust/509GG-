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
    public partial class ReplaceForm : Form
    {
        private RichTextBox RichText;
        MainForm main;
        public ReplaceForm()
        {
            InitializeComponent();
        }
       
        public new void Show()
        {
            base.Show();
            if (RichText.SelectionLength != 0)
                textBox1.Text = RichText.SelectedText;
            if (main.findForm.textBox1.TextLength != 0)
                textBox1.Text = main.findForm.textBox1.Text;
            textBox1.Focus();
            textBox1.SelectAll();
        }
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                startfind.Enabled = false;
            else
                startfind.Enabled = true;
        }

        private void ReplaceForm_Load(object sender, EventArgs e)
        {
            main = (MainForm)Owner;
            RichText = main.richTextBox1;
        }

        private void ReplaceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.TextLength != 0 && textBox2.TextLength != 0)
            {
                button1.Enabled = true;
                button2.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
                button2.Enabled = false;
            }
        }
        private bool Startfind(bool again = false, bool allreplace = false)//第一个参数是循环回溯特例查询，第二个参数是全部替换
        {
            int findindex, searchindex;
            if (!again)
            {
                searchindex = RichText.SelectionStart;//一定要创建这个局部变量来存开始搜索的索引
                if (RichText.SelectedText != "")//选中状态（自己选或者搜索到高亮）
                    searchindex += RichText.SelectedText.Length;//跳过选中区域
            }
            else
                searchindex = 0;
            if (checkBox1.Checked)//大小写匹配
                findindex = RichText.Find(textBox1.Text, searchindex, RichTextBoxFinds.MatchCase);
            else
                findindex = RichText.Find(textBox1.Text, searchindex, RichTextBoxFinds.None);//默认查找
            if (findindex < 0)
            {
                if (checkBox2.Checked && !again && !allreplace)  //判断循环选项
                    return Startfind(true);
                else
                {
                    if (!allreplace)
                        MessageBox.Show("找不到" + "\"" + textBox1.Text + "\"", "记事本", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            return true;
        }
        private void Startfind_Click(object sender, EventArgs e)
        {
            Startfind();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (RichText.SelectionLength != 0)
            {
                if (RichText.SelectedText == textBox1.Text)
                {
                    RichText.SelectedText = textBox2.Text;
                    RichText.Select(RichText.SelectionStart - textBox2.TextLength, textBox2.TextLength);
                }
            }
            Startfind();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            RichText.SelectionStart = 0;
            while (Startfind(false, true))//表示全部替换，不需要提示没找到
            {
                if (RichText.SelectedText == textBox1.Text)
                    RichText.SelectedText = textBox2.Text;
            }
        }
    }
}

