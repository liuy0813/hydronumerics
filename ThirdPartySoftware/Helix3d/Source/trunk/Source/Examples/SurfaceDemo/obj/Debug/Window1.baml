   M S B A M L   `   `   `  ����Z  VHelixToolkit, Version=0.9.4175.40396, Culture=neutral, PublicKeyToken=52aa3500039caf0dA0clr-namespace:HelixToolkit;assembly=HelixToolkitHelixToolkit  5      F BSurfaceDemo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:SurfaceDemoSurfaceDemo 5         SurfaceDemo.Window1   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Bhelix0clr-namespace:HelixToolkit;assembly=HelixToolkit     5      +localclr-namespace:SurfaceDemo     5      -   5          �Name$  
thisWindow��5       � Title$ Parametric surfaces��5      $	��720��6!   $
��1280��6.   
���   �
�������.��5	      ]� .+�5
   
   o� -   5        $��mainMenu��5
      $	��Top=�6   .@�5      m� $
��File��6   .@�5      m� $��Open...��6   {�.  6,   m� 5      $��
Save as...��6   {�.  6/   �� 5      m� 5      -   5        $��	Export...��5      6.   �� 5      m� 5      -   5        $��E_xit��5      6*   5      m� 5      $
��View��6   .@�5      m� -   5        $��
ViewSource��5      $��Source��6-    m� IsCheckable .  m� 	IsChecked . 6a   m� 5      -   5        $��Reset camera��5      61   m� 5      -   5        $��
Fullscreen��5      $��Full screen��6-    . 6U   �� 5      m� 5      -   5        $��Normal��5       .  6?   �� *
ViewMode+ �� Mode$ TwoWay=� �� 	Converter )   #HelixToolkit.EnumToBooleanConverter   �� ConverterParameter$ Normal��6�   m� 5      -	   5        $��Stereo��5       .  6>   �� *
ViewMode+$ TwoWay=�   $ Stereo��6�   m� 5      -
   5        $��Anaglyph��5       .  6@   �� *
ViewMode+$ TwoWay=�   $ Anaglyph��6�   5      m� 5      $��Brush��6   .@�5      m� -   5        $	��Hue��5      6(   m� 5      -   5        $��Hue (stripes)��5      62   m� 5      -   5        $��Rainbow��5      6,   m� 5       -   5        $��Rainbow (stripes)��5       66   m� 5!      -   5        $��Pattern��5!      6,   m� 5"      -   5        $��Image...��5"      6-   m� 5#      $��Media...��6   {�.  6-   5$      m� 5%      -   5        $��surfacesMenu��5%   )   $��Surfaces��6   6)   5&      � 5'   
   $
��Left=�6    � ColumnDefinitions 5(      �� 5)      $��1* �6#   �� 5*      $��2* �6#   .+�5,      ]� -   5        $��SourcePanel��5,      .+�5-      � $��Bottom=�6   $��4q�6/    5.      �� 5/      $
��Auto �6+   �� 50      �� 51      $
��Auto �6+    � RowDefinitions 53      �� 54      �� 55      �� 56      �� 57      .+�59      �� $��Elements in 'u' direction��6    $��0��6A   �� 5:      $��Elements in 'v' direction��6    $��1��6A   �� 5;      $��Parameter 'w'��6    $��2��65   �� 5<      $��
Stereobase��6    $��3��62   �� 5=      -   5        $��slider1��5=      $��1��6,   $��0��6<   	 � Minimum$	 10Q�6I   
 � Maximum$	
 400Q�6V   $��	10 0 10 0q�6�    � Value 6d   �� *	MeshSizeU+6�   �� 5>      -   5        $��slider2��5>      $��1��6,   $��1��6<   $	 10Q�6I   $	
 400Q�6V   $��	10 0 10 0q�6�    6d   �� *	MeshSizeV+6�   �� 5?      -   5        $��slider3��5?      $��1��6,   $��2��6<   $	 0Q�6I   $
 2Q�6U   $��	10 0 10 0q�6~    6a   �� *
ParameterW+6~   �� 5@      -   5        $��slider4��5@      $��1��6,   $��3��6<   $	 0.003Q�6I   $	
 1.2Q�6Y   $��	10 0 10 0q�6�    6g   �� *
StereoBase+{�6�   �� *
ViewStereo+6�   �� 5A      $��2��6T   $��0��6d   ��6    �� *	MeshSizeU+ �� StringFormat$ {0:N0}��6d   �� 5B      $��2��6R   $��1��6b   ��6    �� *	MeshSizeV+$ {0:N0}��6b   �� 5C      $��2��6S   $��2��6c   ��6    �� *
ParameterW+$ {0:N2}��6c   �� 5D      $��2��6t   $��3��6�   {�6    �� *
ViewStereo+��6A   �� *
StereoBase+$ {0:N3}��6�   5E      �� 5F      -   5         ��Name$ 	errorList��5F      $��Bottom=�6.   $	��100��6F    �� View 5G      �� 5H      .P�5I      ��  �� Width$ 40��6-   $
��Line��68    �� DisplayMemberBinding 6F   �� *Line+�� 5J      $	 300��6-   $��Error��69    6H   �� *	ErrorText+5K      5M      �� 5N      -   5         ��Name$ source1��5O      $!��Lucida Console, Courier New)�6&   $ �� HorizontalScrollBarVisibility$
 Auto=�5P      " �� VerticalScrollBarVisibility$
 Auto=�6:    �� AcceptsReturn . 5Q   *   5R      �� 5S      $��Right=�6   $��8��67   $��Stretch=�6A   
������ |�.  6v      HelixToolkit.HelixView3D  5U      -   5          Name$ view1��5U       $��1��6/      CameraRotationMode$ VirtualTrackball=�5V         ShowFrameRate . 
���   �   Title 5W      �� *
ModelTitle+   Children. 5Y      (   "HelixToolkit.DefaultLightsVisual3D  &   SurfaceDemo.DynamicCodeSurface3D  5Z      -   5        surface1��    HelixToolkit.MeshElement3D   Fill 5\      �� *Brush+&    HelixToolkit.ParametricSurface3D   	MeshSizeU 5]      �� *	MeshSizeU+   	MeshSizeV 5^      �� *	MeshSizeV+   
ParameterW 5_      �� *
ParameterW+   Source 5`      �� *Text+ �� ElementName$ source1��  �� UpdateSourceTrigger$  PropertyChanged=�5a         HelixToolkit.StereoView3D  5b      -   5        !  Name$! view2��5b   !   $y�Hidden=�60   $��1��6D   
���   �    HelixToolkit.StereoControl"   CameraRotationMode$" VirtualTrackball=�6�   #   
StereoBase# 6g   �� *
StereoBase+6�   !	   HelixToolkit.AnaglyphView3D	  5c      -   5        $ 	 Name$$ view3��5c   #   $y�Hidden=�62   $��1��6F   
���   �$" VirtualTrackball=�6�   # 6i   �� *
StereoBase+6�   5d      5e      5f      