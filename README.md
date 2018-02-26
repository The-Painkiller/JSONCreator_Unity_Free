# JSON Creator by Painkiller's Works
****************************************************************************************************************************************

# INTRO:

-This is an in editor tool made for creating simple JSON files without leaving Unity environment.

-The free version includes quickly adding data values to the sheet in the editor window and save it as a .json file in the project.
In paid version you can save each sheet in the project as a .asset file which can be loaded back later for more data manipulation.
This way you can save many data sheets and manipulate them to generate updated versions of your JSON file.

Link to paid version: https://www.assetstore.unity3d.com/#!/content/112354

****************************************************************************************************************************************

# DOCUMENTATION:

- To access the JSON Creator, go to Tools > Painkiller's Works > JSON Creator

- You can directly start editing the blank sheet that shows up in the window that appears.

- To add data to root, click on '+' button besides 'Root'. You can add Integer, Float, String, Boolean, Array and Object data types from root.

- Within Array or Object, you can only add Integer, Float, String and Boolean types, since nesting is currently unavailable in this tool.

- Each data type is shown with a specifc abbreviation in front of it. The abbreviations are as follows:
	(I): Integer
	(F): Float
	(S): String
	(B): Boolean
	(A): Array
	(O): Object



- Boolean data items in the sheet are shown as check boxes. So if your value is True, keep it checked, otherwise unchecked.

- Children of an Array are directly assigned keys between 0-n since arrays don't support keys in subset. 
  For the sake of the consistency of the final JSON file, please refrain from editing these keys.

- Children of an Object are capable of having keys.

- Keys should be unique in a given level of hierarchy.
  For example, in Root, no 2 data items can have "Int" as key. You will have to give them 2 different names.
  On the other hand, a data item in Root and a child data item of an Object, can both have "Int" as their keys, since they don't fall in the same level of hierarchy.

- To remove a single data item from the list in the sheet, simply click on 'x' besides it.

- To clear the whole sheet, click 'clear' besides 'Root' on the top of the sheet.

- To generate a JSON file, simply give a name of the JSON file and press Save JSON. This will create a JSON file in Assets/JSONCreator/JSONs.

- In paid version, you can save the sheet you're working on by giving the sheet a name and then pressing 'Save/Load'.
  This shows a dialog that asks for Save or Load. Save will save the file with the given name in Assets/JSONCreator/Sheets.
  Load will end up loading an existing file in the window, if there exists one on the given path.

