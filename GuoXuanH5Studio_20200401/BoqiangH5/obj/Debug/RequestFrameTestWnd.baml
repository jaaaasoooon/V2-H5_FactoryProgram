   M S B A M L   `   `   `  ����  	BoqiangH5#    BoqiangH5.RequestFrameTestWnd   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � 
ResizeMode$ NoResize=�6/    � Title$ RequestFrameTestWnd��5   	   $	��680��6%   $	��600��61   .��5      �  � RowDefinitions 5   
   �� 5      $	��30* �6   �� 5	      $	��60* �6   �� 5
      $	��15* �6   .+�5   
   �� $��0��6   $��20q�6   $��2q�6+   ���	#FFE6E6E6._�5      �  � ColumnDefinitions 5      �� 5      $��80 �6'   �� 5      $
��120* �6'   .+�5      �� -   5        $��labReqFrame��5      $��发送帧：��6+   $��Right=�6:   $	��Top=�6V   $��28��6n   $
��5,15q�6z   $��15(�6�   �� 5      -   5        $��
tbReqFrame��5      $��1��6.   $	��Top=�6>   $��
10,20,20,0q�6V   $��00 01 02 03 04 05 06 07��6k    �� TextWrapping$
 Wrap=�6�   " �� VerticalScrollBarVisibility$
 Auto=�6�   5      5      �� 5   
   $��1��6   $��20q�6   $��2q�6+   ���	#FFE6E6E6._�5      �  5      �� 5      $��80 �6'   �� 5      $
��120* �6'   .+�5      �� -   5        $��labRecvFrame��5      $��接收帧：��6,   $��Right=�6;   $	��Top=�6W   $��28��6p   $
��5,15q�6|   $��15(�6�   �� 5      -   5        $��tbRecvFrame��5      $��1��6.   $	��Top=�6>   $��10,15,20,20q�6V   $��00 01 02 03 04 05 06 07��6l   $
 Wrap=�6�   $	��240��6�   $
 Auto=�6�    �� 
IsReadOnly . 6�   5       5!      �� 5"   
   $��2��6   $��发送��6   $��60��6+   $��25��66   5$      5%      