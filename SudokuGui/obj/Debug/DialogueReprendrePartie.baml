   M S B A M L   `   `   `  ����  	SudokuGui(    "Sudoku.Gui.DialogueReprendrePartie   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � ShowInTaskbar  .   � WindowStartupLocation$ CenterOwner=�5   	    � 
ResizeMode$ NoResize=�5   	    � Title$ Reprendre une partie��5   	   $	��500��5   	   $	��300��5	   	   .��5
      � .+�5   
   &   System.Windows.Controls.DataGrid  -   5        $��listeParties��5      $��0,0,0,35q�6'      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Date sauvegarde��6%      
IsReadOnly .    Width5  /System.Windows.Controls.DataGridLengthConverter$	 275 6P   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6\   �� *Item2+  5      	   
Visibility$	 Hidden=�6%   $ 	Quantité��69    .  6]   �� *Item1+5      �� 5   
   -   5        $��	Reprendre��5      $
��Left=�6%   $	��Top=�6@   $��75��6X   $��
57,439,0,0q�6c   6w   �� 5   
   -   5        $��Annuler��5      $
��Left=�6#   $��157,439,0,0q�6>   $	��Top=�6S   $��75��6k   6v   5      5      