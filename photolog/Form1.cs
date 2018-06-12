using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Word;
using System.Reflection;
using System.Diagnostics;



namespace photolog
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        // Set Form listView and datGridView properties on load
        private void Form1_Load(object sender, EventArgs e)
        {
            // listView1 PROPERTIES... Details, List, Tiles
            listView1.View = System.Windows.Forms.View.Details;
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
            //int totalRows = dataGridView1.Rows.Count;
            // get index of the row for the selected cell
            //int rowIndex = dataGridView1.SelectedCells[0].OwningRow.Index;
            //int colIndex = dataGridView1.SelectedCells[0].OwningColumn.Index;
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
        //private void CreateWordDoc(DataGridView DGV, string filename)
        private void CreateWordDoc(DataGridView DGV)
        {
            if (DGV.Rows.Count != 0)
            {
                //Create a missing variable for missing value
                object oMissing = Missing.Value;
                // \endofdoc is a predefined bookmark
                object oEndOfDoc = "\\endofdoc";

                //Start Word and create a new document.
                Word._Application oWord;
                Word._Document oDoc;
                oWord = new Word.Application();
                oWord.Visible = true;
                oDoc = oWord.Documents.Add(ref oMissing, ref oMissing,
                ref oMissing, ref oMissing);
                int RowCount = DGV.Rows.Count;
                int ColumnCount = DGV.Columns.Count;


                // iterate over DG, 
                //insert image at diff locations as to whether 
                //i % 2==0 or != 0
                // Iterate over each DataGrid row and extract image path and caption text
                for (int i = 0; i <= RowCount - 1; i++)
                {
                    Word.Paragraph oPara;
                    oPara = oDoc.Content.Paragraphs.Add(ref oMissing);
                    string fileName1 = DGV.Rows[i].Cells[2].Value.ToString();
                    string caption = DGV.Rows[i].Cells[1].Value.ToString();
                    int figureNumber = DGV.Rows[i].Index;
                    string figNum = (figureNumber + 1).ToString();
                    object oRngP = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
                    oPara.Range.Text = "\v" + figNum + ". " + caption;
                    var pic = oPara.Range.InlineShapes.AddPicture(fileName1, ref oMissing, ref oMissing, ref oMissing);
                    Console.WriteLine("width is {0}", pic.Width);
                    oPara.Format.SpaceAfter = 12;
                    oPara.Range.InsertParagraphAfter();

                    if ((i+1) % 2 == 0)
                    {
                        oDoc.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
                    }                  
                }


                // Document settings - printed to Console
                var maxHeight = oDoc.PageSetup.PageHeight - oDoc.PageSetup.BottomMargin;
                var leftMargin = oDoc.PageSetup.LeftMargin;
                var rightMargin = oDoc.PageSetup.RightMargin;
                var pageWidth = oDoc.PageSetup.PageWidth;

                Console.WriteLine("max height is {0}", maxHeight);
                Console.WriteLine("pageWidth is {0}", pageWidth);
                Console.WriteLine("left margin is {0}", leftMargin);
                Console.WriteLine("right margin is {0}", rightMargin);
                // set Word image size and position


                float picHeight;
                float ratio;
                float diff;
                foreach (InlineShape inline in oDoc.InlineShapes)
                {
                    if (inline.Height >= 300)
                    {
                        picHeight = inline.Height;
                        ratio = 300 / picHeight;
                        inline.Height = 300;
                        inline.Width = inline.Width * ratio;
                    }
                    //pageWidth = 612
                    // 2*72 for margins
                    //612 - (2*72) = 468
                    //if (inline.Width < 468)
                    //{
                    //    inline.
                    //    diff = 468 - inline.Width;
                    //    // center the pic

                    //}
                }

               


                //Add header into the document
                //foreach (Microsoft.Office.Interop.Word.Section section in oDoc.Sections)
                //{
                //    //Get the header range and add the header details.
                //    Microsoft.Office.Interop.Word.Range headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                //    headerRange.Fields.Add(headerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
                //    headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                //    headerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdBlue;
                //    headerRange.Font.Size = 10;
                //    headerRange.Text = "PronetGroup";
                //}

                ////Add the footers into the document
                //foreach (Microsoft.Office.Interop.Word.Section wordSection in oDoc.Sections)
                //{
                //    //Get the footer range and add the footer details.
                //    Microsoft.Office.Interop.Word.Range footerRange = wordSection.Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                //    footerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdDarkRed;
                //    footerRange.Font.Size = 10;
                //    footerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                //    footerRange.Text = "Report";
                //}


                //save the file
                //oDoc.SaveAs2(filename);

                //Close this form.
                //this.Close();
            }
        }
    }
}




