   M S B A M L   `   `   `  ���� D  @UDSStudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:UDSStudio	UDSStudio      UDSStudio.PackViewWindows   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     a ]DevExpress.Xpf.Core.v15.2, Version=15.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a;dx2http://schemas.devexpress.com/winfx/2008/xaml/core  <dxb2http://schemas.devexpress.com/winfx/2008/xaml/bars  c _DevExpress.Xpf.Gauges.v15.2, Version=15.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a?dxga4http://schemas.devexpress.com/winfx/2008/xaml/gauges  j fDevExpress.Xpf.LayoutControl.v15.2, Version=15.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49aFdxlc;http://schemas.devexpress.com/winfx/2008/xaml/layoutcontrol  #localclr-namespace:UDSStudio  -     � Title$  PackViewWindows�� � WindowStartupLocation$ CenterScreen=� J� CommandBindings )  #System.Windows.Input.CommandBinding  -      Command$( "local:StudioCommands.UpLoadCommand��  -   $. (local:StudioCommands.AdjustManageCommand�� � 	Resources    StatisticsVoltFillBrush �� EndPoint �� 
StartPoint � Color � Offset  textblockStyle	 �� 
TargetType
 �� Property �� Value  packContextMenu  promtUpLoadParam m� Command m� CommandParameter  promtUpLoadAdjust  promtAdjust ! cellVoltStatisticsProgressbar3  -System.Windows.Media.Effects.DropShadowEffect   	Direction   ShadowDepth   Opacity �� 
TargetType � RowDefinitions � ColumnDefinitions �� CornerRadius ��Name �� RelativeSource �� Mode �� AncestorLevel �� AncestorType �� Path �� StringFormat  cellVoltProgressbar �� Triggers �� Binding �� Value N� EnterActions  g� RepeatBehavior! g� 	BeginTime" �� TargetProperty# W� From	$ W� To% g� Duration  AFEBorderStyle& ��Name' g� AutoReverse( N� ExitActions) �� BeginStoryboardName 	 cellListboxDataTemplate* � Value $
  tmprStatisticsLinearGaugeControl+ F�Name  tmprLinearGaugeControl  tmprListboxDataTemplate  groupNormalBackground  groupAFEBackground  packListBoxDataTemplate+  %DevExpress.Xpf.LayoutControl.GroupBox, � ContextMenu4  .DevExpress.Xpf.Core.HeaderedContentControlBase-   Header,  &DevExpress.Xpf.Core.ContentControlBase.   Content  ListBoxStyle1  ListBoxItemSelStyle  MainBorderStyle  txblPackAddr  txblPackSoftVer  txblPackHardVer  groupBoxstyle��%  &	        &	 q     &	 �     &	 �    7   7   7  &	 U    7   7   7  &	 �    &		 �    7  7  7  &	
 �	    &	 �    &	 �    7  7  7  &	 �    &	     &	 j    7  7  7  7  7 	 7  7  7  7  7  7  7  7  7  7  7  &	 �    �� $ 0.5,1&�$ 0.5,0&�.��� $ #00FF00��$ 0Q�� $ White��$	 0.5Q�� $ #00FF00��$ 1Q��� "	 ��.��� 
 � ��$ Center=��� 
 � ��$ Center=��� .@�m� #���  $( "local:StudioCommands.UpLoadCommand��$
 Para���� m� #���  $( "local:StudioCommands.UpLoadCommand��$ Adjust���� m� #���  $. (local:StudioCommands.AdjustManageCommand��$ Adjust���� "	 �.��� 
 �@J�Effect   $	 310Q�$ 5Q�$	 0.5Q��� 
 �@�Maximum$ 5Q��� 
 �@�Minimum$ 0Q��� 
 � ��$ 1q��� 
 � �� �� " �.R�� $��40��$��80��$��80��$	��120�� �� $��* ��� $��3* ��� $��* � �� $��8* ��� $
��1.2* �.+��� $��1��$��1��$ 0,5,5,0��
���   �$��0,5,5,5q�8��  �� $��0��$��0��$��3��$ 5,5,5,5��
���   �$��5,5,5,5q�._��  �� $��* ��� $��* ��� $��* �.+��� $ 
PART_Track��$��0��$��3��
������ �� $ PART_Indicator��$��0��$��3��8�� $
��Left=��� $��0��$��3��8�� ����  �� $ FindAncestor=�$ 1��$ ProgressBarL�$ Value�$ {0:F3}V���� "	 �.��� 
 �@J�Effect   $	 310Q�$ 5Q�$	 0.5Q��� 
 �@�Maximum$ 5Q��� 
 �@�Minimum$ 0Q��� 
 � ��$ 1q��� 
 � �� �� " �.R�(� .��� $��30��$��80��$��50��$	��100�� �� $��* ��� $��3* ��� $��* � �� $��8* ��� $
��1.2* �.+��� $��1��$��1��$ 0,3,3,0��
���   �$��0,3,3,3q�8��  �� $��0��$��0��$��3��$ 3��
���   �$��3q�._��  �� $��* ��� $��* ��� $��* �.+��� $ 
PART_Track��$��0��$��3��
������ �� $ PART_Indicator��$��0��$��3��8�� $
��Left=�����  ��  �� *	IsBalance+ True �� .���� $  Forever��.��W� $! 0X�$" (UIElement.Opacity)�$# 1X�$$ 0X�$% 00:00:02D��� $��0��$��3��8�� ����  �� $ FindAncestor=�$ 1��$ ProgressBarL�$ Value�$ {0:F3}V���� "	 ��.��� 
 � ��
 ���� ��  �� *IsAFE+ True.a��� 
 � ��
 �  ����  �� *IsAFE+ True �� $& borderChangStoryboard��.���� $  Forever��.��W� $)" #(Border.Background).(Brush.Opacity)�$# 1X�$$ 0X�' . $% 00:00:01D�( �� $) borderChangStoryboard���� .R��� $��0��8��  ._��  �� $
��0.2* ��� $
��0.8* �.+��� 8�� ���� *	VarName+� $��1��8�� * �� *
VarValue+�� "	 �.��� 
 �@J�Effect   $	 310Q�$ 5Q�$	 0.5Q��� 
 �@�Maximum$	 150Q��� 
 �@�Minimum$	 -60Q��� 
 � �� �� " �.R�(� $��40��$��80��$��80��$	��120��.���  �� $��* ��� $��* �.+�F� $+ 
PART_Track��$��0��$��2��
������ 
���   �$��1��G���� -1��� � -1 -1 z  z � �� -1� ����� �� ��  ��
	F� $+ PART_Indicator��$��0��$��2��
���  ��
������ $
��Left=�$��2��G���� -1��� � -1 -1 z  z � �� -1� ����� �� ��  ��
	�� "	 �.��� 
 �@J�Effect   $	 310Q�$ 5Q�$	 0.5Q��� 
 �@�Maximum$	 150Q��� 
 �@�Minimum$	 -60Q��� 
 � �� �� " �.R�(� $��20��$��80��$��40��$	��100��.���  �� $��* ��� $��* �.+�F� $+ 
PART_Track��$��0��$��2��
������ 
���   �$��1��G���� -1��� � -1 -1 z  z � �� -1� ����� �� ��  ��
	F� $+ PART_Indicator��$��0��$��2��
���  ��
������ $
��Left=�$��2��G���� -1��� � -1 -1 z  z � �� -1� ����� �� ��  ��
	�� .R��  �� $��* ��� $��* � �� $
��0.2* ��� $
��0.8* �.+��� $��0��$��0��$��2��8��  ���� *	VarName+�� $��0��$��1��$��1��8�� $��Center=�
��� ������� *
VarValue+$ 	{0:F1}℃��� $��1��$��2��8�� * �� *
VarValue+�� $	 1,0&�$	 1,1&�.��� $ 
LightGreen��� $ White��$	 0.5Q��� $	 1,0&�$	 1,1&�.��� $ 	OrangeRed��� $ White��$	 0.5Q��� .R�  8,   - �� *
PackName+ ���� '        "	  .��� 
 �@J�Opacity$	 0.3Q��� 
 � ��8   ��  �� *
IsOnLine+ True.a��� 
 �@J�Opacity$	 1.0Q���  �� *HasAFE+ True.a��� 
 � ��8  .. �  �� $��* ��� $��* � �� $��* ��� $��* �.+��� $��0��$��2��$��0��8�� 8�� 8�� $
��Auto=����� *CellsListDigitVar+�� $��0��$��1��$��1��8�� 8�� 8�� $
��Auto=����� *TmprListDigitVar+�� 8��	 $��1��$��1��._��  �� $
��Auto ��� $��* � �� $��* ��� $��* ��� $��* �.+��� $��0��$��0��8��
 #���  �� $��0��$��1��8�� ���� *PackAddress+�� $��1��$��0��8�� #���  �� $��1��$��1��8�� ���� *PackSoftVer+�� $��2��$��0��8�� #���  �� $��2��$��1��8�� ���� *PackHardVer+�� "	 ��.��� 
 � ��
 ������� 
 � ��$ 1q��� 
 � ��$ 1q�.���  �� $��* ��� $��6* ��� $
��Auto � �� $��7* ��� $��3* �.+��� $��0��$��0��#��[ ._��  �� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� $��0��#��[ / � DataContext/ �� *	MasterVar+���� *MaxCellVoltIndex+���� .R��� ��`�   ExtremumValueConverter0 `� 	Converter#0 [ .1��� $ VarName��� $ VarValue�.��� #��[ / �� *MaxCellVoltDigitVar+* �� *
VarValue+�� $��1��#��[ / �� *	MasterVar+���� *MinCellVoltIndex+���� .R��� ��`� #0 [ .1��� $ VarName��� $ VarValue�.��� #��[ / �� *MinCellVoltDigitVar+* �� $ VarValue��� $��2��#��[ / �� *	MasterVar+���� *AvgCellVoltDigitVar+���� .R��� ���� *	VarName+.��� #��[ / �� *AvgCellVoltDigitVar+* �� $ VarValue��� $��3��#��[ / �� *	MasterVar+���� *MaxTmprDigitVarIndex+���� .R��� ��`� #0 [ .1��� $ VarName��� $ VarValue�.��� / �� *MaxTmprDigitVar+ �� $��* ��� $��* �.+�� $��2��#��[
 $��Center=�$��Center=�* �� *
VarValue+�� $��0��#��[ $��Center=�
��� ������� *
VarValue+$ 	{0:F1}℃���� $��4��#��[ / �� *	MasterVar+���� *MinTmprDigitVarIndex+���� .R��� ��`� #0 [ .1��� $ VarName��� $ VarValue�.��� / �� *MinTmprDigitVar+ �� $��* ��� $��* �.+�� $��2��#��[
 $��Center=�$��Center=�* �� *
VarValue+�� $��0��#��[ $��Center=�
��� ������� *
VarValue+$ 	{0:F1}℃���� $��0��$��1��#��[ ._��  �� $��* ��� $��* � �� $��8* ��� $��2* ��� $��8* ��� $��2* �.+��� $��0��$��0��#��[   txblVoltagedifference#���  �� $��0��$��1��#��[ / �� *	MasterVar+���� *VoltageDifference+$ {0:F3}V���� $��0��$��2��#��[   txblTemperaturedifference#���  �� $��0��$��3��#��[ / �� *	MasterVar+���� *TemperatureDifference+$ {0}℃���� $��1��$��0��#��[   txblBalanceMask#���  A� $��1��$��1��
������$��20��$��20��$��Center=�$��Center=�����  :� 1 :� RoutedEvent$1 Window.Loaded��[��� .���� $  Forever��.��W� $! 0X�$" (UIElement.Opacity)�$# 1X�$$ 0X�$% 00:00:02D��� $��1��$��2��#��[   txblAFEMask#���  A� $��1��$��3��
���  ��$��20��$��20��$��Center=�$��Center=�����  :� $1 Window.Loaded��[��� .���� $  Forever��.��W� $! 0X�$" (UIElement.Opacity)�$# 1X�$$ 0X�$% 00:00:02D��� $��1��$��0��$��2��#��[ ._��� -   2 ��Name$2 packList��#��[ #��[ #��[ $��Disabled=�/ �� *	MasterVar+���� $ PackList����� .R�� *  $DevExpress.Xpf.Bars.StatusBarControl  $��2��,  &DevExpress.Xpf.Bars.ToolBarControlBase3   Items.3 '  !DevExpress.Xpf.Bars.BarStaticItem    bsiCurrentPack!	  DevExpress.Xpf.Bars.BarItem4 	  Content#4 �    4 �� 5 �� ElementName$5 packList��$ SelectedIndex�$ Pack#{0:d2}��