   M S B A M L   `   `   `  ����J  FTestApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:TestApplicationTestApplication  5      P LAvalonControlsLibrary, Version=2.0.0.0, Culture=neutral, PublicKeyToken=nulllJclr-namespace:AC.AvalonControlsLibrary.Core;assembly=AvalonControlsLibraryAC.AvalonControlsLibrary.Core 5      1clr-namespace:TestApplicationTestApplication  5   	   %    TestApplication.ListViewSorting   5   	   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      Kcontrols<http://schemas.AvalonControls/AvalonControlsLibrary/Controls  5      )localclr-namespace:TestApplication  5      UcoreJclr-namespace:AC.AvalonControlsLibrary.Core;assembly=AvalonControlsLibrary  5      +currentclr-namespace:TestApplication  5   	     � Title$  ListViewSorting��5       � 	Resources 5	         HeaderTemplateArrowUp F�Name  HeaderTemplateArrowDown  HeaderTemplateTransparent7  1AC.AvalonControlsLibrary.Core.GridViewSortHandler  sortHandler*   #ColumnHeaderSortedAscendingTemplate+   $ColumnHeaderSortedDescendingTemplate$   ColumnHeaderNotSortedTemplate$   TestApplication.PersonComparer  personComparer;  5AC.AvalonControlsLibrary.Core.DefaultListViewComparer   UseSimpleBinding  defaultComparer��%J  &	        &	 �     &	 �    &	 ^    &	 �    &	 �    �� 5   
   .R�5      ]� .+�5      �� $��Center=�6   ��69   �� F� 5      $ arrow��6   $��1��6&   
�������+����@KL ��� ���� ��� р�� @KL �@KL ��� 	6J   5      5      �� 5   
   .R�5      ]� .+�5      �� $��Center=�6   ��69   �� F� 5      $ arrow��6   $��1��6&   
�������+����@KL @KL р�� ��� ���� @KL �@KL @KL 	6J   5      5      �� 5   
   .R�5      ]� .+�5      �� $��Center=�6   ��69   �� F� 5      $ arrow��6   $��1��6&   
������ +����@KL @KL р�� ��� ���� @KL �@KL @KL 	6M   5      5        5   
   $ HeaderTemplateArrowUp��5    #   $ HeaderTemplateArrowDown��5!   #   $ HeaderTemplateTransparent��5"   #     5%   
   6     5(   
    . 6?   5*      .��5,      ]� ��. .+�5.   
   �� $	��Top=�6   !   TestApplication.BrowserLink  5/      $C��=http://marlongrech.wordpress.com/2007/09/12/listview-sorting/?�6"   .�6n   For more info click here6�   50      �� 52   
   .+�53      �� $:��4Listview with sorting without using custom comparers��6   �� 54   
   -   5        $��	listView1��54   _      GridViewSortHandler# [ 6   $	��100��6p    �� View 55      �� 56      .P�57      >  8AC.AvalonControlsLibrary.Controls.SortableGridViewColumn  $
��Name��66   	   SortPropertyName$
	 Name��6D   
 �� DisplayMemberBinding
 6\   ��  �� Path$
 Name�  58      $��Surame��66   $	 Surname��6F   
 6a   �� $ Surname�59      5;      �� 5=   
   $1��+Listview with sorting using PersonComparer.��6   $��0,5,0,0q�6G   �� 5>   
   -   5        $��	listView2��5A      # [ 5>         CustomComparer# [ 5?      $	��100��5@       5B      �� 5C      .P�5D        $
��Name��66   $
	 Name��6D   
 6\   �� $
 Name�  5E      $��Surame��66   $	 Surname��6F   
 6a   �� $ Surname�5F      5H      �� 5J   
   $6��0Listview with sorting using the DefaultComparer.��6   $��0,5,0,0q�6L   �� 5K   
   -   5        $��	listView3��5N      # [ 5K      # [ 5L      $	��100��5M       5O      �� 5P      .P�5Q        $
��Name��66   $
	 Name��6D   
 6\   �� $
 Name�  5R      $��Surame��66   $	 Surname��6F   
 6a   �� $ Surname�5S      5U      �� 5W   
   -   5        $6��0Load bulk records to test performance of sorting��5W      $��0,5,0,0q�6L   6]   �� 5Y   
   $o��iUse the Load bulk button to stress test the sorting and see which one is faster (i.e the custom sort one)��6   5Z      5\      5]      