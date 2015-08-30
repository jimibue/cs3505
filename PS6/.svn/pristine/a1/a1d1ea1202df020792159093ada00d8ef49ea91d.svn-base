Author James Yeates
Date 11/5/2014

//Version 1.0  10/31/2014 - The GUI is set up and working with basic functions.  Design is good.  Things that need to be worked On
//-cleaning code, more helper methods.
//-a way to clear a cell (cntrl-x)
//backspace is not working esspically in the case when I erase all of the contents. cell won't clear
//error handling and making it look good.
//still have a bug in ps5 not handling circular exceptions well
//key preveiw property

//Version 1.1 11/1/14
//-cleaned code with help private method updateModel and setRowAndCol and cleaned up arrowkey processing
//-added feature to not allow a user to enter a invalid formula or cause a circular dependency.  User must
//resolve this problem before any thing can proceed, probably should not allow a user to save if there is 
// this problem?
// -added basic open functionality opened a process the basic xml file correctly. Except the way my class is it won't
//  allowed a bad file to be uploaded, but this is probably desirable 
// -found a wierd bug that I can't duplicate at the start don't know what I did.
//- over all looking good need to start debugging and do more formal testing.

//Version 1.2 11/3/14
//-fixed two bug in ps5 both had to do with set contents of a cell to an empty string
// to remove them from graph and dict. Also make sure dependents of cell that was changed
// to an empty string was set to a Formulla error
//-add cntrl-x funtion to clear cell in gui and model
//-spreadsheet dependent cells now updates correctly when values are changed
//-Not sure if the circular expcetion bug is fixed. it passed the test but I didn't change anything
//-fixed bug where arrow keys were causing an exception at the lower level.
//-there is still "off by one error" that occurs rarely with cntrl-x key command

//version 1.3 updated to repsitory 11/4/14

//version 1.4 11/5/14
// -fixed most bugs except found a corner case in which I enter =a1 in A2 then put 1 in A1 which
// resolves the formula error I clear A2 completly than try to delete a1 but it won't let me
// -Not happy with my Coded UI test
// -I feel overall this program is fairly robust but not perfect


THOUGHTS, IDEAS, NOTES, ETC
This soultion does not handle values yet just handles the contents of the cell and keeps track
of the spreadsheets cells depndencies.  I noticed that a stack overflow occured when a cell
had about 10000 dependencies.  I think this comes from the recursive call but am not 100% sure.


Changes needed for PS5
	Names of cells -(from AbstracSpreedSheet documentation)
		-One or more letters followed by one or more numbers
		-and must satisfy is valid predicate. ^[a-zA-Z]+[0-9]+$