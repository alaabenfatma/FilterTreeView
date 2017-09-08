using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace ExpTreeView
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            var Root = new ObservableCollection<NodeItem>
            {
                new NodeItem(@"Numbers")
            };
            tv.ItemsSource = Root;
            b.Click += (sender, args) =>
            {
                Root.Add(new NodeItem("Numbers in [20,100]"));
                for (var i = 0; i < 100; i++)
                    Root[0].Nodes.Add(new NodeItem(i.ToString()));
                for (var i = 20; i < 100; i++)
                    Root[1].Nodes.Add(new NodeItem(i.ToString()));
            };

            tb.TextChanged += (sender, args) =>
            {
                foreach (var root in Root)
                {
                    if (root.BackUpNodes.Count == 0)
                        foreach (NodeItem item in root.Nodes)
                            root.BackUpNodes.Add(item);

                    foreach (NodeItem item in root.BackUpNodes)
                        if (!item.NodeName.Contains(tb.Text))
                        {
                            for (var i = 0; i < root.DeletedItems.Count; i++)
                            {
                                var deleteditem = root.DeletedItems[i];
                                if (deleteditem.NodeName.Contains(tb.Text))
                                {
                                    root.DeletedItems.Remove(deleteditem);
                                    root.Nodes.Add(deleteditem);
                                }
                            }
                            root.Nodes.Remove(item);
                            root.DeletedItems.Add(item);
                        }
                        else
                        {
                            for (var i = 0; i < root.DeletedItems.Count; i++)
                            {
                                var deleteditem = root.DeletedItems[i];
                                if (deleteditem.NodeName.Contains(tb.Text))
                                    root.DeletedItems.Remove(deleteditem);
                            }
                        }
                    if (tb.Text == "")
                    {
                        root.Nodes.Clear();
                        foreach (NodeItem item in root.BackUpNodes)
                            root.Nodes.Add(item);
                    }
                }
                
            };
        }
    }


    public class NodeItem
    {
        public CompositeCollection BackUpNodes = new CompositeCollection();
        public IList<NodeItem> DeletedItems = new List<NodeItem>();
        public CompositeCollection Nodes = new CompositeCollection();

        public NodeItem(string name)
        {
            NodeName = name;
        }

        public string NodeName { get; set; }

        public IList Children => Nodes;
    }
}