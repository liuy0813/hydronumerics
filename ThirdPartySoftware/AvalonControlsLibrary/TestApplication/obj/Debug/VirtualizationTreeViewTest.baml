   M S B A M L   `   `   `  ����  TestApplication0    *TestApplication.VirtualizationTreeViewTest   5   	   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      P LAvalonControlsLibrary, Version=2.0.0.0, Culture=neutral, PublicKeyToken=nullHlocal<http://schemas.AvalonControls/AvalonControlsLibrary/Controls  5        � Title$   VirtualizationTreeViewTest��5      $	��500��6(   $	��600��65    � 	Resources 5         
SolidBlack �� Color  SolidSilver  
SolidWhite  ExpandCollapseToggleStyle �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius J� SnapsToDevicePixels	 F�Name
 �� Triggers P� Property P� Value �� 
TargetName7  1AC.AvalonControlsLibrary.Controls.TreeListBoxItem �� Padding � DataContext �� RelativeSource �� AncestorType d�Name �� 	ClickMode��%�  &	        &	      &	 ?     &	 ^     7  7  '  J    7  �� 5   
   $ Black��6-   �� 5	   
   $ Silver��6.   �� 5
   
   $ White��6-   �� 5   
   " d�5      .�5      ��  � |� .  5      �� 5       � ��$ 19��5      �� 5       � ��$ 13��5      �� 5       � �� 5      �� 5      " d�6&   .R�5      �� $��19��6!   $��13��5   	   ._�5      �� $��9��6%   $��9��5   
   $ 1��5   
    . 8��  ._�5   "   F� $	 
ExpandPath��6'   $��2,1,1,1q�5      8�� s����    �� �    ��- р� ��- р� @KL ���- @KL ���- ��- �@KL ��- �@KL �� ���- �� ���-     р�     р� �� 	5!      5"      5#      
 5$      P� 5%       �@d�	IsChecked$
 TrueY�5&      . �5'   "   �� $ 
ExpandPath��5(       � ��+ ��    �� �    ��- �@KL ��- �@KL �� 	5)      5*      5,      5.      5/      �� 52   
   "  6   .�53      ��  � �� 54      �� 55      "  6&   .R�56      �� $��Bd��6!   #��z��57      #��z��58      #��z��59      # z��5:      ._�5;      �  6(   ��  �� "  .+�5=   "   d� $ PART_Header��6/   8��  $ Press=�5?      �� 5@   "   5A      5B      
 5D      P� 5E       �@��
IsSelected . . �5G   "   �� $ Bd��6)   # � ��5I       � ���� 5J   "   # � ��5K       � ��5L      P� 5M       � {� .  . �5O   "   �� # � ��5P       � ��5Q      5T      5V      5W      5X      .��5Y      �  � RowDefinitions 5Z   
   �� 5[      $��9* �6   �� 5\       5^   
   ��%    5g      .+�5i   
   3  -AC.AvalonControlsLibrary.Controls.TreeListBox  -   5        $��treeListBox��5i      ��5j      �� 5k      .R�5l      �� $��20��6   
�������._�5m      �� ��6$   ��  �� Path$ 
MyProperty�5n      5o      5r      �� 5t   
   $��1��6   .+�5u      �� -   5        $��Bulk Insert��5u      6+   �� 5v      -   5        $��visualCount��5v      5w      5y      5z      