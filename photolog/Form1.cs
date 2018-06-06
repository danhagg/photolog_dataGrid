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
            //listView1.View = View.Details;
            listView1.View = View.Details;
            listView1.Columns.Add("listView1 Column1", 150);
            listView1.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);

            // listView2 PROPERTIES... Details, List, Tiles
            listView2.View = View.Details;
            listView2.Columns.Add("listView2 Column1", 150);
            listView2.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);

            // DataGridView
            //DataGridViewImageColumn dgvImage = new DataGridViewImageColumn();
            //dgvImage.HeaderText = "Image";
            //dgvImage.ImageLayout = DataGridViewImageCellLayout.Stretch;


            //DataGridViewTextBoxColumn dgvId = new DataGridViewTextBoxColumn();
            //dgvId.HeaderText = "Id";

            //dataGridView1.Columns.Add(dgvImage);
            //dataGridView1.Columns.Add(dgvId);

            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //dataGridView1.RowTemplate.Height = 120;

            //dataGridView1.AllowUserToAddRows = false;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            dataGridView1.Columns.Clear();
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

                // clear previous list
                listView1.Items.Clear();

            // IMGLISTS TO HOLD IMAGES
            ImageList imgList1 = new ImageList();
            imgList1.ImageSize = new Size(100, 100);
            listView1.SmallImageList = imgList1;
            listView2.SmallImageList = imgList1;

            string[] files = Directory.GetFiles(fbd.SelectedPath);
            for (int i = 0; i < files.Length; i++)
            {
                imgList1.Images.Add(System.Drawing.Image.FromFile(files[i]));
                string fileName = Path.GetFileNameWithoutExtension(files[i]);
                listView1.Items.Add(fileName, i);

            }
        }

        // METHOD - Move selected items from one List to another.
        private void CopySelectedItems(ListView source, ListView target)
        {
            foreach (ListViewItem item in source.SelectedItems)
            {
                target.Items.Add((ListViewItem)item.Clone());
            }
        }

        //METHOD - Move selected items from one List to dataGrid.
        // https://stackoverflow.com/questions/28747413/retrieving-selected-image-only-from-listview
        private void CopySelectedItemsToGrid(ListView source, DataGridView target)
        {
            foreach (ListViewItem item in source.SelectedItems)
            {
               
                target.Rows.Add((ListViewItem)item.Clone());
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            CopySelectedItems(listView1, listView2);
            //CopySelectedItemsToGrid(listView1, dataGridView1);

        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem eachItem in listView2.SelectedItems)
            {
                listView2.Items.Remove(eachItem);
            }
        }

        private void list_img_SelectedIndexChanged(ListView source, DataGridView target)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var item = listView1.SelectedItems[0];
                var img = item.ImageList.Images[item.ImageIndex];
                //var f = new Form2();
                //f.pictureBox1.Image = img;
                //MessageBox.Show("pause");
                //f.Show();
                dataGridView1.Rows.Add(img);
            }
        }

        private void addDGButton_Click(object sender, EventArgs e)
        {
            list_img_SelectedIndexChanged(listView1, dataGridView1);
        }
    }
}



