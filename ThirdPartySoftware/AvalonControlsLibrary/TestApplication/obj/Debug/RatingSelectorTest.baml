   M S B A M L   `   `   `  ����J  FTestApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:TestApplicationTestApplication  5      (    "TestApplication.RatingSelectorTest   5   	   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      P LAvalonControlsLibrary, Version=2.0.0.0, Culture=neutral, PublicKeyToken=nullHlocal<http://schemas.AvalonControls/AvalonControlsLibrary/Controls  5      +currentclr-namespace:TestApplication  5      -   5          � Title$  RatingSelector��5      $	��600��6   $	��600��6)   .��5      ]� ��. .+�5	   
   �� $	��Top=�6   !   TestApplication.BrowserLink  5
      $B��<http://marlongrech.wordpress.com/2007/12/17/rating-selector/?�6"   .�6m   For more info click here6�   5      � 5   
    � RowDefinitions 5   
   �� 5      �� 5      �� 5      .+�5   
   �  5      �� 5      �� 5      $��20 �6    .+�5      �� ._�5      6  0AC.AvalonControlsLibrary.Controls.RatingSelector  -   5        $��rating��5   '      RateItemColor
 �r���   RateItemHoverColor
 �����   RatingSelected$ 1��6f   5      � 5      $��1��6   .+�5      �� $��Rating Selected��6   $��0,0,5,0q�63   �� 5       ��6   ��  �� ElementName$ rating�� �� Path$ RatingSelected��� 5!      $��10,0,0,0q�6   $��Make read only��6-    d� 	IsChecked 6F   �� $ rating��$ 
IsReadOnly�5"      5#      � 5%   
   $��1��6    5&      �� 5'      $��20 �6    �� 5(      �� 5)      $��20 �6    .+�5,      �� $?��9Change the Look of the control by using Control Templates��6   �� 5.      $��1��6    � 	Resources 5/      :  4AC.AvalonControlsLibrary.Controls.RatingSelectorItem	 �� 
TargetType
 �� Property �� Value �� 
TargetType � Cursor �� RelativeSource �� AncestorType �� CornerRadius �� Triggers �� Value �� Binding �� 
TargetName ^� 
Conditions �� Binding �� Value��%�  '        �� 51      "	  6F   .�52      �� 
 � �� 53      �� 54   "   "  62   .R�55   &   �� $'��!local:RatingSelector.SelectRating��6-   $
 Hand��57   )   ��56   )   ��  �� "  $ 	RateValue���58   *   �� 5:   .   " ��6>   .R�5;   2   �� $
��path��69   
�������$��1q�6W   $ 5��6b   $��10��6s   $��10��6    5<   2   �� 5=   6   $
 True��5>   B    5=   B   ��  �� "  $ 
IsSelected�.a�5?   :   �� $
 path��6A    6i   ��  �� "  $ RateItemColor�
 � ��5@   7   ^� 5B   6    5C   :   �� 5D   >    6H   ��  �� "  $ IsHigherRateMouseOver� True5E   B   �� 5F   >    6H   ��  �� "  $ 
IsSelected�	 False5G   B   �� 5H   >    6H   ��  �� "  $ 
IsReadOnly�	 False5I   A   0�5K   :   �� 5L   >   $
 path��6E    6m   ��  �� "  $ RateItemHoverColor�
 � ��5N   7   5P   /   5S   '   5T   #   5V      5W      5X      ._�5Z        -   5        $��rating2��5Z   '      	MinRating$ 0��66      	MaxRating$ 10��6D   $ 3��6S   ��5[      �� 5\      "  6*   .R�5^      �� $ 5��6%   
����  �$��1q�6I   
������ $��Center=�5_   %   $��Center=�6B    �� Padding$ 5q�6]   ._�5`   "   ��  K� IsItemsHost . 6-   5a      5b      5j      5l      � 5n      $��2��6   .+�5o      �� $��Rating Selected��6   $��0,0,5,0q�63   �� 5p      ��6   �� $ rating2��$ RatingSelected�5q      5r      � 5t   
   $��2��6    5u      �� 5v      �� 5w      .+�5y      �� ._�5z        -   5        $��rating3��5z   '   $ 0��66   $ 8��6D   $ 3��6R   5{      � 5}      $��1��6    5~      �� 5      �� 5�      .+�5�      � .+�5�      �� $��Rating Selected��6    $��0,0,5,0q�67   �� 5�      ��6    �� $ rating3��$ RatingSelected�5�      � 5�      $��1��6   $��Center=�6)   .+�5�      �� $"��Change the range >> Min Rate��6    $��Center=�6D   �� 5�       �� TextAlignment$ Center=�6�   $��5,0,10,0q�6�   $��30��6�   ��6   ��  �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�$ rating3��$ 	MinRating�6�   �� 5�      $��Max Rate��6    $��Center=�60   �� 5�      $ Center=�6�   $��5,0,0,0q�6�   $��30��6�   ��6   �� $ TwoWay=�$ PropertyChanged=�$ rating3��$ 	MaxRating�6�   5�      5�      5�      5�      5�      5�      