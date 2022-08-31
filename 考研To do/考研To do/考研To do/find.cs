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
    public partial class FindForm : Form
    {
        private RichTextBox RichText = new RichTextBox();
        public FindForm()
        {
            InitializeComponent();
        }
        private void FindForm_Load(object sender, EventArgs e)
        {
            MainForm main = (MainForm)this.Owner;
            RichText = main.richTextBox1;
        }
        public new void Show()
        {
            base.Show();
            if (RichText.SelectionLength != 0)
                textBox1.Text = RichText.SelectedText;
            textBox1.Focus();
            textBox1.SelectAll();
        }
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                startfind.Enabled = false;
            else
                startfind.Enabled = true;
        }
        private void Startfind(bool again = false)
        {
            int findindex, searchindex;
            if (radioButton2.Checked)
            {
                if (again)//判断是循环回归的特殊次
                    searchindex = 0;
                else
                {
                    searchindex = RichText.SelectionStart;//创建局部变量来存开始搜索的索引
                    if (RichText.SelectedText != "")//选中状态（自己选或者搜索到高亮）
                        searchindex += RichText.SelectedText.Length;//跳过选中区域
                }
                if (checkBox1.Checked)//大小写匹配
                    findindex = RichText.Find(textBox1.Text, searchindex, RichTextBoxFinds.MatchCase);
                else
                    findindex = RichText.Find(textBox1.Text, searchindex, RichTextBoxFinds.None);//默认查找
                if (findindex < 0)
                {
                    if (checkBox2.Checked && !again) //判断循环选项
                        Startfind(true);
                    else
                        MessageBox.Show("找不到" + "\"" + textBox1.Text + "\"", "记事本", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (again)
                    searchindex = RichText.TextLength;
                else
                {
                    searchindex = RichText.SelectionStart;
                    if (RichText.SelectedText != "")//选中状态（自己选或者搜索到高亮）
                        searchindex -= RichText.SelectedText.Length;//跳过选中区域
                }
                findindex = RichText.Find(textBox1.Text, 0, searchindex, RichTextBoxFinds.Reverse);//倒着找
                if (findindex < 0)
                {
                    if (checkBox2.Checked && !again)//判断循环
                        Startfind(true);
                    else
                        MessageBox.Show("找不到" + "\"" + textBox1.Text + "\"", "记事本", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public void Startfind_Click(object sender, EventArgs e)
        {
            Startfind();
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Enabled = radioButton2.Checked;
        }

        private void FindForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
