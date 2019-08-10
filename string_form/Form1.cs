using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace string_form
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label1.Text = "将strings.xml放入‘英语’文件夹\r\n按下<导出>会生成strings_eng.txt\r\n将对应的翻译放入‘翻译'文件夹\r\n并改名为strings_trans.txt\r\n按下<替换>即可生成strings_trans.xml ";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String conpath = "./英语/strings.xml";//定义要获取的文件内容地址
            getContentToFile(conpath);//将strings.xml内容写入到自己的文件中`
            MessageBox.Show("生成string_eng.txt");
        }
        public static void getContentToFile(String conpath)
        {

            FileStream fs = null;
            StreamReader sr = null;
            FileStream stream = File.Open("./英语/strings_eng.txt", FileMode.OpenOrCreate, FileAccess.Write);
            stream.Seek(0, SeekOrigin.Begin);
            stream.SetLength(0);
            stream.Close();
            StreamWriter sw = new StreamWriter("./英语/strings_eng.txt", true);
            try
            {
                String content = String.Empty;
                fs = new FileStream(conpath, FileMode.Open);
                sr = new StreamReader(fs);
                int a = 0;
                while ((content = sr.ReadLine()) != null)
                {
                    int i = content.IndexOf(">");
                    //int x = content.IndexOf("<",9);
                    if (i == -1) continue;
                    int x = content.IndexOf("<",9);
                    if (x == -1) continue;
                    string[] sArray = content.Split('>');
                    string[] sArray1 = sArray[1].Split('<');
                    sw.WriteLine("m "+a+"  "+ sArray1[0].ToString());
                    //sw.WriteLine(sArray1[0].ToString());
                    a++;
                }
            }
            catch
            {
                Console.WriteLine("读取内容到文件方法错误");
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
                if (sw != null)
                {
                    sw.Close();
                }
            }

        }

        /*
         * 读取时，首先要获取textBox1的字符
         * 然后和textBox2对比，如果一样，则将textBox3替换写入textBox1
        */
        private void button2_Click(object sender, EventArgs e)
        {
            StreamReader orgCodeStream = new StreamReader("./英语/strings.xml", Encoding.UTF8);
            StreamReader jpStream = new StreamReader("./英语/strings_eng.txt", Encoding.UTF8);
            StreamReader chsStream = new StreamReader("./翻译/strings_trans.txt", Encoding.UTF8);
            String jp = null;
            String chs = null;
            String newCode = null;
            String temp = null;
            while (true)
            {
                jp = jpStream.ReadLine();
                chs = chsStream.ReadLine();
                if (String.IsNullOrEmpty(jp) || String.IsNullOrEmpty(chs)) break;
                while (true)
                {
                    temp = orgCodeStream.ReadLine();
                    if (temp.Equals(null)) System.Console.WriteLine("Error1");
                    if (temp.IndexOf("\">") != -1) break;
                    else if (!String.IsNullOrEmpty(newCode)) newCode = String.Concat(newCode, "\r\n", temp);
                    else newCode = String.Concat(newCode, temp);
                }
                string[] sArray = temp.Split('>');
                sArray[1] = sArray[1].Replace(jp.Substring(jp.IndexOf("  ") + 2), chs.Substring(chs.IndexOf("  ") + 2));
                // temp = temp.Replace(jp.Substring(jp.IndexOf("  ") + 2), chs.Substring(chs.IndexOf("  ") + 2));
                if (!String.IsNullOrEmpty(newCode)) newCode = String.Concat(newCode, "\r\n", sArray[0] + ">" + sArray[1] + ">");
                else newCode = String.Concat(newCode, sArray[0] + sArray[1]);
            }

            temp = orgCodeStream.ReadLine();
            while (!String.IsNullOrEmpty(temp))
            {
                if (!String.IsNullOrEmpty(newCode)) newCode = String.Concat(newCode, "\r\n", temp);
                else newCode = String.Concat(newCode, temp);
                temp = orgCodeStream.ReadLine();
            }
            newCode = String.Concat(newCode, "\r\n");

            StreamWriter sw = new StreamWriter("./翻译/strings_trans.xml");
            sw.Write(newCode);
            sw.Flush();
            sw.Close();
            jpStream.Close();
            chsStream.Close();
            orgCodeStream.Close();

            MessageBox.Show("替换成功");
        }
    }
}
/*            
            StreamReader reader = new StreamReader("strings.xml");
            String a = reader.ReadToEnd();
            a = a.Replace("Factory Menu", "工厂菜单");
            StreamWriter readTxt = new StreamWriter("strings1.xml", false);
            readTxt.Write(a);
            readTxt.Flush();
            readTxt.Close();
            reader.Close();
 */
