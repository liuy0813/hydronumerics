   M S B A M L   `   `   `  ����Z  VHelixToolkit, Version=0.9.4175.40396, Culture=neutral, PublicKeyToken=52aa3500039caf0dA0clr-namespace:HelixToolkit;assembly=HelixToolkitHelixToolkit  5      T PHelixToolkit.Input, Version=0.9.4175.40409, Culture=neutral, PublicKeyToken=nullS<clr-namespace:HelixToolkit.Input;assembly=HelixToolkit.InputHelixToolkit.Input 5       
SimpleDemo   SimpleDemo.Window1   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Bhelix0clr-namespace:HelixToolkit;assembly=HelixToolkit     5      S
helixInput<clr-namespace:HelixToolkit.Input;assembly=HelixToolkit.Input     5        � Title$  
SimpleDemo��5      $	��480��6   $	��640��6%   .��5      �� $��8q�6   .@�5   
   �� $��Information��6   .��5	      ,� .��5
      /� .T�5      J� .*�6    31This demo shows usage of the HelixView3D control.5      J� 5      .*�6    .*�5      
See the �� -   5        $4��.http://helixtoolkit.codeplex.com/documentation?�5   $   .�6�   documentation6�   0. for information on how to control the camera.5      5      5      5      �� 5   
   $��	HeadLight��6   .��5         HelixToolkit.HelixView3D     IsHeadLightEnabled .    Children. 5         HelixToolkit.CubeVisual3D     
SideLength$ 4Q�6%       HelixToolkit.MeshElement3D   Fill
 ��  �64   5      5      �� 5   
   $��Default light setup��6   .��5        -   5          Name$ view2��5       
���   �. 5      (   "HelixToolkit.DefaultLightsVisual3D    5      $ 4Q�6%      HelixToolkit.BoxVisual3D   Center$ 0,0,2.51�64       HelixToolkit.PlaneVisual3D  5         	DivLength$	 128��6&      DivWidth$	 128��66   	   Origin$	 0,0,01�6E   
   Width$
 10Q�6T      Length$ 10Q�6_   
 �����6k   5      5       �� 5!   
   $��Binding��6   .��5"      ]� -   5         ]�Name$ bindingPanel��5"      .+�5#      ��  �� Orientation$ 
Horizontal=�6   $��Bottom=�66   $��8q�6N   .+�5$      �� $��
Cube size:��6   �� 5%      -   5        $��slider1��5%      $	��200��6,    � Maximum$ 10Q�68    � Value$ 2Q�6E   $��8 0 0 0q�6O   5&      � 5'      .+�5)        -   5        $ view1��5)   (    .  � DataContext 5+      ��  �� ElementName$ slider1��   ShowFrameRate 5,      �� *	IsChecked+$ showFramerate��   ShowFieldOfView 5-      �� *	IsChecked+$ showFieldOfView��   ShowCoordinateSystem 5.      �� *	IsChecked+$ showCoordSystem��   ShowViewCube 5/      �� *	IsChecked+$ showViewCube��   InfiniteSpin 50      �� *	IsChecked+$ infiniteSpin��. 52        -   5        	cube1��
 ��  � 52   <   �� *Value+6Y     53      $ -6,-6,01�6-   $	 0.3Q�6>   
 � � �6O     54      $ -6,6,01�6-   $	 0.3Q�6=   
 � � �6N     55      $ 6,-6,01�6-   $	 0.3Q�6=   
 � � �6N     56      $ 6,6,01�6-   $	 0.3Q�6<   
 � � �6M   57      �� 58      $
��Left=�6!   $	��Top=�6<   $��8q�6T   .+�59      �� -   5        $��showFramerate��59   #   $��Show framerate��68   �� 5:      -   5        $��showFieldOfView��5:   #   $��Show field of view (FOV)��6:   �� 5;      -	   5        $��showCoordSystem��5;   #   $��Show coordinate system��6:   �� 5<      -
   5        $��showViewCube��5<   #   $��Show view cube��67   �� 5=      -   5        $��infiniteSpin��5=   #   $��Infinite spin��67   5>      5?      5@      5A      �� 5B   
   $��Camera decorator��6   .��5C      ]� .+�5D      �� $ 
Horizontal=�6   $��Bottom=�66   $��8q�6N   .+�5E      �� $��
Cube size:��6   �� 5F      -   5        $��slider2��5F      $	��200��6,   $ 10Q�68   $ 2Q�6E   $��8 0 0 0q�6O   5G      � 5H       � RowDefinitions 5I      �� 5J      �� 5K      .+�5M      �� $��Perspective camera��6   '   !HelixToolkit.Viewport3DController  5N      .��5O      '�  '� Camera 5P      =� 5Q   "    � LookDirection$ -10,-10,-105�64    � Position$ 10,10,101�6P    � UpDirection$ 0,0,15�6d   .x�5S          5T      
 ��  � 61   �� *Value+$ slider2��6c   5U      5W      �� 5X      $��Orthographic camera��6   $��1��6:     5Y      $��1��61   .��5Z      '�  5[      P� 5\   "   $ -10,-10,-105�65   $ 10,10,101�6Q   $ 0,0,15�6e    P� Width$ 12Q�6y   .x�5^          5_      
 ��  � 61   �� *Value+$ slider2��6c   5`      5a      5b      5c      5d      �� 5e   
   $��Orthographic Camera��6   .��5f      � .+�5g           Orthographic 6$   �� *	IsChecked+$ UseOrthographicCamera��. 5h          5i      $ 4Q�6)   
 ��  �68   5j      �� 5k      $
��Left=�6   $	��Top=�68   $��8q�6P   .+�5l      �� -   5        $��UseOrthographicCamera��5l      $��Orthographic camera��6<   5m      5o      5p      �� 5q   
   $��Camera modes��6   .��5r      �  � ColumnDefinitions 5s      �� 5t      �� 5u      �� 5v      .+�5x           
CameraMode$ Inspect=�6$       Title$  Inspect��69   . 5y          5z      $ 4Q�6)   
 ��  �68   5{        5|      $ 
WalkAround=�6$   $  
WalkAround��6<   $��1��6O   . 5}          5~      $ 4Q�6)   
 ��  �68   5        5�      $ FixedPosition=�6$   $  FixedPosition��6?   $��2��6U   . 5�          5�      $ 4Q�6)   
 ��  �68   5�      5�      5�      �� 5�   
   $��Space navigator��6   .��5�      ]� .+�5�      �� $��Bottom=�6I   ��6   �� *Type+$ 	navigator��6I   0  *HelixToolkit.Input.SpaceNavigatorDecorator  5�      -   5        !  Name$! 	navigator��5�   5   ._�5�         . . 5�        $ 4Q�6-   
 ��  �6<   5�      5�      5�      5�      �� 5�   
   $��Transformed model��6   .��5�         . . 5�      g� " g� 	Transform" 5�      S� 5�      # S� OffsetX$# 4Q�6/   $ S� OffsetZ$$ 3Q�6;   .2�5�        $ 4Q�6)   
 ��  �" 5�      Y� 5�      .�5�   "   S� $# 2Q�67   $$ 1Q�6C   �� 5�   "   % �� Rotation% 5�   &   �� 5�   *   & �� Axis$& 0,1,15�6>   ' �� Angle$' 30Q�6K   5�   #   S� 5�   "   $$ 3Q�67   5�      5�      5�      +	   %HelixToolkit.CoordinateSystemVisual3D	  5�      5�      5�      �� 5�   
   $��Zoom to fit��6   .��5�        -   5        $ view7��5�        . . 5�      g� " 5�      S� 5�      $# 4Q�6/   $$ 3Q�6;   .2�5�        $ 4Q�6)   
 ��  �" 5�      Y� 5�      .�5�   "   S� $# 2Q�67   $$ 1Q�6C   �� 5�   "   % 5�   &   �� 5�   *   $& 0,1,15�6>   $' 30Q�6K   5�   #   S� 5�   "   $$ 3Q�67   5�      5�      5�      5�      5�      5�      5�      