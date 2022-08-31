using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace 考研ToDo
{
    public partial class MainForm : Form
    {
        public FindForm findForm = new FindForm();
        public ReplaceForm replaceForm = new ReplaceForm();
        public static int Zoom = 10;
        public MainForm()
        {
            InitializeComponent();
            findForm.Owner = this;
            replaceForm.Owner = this;
            转到GToolStripMenuItem.Enabled = false;
            richTextBox1.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
            if (Clipboard.GetText()=="")//判断剪贴板是否为空（粘贴选项En/disable）
                PastePToolStripMenuItem.Enabled = false;
            else
                PastePToolStripMenuItem.Enabled = true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void IndexChanged(object sender,EventArgs e)
        {
            /*SelectionStart返回的是从选中第一个字符开始的光标位置索引
             * GetLineFromCharIndex(index)方法会返回index索引的行号(从0开始)
             * GetFirstCharIndexOfCurrentLine()方法会返回当前光标行的第一个字符的索引
             */
            int line = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart)+1;
            int column = richTextBox1.SelectionStart-richTextBox1.GetFirstCharIndexOfCurrentLine()+1;
            toolStripStatusLabel3.Text = "  第 " + line.ToString()+" 行，第 "+column.ToString()+" 列";
            if(richTextBox1.SelectedText.Length>0)
            {
                CopyToolStripMenuItem.Enabled = true;
                CutToolStripMenuItem.Enabled = true;
                DelToolStripMenuItem.Enabled = true;
            }
            else
            {
                CopyToolStripMenuItem.Enabled = false;
                CutToolStripMenuItem.Enabled = false;
                DelToolStripMenuItem.Enabled = false;
            }
            if(Clipboard.GetText()=="")
                PastePToolStripMenuItem.Enabled = false;
            else
                PastePToolStripMenuItem.Enabled = true;
        }
        private DialogResult AskChangeSave()
        {
            DialogResult choice;
            if (openfile.isopen)
            choice = MessageBox.Show("是否将改动保存到 " + openfile.openpath, "保存", MessageBoxButtons.YesNoCancel);
            else
            choice = MessageBox.Show("是否将改动保存到 " + openfile.name, "保存", MessageBoxButtons.YesNoCancel);
            if (choice == DialogResult.Yes)
            {
                if (Save())
                    return DialogResult.Yes;
                else
                    return DialogResult.Cancel;
            }
            else return choice;
        }
        private bool Save()//true保存成功 false取消保存
        {
            if (!openfile.isopen)//未打开文件（无标题）
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "文本文件|*.txt";
                if (saveFile.ShowDialog() == DialogResult.OK)//选择保存路径
                {
                    richTextBox1.SaveFile(saveFile.FileName, RichTextBoxStreamType.PlainText);
                    richTextBox1.Modified = false;
                }
                else return false;//文件不选
            }
            else//打开了直接保存
            {
                richTextBox1.SaveFile(openfile.openpath, RichTextBoxStreamType.PlainText);
                richTextBox1.Modified = false;
            } 
            return true;//保存完成
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(richTextBox1.Modified)
            {
                if (AskChangeSave() == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
            findForm.Dispose();
            replaceForm.Dispose();
            e.Cancel = false;
        }
        private void 文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(richTextBox1.Modified)
            {
                if(AskChangeSave()==DialogResult.Cancel)
                    return;
            }
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "文本文件|*.txt";
            if (open.ShowDialog()==DialogResult.OK)
            {
                richTextBox1.Clear();
                StreamReader sr = new StreamReader(open.FileName, Encoding.GetEncoding("gbk"));
                string str = sr.ReadLine();
                while (str != null)
                {
                    richTextBox1.AppendText(str + "\n");
                    str = sr.ReadLine();
                }
                openfile.openFileChangeType(open);
                this.Text = openfile.name+" - 记事本";
                sr.Close();
                richTextBox1.Modified = false;
            }
        }
        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox1.CanUndo)
                撤销ZToolStripMenuItem.Enabled = true;
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }
        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(richTextBox1.Modified)
            {
                if(AskChangeSave()==DialogResult.Cancel)
                    return;
            }
            openfile.isOpenInit();
            richTextBox1.Clear();
            this.Text = "To do";
            openfile.name = "无标题";
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "文本文件|*.txt";
            if (saveFile.ShowDialog() == DialogResult.OK)//选择保存路径
            {
                richTextBox1.SaveFile(saveFile.FileName, RichTextBoxStreamType.PlainText);
                openfile.openFileChangeType(saveFile);
                this.Text = openfile.name+" - 记事本";
                richTextBox1.Modified = false;
            }
        }

        private void FontDialog1_Apply(object sender, EventArgs e)
        {

        }

        private void 页面设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PageSetupDialog pageSetupDialog = new PageSetupDialog();
            pageSetupDialog.PageSettings = new System.Drawing.Printing.PageSettings();
            pageSetupDialog.ShowDialog();
        }

        private void 打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDialog print= new PrintDialog();
            print.ShowDialog();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 格式ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 字体ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FontDialog font = new FontDialog();
            if(font.ShowDialog()==DialogResult.OK)
                richTextBox1.Font = font.Font;
        }

        private void 自动换行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.WordWrap = 自动换行ToolStripMenuItem.Checked;
            if (!richTextBox1.WordWrap)
                richTextBox1.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
            else
                richTextBox1.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
            转到GToolStripMenuItem.Enabled = !richTextBox1.WordWrap;
        }

        private void MenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void 撤销ZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }
        private void 前进ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
        }
        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }
        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }
        private void PastePToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void 使用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://cn.bing.com/?scope=web&FORM=NPCTXT");
        }
        //子窗口传值
        private void 查找FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            findForm.Show();
        }

        private void 查找下一个ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (findForm.textBox1.TextLength == 0)
                findForm.Show();
            else
                findForm.Startfind_Click(findForm, new EventArgs());
        }

        private void 替换RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            replaceForm.Show();
        }

        private void ZTLMenu_CheckedChanged(object sender, EventArgs e)
        {
            if (ZTLMenu.Checked)
                statusStrip1.Show();
            else
                statusStrip1.Hide();
        }

        private void 查看帮助HToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://go.microsoft.com/fwlink/?LinkId=834783");
        }

        private void 放大ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Zoom<50)
            {
                Zoom++;
                richTextBox1.ZoomFactor = 0.1F * Zoom;
                toolStripStatusLabel4.Text = Zoom * 10 + "%";
            }
        }

        private void 缩小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Zoom>1)
            {
                Zoom--;
                richTextBox1.ZoomFactor = 0.1F * Zoom;
                toolStripStatusLabel4.Text = Zoom * 10 + "%";
            }
        }

        private void 恢复默认缩放ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Zoom = 10;
            richTextBox1.ZoomFactor = 1;
            toolStripStatusLabel4.Text = 100 + "%";
        }

        private void 关于记事本AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void 全选AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        private void 时间日期ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            richTextBox1.SelectedText = now.ToShortTimeString().ToString() + " " + now.ToShortDateString().ToString();
        }

        private void 转到GToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GotoLineForm gotoLine = new GotoLineForm();
            gotoLine.Owner = this;
            gotoLine.ShowDialog();
        }

        private void Rtbmenu_Opened(object sender, EventArgs e)
        {
            RichTextBox rtb = (RichTextBox)rtbmenu.SourceControl;
            撤销ToolStripMenuItem.Enabled = rtb.CanUndo;
            if(rtb.SelectionLength!=0)
            {
                汉字重选ToolStripMenuItem.Enabled = true;
            }
            else
            {
                汉字重选ToolStripMenuItem.Enabled = false;
            }
            if(rtb.ImeMode==ImeMode.Close)
            {
                打开输入法ToolStripMenuItem.Visible = true;
                关闭输入法toolStripMenuItem1.Visible = false;
            }
            else if(rtb.ImeMode==ImeMode.On)
            {
                打开输入法ToolStripMenuItem.Visible = false;
                关闭输入法toolStripMenuItem1.Visible = true;
            }
            if (rtb.SelectionLength != 0) 
            {
                剪切ToolStripMenuItem.Enabled = true;
                复制ToolStripMenuItem.Enabled = true;
                删除DToolStripMenuItem.Enabled = true;
            }
            else
            {
                剪切ToolStripMenuItem.Enabled = false;
                复制ToolStripMenuItem.Enabled = false;
                删除DToolStripMenuItem.Enabled = false;
            }
        }

        private void 撤销ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = (RichTextBox)rtbmenu.SourceControl;
            rtb.Undo();
        }

        private void 剪切ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = (RichTextBox)rtbmenu.SourceControl;
            rtb.Cut();
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = (RichTextBox)rtbmenu.SourceControl;
            rtb.Copy();
        }

        private void 粘贴PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = (RichTextBox)rtbmenu.SourceControl;
            rtb.Paste();
        }

        private void 删除DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = (RichTextBox)rtbmenu.SourceControl;
            rtb.SelectedText = "";
        }

        private void 全选AToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = (RichTextBox)rtbmenu.SourceControl;
            rtb.SelectAll();
        }

        private void 从右到左的阅读顺序RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = (RichTextBox)rtbmenu.SourceControl;
            switch(从右到左的阅读顺序RToolStripMenuItem.Checked)
            {
                case true:rtb.RightToLeft = RightToLeft.Yes;break;
                case false:rtb.RightToLeft = RightToLeft.No;break;
            }
        }

        private void 打开输入法ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = (RichTextBox)rtbmenu.SourceControl;
            rtb.ImeMode = ImeMode.OnHalf;
        }

        private void 关闭输入法toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = (RichTextBox)rtbmenu.SourceControl;
            rtb.ImeMode = ImeMode.Close;
        }

        private void 汉字重选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = (RichTextBox)rtbmenu.SourceControl;
            int temp = rtb.SelectionLength;
            rtb.SelectionLength = 0;
            rtb.SelectionStart += temp;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }
    }
    public static class openfile
    {
        public static string openpath;//当前打开文件路径
        public static string name="无标题";//当前打开文件名
        public static bool isopen = false;//是否打开了文本
        public static void openFileChangeType(OpenFileDialog open)//打开文件改变类型
        {
            openpath = open.FileName;
            name = Path.GetFileName(open.FileName);
            isopen = true;
        }
        public static void openFileChangeType(SaveFileDialog save)//另存为后进入
        {
            openpath = save.FileName;
            name = Path.GetFileName(save.FileName);
            isopen = true;
        }
        public static void isOpenInit()//打开文件初始化
        {
            openpath = null;
            isopen = false;
        }
    }
}
