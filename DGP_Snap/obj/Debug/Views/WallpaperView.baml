   M S B A M L   `   `   `  ����C  ?DGP_Snap, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:DGP_Snap.ViewsDGP_Snap.Views  5      "    DGP_Snap.Views.WallpaperView   5      T PMahApps.Metro, Version=2.0.0.0, Culture=neutral, PublicKeyToken=51482d6f650b2b3f6mah,http://metro.mahapps.com/winfx/xaml/controls  5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .localclr-namespace:DGP_Snap.Views     5      -   5          >�Name$
  Root��5
       � 	Resources 5      ��%    5      .��5      � 
���   �.+�5   
   �� -   5        $��OpacityMaskRect��5        J� Opacity$ 1Q�6   
���   �67   �� 5   
   -   5         ��Name$ WallPaperImage��5   W   $��UniformToFill=�6o   $��Center=�6�   $��Center=�6�    6   ��  �� ElementName$! WallPaperImageOpacitySlider�� �� Path$ Value���6�   �� *CurrentImageSource+6�   � 5   
   -   5        $��	BottomBar��5      $��8��6    $��Bottom=�6+   
������  � Triggers 5      :� 5       :� RoutedEvent$ Grid.MouseEnter��6   .[�5      �� .��5      �� .��5      W�  g� 	BeginTime$ 0X�6.   	 g� Duration$	 0:0:0.3D�6<   
 �� 
TargetName$
 	BottomBar��6O    �� TargetProperty$ Height�6q   	 W� To$ 64X�6�    W� EasingFunction 5   "   2  ,System.Windows.Media.Animation.QuadraticEase  5   &   7  1System.Windows.Media.Animation.EasingFunctionBase   
EasingMode$ EaseOut=�64   5      5      5      5      :� 5      $ Grid.MouseLeave��6   .[�5       �� .��5!      �� .��5"      W� $ 0X�6/   $	 0:0:0.3D�6=   $
 	BottomBar��6P   $ Height�6r   $ 8X�6�    5#   "     5$   &   $ EaseIn=�64   5&      5'      5(      5)      .+�5+      � $	��Top=�6   $��64��6+   .+�5-      �� $��48,8,0,8q�6    �� Orientation$ 
Horizontal=�6/   $��Bottom=�6H   $��Right=�6c   .+�5.      )  #MahApps.Metro.Controls.ToggleSwitch  -   5          Name$ SwitchBaidu��5.   '      	IsChecked$
 TrueY�6<   
 � Tag$ Baidu��6M      OnLabel$  ��6Y      OffLabel$  ��6d    +  'MahApps.Metro.Styles.ToggleSwitch.Win10#��[  6p   
�������
�������$��Baidu��6�   {�. 6�     5/      -   5        $ 	Switch360��5/   '   $
 TrueY�6:   $	 360��6K   $  ��6U   $  ��6`   #��[  6l   
�������
�������$	��360��6�   {�. 6�     50      -   5        $ 
SwitchBing��50   '   $
 TrueY�6;   $
 Bing��6L   $  ��6W   $  ��6b   #��[  6n   
�������
�������$
��Bing��6�   {�. 6�   �� 51      $��48��6    $��18(�6+    �� TextAlignment$ Center=�69   $��Center=�6P   
�������  	Icon_Font#��[ 6~   $	����6�   �� 52      -   5         ��Name$! WallPaperImageOpacitySlider��52       � Minimum$ 0Q�6B    � Maximum$ 1Q�6N   $	��192��6Z    � Value$	 0.5Q�6f    % !MahApps.Metro.Styles.Slider.Win10#��[ 6r   5@      �� 5A      -	   5        $��DownloadWallPaperButton��5A      $��0q�6<   $��48��6G   $��48��6R     DefaultButtonStyle#��[ 6^   $��Right=�6�   $��Bottom=�6�   $	����6�   
�������$��16(�6  #���  6  �� 5B      -
   5        $��SwitchWallPaperButton��5B      $��0q�6:   $��48��6E   $��48��6P   #��[ 6\   $��Right=�6�   $��Bottom=�6�   $	����6�   
�������$��16(�6	  #���  6  5C      5D      5E      5F      5G      