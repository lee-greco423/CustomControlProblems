# CustomControlProblems
Solution demonstrating build problems with custom controls that use custom class properties

Steps to cause error in the designer:

- Build the solution
- Open WindowsFormsApp1/Form1.cs in the WinForms Designer
- Change size of UserControl1 (works OK)
- Change the size of the container form (works OK)
- Save Form1 (works OK)
- *Rebuild* the solution
- Open WindowsFormsApp1/Form1.cs in the WinForms Designer
- Change size of UserControl1 (fails with message)
- Change the size of the container form (works OK)
- Save Form1 (fails with message)

NOTE: If the ColorPair class is contained in the WindowsFormControlLibrary project then the problem occurs with each build. Placing that class in its own project reduced the need to clear the component cache and restart Visual Studio unless the solution was re-built.
