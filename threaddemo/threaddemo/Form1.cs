using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace threaddemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //取消跨线程检查
            Control.CheckForIllegalCrossThreadCalls = false;
            button4.Enabled = false;
            button5.Enabled = false;
        }

        private CTest _test;

        private int idx = 1;

        private htTest ht;


        private DateTime dtclear;


        private void TextShowNew(RichTextBox tbShow, string sMsg, int color = 0)
        {
                //当文本行数大于500后清空
                if (tbShow.Lines.Length > 500)
                {
                    tbShow.Clear();
                }

                switch (color)
                {
                    case 0:
                        tbShow.SelectionColor = Color.Black;
                        break;
                    case 1:
                        tbShow.SelectionColor = Color.Red;
                        break;
                    case 2:
                        tbShow.SelectionColor = Color.Blue;
                        break;
                    default:
                        tbShow.SelectionColor = Color.Black;
                        break;
                }

                string ShowMsg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "  " + sMsg + "\r\n";

                tbShow.AppendText(ShowMsg);

                //让文本框获取焦点 
                tbShow.Focus();
                //设置光标的位置到文本尾 
                tbShow.Select(tbShow.TextLength, 0);
                //滚动到控件光标处 
                tbShow.ScrollToCaret();


        }


        private void Openinf()
        {
            try
            {
                if (_test != null)
                {
                    _test.Stop();
                    _test.Dispose();
                }
                _test = null;
                _test = new CTest(idx);
                _test.DataReceived += _test_DataReceived;
                _test.Start();
                
                TextShowNew(richTextBox1, idx + "开启");
                idx++;
            }
            catch (Exception ex)
            {
                TextShowNew(richTextBox1, "Error:" + ex.Message, 1);
            }
        }

        private void _test_DataReceived(object sender, testEvent e)
        {

            try
            {
                TextShowNew(richTextBox1, e.datastr);
                int count = -1;
                try
                {
                    count = Convert.ToInt32(e.datastr);
                }
                catch(Exception ex)
                {
                    count = -1;
                }


                if (count >= 0)
                {
                    CItem item = new CItem(0, count, "测试" + count, DateTime.Now);
                    ht.checkHashTest(item, 5000);
                }

                if (!ht.checkDatetime(dtclear, 5000))
                {
                    ht.RemoveHashTableFromTime(5000);
                    dtclear = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                TextShowNew(richTextBox1, ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dtclear = DateTime.Now;
            ht = new htTest(listBox1);

            Openinf();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(_test!=null) _test.Stop();
            //_test.Dispose();
            Thread.Sleep(1000);
            System.Environment.Exit(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(_test != null)
            {
                _test.Reset();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (_test != null)
            {
                _test.Stop();
            }
        }

        

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string name = listBox1.SelectedItem.ToString();
                ht.removeHashCardInfo(name);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int index = listBox1.IndexFromPoint(e.X, e.Y);
            listBox1.SelectedIndex = index;
            if (listBox1.SelectedIndex != -1)
            {
                button4.Enabled = true;
                button5.Enabled = true;
            }
            else
            {
                button4.Enabled = false;
                button5.Enabled = false;
            }
        }
    }
}
