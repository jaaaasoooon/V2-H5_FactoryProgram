   M S B A M L   `   `   `  ���� D  @UDSStudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:UDSStudio	UDSStudio      UDSStudio.CANFrameRecord   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     a ]DevExpress.Xpf.Core.v15.2, Version=15.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a;dx2http://schemas.devexpress.com/winfx/2008/xaml/core  <dxb2http://schemas.devexpress.com/winfx/2008/xaml/bars  a ]DevExpress.Xpf.Grid.v15.2, Version=15.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49af bDevExpress.Xpf.Grid.v15.2.Core, Version=15.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a>dxg2http://schemas.devexpress.com/winfx/2008/xaml/grid   j fDevExpress.Xpf.LayoutControl.v15.2, Version=15.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49aFdxlc;http://schemas.devexpress.com/winfx/2008/xaml/layoutcontrol  Gdxgt<http://schemas.devexpress.com/winfx/2008/xaml/grid/themekeys  ?dxe5http://schemas.devexpress.com/winfx/2008/xaml/editors  #localclr-namespace:UDSStudio  -     � Title$  CANFrameRecord�� � WindowStartupLocation$ CenterScreen=�$
��1440��.���  � RowDefinitions �� $��* ��� $��60 �.+�&   System.Windows.Controls.DataGrid  -   $��DataGridFrame��   AutoGenerateColumns .  $��16(�   CanUserAddRows .   �� AlternationCount$ 2��   Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn   Header$  序号��   Width5  /System.Windows.Controls.DataGridLengthConverter$ 70 	   
IsReadOnly	 .    DataGridHeaderStyle
   HeaderStyle#
 [     CellTemplate �� .R��� $��Center=����� *Index+0  *System.Windows.Controls.DataGridTextColumn  $ 时间��$	 200 	 . #
 [  1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *	FrameTime+  $ 收发��$	 100 	 . #
 [   �� *	Direction+  $ ID��$	 120 	 . #
 [   �� *	FrameID+  $ CAN数据类型��$	 120 	 . #
 [   �� *CANFrameType+  $ 协议数据类型��$	 120 	 . #
 [   �� *ProtocolFrameType+  $ 服务类型��$	 100 	 . #
 [   �� *ServiceType+  $ 数据��$
 150* 	 . #
 [   �� *	FrameData+� $��1��$��10q� � ColumnDefinitions �� $��15 ��� $	��100 ��� $	��120 ��� $	��120 ��� $��* ��� $	��120 ��� $	��120 ��� $��30 �.+��� -   $��cbSaveFrameData��$��1��$��Center=�$��保存数据��$��Center=�$	��5,8q� d� 	IsChecked ��  �� Path$ IsSaveFrameData��� -    ��Name$ btnStart��$��2��$
��10,5q�$��
启    动���� -   $ btnSave��$��3��$
��10,5q�$��	另存为���� -   $ btnClear��$��5��$
��10,5q�$��清空记录���� -   $ btnClose��$��6��$
��10,5q�$��
退    出��