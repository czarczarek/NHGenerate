using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using NHGenerate.Core;
using Thread = System.Threading.Thread;

namespace NHGenerate.UI
{
    public partial class SearchProgress : Form
    {
        private readonly string[] ExclueExtension = { ".xap", ".tdf", ".resx", ".dll", ".pdb" };

        readonly CancellationTokenSource _cts;
        readonly ParallelOptions _po = new ParallelOptions();
        private Task _task;
        private object obj = new object();

        public DTE2 ApplicationObject { get; set; }
        public AddIn AddInstance { get; set; }
        public List<Procedure> Procedures { get; set; }

        public SearchProgress()
        {
            InitializeComponent();
            btnStart.Visible = false;
            _cts = new CancellationTokenSource();
            _po = new ParallelOptions { CancellationToken = _cts.Token };
        }

        public string HeaderText
        {
            set { lblHeader.Text = value; }
        }




        private void BtnCancelMouseClick(object sender, MouseEventArgs e)
        {
            _cts.Cancel();

            while (_task != null && _task.Status == TaskStatus.Running)
            {
                Thread.Sleep(50);
            }

            Close();
        }

        private void SetHeaderText(string text)
        {
            if (lblHeader.InvokeRequired)
            {
                Invoke(new Action<string>(SetHeaderText), new object[] { text });
            }
            else
            {
                lblHeader.Text = text;
            }
        }

        private void SetProgressText(string text)
        {
            if (lblProgress.InvokeRequired)
            {
                Invoke(new Action<string>(SetProgressText), new object[] { text });
            }
            else
            {
                lblProgress.Text = text;
                ProgressBar.Value += ProgressBar.Step;
                lblCount.Text = string.Format("{0}/{1}", ProgressBar.Value, ProgressBar.Maximum);

                lblProgress.Update();
                ProgressBar.Update();
                lblCount.Update();
            }
        }

        public void InitProgressBar(int maximum)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<int>(InitProgressBar), new object[] { maximum });
            }
            else
            {
                ProgressBar.Maximum = maximum;
                ProgressBar.Value = 0;

                lblCount.Text = string.Format("{0}/{1}", ProgressBar.Value, ProgressBar.Maximum);
            }
        }

        private ProjectItem2 GetFiles(ProjectItem item, ref List<ProjectItem2> projectItems)
        {
            //base case
            if (item.ProjectItems == null)
            {
                if (item.Kind == "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}")
                {
                    var projItem = new ProjectItem2 { Name = item.Name, ProjectName = item.ContainingProject.Name };

                    if (!string.IsNullOrEmpty(item.FileNames[0]))
                    {
                        projItem.Path = item.FileNames[0];
                    }

                    return projItem;
                }
                return null;
            }

            var items = item.ProjectItems.GetEnumerator();
            while (items.MoveNext())
            {
                var currentItem = (ProjectItem)items.Current;

                var pi = GetFiles(currentItem, ref projectItems);
                if (pi != null && !ExclueExtension.Contains(Path.GetExtension(pi.Path)))
                {
                    projectItems.Add(pi);
                }
            }

            if (item.Kind == "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}")
            {
                var projItem2 = new ProjectItem2 { Name = item.Name, ProjectName = item.ContainingProject.Name };
                if (!string.IsNullOrEmpty(item.FileNames[0]))
                {
                    projItem2.Path = item.FileNames[0];
                }

                return projItem2;
            }

            return null;
        }

        public List<SearchObject> Start2(List<string> procedures)
        {
            var projectItems = new List<ProjectItem2>();

            Solution solution = ApplicationObject.Solution;

            SetHeaderText("Searching files to search in existing projects");
            InitProgressBar(solution.Projects.Count);
            Thread.Sleep(250);

            var projects = solution.Projects.GetEnumerator();
            while (projects.MoveNext())
            {
                var project = ((Project)projects.Current).ProjectItems.GetEnumerator();
                SetProgressText(string.Format("Scanning project: \"{0}\"", ((Project)projects.Current).Name));
                while (project.MoveNext())
                {
                    ProjectItem item = (ProjectItem)project.Current;

                    var pi = GetFiles(item, ref projectItems);
                    if (pi != null && !ExclueExtension.Contains(Path.GetExtension(pi.Path)))
                    {
                        projectItems.Add(pi);
                    }
                }
            }

            SetHeaderText("Searching for usages of stored procedures...");
            InitProgressBar(projectItems.Count);
            Thread.Sleep(250);

            try
            {
                var existing = new List<string>();

                var searchObjects = new List<SearchObject>();

                var result = Parallel.ForEach(projectItems, _po, (file, loopState) =>
                {
                    SetProgressText(string.Format("Scanning file: \"{0}\"", Path.GetFileName(file.Name)));

                    using (var fs = new FileStream(file.Path, FileMode.Open, FileAccess.Read))
                    {
                        using (var sr = new StreamReader(fs, Encoding.UTF8))
                        {
                            int i = 1;
                            while (!sr.EndOfStream)
                            {
                                var line = sr.ReadLine();

                                foreach (string p in procedures)
                                {
                                    if (line != null && line.ToUpper().Contains(p.ToUpper()))
                                    {
                                        lock (obj)
                                        {
                                            if (!searchObjects.Any(s => s.Name == p))
                                            {
                                                var so = new SearchObject { Name = p, Occurences = new List<Occurence>() };

                                                var o = new Occurence
                                                {
                                                    File = file.Path,
                                                    FileName = file.Name,
                                                    Project = file.ProjectName,
                                                    Line = i,
                                                    LineValue = line
                                                };

                                                so.Occurences.Add(o);
                                                searchObjects.Add(so);
                                                existing.Add(p);
                                            }
                                            else
                                            {
                                                var so = searchObjects.Single(s => s.Name == p);

                                                var o = new Occurence
                                                {
                                                    File = file.Path,
                                                    FileName = file.Name,
                                                    Project = file.ProjectName,
                                                    Line = i,
                                                    LineValue = line
                                                };

                                                so.Occurences.Add(o);
                                            }
                                        }
                                    }
                                }

                                i++;
                            }
                        }
                    }

                    _po.CancellationToken.ThrowIfCancellationRequested();
                });

                while (!result.IsCompleted)
                {
                    Thread.Sleep(50);
                }

                return searchObjects;
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show(ex.Message);
            }

            return new List<SearchObject>();
        }

        public List<SearchObject> Start(List<string> procedures)
        {

            Solution solution = ApplicationObject.Solution;
            var files = new List<string>();

            SetHeaderText("Searching files to search in existing projects");
            InitProgressBar(solution.Projects.Count);
            Thread.Sleep(250);

            foreach (Project p in solution.Projects)
            {
                SetProgressText(string.Format("Project file: \"{0}\"", p.Name));

                if (!string.IsNullOrEmpty(p.FileName))
                {
                    string dir = Path.GetDirectoryName(p.FileName);
                    if (dir != null && Directory.Exists(dir))
                    {
                        string[] items = Directory.GetFiles(dir, "*.cs", SearchOption.AllDirectories);

                        files.AddRange(items.Except(files));

                        //p.ProjectItems.Cast<ProjectItem>().Where(f => f.F)

                        //items.AddRange(p.ProjectItems.Cast<ProjectItem>().Select(pi => files.SingleOrDefault(f => f.EndsWith(pi.Name))).Where(fileName => fileName != null));
                    }
                }
            }


            SetHeaderText("Searching for usages of stored procedures...");
            InitProgressBar(files.Count);
            Thread.Sleep(250);

            try
            {
                var existing = new List<string>();

                var searchObjects = new List<SearchObject>();

                var result = Parallel.ForEach(files, _po, (file, loopState) =>
                {
                    SetProgressText(string.Format("Scanning file: \"{0}\"", Path.GetFileName(file)));

                    using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        using (var sr = new StreamReader(fs, Encoding.UTF8))
                        {
                            int i = 1;
                            while (!sr.EndOfStream)
                            {
                                var line = sr.ReadLine();

                                foreach (string p in procedures)
                                {
                                    if (line != null && line.ToUpper().Contains(p.ToUpper()))
                                    {
                                        lock (obj)
                                        {
                                            if (!searchObjects.Any(s => s.Name == p))
                                            {
                                                var so = new SearchObject { Name = p, Occurences = new List<Occurence>() };

                                                var o = new Occurence
                                                {
                                                    File = file,
                                                    FileName = Path.GetFileName(file),
                                                    Line = i,
                                                    LineValue = line
                                                };

                                                so.Occurences.Add(o);
                                                searchObjects.Add(so);
                                                existing.Add(p);
                                            }
                                            else
                                            {
                                                var so = searchObjects.Single(s => s.Name == p);

                                                var o = new Occurence
                                                {
                                                    File = file,
                                                    FileName = Path.GetFileName(file),
                                                    Line = i,
                                                    LineValue = line
                                                };

                                                so.Occurences.Add(o);
                                            }
                                        }
                                    }
                                }

                                i++;
                            }
                        }
                    }

                    _po.CancellationToken.ThrowIfCancellationRequested();
                });

                while (!result.IsCompleted)
                {
                    Thread.Sleep(50);
                }

                return searchObjects;
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show(ex.Message);
            }

            return new List<SearchObject>();
        }

        private EnvDTE.Window myToolWindow;
        private void Start()
        {
            const string TOOLWINDOW_GUID = "{6CCD0EE9-20DB-4636-9149-665A958D8A9A}";
            EnvDTE80.Windows2 windows2;
            string assembly;
            object myUserControlObject = null;
            SearchResultControl searchResultControl;

            var proc = Procedures.Select(p => p.Name).ToList();

            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();

            _task = Task.Factory.StartNew(() =>
            {
                return Start2(proc);
            }, _po.CancellationToken).ContinueWith((r) =>
            {
                List<SearchObject> list = r.Result;

                var unused = proc.Except(list.Select(a => a.Name).ToList()).ToList();

                try
                {
                    if (myToolWindow == null) // First time, create it
                    {
                        windows2 = (EnvDTE80.Windows2)ApplicationObject.Windows;

                        assembly = System.Reflection.Assembly.GetExecutingAssembly().Location;

                        myToolWindow = windows2.CreateToolWindow2(AddInstance, assembly,
                           typeof(SearchResultControl).FullName, "Search result", TOOLWINDOW_GUID, ref myUserControlObject);

                        searchResultControl = (SearchResultControl)myUserControlObject;

                        searchResultControl.SearchProgress = this;

                        searchResultControl.SearchObjects = list;
                        searchResultControl.UnusedElements = unused;
                        searchResultControl.Load();
                    }

                    myToolWindow.Visible = true;
                }
                catch (System.Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.ToString());
                }

                //win.Show();

            }, _po.CancellationToken, TaskContinuationOptions.NotOnFaulted, scheduler);
        }

        private void SearchProgress_Load(object sender, EventArgs e)
        {
            Thread.Sleep(50);
            Start();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Start();
        }
    }
}
