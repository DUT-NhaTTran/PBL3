   M S B A M L   `   `   `  ����@  <Demo1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:Demo1Demo1  5      1clr-namespace:Demo1.ViewModelDemo1.ViewModel  5          Demo1.View.OrderTracking   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      %localclr-namespace:Demo1     5      X TFontAwesome.Sharp, Version=6.3.0.0, Culture=neutral, PublicKeyToken=d16d1e4e568ec10fFfa=http://schemas.awesome.incremented/wpf/xaml/fontawesome.sharp  5      -	viewModelclr-namespace:Demo1.ViewModel  5      _ [MaterialDesignThemes.Wpf, Version=4.8.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      -   5           OrderTM  � DataContext#  [  5      .��5      ��  �� CornerRadius$ 20��6   ���#434343._�5   
   �  � ColumnDefinitions 5      �� 5      $	��530 �6#   �� 5      $	��420 �6#   .+�5      � $��0��6    � RowDefinitions 5      �� 5      $	��100 �6$   �� 5      $	��550 �6$   .+�5      � $��0��6   .+�5      �� $ 20��6   ���#212325$��15q�5       $	��350��5!      ._�5"      ��  �� Orientation$ 
Horizontal=�6%   .+�5#      �� $��63��6%   $��55��60   $��10 0 0 0q�5$   %   
������ 
������ ��5%   %   �� *ParcelTrackingCommand+.��5'   "   '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Magnify=�6:   $��40��6I   $��40��6U   ���#B0B0B2$��Center=�5)   :   $��Center=�5*   :   5+      �� 5-      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Nhập mã đơn hàng��6(      FloatingScale$ 0Q�5.   (      
Foreground
 ��  �$��25(�50   (   $��0q�66    % !MaterialDesignFloatingHintTextBox#��[ 51   (   $��5 0 0 0q�53   (   ���#B0B0B2$	��250��54   =   $��70��6I   ��52   (   �� *
ParcelID+	 �� Mode$	 TwoWay=�54   I   55      56      57      � 5;      $��1��6   $��10q�5<      .+�5=      �� -   5        ��5=      �� *ParcelInfoList+��5?      �� 5@      .R�5A   "   � 
 � Orientation$
 
Horizontal=�6,   $��Center=�6E   $��Center=�6b   .+�5B   &   �� $ 10��6-   ���#303030$	��235��5D   -   $	��200��69   $��Center=�5E   -   $��Center=�5F   -   $��0 0 0 0q�5G   -   ._�5H   *   �  5I   .   �� 5J   2   $��40 �6@   �� 5K   2   $	��150 �6@   .+�5N   .   � $��0��63   .+�5O   2   � .+�5P   6   �� $ 
Horizontal=�6A   .+�5Q   :   �� $��
20 18 18 0q�5R   D   $��10(�5S   D    �� TextAlignment$ Center=�5T   D   $	��Top=�5U   D   $��Center=�6\   ���#FFFFFF��5Q   D   �� *ID+5V   D   �� 5X   :   $��	40 18 0 0q�5Y   D   $��10(�5Z   D   $	��Top=�5[   D   $��Center=�6\   ���#EDF259��5X   D   �� *SimpleStatus+5\   D   5]   7   5^   3   5_   /   � 5a   .   $��1��63   .+�5b   2   �� $ 10��69   $	��0.5q�5c   =   $��5q�5d   =   ���#999898._�5f   6   �  5g   :   �� 5h   >   $��95 �6O   �� 5i   >   $��95 �6O   .+�5k   :   � $��0��6?   .+�5l   >   �� $��0 0 0 0q�6I   $ Vertical=�6Z   .+�5m   B   �� $��Tên đơn hàng��6L   $��
0 18.5 0 0q�5n   P   $��10(�5o   P   $��Center=�5p   P   $	��Top=�5q   P   ���#B0B0B2$
��Auto��5r   e   �� 5t   B   $��
0 18.5 0 0q�5u   P   $��10(�5v   P   $
��Wrap=�5w   P   $��Center=�5x   P   $	��Top=�5y   P   ���#B0B0B2��5t   L   �� *
ParcelName+5z   P   5{   ?   5|   ;   � 5   :   $��1��6?   .+�5�   >   �� $��0,0,0,0q�6I   $ Vertical=�6Z   .+�5�   B   �� $ ��Trạng thái hiện tại��6L   $��
0 18.5 0 0q�5�   P   $��10(�5�   P   $��Center=�5�   P   $	��Top=�5�   P   ���#B0B0B2$
��Auto��5�   e   �� 5�   B   $��
3 18.5 0 0q�5�   P   $	��Top=�5�   P   $��Center=�5�   P   $��10(�5�   P   $
��Wrap=�5�   P   ���#B0B0B2��5�   L   �� *	Details+5�   P   5�   ?   5�   ;   5�   7   5�   3   5�   /   5�   +   5�   '   5�   #   5�      ��5�      �� 5�      .R�5�   "   � $
 
Horizontal=�6,   5�      5�      5�      5�      � 5�      $��1��6    5�      �� 5�      $��80 �6$   �� 5�      $��80 �6$   �� 5�      $��* �6$   .+�5�      � $��0��6   $��0 20 0 0q�5�      .+�5�      �� $��Mã đơn hàng:��6"   $��20(�5�       $��10,0,0,0q�5�       $
��Bold"�5�       $��Center=�5�       $
��Left=�5�       ���#B0B0B2$	��130��5�   5   $��30��6A   �� 5�      $��20(�6    $��0q�6.   $��	0,0,120,0q�5�       $
��Bold"�5�       $��Center=�5�       $��Right=�5�       $��Center=�5�       ���#EDF259$	��100��5�   5   $��70��6A   ��5�       �� *
ParcelID+5�   A   �� 5�      $ 0Q�5�       
 ��  �$��20(�5�       $��0q�6.   #��[ 5�       $��0 0 40 0q�5�       $��Center=�5�       $��Right=�5�       $��Center=�5�       
���2͚�$	��170��5�   9   $��70��6E    5�       �� *
StatusText+��5�       �� *
SearchText+5�   E   5�      � 5�      $��1��6   .+�5�      �� $��Lộ trình��6    $��25(�5�       $��Center=�5�       $
��Left=�6;   $��20 6 0 0q�5�       ���#B0B0B25�       5�      � 5�      $��2��6   .+�5�      �� $��
0,0,-45,30q�6#   .��5�      �� $��0,0,0,0q�6M   $	��NaN��6^   ��6'   �� *RouteInfoList+��5�      �� 5�   "   .R�5�   &   �� $ 
Horizontal=�61   .+�5�   *   A� $��10��62   $��10��6=   ���#B0B0B2$��0,0,10,0q�6Y   �� 5�   *   ���#B0B0B2$��	0,12,10,0q�6Z   ��64   �� 6Z   5�   '   5�   #   5�      5�      5�      5�      5�      5�      5�      