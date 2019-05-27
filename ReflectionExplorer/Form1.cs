using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReflectionExplorer
{
    public partial class Form1 : Form
    {
        string path;

        public Form1()
        {
            InitializeComponent();
        }

        private void BrowseButton_click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = ".NET Assembly files (*.dll)|*.dll|.NET Executable files (*.exe)|*.exe|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog.FileName;
                InfoHolder.asm = Assembly.LoadFile(path);
                filePath.Text = path;
                updateTreeView();
            }
        }

        public void updateTreeView()
        {
            foreach(Type t in InfoHolder.asm.GetTypes())
            {
                TreeNode tmp = treeView1.Nodes.Add(t.FullName);
                tmp.Tag = t;
                foreach(MethodInfo mt in t.GetMethods())
                {
                    ParameterInfo[] pms = mt.GetParameters();
                    StringBuilder str = new StringBuilder();
                    for(int i=0; i<pms.Length; i++)
                    {
                        str.Append(string.Format("{1} {0}", pms[i].Name, pms[i].ParameterType.Name));
                        if(i != pms.Length - 1)
                        {
                            str.Append(", ");
                        }
                    }
                    tmp.Nodes.Add(string.Format("{4} {3} {1} {0}({2})", mt.Name, mt.ReturnType.Name, str.ToString(), mt.IsStatic?"static":"", (mt.IsPrivate?"private":(mt.IsPublic?"public":"protected"))));
                }
            }
        }
    }
}
