   M S B A M L   `   `   `  ����W  SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35eDclr-namespace:System.Windows.Media.Media3D;assembly=PresentationCoreSystem.Windows.Media.Media3D  5      Z VHelixToolkit, Version=0.9.4175.40396, Culture=neutral, PublicKeyToken=52aa3500039caf0dA0clr-namespace:HelixToolkit;assembly=HelixToolkitHelixToolkit 5       
StudioDemo   StudioDemo.Window1   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml      5      Xmedia3dDclr-namespace:System.Windows.Media.Media3D;assembly=PresentationCore      5      Bhelix0clr-namespace:HelixToolkit;assembly=HelixToolkit      5      $	��480��5   (   $	��640��65   
���   �  � Title  6   �� *ApplicationTitle+ � 	Resources 5       �� DataType �� ItemsSource �� Path �� Orientation��%  (��       5   #   *,"�+)�� 6
   " "�6#    6J   �� $ Children�.R�5      �� $ 
Horizontal=�6   .+�5      A� $��8��6   $��8��6$   
����  �
���   �6;   �� 5       ��6   �� �� 5!      ��6   �� *Visual3DModel+5"      5#      5$      .��5%      ]� .+�5&   
   o� $	��Top=�6   .@�5'      m� $��_File��6   .@�5(      m� $��_Open...��6    m� Command 6-   �� *FileOpenCommand+m� 5)      $��
_Export...��6    6/   �� *FileExportCommand+�� 5*      m� 5+      $��E_xit��6    6*   �� *FileExitCommand+5,      m� 5-      $��_Edit��6   .@�5.      m� $��
_Copy XAML��6    6/   �� *EditCopyXamlCommand+5/      m� 50      $��_View��6   .@�51      m� $��_Zoom to fit��6    61   �� *ViewZoomToFitCommand+52      m� 53      $��_Help��6   .@�54      m� $��_About��6    6+   �� *HelpAboutCommand+55      56      ]� 58   
   $
��Left=�6   $	��400��6*   .+�59      ��  � Minimum$ 1Q�61    � Maximum$ 2Q�6=   $��Bottom=�6I   	 � Value	 6   �� *	Expansion+6I   �� 5:      $��Bottom=�6   $	��200��6.   ��6;   �� *Info+R� 5;      ��6   �� *
Children+
 �� ElementName$
 view1��5<      5=        HelixToolkit.HelixView3D  5?   
   -   5          Name$ view1��5?         CameraRotationMode$ VirtualTrackball=�6+      Children. 5A      (  "HelixToolkit.DefaultLightsVisual3D    HelixToolkit.Expander3D  5C         	Expansion 6   �� *	Expansion+ g� Content 6?   �� *CurrentModel+5G      5H      5I      