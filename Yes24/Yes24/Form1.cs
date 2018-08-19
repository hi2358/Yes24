using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace Yes24
{
    public partial class Form1 : Form
    {
        //yes24Search 전역변수는 button1에서 받아서 webBrowser1_DocumentCompleted에
        //넘겨줘서 처리하기위해 정의
        String yes24Search;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String search = textBox1.Text;

            //euc-kr변환
            String wordsum = null;
            int euckr = 51949;

            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding(euckr);
            byte[] euckrbyte = encoding.GetBytes(search);
            foreach(var b in euckrbyte)
            {
                wordsum = wordsum + "%" + Convert.ToString(b, 16);
            }

            //yes24 검색 파싱 시작
            yes24Search = "http://www.yes24.com/searchcorner/Search?keywordAd=&keyword=&domain=ALL&qdomain=%C0%FC%C3%BC&Wcode=001_005&query=" + wordsum ;
            webBrowser1.Navigate(yes24Search);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            List<string> resultlist = new List<string>();

            HtmlDocument document = this.webBrowser1.Document;
                
            String tableid = "schMid_wrap";
                
            HtmlElement cm = document.GetElementById(tableid);
            HtmlElementCollection trs = cm.GetElementsByTagName("p");

            foreach (HtmlElement el in trs)
            {
                if (el.GetAttribute("className") == "goods_name goods_icon")
                {
                    el.Focus();
                    //MessageBox.Show(el.InnerText);
                    resultlist.Add(el.InnerText);
                }
            }
            int a = 1;
            //파싱된 데이터 Datagridview1에 삽입
            foreach (String result in resultlist)
            {
                dataGridView1.Rows.Add(a, result);
                a=a+1;
            }
        }
    }
}
