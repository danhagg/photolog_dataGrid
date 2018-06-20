using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Word;
using System.Reflection;
using System.Diagnostics;
using System.Data;
using System.Xml.Linq;

namespace photolog
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string StringA { get; set; }
        string parentFolder { get; set; }

        // Set Form listView and datGridView properties on load
        private void Form1_Load(object sender, EventArgs e)
        {
            // listView1 PROPERTIES... Details, List, Tiles
            listView1.View = System.Windows.Forms.View.Details;
            //listView1.View = System.Windows.Forms.View.Tile;
            //listView1.Columns.Add("", 250);
            //listView1.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
            listView1.MouseDoubleClick += new MouseEventHandler(listView1_MouseDoubleClick);
            this.Load += new EventHandler(Form1_Load);

            // DataGridView
            dataGridView1.RowTemplate.Height = 150;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }


        // BUTTON - Open Folder
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // create instance of folderBrowserDialog class
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            // set root folder
            // fbd.RootFolder = Environment.SpecialFolder.MyDocuments;
            fbd.Description = "Choose an UNZIPPED folder of pictures to upload";

            // check user selects pass
            if (fbd.ShowDialog() == DialogResult.OK)

                // clear previous list
                listView1.Items.Clear();

            // IMGLISTS TO HOLD IMAGES
            ImageList imgList1 = new ImageList();
            imgList1.ColorDepth = ColorDepth.Depth16Bit;
            imgList1.ImageSize = new Size(150, 150);
            listView1.SmallImageList = imgList1;

            string[] files = Directory.GetFiles(fbd.SelectedPath);
            StringA = fbd.SelectedPath;
            Console.WriteLine(StringA);

            for (int i = 0; i < files.Length; i++)
            {
                string fileNameFull = Path.GetFullPath(files[i]);
                ListViewItem item = new ListViewItem(fileNameFull, i);
                imgList1.Images.Add(Image.FromFile(files[i]));
                item.SubItems.Add(i.ToString());
                listView1.Items.Add(item);

            }
        }


        // calculate listView Length
        private void dgLength()
        {
            int dgRows = dataGridView1.Rows.Count;
            textBox1.Text = dgRows.ToString();

        }


        // calculate listView Length
        private void capLength()
        {
            if (dataGridView1.SelectedRows.Count > 0) // make sure user select at least 1 row 
            {
                string cap = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                textBox2.Text = cap.Length.ToString();
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            capLength();
        }



        // View Full-size Image
        void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = listView1.HitTest(e.X, e.Y);
            ListViewItem item = info.Item;

            if (item != null)
            {
                Image newImage = Image.FromFile(item.Text);
                Process.Start(item.Text);
            }
            else
            {
                this.listView1.SelectedItems.Clear();
                MessageBox.Show("No Item is selected");
            }
        }


        // METHOD - Move selected items from listView to dataGridView
        private void list_to_grid(ListView source, DataGridView target)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var item = listView1.SelectedItems[0];
                var img = item.ImageList.Images[item.ImageIndex];
                var pth = item.Text;
                var ind = item.ImageIndex;
                dataGridView1.Rows.Add(img, "Insert Caption Here", pth, ind);
                listView1.SelectedItems[0].Remove();
            }
            dgLength();
        }


        // BUTTON - Move selected items from listView to dataGridView
        private void addDGButton_Click(object sender, EventArgs e)
        {
            list_to_grid(listView1, dataGridView1);
        }


        // METHOD - Move selected items from dataGridView to listView
        private void grid_to_list(DataGridView source, ListView target)
        {
            string patherooney;
            string indexerooney;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                patherooney = row.Cells[2].Value.ToString();
                indexerooney = row.Cells[3].Value.ToString();

                // Need to match the index to put back the correct image
                ListViewItem item = new ListViewItem(patherooney, Int32.Parse(indexerooney));
                item.SubItems.Add(indexerooney.ToString());
                listView1.Items.Add(item);

                // remove from dataGridView
                dataGridView1.Rows.Remove(row);
                dataGridView1.ClearSelection();
            }
            dgLength();
        }


        // BUTTON - Move selected items from dataGridView to listView
        private void removeDGButton_Click(object sender, EventArgs e)
        {
            grid_to_list(dataGridView1, listView1);
        }


        // BUTTON - Up
        private void upButton_Click(object sender, EventArgs e)
        {
            DataGridView dgv = dataGridView1;
            try
            {
                int totalRows = dgv.Rows.Count;
                // get index of the row for the selected cell
                int rowIndex = dgv.SelectedCells[0].OwningRow.Index;
                if (rowIndex == 0)
                    return;
                // get index of the column for the selected cell
                int colIndex = dgv.SelectedCells[0].OwningColumn.Index;
                DataGridViewRow selectedRow = dgv.Rows[rowIndex];
                dgv.Rows.Remove(selectedRow);
                dgv.Rows.Insert(rowIndex - 1, selectedRow);
                dgv.ClearSelection();
                dgv.Rows[rowIndex - 1].Cells[colIndex].Selected = true;
            }
            catch { }
        }


        // BUTTON - Down
        private void downButton_Click(object sender, EventArgs e)
        {
            DataGridView dgv = dataGridView1;
            try
            {
                int totalRows = dgv.Rows.Count;
                // get index of the row for the selected cell
                int rowIndex = dgv.SelectedCells[0].OwningRow.Index;
                if (rowIndex == totalRows - 1)
                    return;
                // get index of the column for the selected cell
                int colIndex = dgv.SelectedCells[0].OwningColumn.Index;
                DataGridViewRow selectedRow = dgv.Rows[rowIndex];
                dgv.Rows.Remove(selectedRow);
                dgv.Rows.Insert(rowIndex + 1, selectedRow);
                dgv.ClearSelection();
                dgv.Rows[rowIndex + 1].Cells[colIndex].Selected = true;
            }
            catch { }
        }





    // BUTTON - CreateWordDoc
    private void button3_Click(object sender, EventArgs e)
        {
            //SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "Word Documents (*.docx)|*.docx";
            //sfd.FileName = ".docx";
            //if (sfd.ShowDialog() == DialogResult.OK)
            {
                //CreateWordDoc(dataGridView1, sfd.FileName);              
                CreateWordDoc(dataGridView1);
                //MessageBox.Show("Creating You Word Document \nPlease Wait!");
            }
        }


        // METHOD - Create the Word doc
        private void CreateWordDoc(DataGridView DGV)
        {
            if (DGV.Rows.Count != 0)
            {
                //Create a missing variable for missing value
                object oMissing = Missing.Value;
                // \endofdoc is a predefined bookmark
                object oEndOfDoc = "\\endofdoc";

                //Start Word and create a new document.          
                _Application oWord = new Word.Application();
                oWord.Visible = true;
                _Document oDoc = oWord.Documents.Add(ref oMissing, ref oMissing,
                ref oMissing, ref oMissing);

                int RowCount = DGV.Rows.Count;
                
                // Iterate over each DataGrid row and extract image path and caption text
                for (int i = 0; i <= RowCount - 1; i++)
                {

                    // make a para as numbered list
                    Paragraph oPara0 = oDoc.Content.Paragraphs.Add(ref oMissing);
                    oPara0.KeepWithNext = 0;
                    oPara0.Format.SpaceAfter=0;
                    Paragraph oPara1 = oDoc.Content.Paragraphs.Add(ref oMissing);
                    Range rngTarget0 = oPara0.Range;
                    Range rngTarget1 = oPara1.Range;
                    rngTarget0.Font.Size = 12;
                    rngTarget0.Font.Name = "Tahoma";
                    rngTarget1.Font.Size = 12;
                    rngTarget1.Font.Name = "Tahoma";
                    object anchor = rngTarget1;
                    
                    rngTarget0.ListFormat.ApplyNumberDefault();


                    // Get image path and caption from dataGridView
                    string fileName1 = DGV.Rows[i].Cells[2].Value.ToString();
                    string caption = DGV.Rows[i].Cells[1].Value.ToString();

                    
                    InlineShape pic = rngTarget1.InlineShapes.AddPicture(fileName1, ref oMissing, ref oMissing, ref anchor);
                    
                    Shape sh = pic.ConvertToShape();
                    sh.LockAspectRatio = Microsoft.Office.Core.MsoTriState.msoCTrue;

                    // Windows runs as default at 96dpi (display) Macs run as default at 72 dpi (display)
                    // Assuming 72 points per inch
                    // 3.5 inches is 3.5*72 = 252
                    // 3.25 inches is 3.25*72 = 234

                    sh.Height = 252;

                    if (sh.Width >400)
                    {
                        sh.Width = 400;
                    }

                    sh.Left = (float)WdShapePosition.wdShapeCenter;
                    sh.Top = 0;

                    //Write substring into Word doc with a bullet before it.
                    rngTarget0.InsertBefore(caption + "\v");
                    oPara1.Format.SpaceAfter = 264;
                    //rngTarget1.InsertParagraphAfter();                
                }               
            }
        }

        
        // Create Datable of ListView
        private System.Data.DataTable GetDataTableFromLV(ListView lv)
        {
            System.Data.DataTable dt1 = new System.Data.DataTable();
            dt1.Columns.Add("listView1Path", typeof(string));
            dt1.Columns.Add("listView1ImageNumber", typeof(string));

            object[] cellValues1 = new object[lv.Columns.Count];
            foreach (ListViewItem item in lv.Items)
            {
                for (int i = 0; i < item.SubItems.Count; i++)
                {
                    cellValues1[i] = item.SubItems[i].Text;
                }
                dt1.Rows.Add(cellValues1);
            }
            return dt1;
        }


        // Create Datable of datagridViewView
        private System.Data.DataTable GetDataTableFromDGV(DataGridView dgv)
        {
            System.Data.DataTable dt2 = new System.Data.DataTable();
            dt2.Columns.Add("dataGridView1Bitmap", typeof(string));
            dt2.Columns.Add("dataGridView1Caption", typeof(string));
            dt2.Columns.Add("dataGridView1Path", typeof(string));
            dt2.Columns.Add("dataGridView1ImageNumber", typeof(string));

            object[] cellValues2 = new object[dgv.Columns.Count];
            foreach (DataGridViewRow row in dgv.Rows)
            {
                for (int ii = 0; ii < row.Cells.Count; ii++)
                {
                    cellValues2[ii] = row.Cells[ii].Value;
                }
                dt2.Rows.Add(cellValues2);
            }
            return dt2;
        }

        // Create single variable for parent folder
        private System.Data.DataTable GetDataTableFromFolder()
        {
            System.Data.DataTable dt0 = new System.Data.DataTable();
            dt0.Columns.Add("parentFolder", typeof(string));
            dt0.Rows.Add(StringA);         
            return dt0;
        }


        // BUTTON - save method
        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataSet dS = new DataSet();
            System.Data.DataTable dT0 = GetDataTableFromFolder();
            System.Data.DataTable dT1 = GetDataTableFromLV(listView1);
            System.Data.DataTable dT2 = GetDataTableFromDGV(dataGridView1);

            dS.Tables.Add(dT0);
            dS.Tables.Add(dT1);
            dS.Tables.Add(dT2);
            
            // Console.WriteLine(dT1);
            // Save a single variable... Filepath
            // String directoryPath = Path.GetDirectoryName();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML files(.xml)|*.xml|all Files(*.*)|*.*";
            saveFileDialog.Title = "Save work as .XML file";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                dS.WriteXml(File.Open(saveFileDialog.FileName, FileMode.CreateNew));
            }
        }

        private void resumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FolderBrowserDialog fbd = new FolderBrowserDialog();
            //fbd.Description = "Choose an your photolog .XML file";

            OpenFileDialog ofd = new OpenFileDialog();


            ofd.Filter = "XML Files (*.xml)|*.xml";
            ofd.FilterIndex = 1;
            //ofd.Multiselect = false;

            // check user selects pass
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //Console.WriteLine("This {0}", ofd.FileName.ToString());
                //Console.WriteLine("This {0}", ofd.FileName);
                string xmlFileName = ofd.FileName;

                //string fileName = Path.GetFileName(path);
                Console.WriteLine(xmlFileName);
                //Console.WriteLine(StringA);

                listView1.Items.Clear();
                XDocument doc = XDocument.Load(xmlFileName);
                //Console.WriteLine(doc);

                // IMGLISTS TO HOLD IMAGES
                ImageList imgList1 = new ImageList();
                imgList1.ColorDepth = ColorDepth.Depth16Bit;
                imgList1.ImageSize = new Size(150, 150);
                listView1.SmallImageList = imgList1;

                // Add required columns
                //listView1.Columns.Add("listView1Path");
                //listView1.Columns.Add("listView1ImageNumber");

                foreach (var dm0 in doc.Descendants("Table1"))
                {
                    string parentFolder = dm0.Element("parentFolder").Value;
                }
                Console.WriteLine(parentFolder);

                foreach (var dm1 in doc.Descendants("Table2"))
                {
                    
                    string fileNameFull = dm1.Element("listView1Path").Value;
                    int i = Int32.Parse(dm1.Element("listView1ImageNumber").Value);
                    Console.WriteLine(fileNameFull);
                    Console.WriteLine(i);

                    ListViewItem item = new ListViewItem(fileNameFull, i);
                    //imgList1.Images.Add(Image.FromFile(parentFolder[Int32.Parse(i)]));
                    imgList1.Images.Add(Image.FromFile(fileNameFull));
                    item.SubItems.Add(i.ToString());
                    listView1.Items.Add(item);
                    Console.WriteLine("item", item.ToString());

                    //for (int i = 0; i < files.Length; i++)
                    //{
                    //    string fileNameFull = Path.GetFullPath(files[i]);
                    //    ListViewItem item = new ListViewItem(fileNameFull, i);
                    //    imgList1.Images.Add(Image.FromFile(files[i]));
                    //    item.SubItems.Add(i.ToString());
                    //    listView1.Items.Add(item);

                }

                //foreach (var dm2 in doc.Descendants("Table2"))
                //{

                //}


            }

        }   
    }
}




