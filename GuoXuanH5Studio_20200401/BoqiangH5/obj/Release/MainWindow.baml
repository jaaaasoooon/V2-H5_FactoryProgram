   M S B A M L   `   `   `  ���� D  @UDSStudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:UDSStudio	UDSStudio  M ISystem, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089M3clr-namespace:System.ComponentModel;assembly=SystemSystem.ComponentModel D @UDSEntity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:UDSEntity;assembly=UDSEntity	UDSEntity     UDSStudio.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     a ]DevExpress.Xpf.Core.v15.2, Version=15.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a;dx2http://schemas.devexpress.com/winfx/2008/xaml/core  <dxb2http://schemas.devexpress.com/winfx/2008/xaml/bars  c _DevExpress.Xpf.Gauges.v15.2, Version=15.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a?dxga4http://schemas.devexpress.com/winfx/2008/xaml/gauges  c	 _DevExpress.Xpf.Charts.v15.2, Version=15.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a>dxc4http://schemas.devexpress.com/winfx/2008/xaml/charts 	 ?dxe5http://schemas.devexpress.com/winfx/2008/xaml/editors  j
 fDevExpress.Xpf.LayoutControl.v15.2, Version=15.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49aFdxlc;http://schemas.devexpress.com/winfx/2008/xaml/layoutcontrol 
 #localclr-namespace:UDSStudio 
 <cm3clr-namespace:System.ComponentModel;assembly=System 
 :	entitydll*clr-namespace:UDSEntity;assembly=UDSEntity 
 -     � Title$  
MainWindow�� � WindowStartupLocation$ CenterScreen=� � SizeToContent$ Manual=� � 	Resources   UDSEntity.DigitVar   BatVoltDigitVar  BatCurDigitVar  BatSocDigitVar  BatSohDigitVar  MaxCellVoltDigitVar  MaxCellVoltIndex  MinCellVoltDigitVar  MinCellVoltIndex  AvgCellVoltDigitVar 	 MaxTmprDigitVar 
 MaxTmprDigitVarIndex  MinTmprDigitVar  MinTmprDigitVarIndex  BatPositiveInsulationDigit  BatNegativeInsulationDigit  ProtectOnBrush �� Color  WarnOnBrush  NormalBrush  packOnLineStateText �� 
TargetType �� Property �� Value �� Triggers	 �� Binding
 �� Path �� Value  packOnLineState  SocFillBrush �� EndPoint �� 
StartPoint � Color � Offset  SohFillBrush  EmptyFillBrush  MetalBaseBrush  MetalPodBrush  StatisticsVoltFillBrush  DisabledForegroundBrush  warnCheckBox �� 
TargetType (� Stretch ��Name J� SnapsToDevicePixels �� CornerRadius �� Orientation T� X ��Name �� TextAlignment �� Triggers P� Property P� Value �� 
TargetName N� EnterActions �� 
TargetName �� TargetProperty	  W� To! g� Duration" N� ExitActions  groupHeardTemp  dxglWarnAndProtect# � DataContext$ �� RelativeSource% �� Mode& �� AncestorLevel' �� AncestorType( d� 	IsChecked) �� Mode  textboxStyle  batterySOCProgressBarStyle3  -System.Windows.Media.Effects.DropShadowEffect*   ShadowDepth+   
BlurRadius,   	Direction-   Color. � RowDefinitions/ � ColumnDefinitions0 � ToolTip1 �� StringFormat   batterySOHProgressBarStyle ! MaxCellProcessbarStyle2 � LayoutTransform3 �� Angle4 �� Source " ExtremumValueConverter5 `� 	Converter # MinCellProcessbarStyle $ AvgCellProcessbarStyle��%n  &	        &	      &	 
     &	      &	      &	      &	      &	 #     &	 (     &		 -     &	
 2     &	 7     &	 <     &	 A     &	 F     &	 K     &	 Z     &	 l     &	 �     &	 K    7  &	 ]    &	 �    &	 ?    &	 �    &	 *    &	 �    &	     &	      7  &	 �    7  &	 �    &	 �	    7  7  7  7  7  &	  4    7  7  7  7  7  &	! �    7  7  7  7 " 7  &	# t    7  7  7  7 " 7  &	$ !    7  7  7  7                                �� $	 Red���� $ Yellow���� $ 
LightGreen���� " ��.���  � �� �� *
PackName+��  � ��
 �������  �@J�Opacity$	 0.3Q� �� 	 �� $
 IsOnLine� True.a���  � ��
 ������  �@J�Opacity$ 1Q��� .R��� 8��  �� $ 0.5,1&�$ 0.5,0&�.��� $ #00FF00��$ 0Q�� $ White��$	 0.4Q�� $ #00FF00��$ 1Q��� $ 0.5,1&�$ 0.5,0&�.��� $ #0000FF��$ 0Q�� $ White��$	 0.4Q�� $ #0000FF��$ 1Q��� $ 0.5,1&�$ 0.5,0&�.��� $ 	#FFC5C5E5��$ 0Q�� $ White��$	 0.4Q�� $ 	#FFC5C5E5��$ 1Q��� $ 0.5,1&�$ 0.5,0&�.��� $ 	LightBlue��$ 0Q�� $ White��$
 0.45Q�� $ 	LightBlue��$ 1Q��� $ 0.5,1&�$ 0.5,0&�.��� $ 	LightBlue��$ 0Q�� $ White��$	 0.5Q�� $ 	LightBlue��$ 1Q��� $ 0.5,1&�$ 0.5,0&�.��� $ #00FF00��$ 0Q�� $ White��$	 0.5Q�� $ #00FF00��$ 1Q��� $
 #AAA���� " ��.���  � ��
 �������  � �� �#DDE8E1��  � ��$ 13(���  �@�Cursor$
 Hand����  � ��$ 75����  � ��$ 28����  � ��$ 1q���  � �� �� " ��.R�(� $ Uniform=�$��Center=�$��Center=�.���� $ border��$��75��$��28��#��z�� . #��z��$ 14��._��� $ 
Horizontal=�.+��� $ state��$��24��$��24��$��3,2,1,2q�$ 12�� . #��z��z�T� 
transState��$ 0Q��� $	 txt��$��40��#��z��#��z��$��Center=�$ Center=�z�T� transTxt��$ 0Q� P�  �@d�	IsChecked$
 TrueY�. ��� $	 txt�� � ��$ 精简����  � �� �	#FFFBAA49 �� .���� .��W� $ 
transState��$ X�$  45X�$! 0:0:0.2D�W� $ transTxt��$ X�$	  -24X�$! 0:0:0.2D�" �� .���� .��W� $ 
transState��$ X�$  0X�$! 0:0:0.2D�W� $ transTxt��$ X�$  0X�$! 0:0:0.2D�P�  � {� .  . ��� 8    � ���� .R��� $ 
Horizontal=�.+��� $��Center=�$��Center=�#���  �� -   8��  $��详细��# �� $ �� $% FindAncestor=�$& 1��$' dxlc:GroupBoxL�$
 DataContext�( �� *CompactWarn+$) TwoWay=��� " ��.���  � ��
 �   ���  � ��$ 宋体)���  � ��$ SemiCondensed&���  � ��$ 16(���  � ��$ Black"���  � ��$ Normal$���  � ��$ Center=���  � ��$ Stretch=��� " �.���  �@J�Effect   $* 10Q�$+ 5Q�$	, 310Q�$- 	LightGray����  � �� �� " �.R�� $��14��. �� $��* ��� $��2* ��� $��* �/ �� $
��Auto ��� $��* ��� $
��Auto ��� $
��Auto �.+��� $��0��$��0��$��3��8��  $��10��
�������$��1q�$ 3,0,0,3���� $��
PART_Track��$��0��$��1��$��3��8�� �� $ PART_Indicator��$��0��$��1��$��3��$
��Left=�$��0,1,1,0q�8�� $ 0,3,3,0��� $��1��$��1��/ �� $��* ��� $��* �.+��� $��0��$��Italic$�$	0 SOC��$	��SOC��$��Center=�$��20(�
���   �$��5,-5,0,0q��� $��1��$��Italic$�$��DigifaceWide)�$0 剩余电量��$��Center=�$��20(�
���   �$��5,0,0,0q����� $
 Value�$ �� *TemplatedParent+$1 {0:F1}%���� $��0��$��2��$��3��$��10��
�������$��1q�$ 0,5,5,0��8�� �� $��1��$��3��$��8�����	#FF696767$��1q�$ 0,3,3,0��8�� �� " �.���  �@J�Effect   $* 10Q�$+ 5Q�$	, 310Q�$- 	LightGray����  � �� �� " �.R�� $��14��. �� $��* ��� $��2* ��� $��* �/ �� $
��Auto ��� $��* ��� $
��Auto ��� $
��Auto �.+��� $��0��$��0��$��3��8��  $��10��
�������$��1q�$ 3,0,0,3���� $��
PART_Track��$��0��$��1��$��3��8�� �� $ PART_Indicator��$��0��$��1��$��3��$
��Left=�$��0,1,1,0q�8�� $ 0,3,3,0��� $��1��$��1��/ �� $��* ��� $��* �.+��� $��0��$��Italic$�$	0 SOH��$	��SOH��$��Center=�$��20(�
���   �$��5,-5,0,0q��� $��1��$��Italic$�$��DigifaceWide)�$��Center=�$��20(�
���   �$��5,0,0,0q����� $
 Value�$ �� *TemplatedParent+$1 {0:F1}%���� $��0��$��2��$��3��$��10��
�������$��1q�$ 0,5,5,0��8�� �� $��1��$��3��$��8�����	#FF696767$��1q�$ 0,3,3,0��8�� �� " �.���  �@J�Effect   $* 5Q�$+ 5Q�$	, 310Q�$- 	LightGray����  � �� �� " �.R�� $��5q�$	��200��. �� $
��1.5* ��� $��8* �/ �� $��* ��� $��4* ��� $��* �.+��� $��0��$��1��$��1��
����ح�8��  $��0,5,5,5q�$ 0,3,3,0��2 V� .~��� $	3 -90Q��� $��1��$��3��
����ح�$��5q�._�� . �� $��* ��� $��* �2 V� .~��� $	3 -90Q�.+��� $ 
PART_Track��$��0��$��0��$��2��
������ �� $ PART_Indicator��$��0��$��0��$��2��$ 0,1,1,0��$��0,1,1,0q�8�� $
��Left=��� $��0��$��0��$��2��$ 0,1,1,0��._�� . �� $��* ��� $��* �.+��� $��Center=�$��Center=�$��0��$��14(�
���   �$��WrapWithOverflow=�# �� 84  ��`� 85  .1��� $
 VarName��� $
 VarValue��� $��Center=�$��Center=�$��1��
���   �$��14(����� 84  $
 VarValue�$1 {0:F3}V��2 V� .~��� $3 90Q��� " �.���  �@J�Effect   $* 5Q�$+ 5Q�$	, 310Q�$- 	LightGray����  � �� �� " �.R�� $��5q�$	��200��. �� $
��1.5* ��� $��8* �/ �� $��* ��� $��4* ��� $��* �.+��� $��0��$��1��$��1��
����ح�8��  $��0,5,5,5q�$ 0,3,3,0��2 V� .~��� $	3 -90Q��� $��1��$��3��
����ح�$��5q�._�� . �� $��* ��� $��* �2 V� .~��� $	3 -90Q�.+��� $ 
PART_Track��$��0��$��0��$��2��
������ �� $ PART_Indicator��$��0��$��0��$��2��$ 0,1,1,0��$��0,1,1,0q�8�� $
��Left=��� $��0��$��0��$��2��$ 0,1,1,0��._�� . �� $��* ��� $��* �.+��� $��Center=�$��Center=�$��0��$��14(�
���   �$��WrapWithOverflow=�# �� 84  ��`� 85  .1��� $
 VarName��� $
 VarValue��� $��Center=�$��Center=�$��1��
���   �$��14(����� 84  $
 VarValue�$1 {0:F3}V��2 V� .~��� $3 90Q��� " �.���  �@J�Effect   $* 5Q�$+ 5Q�$	, 310Q�$- 	LightGray����  � �� �� " �.R�� $��5q�$	��200��. �� $
��1.5* ��� $��8* �/ �� $��* ��� $��4* ��� $��* �.+��� $��0��$��1��$��1��
����ح�8��  $��0,5,5,5q�$ 0,3,3,0��2 V� .~��� $	3 -90Q��� $��1��$��3��
����ح�$��5q�._�� . �� $��* ��� $��* �2 V� .~��� $	3 -90Q�.+��� $ 
PART_Track��$��0��$��0��$��2��
������ �� $ PART_Indicator��$��0��$��0��$��2��$ 0,1,1,0��$��0,1,1,0q�8�� $
��Left=��� $��0��$��0��$��2��$ 0,1,1,0��._�� . �� $��* ��� $��* �.+��� $��Center=�$��Center=�$��0��$��WrapWithOverflow=�$��14(�
���   ����� 84  $
 VarName��� $��Center=�$��Center=�$��1��
���   �$��14(����� 84  $
 VarValue�$1 {0:F3}V��2 V� .~��� $3 90Q�6 J� CommandBindings6 )  #System.Windows.Input.CommandBinding  -   7   Command$&7  local:StudioCommands.LoadCommand��  -   $(7 "local:StudioCommands.UpLoadCommand��  -   $-7 'local:StudioCommands.ViewRecordsCommand��  -   $.7 (local:StudioCommands.AdjustManageCommand��  -   $37 -local:StudioCommands.WriteAdjustManageCommand��  -   $-7 'local:StudioCommands.DebugManageCommand��  -	   $.7 (local:StudioCommands.ViewPackInfoCommand��  -
   $)7 #local:StudioCommands.SettingCommand��  -   $07 *local:StudioCommands.LanguageSelectCommand��  -   $/7 )local:StudioCommands.OpenDTCWindowCommand��.��� -   8 �Name$8 mainGrid��. �� $��28 ��� $��* ��� $��30 �.+�o� $��0�����#4080BA
�������.@�m� -   $��menuConnect��$��连接��{��� *LinkOn+ % rc9 �� 	Converter#9 [% : m� Icon: �� $!��Images/icon-start-18x18.png��$	��2,5q�m� -   $��	menuBreak��$��断开��{��� *LinkOn+: �� $ ��Images/icon-stop-18x18.png��$��6q�m�  & tbLoadProtocol#��� & ; m� Command$&;  local:StudioCommands.LoadCommand��< m� CommandParameter$< protocol��m�  ' tbLoadParam#��� ' $&;  local:StudioCommands.LoadCommand��$< param���� m�  ( tbUpLoadParam#��� ( $(; "local:StudioCommands.UpLoadCommand��$< param���� �� m�  ) tbCommunicationRecords#��� ) $-; 'local:StudioCommands.ViewRecordsCommand��$
< comm���� m�  * tbRealtimeRecords#��� * $-; 'local:StudioCommands.ViewRecordsCommand��$< realTime���� m�  + tbHistoryRecords#��� + $/; )local:StudioCommands.OpenDTCWindowCommand���� m�  , tbAdjustAndControl#��� , $.; (local:StudioCommands.AdjustManageCommand��m� $��写入校准��$3; -local:StudioCommands.WriteAdjustManageCommand���� m� -   = m�Name$= menuMasterPowerOn��$��主控上电��m�  - tbDebug#��� - $-; 'local:StudioCommands.DebugManageCommand���� m�  . 
tbPackInfo#��� . $.; (local:StudioCommands.ViewPackInfoCommand���� m�  / 	tbSetting#��� / $); #local:StudioCommands.SettingCommand��: �� $$��Images/icon-setting2-20x20.png��$	��2,5q��� �� m� -   $��
menuUnlock�� 0 tbUnlock#��� 0 �� -   > ��Name$> btnUpdateSystem��$��程序下载�����#4080BA� $��1��y��� *ProtocolLoaded+ 1 BoolenToVisibilityConvert#9 [1 / �� $	��40* ��� $
��Auto ��� $	��20* ��� $
��Auto ��� $	��30* �.+�� $��0��. �� $	��30* ��� $	��20* ��� $	��40* �.+�� / �� $	��50* ��� $	��50* �.+��� $��0�� 2 MainBorderStyle#��[2 $0 	总电压��._��  . . �� $��2* ��� $��* �/ �� $��* ��� $��* �.+�0  *DevExpress.Xpf.Gauges.CircularGaugeControl  -   ?  Name$? BatVoltGauge��$��0��$��0��$��2��@   Model@ 2  ,DevExpress.Xpf.Gauges.CircularFlatLightModel  A   ScalesA $  DevExpress.Xpf.Gauges.ArcScale  $��Center=�B   
StartAngle$	B 180Q�C   EndAngle$	C 360Q�!  DevExpress.Xpf.Gauges.ScaleD   MajorIntervalCountD �� *VoltMajorIntervalCount+E   
StartValueE �� *VoltRangeStartValue+F   EndValueF �� *VoltRangeEndValue+G   LayersG )  #DevExpress.Xpf.Gauges.ArcScaleLayer    H   NeedlesH *	  $DevExpress.Xpf.Gauges.ArcScaleNeedle	  .
  (DevExpress.Xpf.Gauges.ValueIndicatorBaseI 
  ValueI �� #4 [  $
 VarValue�J 	  PresentationJ =  7DevExpress.Xpf.Gauges.DefaultArcScaleNeedlePresentation  @  :DevExpress.Xpf.Gauges.PredefinedArcScaleNeedlePresentationK   Fill
K �����L   MarkersL *  $DevExpress.Xpf.Gauges.ArcScaleMarker  I �� #4 [  $
 VarValue�M   	RangeBarsM ,  &DevExpress.Xpf.Gauges.ArcScaleRangeBar  I �� #4 [  $
 VarValue�N   RangesN )  #DevExpress.Xpf.Gauges.ArcScaleRange  %  DevExpress.Xpf.Gauges.RangeBaseO   
StartValueO �� *VoltNormalStartValue+P   EndValueP �� *VoltNormalEndValue+Q   PresentationQ <  6DevExpress.Xpf.Gauges.DefaultArcScaleRangePresentation  ?  9DevExpress.Xpf.Gauges.PredefinedArcScaleRangePresentationR   FillR �#00FF00  O �� *VoltLowWarnStartValue+P �� *VoltLowWarnEndValue+Q   
R � ���  O �� *VoltLowProtStartValue+P �� *VoltLowProtEndValue+Q   
R �  ��  O �� *VoltHighWarnStartValue+P �� *VoltHighWarnEndValue+Q   
R � ���  O �� *VoltHighProtStartValue+P �� *VoltHighProtEndValue+Q   
R �  ��� $��1��$��0��$��2��/ �� $��* ��� $��* �.+��� $��0�� 3 	gcBatVolt#��� 3 $��20(�$��5q�$��Center=�$��Center=��� $��1��$��Center=�$��Center=�$��24(�$��SemiCondensed&����� #4 [  $
 VarValue�$1 {0:F2}V���� #��[2 $0 电流��$��1��._�� .+�� $��0�� . . �� $��2* ��� $��* �/ �� $��* ��� $��* �.+�  -   $? CurrentVoltGauge��$��0��$��0��$��2��@   A   $��Center=�$	B 180Q�$	C 360Q�D �� *CurMajorIntervalCount+E �� *CurRangeStartValue+F �� *CurRangeEndValue+G     H 	  I �� #4 [ $
 VarValue�J   
K �����L   I �� #4 [ $
 VarValue�M   I �� #4 [ $
 VarValue�N   O �� *CurNormalStartValue+P �� *CurNormalEndValue+Q   R �#00FF00  O �� *CurLowWarnStartValue+P �� *CurLowWarnEndValue+Q   
R � ���  O �� *CurLowProtStartValue+P �� *CurLowProtEndValue+Q   
R �  ��  O �� *CurHighWarnStartValue+P �� *CurHighWarnEndValue+Q   
R � ���  O �� *CurHighProtStartValue+P �� *CurHighProtEndValue+Q   
R �  ��� $��1��$��0��$��2��/ �� $��* ��� $��* �.+��� $��0�� 4 gcBatCur#��� 4 
������ $��20(�$��Center=�$��Center=��� $��1��$��Center=�$��Center=�$��24(����� #4 [ $
 VarValue�$1 {0:F2}A��� $��1��$��2��/ �� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+�� $��0��#��[! $
��10,5q�S � Minimum$S 0Q�T � Maximum$T 5Q�U � ValueU �� #4 [ $
 VarValue�� $��1��#��[# $
��10,5q�$S 0Q�$T 5Q�U �� #4 [ $
 VarValue�� $��2��#��[$ $
��10,5q�$S 0Q�$T 5Q�U �� #4 [ $
 VarValue��� $��3��#��[2 ._�� / �� $��2* ��� $��* �. �� $
��1.5* ��� $	��10* �.+��� $��Center=�$��Center=�$��0��$��2��$��14(�
���   �$
��Wrap=�# �� #4 [
 ��`� #5 [" .1��� $
 VarName��� $
 VarValue�.  (DevExpress.Xpf.Gauges.LinearGaugeControl  $��20��$��1q�$��0��$��1��$��3��$
��Left=�V   ModelV 5  /DevExpress.Xpf.Gauges.LinearRedThermometerModel  $��1q�W   LayersW ,  &DevExpress.Xpf.Gauges.LinearGaugeLayer  X   PresentationX K  EDevExpress.Xpf.Gauges.FlatLightLinearGaugeBackgroundLayerPresentation  Y   ScalesY '  !DevExpress.Xpf.Gauges.LinearScale  $��0��$	E -40Q�$	F 120Q�Z   MinorIntervalCount$Z 2��$D 2��[   ShowMajorTickmarks] YDevExpress.Data.v15.2, Version=15.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a5  /DevExpress.Utils.Design.DefaultBooleanConverter$
[ True \   ShowMinorTickmarks$\ False ]   
ShowLabels$
] True $��1q�^   Ranges^ ,  &DevExpress.Xpf.Gauges.LinearScaleRange  /  )DevExpress.Xpf.Gauges.RangeValueConverter$	O -40 $P 0 _   Presentation_ ?  9DevExpress.Xpf.Gauges.DefaultLinearScaleRangePresentation  B  <DevExpress.Xpf.Gauges.PredefinedLinearScaleRangePresentation`   Fill
` ��ح�  $O 0 $P 40 _   
` ����  $O 40 $	P 120 _   
` �  ��a   	LevelBarsa /  )DevExpress.Xpf.Gauges.LinearScaleLevelBar  I �� #4 [	 $
 VarValue�b   CustomLabelsb �� $��1��$��2��$��14(�$
��Left=�$��Center=����� #4 [	 $
 VarValue�$1 	{0:F1}℃���� $��4��#��[2 ._�� / �� $��2* ��� $��* �. �� $
��1.5* ��� $	��10* �.+��� $��Center=�$��Center=�$��0��$��2��$��14(�
���   �$
��Wrap=�# �� #4 [ ��`� #5 [" .1��� $
 VarName��� $
 VarValue�  $��20��$��1q�$��0��$��1��$��3��$
��Left=�V   $��1q�W   X   Y   $��0��$	E -40Q�$	F 120Q�$Z 2��$D 2��$
[ True $\ False $
] True $��1q�^   $	O -40 $P 0 _   
` ��ح�  $O 0 $P 40 _   
` ����  $O 40 $	P 120 _   
` �  ��a   I �� #4 [ $
 VarValue�b �� $��1��$��2��$��14(�$
��Left=�$��Center=����� #4 [ $
 VarValue�$1 	{0:F1}℃���� $��2��$��2��#��[2 ._�+ 
 %DevExpress.Xpf.LayoutControl.GroupBox  4  .DevExpress.Xpf.Core.HeaderedContentControlBasec   HeaderTemplate#c [ ,   &DevExpress.Xpf.Core.ContentControlBased    Content.d �� -   e ��Name$e warnStateListBox�� 5 ListBoxStyle1#��[5  "6 CompactWarnListBoxDataTemplate#��[6  7 ListBoxItemSelStyle#��[7 $��Disabled=�# �� *	MasterVar+���� *WarnAndProtect+���� .R�� �� $��1��$��Stretch=� 8 VerGridSplitterStyle#��[8 � $��2��. �� $	��30* ��� $	��10* ��� $	��10* �.+��� $��0��#��[2 ._�� . �� $��* ��� $��* ��� $	��.8* ��� $	��.8* �.+�� $��0��#��[ $
��15,8q�U �� #4 [ $
 VarValue�� $��1��#��[  $
��15,8q�U �� #4 [ $
 VarValue��� $��2��#��[2 $��5q�._�� / �� $
��Auto ��� $
��Auto ��� $��* �.+��� $��0��$��1��$��5q� 	9 brOSI#��[9 #��[ �� $��10q�$��1��$��5q�2 V� .~��� $	3 -90Q��� $��2��$��1��$��Center=�$��Center=����� #4 [ $
 VarValue�$1 {0:F2}K���� $��3��#��[2 $��5q�# �� *	MasterVar+._�� / �� $
��Auto ��� $
��Auto ��� $��* �.+��� $��0��$��1��$��5q�#��[  
: brMode#��[: �� $��10q�$��1��$��5q�2 V� .~��� $	3 -90Q��� $��2��$��1��#��[ $��Center=�$��Center=����� *RcvModeText+�� $��1��#��[2 ._��� #��[5  ; otherListBoxDataTemplate#��[; #��[7 # �� *	MasterVar+���� *OtherMasterDataListDigitVar+�� $��2��#��[2 ._�� . �� $��* ��� $��* ��� $��* ��� $��* ��� $��* �/ �� $
��Auto ��� $��* �.+��� $��0��$��0�� < 	teBatType#��� < $��Right=�$��Center=�$ Right=��� $��0��$��1��$��Center=�$��Center=�$ Center=����� *	BatType+�� $��1��$��0�� = teManufacturers#��� = $��Right=�$��Center=�$ Right=��� $��1��$��1��$��Center=�$��Center=�$ Center=����� *Manufacturers+�� $��2��$��0�� > 	teSoftVer#��� > $��Right=�$��Center=�$ Right=��� $��2��$��1��$��Center=�$��Center=�$ Center=����� *	SoftVer+�� $��3��$��0�� ? 	teHardVer#��� ? $��Right=�$��Center=�$ Right=��� $��3��$��1��$��Center=�$��Center=�$ Center=����� *	HardVer+�� $��4��$��0�� @ 
teBatCells#��� @ $��Right=�$��Center=�$ Right=��� $��4��$��1��$��Center=�$��Center=�$ Center=����� *	CellCnt+�� $��3��$��Stretch=�#��[8 � $��4��. �� $��* ��� $��* ��� $��* �.+��� $��0��#��[2 ._�   A dxglRelayStatusf   Header#f � A .d �� #��[5  !B relayStateListBoxDataTemplate#��[B #��[7 $��Disabled=�# �� *	MasterVar+���� *RelayStateVar+���� .R�� �� $��1��#��[2 ._�   C dxglInputSignals#f � C .d �� #��[5  "D normalStateListBoxDataTemplate#��[D #��[7 $��Disabled=�# �� *	MasterVar+���� *AuxInputSignalStateVar+���� .R�� �� $��2��#��[2 ._�   E gbPackStatus#f � E .d �� #��[5 #��[ #��[7 $��Disabled=�$��
0,10,0,-10q�# �� *	MasterVar+���� *
PackList+���� .R�� � $��2�����#4080BA/ �� $
��Auto ��� $
��Auto ��� $��* �.+�A� $��0��$��20��$��20��
�������$��3��$	��5,0q����� -   commAnimatedBrush��$ Black���� -   $��	labOnLine��$��Center=�$��26��$	��5,0q�$��50��$��14(�
�������$��1������ *OnlineStatus+� $��2��/ �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $��* ��� $
��Auto ��� $
��Auto � ��%�   '��       'A� A     �� " ��.���  � ��$ Center=���  � ��$ Center=��� " A�.���  � ��$ 20����  � ��$ 20��.+�A� $��0��$��0�� F eclpColorMaskProtect#0 � F #��[ $	��5,0q��� $��0��$��1�� G txblProtection#��� G 
�������$��14(�$	��5,0q�A� $��0��$��2�� H eclpColorMaskWarn#0 � H #��[ $	��5,0q��� $��0��$��3�� I txblWarn#��� I 
�������$��14(�$	��5,0q�A� $��0��$��4�� J eclpColorMaskNormal#0 � J #��[ $	��5,0q��� $��0��$��5�� K 
txblNormal#��� K 
�������$��14(�$	��5,0q��� -   $��
labOnLine1��$��实时记录：��$��Center=�$��26��$��5,0,0,0q�$��75��$��14(�
�������$��7���� -   $��
labOnLine2��$��Center=�$��26��$��0,0,10,0q�$��40��$��14(�
�������$��8������ *DigitRecordCount+