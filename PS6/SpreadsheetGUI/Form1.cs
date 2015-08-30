///CS3500 PS6 Author James Yeates
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SS;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;

//see ReadMe File for more specific information about versions
//Version 1.0  10/31/2014 
//Version 1.1 11/1/14
//Version 1.2 11/3/14
//version 1.3 11/4/14
//version 1.4 11/5/14



namespace SpreadsheetGUI
{
    /// <summary>
    /// This is a simple spreadsheet GUI
    /// special features: Arrow keys can be used to navigate cells and also will 
    /// change the view and update the model if need be
    /// -enter can be use to update model
    /// -cntrl-x will delete the selected cell from model if it exist (ie contents are not empty)
    /// 
    /// </summary>
    public partial class SpreadsheetGUI : Form
    {
       
        /// <summary>
        /// keeps track of the previous row
        /// </summary>
        private int row;
        /// <summary>
        /// keeps track of the previous col
        /// </summary>
        private int col;
        /// <summary>
        /// Model view of the spreadsheet
        /// </summary>
        AbstractSpreadsheet SSModel;
      
        /// <summary>
        /// Keeps track if a cell has been changed
        /// </summary>
        private bool cellChanged;
        /// <summary>
        /// constructor 
        /// </summary>
        public SpreadsheetGUI()
        {
            

            SSModel = new Spreadsheet(s => Regex.IsMatch(s, @"^[a-zA-Z][1-9][0-9]?$"), s => s.ToUpper(), "ps6");
            cellChanged = false;
            InitializeComponent();

            // This an example of registering a method so that it is notified when
            // an event happens.  The SelectionChanged event is declared with a
            // delegate that specifies that all methods that register with it must
            // take a SpreadsheetPanel as its parameter and return nothing.  So we
            // register the displaySelection method below.

            // This could also be done graphically in the designer, as has been
            // demonstrated in class.
            spreadsheetPanel1.SelectionChanged += displaySelection;
            spreadsheetPanel1.SetSelection(0, 0);
            displayCurrentCell(0, 0);
            
        }

        // Every time the selection changes, this method is called with the
        // Spreadsheet as its parameter.  We display the current time in the cell.

        private void displaySelection(SpreadsheetPanel ss)
        {
            bool succesfull = true;
            int newRow, newCol;
            //String value;
            // if a cell has been updated then add it to the model and
            //update the gui
            if(cellChanged)
            {
                succesfull = updateModel(row, col);
            }
            //if model was succesfully changed than make changes to the GUI
            if (succesfull)
            {
                //get the newly selected cell
                ss.GetSelection(out newCol, out newRow);
                displayCurrentCell(newCol, newRow);

                //save cells' postion
                row = newRow;
                col = newCol;
            }
            //model was not succesfull updated keep user on the incorrect cell
            else
            {
                spreadsheetPanel1.SetSelection(col, row);
            }
        }
        /// <summary>
        /// this controls the functionalty of the arrow keys
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //cont-x command to clear selected cell
            if (keyData == (Keys.Control | Keys.X))
            {
                textBox_content.Text = "";
                updateModel(row, col);
                spreadsheetPanel1.SetSelection(col, row);
                return true;
            }
            
            bool succesfull = true;
            //see if a arrow was pressed and if bounds are ok
            if (keyData == Keys.Up && row > 0 || keyData == Keys.Down && row < 98 ||
                keyData == Keys.Right && col < 25 || keyData == Keys.Left && col > 0)
            {
                //
                if (cellChanged) { 
                    //will be set to false if something goes wrong
                    succesfull = updateModel(row, col);
                }
                if (succesfull)
                {
                    if (keyData == Keys.Up) { row -= 1; }
                    else if (keyData == Keys.Down) { row += 1; }
                    else if (keyData == Keys.Right) { col += 1; }
                    else if (keyData == Keys.Left) { col -= 1; }

                    displayCurrentCell(col,row);
                    return true;
                }
               
            }
            return false;
        }
        /// <summary>
        /// used to update the GUI to show the appropriate values for the cell
        /// that is Currently selected
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>

        private void displayCurrentCell(int col, int row)
        {
            textBox_content.Focus();//brings cursor to text box already there?
            textBox_content.SelectionStart = textBox_content.Text.Length;//sets cursor at the end of the word
            string value = "";
            cellChanged = false;
            
            //get the newly selected cell
            spreadsheetPanel1.SetSelection(col, row);
            spreadsheetPanel1.GetValue(col, row, out value);
            textBox_value.Text = value;

            //get newName
            string name = getCellName(row, col);
            textBox_cellName.Text = name;
            
            textBox_content.Text = getCellContents(row, col);
             
        }
        
         /// <summary>
       /// Deals with the new menu.  Opens a new spreadsheet while keeping the previous spreadsheet
       /// open
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tell the application context to run the form on the same
            // thread as the other forms.
            DemoApplicationContext.getAppContext().RunForm(new SpreadsheetGUI());
        }

        /// <summary>
        /// deals with the close menu. Checks if a changed has been made
        /// if so as the user if they want to save otherwise closes the program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// overrides the closing method in order to see if the user needs to be prompted to 
        /// save the sheet
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (SSModel.Changed)
            {
                //with help from msdn documentation
                //ask to save program
                String message = "Would you like to save before closing?";
                //create a "yes/no" box
                var result = MessageBox.Show(message, "Closing", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                    saveFile();
            }
            
        }
   
        /// <summary>
        /// opens up a dialog given that the save menu item is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile();
        }
        /// <summary>
        /// helper method Deals with saving a file
        /// </summary>

        private void saveFile()
        {
            //note Filter set up in Form1.cs[Design]
            saveFileDialog1.ShowDialog();
            //check to see that user entered something
            if (saveFileDialog1.FileName != "") 
                  SSModel.Save(saveFileDialog1.FileName);

        }
        /// <summary>
        /// deals with open a new file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //check if orignal file has been changed if so ask if they want to save.
            String message = "Would you like to save before closing?";
            var result1 = MessageBox.Show(message, "Closing", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result1 == DialogResult.Yes)
                saveFile();
           
            
            //With help from www.dotnetperls.com
            //show dialog
            DialogResult result = openFileDialog1.ShowDialog();
            //check result
            if (result == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                
                try
                {
                    //make new spreadsheet
                    SSModel = new Spreadsheet(fileName, s => Regex.IsMatch(s, @"^[a-zA-Z][1-9][0-9]?$"), s => s.ToUpper(), "ps6");
                    //fill in the cells in the GUI
                    foreach (String cellName in SSModel.GetNamesOfAllNonemptyCells())
                    {
                        setRowAndCol(cellName);
                        spreadsheetPanel1.SetValue(col, row, SSModel.GetCellValue(cellName).ToString());
                    }

                    //set up the spreadsheet
                    spreadsheetPanel1.SetSelection(0, 0);
                    displayCurrentCell(0, 0);

                }catch(Exception ex )
                {
                    MessageBox.Show("An error occured:\n "+ ex.Message);
                }
            }
        }
        /// <summary>
        /// handles enter key and back space keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void valueTextBox_KeyPressed(object sender, KeyPressEventArgs e)
        {
            bool succesfull = true;
            //enter Key pressed see if cell has changed and needs to be updated
            if (e.KeyChar == (char)(Keys.Enter))
            {
                if (cellChanged)
                {
                    //will be set to false if something goes wrong
                    succesfull = updateModel(row, col);
                }
            }
            
            else
            {
                //this was a hack to deal with key characters bing pressed
                if (e.KeyChar == (char)Keys.Back) 
                {
                    if(textBox_content.Text.Length > 0)
                        spreadsheetPanel1.SetValue(col, row, textBox_content.Text.Substring(0,textBox_content.Text.Length-1));
                }
                else {
                    spreadsheetPanel1.SetValue(col, row, textBox_content.Text + e.KeyChar);
                }
                cellChanged = true;
              
            }
            //spreadsheetPanel1.GetValue(col, row, out cellString);

        }

        /// <summary>
        /// return the name of the cell. ie A1
        /// </summary>
        /// <returns>string-name of the cell</returns>
        private string getCellName(int row, int col)
        {
            return "" + (char)(col + 65) + (row + 1);
        }
        /// <summary>
        /// given a cell name set the row and col. IE given A1 
        /// row will be set to 0 and col will be set to 0
        /// </summary>
        
        private void setRowAndCol(String cellName)
        {
            col = (int)cellName[0] - 65;
            //get the number
            string rowString = cellName.Substring(1,cellName.Length-1);
            row = Int16.Parse(rowString) - 1;
        }
        /// <summary>
        /// gets the contents of the cell
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private string getCellContents(int row, int col)
        {
            string cell = getCellName(row, col);
            if( SSModel.GetCellContents(cell) is Formula)
                 return "="+SSModel.GetCellContents(cell).ToString();
            return  SSModel.GetCellContents(cell).ToString();
        }
        /// <summary>
        /// helper method that redraws the spread sheet panel
        /// </summary>
        private void updateSpreadsheetGUI()
        {
            //save the current cell because method below might change these
            int saveRow = row;
            int saveCol = col;
            foreach(string cell in SSModel.GetNamesOfAllNonemptyCells())
            {
                setRowAndCol(cell);
                spreadsheetPanel1.SetValue(col, row, SSModel.GetCellValue(cell).ToString());

            }
            //done updating spreading sheet set row and col to
            //intial values
            row = saveRow;
            col = saveCol;
        }

        /// <summary>
        /// This method given the row and col updates the Model and catches
        /// Circular exception and invalid formula exception.  
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>

        private bool updateModel(int row, int col)
        {
            try
            {
                string content = textBox_content.Text;
                
                SSModel.SetContentsOfCell(getCellName(row, col), content);
                updateSpreadsheetGUI();
               
                textBox_content.Text = getCellContents(row, col);
                
                textBox_value.Text = SSModel.GetCellValue(getCellName(row, col)).ToString();
                spreadsheetPanel1.SetValue(col, row, textBox_value.Text);
                cellChanged = false;
                return true;
            }
            catch (FormulaFormatException e)
            {
                // SSModel.SetContentsOfCell(getCellName(row, col), "");
                // textBox_content.Text = "";
                MessageBox.Show(textBox_content.Text.Substring(1) + " is an invalid formula please change it");
                return false;
                
            }


            catch (CircularException e)
            {
                MessageBox.Show("Found a Circular Depenedency");
                textBox_content.Text = "";
                spreadsheetPanel1.SetValue(col, row, SSModel.GetCellValue(getCellName(row, col)).ToString());
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Sorry an Unexpected error occurred " + e.Message);
                return true;
            }
        }
        /// <summary>
        /// Display help info to the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void infoMenuStripItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To enter a value in a empty cell simply click on the desired cell or "+
                           "use the arrow keys to navigate to the desired cell and enter the value."  
                           +"  If the desired cell is" +" not in view use the scrollbars to find it."
                           +"  If the newly selected cell "+
                           "contains a value use the content text box to edit it or"
            +               " hit control-x to delete the contents and it will then be treated as "
                            + "an empty cell.   Selecting a new cell with "
            +
                              "the mouse or arrow keys will cause the current value in the cell to"+
                              " be added to spreadsheet.  Hitting enter will also do this as well "+
                              "except will leave the currently selected cell the same");
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }



    }
}
