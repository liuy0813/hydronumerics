   M S B A M L   `   `   `  ����Z  VHelixToolkit, Version=0.9.4175.40396, Culture=neutral, PublicKeyToken=52aa3500039caf0dA0clr-namespace:HelixToolkit;assembly=HelixToolkitHelixToolkit  5   @   C ?LegoDemo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:HelixToolkitHelixToolkit 6{      LegoDemo.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   >t0clr-namespace:HelixToolkit;assembly=HelixToolkit     6@   ,localclr-namespace:HelixToolkit     6{     � Title$  LegoDemo��6�   $	��480��5   	   $	��640��6   .��5      ]� .+�5   
   o� $	��Top=�6   .@�5      m� $
��File��6   .@�5      m� -   5        $��	Export...��5      6.   �� 5	      m� 5
      -   5        $��E_xit��5
      6*   5      5      � 5   
   .+�5         HelixToolkit.HelixView3D  -   5          Name$ view1��5      
���   �   Children. 5      (   "HelixToolkit.DefaultLightsVisual3D    HelixToolkit.LegoVisual3D  5         Rows$ 40��6%      Columns$ 40��6/      Height$ 1��6<       HelixToolkit.MeshElement3D   Fill
 � � �   	Divisions$ 8��6T       HelixToolkit.PlaneVisual3D  5         Width$
 0.02Q�6A   	   Length$
	 0.02Q�6N   
   Normal$
 0,1,05�6\      Origin$ 0.01,-0.0,0.0131�6k      LengthDirection$ 0,0,-15�6�    6"   &    HelixToolkit.ImageBrushExtension  *
lego.png+6�   5      �� 5       �� Orientation$ 
Horizontal=�6   $
��Left=�62   $	��Top=�6M   .+�5      ��  � ToolTip$
 Rows��6   $��30��6i   $��2q�6t   ��6)   �� *	BrickRows+ �� UpdateSourceTrigger$ PropertyChanged=�6t   �� 5      $ Columns��6   $��30��6o   $��2q�6z   ��6,   �� *BrickColumns+$ PropertyChanged=�6z   �� 5      $ Height��6   $��30��6m   $��2q�6x   ��6+   �� *BrickHeight+$ PropertyChanged=�6x   �� 5      -   5        $��2q�5   5   $��30��6@   ��5      �� 5      $��16��6$   $��16��6/   ��6;   �� *CurrentBrush+5      5      5      5      5       