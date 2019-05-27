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
                TreeNode tmpRoot = treeView1.Nodes.Add(t.FullName);
                tmpRoot.Tag = t;

                //Fields
                TreeNode tmp = tmpRoot.Nodes.Add("Fields");
                foreach(FieldInfo fi in t.GetRuntimeFields())
                {
                    var tmpFi = tmp.Nodes.Add(string.Format("{3} {2} {1} {0}", fi.Name, fi.FieldType.Name, fi.IsStatic ? "static" : "", (fi.IsPrivate ? "private" : (fi.IsPublic ? "public" : "protected"))));
                    tmpFi.Tag = fi;
                }


                //Methods
                tmp = tmpRoot.Nodes.Add("Methods");
                foreach(MethodInfo mt in t.GetRuntimeMethods())
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
                    var tmpMt = tmp.Nodes.Add(string.Format("{4} {3} {1} {0}({2})", mt.Name, mt.ReturnType.Name, str.ToString(), mt.IsStatic?"static":"", (mt.IsPrivate?"private":(mt.IsPublic?"public":"protected"))));
                    tmpMt.Tag = mt;
                }
            }
        }
    }
}
