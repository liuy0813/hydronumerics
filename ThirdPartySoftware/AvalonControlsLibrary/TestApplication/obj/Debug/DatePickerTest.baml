   M S B A M L   `   `   `  ����J  FTestApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:TestApplicationTestApplication  5      $    TestApplication.DatePickerTest   5   	   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      P LAvalonControlsLibrary, Version=2.0.0.0, Culture=neutral, PublicKeyToken=nullHlocal<http://schemas.AvalonControls/AvalonControlsLibrary/Controls  5      +currentclr-namespace:TestApplication  5        � Title$  DateTimePickerTest��5       � 	Resources 5	         DatepickerTemplate �� 
TargetType2  ,AC.AvalonControlsLibrary.Controls.DatePicker � DataContext �� RelativeSource �� AncestorType � RowDefinitions �� 
IsReadOnly �� Text	   CurrentlySelectedDate��%;  &	        �� 5   
   "  65   .R�5      �  6   ��  �� "   5      �� 5      �� 5      �� 5      .+�5      ]� ��. $��0��61   .+�5      �� $��PART_MonthBack��6,   $
��Prev��6   $
��Left=�6B   �� 5      $��PART_MonthForward��6,   $
��Next��6   $��Right=�6E   �� 5      $��Month: ��6    $��35,0,0,0q�6/   �� 5      ��6    �� *CurrentlyViewedMonth+5      �� 5      $��
PART_Dates��6(   $��1��6    .  6L   �� *CurrentlySelectedDate+��5      �� 5      .R�5      � $	��200��6(   $	��130��64   5      ��5      �� 5       .R�5!      �� 
�������$��1q�6:   $��25��6N   ._�5"   "   �� ��6,   �� *	DayNumber+5#      5$      5&      �� 5'      #��z	 6   $��2��6K   5(      5)      5*      .��5,      ]� ��. .+�5.   
   �� $	��Top=�6   !   TestApplication.BrowserLink  5/      $A��;http://marlongrech.wordpress.com/2007/09/11/wpf-datepicker/?�6"   .�6l   For more info click here6�   50      �� 52   
   .+�54      �� $��Original DatePicker��6   $
��Bold"�6*   �� 55      $5��/This also shows the MinDate and MaxDate feature��6     57      -   5        $��
datePicker��57   4   $��0,10,0,0q�6   $	��200��6(   64   �� 58      $��0,10,0,0q�6   $��Current Selected Date��6!   �� 59      ��6   �� 
 �� ElementName$
 
datePicker�� �� Path$ CurrentlySelectedDate��� 5;      $��MultiSelect DatePicker��6   $
��Bold"�6-     5=      -   5        $��multiSelectDatePicker��5=   4   $��0,10,0,0q�6   $	��200��6(      DatesSelectionMode$ Extended=�6Q   �� 5>      $��0,10,0,0q�6   $��Current Selected Dates��6!   �� 5?      $	��200��6   $	��150��6   ��6&   �� $
 multiSelectDatePicker��$ CurrentlySelectedDates��� 5B      $!��Custom Templated DatePicker��6   $
��Bold"�62   $��0,30,0,0q�6D     5C      -   5        $��datePicker2��5C   4   $	��200��6   $��0,10,0,0q�6"   #��[  6G   �� 5D      $��0,10,0,0q�6   $��Current Selected Date��6!   �� 5E      ��6   �� $
 datePicker2��$ CurrentlySelectedDate�5G      5I      5J      