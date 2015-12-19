using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EnvDTE;
using NHGenerate.Core;

namespace NHGenerate.UI
{
    public partial class SearchResultControl : UserControl
    {
        public SearchProgress SearchProgress { get; set; }
        public List<SearchObject> SearchObjects { get; set; }
        public List<string> UnusedElements { get; set; }

        public SearchResultControl()
        {
            InitializeComponent();
        }

        public void Load()
        {
            var images = new ImageList();

            images.Images.Add(ResourceUI._7);
            images.Images.Add(ResourceUI._3);
            images.Images.Add(ResourceUI._6);

            resultView.ImageList = images;

            SearchProgress.Hide();

            if (SearchObjects != null)
            {
                var occurences = new TreeNode[SearchObjects.Count];

                int i = 0;
                foreach (var so in SearchObjects)
                {
                    var node = new TreeNode
                    {
                        Text = string.Format("{0} ({1})", so.Name, so.Occurences.Count)
                    };

                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 0;

                    foreach (var o in so.Occurences)
                    {
                        var fileNode = new TreeNode { Text = string.Format("({0}) {1}", o.Line, o.FileName) };
                        fileNode.ImageIndex = 1;
                        fileNode.SelectedImageIndex = 1;
                        fileNode.Tag = o;

                        var lineNode = new TreeNode { Text = string.Format("{0}", o.LineValue.Trim()) };
                        lineNode.ImageIndex = 2;
                        lineNode.SelectedImageIndex = 2;
                        lineNode.Tag = o;

                        fileNode.Nodes.Add(lineNode);
                        node.Nodes.Add(fileNode);
                    }

                    occurences[i++] = node;
                }

                resultView.Nodes.AddRange(occurences);
            }

            if (UnusedElements != null)
            {

                var sb = new StringBuilder();
                foreach (string element in UnusedElements)
                {
                    sb.AppendLine(string.Format("DROP PROCEDURE {0}", element));
                }

                txtDeleteSql.Text = sb.ToString();
            }
        }

        private void resultView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag as Occurence == null) return;

            try
            {
                var occurence = e.Node.Tag as Occurence;

                var solutionProjects = SearchProgress.ApplicationObject.Solution.Projects;

                var projects = solutionProjects.GetEnumerator();
                while (projects.MoveNext())
                {
                    if (occurence.Project != ((Project)projects.Current).Name)
                    {
                        continue;
                    }

                    var items = ((Project)projects.Current).ProjectItems.GetEnumerator();
                    while (items.MoveNext())
                    {
                        string name = ((ProjectItem)items.Current).Name;

                        var item = FindItem(occurence.File, (ProjectItem)items.Current);
                        if (item != null)
                        {
                            item.Open(Constants.vsViewKindCode);

                            if (item.Document != null)
                            {
                                item.Document.Activate();
                                var ts = SearchProgress.ApplicationObject.ActiveDocument.Selection as TextSelection;
                                if (ts != null)
                                {
                                    ts.GotoLine(occurence.Line);
                                    ts.SelectLine();
                                }
                            }
                            break;
                        }
                    }

                    if (occurence.Project == ((Project)projects.Current).Name)
                    {
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private ProjectItem FindItem(string path, ProjectItem item)
        {
            ProjectItem result = item;

            if (result.FileNames[0] == path && result.Kind == "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}")
            {
                return result;
            }

            if (item.ProjectItems == null)
            {
                return null;
            }

            result = null;
            var items = item.ProjectItems.GetEnumerator();
            while (items.MoveNext())
            {
                var currentItem = (ProjectItem)items.Current;
                string name = currentItem.Name;
                var pi = FindItem(path, currentItem);
                if (pi != null)
                {
                    result = pi;
                    break;
                }
            }

            return result;
        }
    }
}
