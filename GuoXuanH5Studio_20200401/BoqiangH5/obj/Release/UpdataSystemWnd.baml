   M S B A M L   `   `   `  ����   	UDSStudio    UDSStudio.UpdataSystemWnd   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  UpdataWindow��$	��400��$	��600�� � WindowStartupLocation$ CenterScreen=� � 
ResizeMode$ NoResize=�.���  � RowDefinitions �� $	��25* ��� $	��25* ��� $	��25* ��� $	��25* �.+�� $��0�� � ColumnDefinitions �� $��80 ��� $
��300* �.+��� $��目标地址：��$��10,20q�$��60��$��28��$��Right=�$��Center=��� -   $��tbTargetAddress��$
��0x3f��$	��100��$��30��$��10q�$
��Left=�$��Center=�$��1��� $��1�� �� $��80 ��� $
��300* ��� $��80 �.+��� $��文件名：��$��10,20q�$��60��$��28��$��Right=�$��Center=��� -   $��
tbFileName��$��10q�$	��400��$��28��$
��Left=�$��Center=�$��1�� �� 
IsReadOnly . �� -   $��btnLoadFile��$��浏览��$��60��$��30��$
��5,10q�$
��Left=�$��Center=�$��2��� $��2�� �� $��80 ��� $
��300* �.+��� $��下载进度：��$��10,20q�$��60��$��28��$��Right=�$��Center=�� -   $��ProgressInfo��$	��420��$��10��$��10,20q�$
��Left=�$��Center=�$��1�� � SmallChange$	 1.0Q� � Maximum �� *Max+ � Minimum �� *Min+	 � Value	 �� *CurrentValue+� $��3�� �� $��50 ��� $	��200 ��� $
��150* ��� $	��180 ��� $��50 �.+��� -   $��btnUpdateSys��$��下  载��$	��100��$��30��$��10,10q�$��3��{��� *BtnIsEnabled+�� -   $��
MultiFrame��$��多帧发送��$��2��$��Center=�$��26��$��10,20q�$��Center=�$��68��$y�Hidden=��� -   $��labUpdateProgress��$��1��$
��5,32q�$��Center=�$��宋体)�$��18(����� *UpdateProgress+