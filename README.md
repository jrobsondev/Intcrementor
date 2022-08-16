# Intcrementor

Intcrementor is a Visual Studio 2022 extension that allows for the incrementation/decrementation of integers within the currently open file.

It was written as a learning experience on how to create a VS extension as well as helping me with code refactoring in my day-to-day work (mainly reordering columns in PDFs, grids etc.)

**Features**
- Increment all - `CTRL + NUM_MINUS` - Automatically decrements all integers by the value specified in the options.
- Decrement all - `CTRL + NUM_PLUS` - Automatically increments all integers by the value specified in the options.
- Update Range - `CTRL + NUM_TIMES` - Displays a popup in which you can set the range of numbers to be modified and the step by which they should be changed.

**Options**
- Default Step - The default step that should be used when using the increment/decrement all functionality as well as the default value in the update range popup.
- Integer length - The length of the integer that should be modified. This defaults to 2. e.g. 11 = length of 2, 111 = length of 3, etc.

---

**Future development**
- Add ability to specify custom regex for finding numbers
- Step through modification - Similar to find and replace when replacing one value at a time i.e. the editor window jumps to th enext match and allows you to choose whether to increment/decrement and by how much
- Modify selected text only
- Add text to Range popup that specifies how many records will be affected on update.
