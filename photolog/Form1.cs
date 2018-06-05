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

namespace photolog
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // listView1 PROPERTIES... Details, List, Tiles
            listView1.View = View.Details;
            listView1.Columns.Add("listView1 Column1", 150);
            listView1.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);

            // listView2 PROPERTIES... Details, List, Tiles
            listView2.View = View.Details;
            listView2.Columns.Add("listView2 Column1", 150);
            listView2.Columns.Add("listView2 Column2", 300);
            listView2.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);

            // listView2 PROPERTIES... Details, List, Tiles
            listView3.View = View.Details;
            listView3.Columns.Add("listView3 Column1", 300);
            //listView2.Columns.Add("listView2 Column2", 200);
            listView3.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
        }

        private void buttonOpenFolder_Click(object sender, EventArgs e)
        {
            // create instance of folderBrowserDialog class
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            // set root folder
            // fbd.RootFolder = Environment.SpecialFolder.MyDocuments;
            fbd.Description = "Choose a folder of pictures to upload";
            // check user selects pass
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                // selected path most import property in folderBrowserDialog is SelectedPath
                // MessageBox.Show(fbd.SelectedPath);

            // clear previous list
            listView1.Items.Clear();

            // IMGLISTS TO HOLD IMAGES
            ImageList imgList1 = new ImageList();
            imgList1.ImageSize = new Size(100, 100);
            listView1.SmallImageList = imgList1;
            listView2.SmallImageList = imgList1;

            //string[] files = Directory.GetFiles(fbd.SelectedPath);

            //foreach (string file in files)
            //{
            //    imgList1.Images.Add(Image.FromFile(file));
            //    string fileName = Path.GetFileNameWithoutExtension(file);
            //    int indexer;
            //    int.TryParse(file, out indexer);
            //    // change below '0' to each filename iteration
            //    // listView1.Items.Add(fileName, 0);
            //    listView1.Items.Add(fileName, indexer);
            //    Console.WriteLine(indexer);
            //}

            string[] files = Directory.GetFiles(fbd.SelectedPath);
            for (int i = 0; i < files.Length; i++)
            {
                imgList1.Images.Add(System.Drawing.Image.FromFile(files[i]));
                string fileName = Path.GetFileNameWithoutExtension(files[i]);
                //listView1.Items.Add(files[i], i);
                listView1.Items.Add(fileName, i);
            }
        }

        // Move selected items from one List to another.
        private void CopySelectedItems(ListView source, ListView target)
        {
            foreach (ListViewItem item in source.SelectedItems)
            {
                target.Items.Add((ListViewItem)item.Clone());
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            CopySelectedItems(listView1, listView2);
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem eachItem in listView2.SelectedItems)
            {
                listView2.Items.Remove(eachItem);
            }
        }

        private void clearTextButton_Click(object sender, EventArgs e)
        {
            listView3.Items.Clear();
        }
    }
}

