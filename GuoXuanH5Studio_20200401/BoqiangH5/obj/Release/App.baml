   M S B A M L   `   `   `  ���� D  @UDSStudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:UDSStudio	UDSStudio      UDSStudio.App   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     a ]DevExpress.Xpf.Core.v15.2, Version=15.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a?dxe5http://schemas.devexpress.com/winfx/2008/xaml/editors  ;dx2http://schemas.devexpress.com/winfx/2008/xaml/core  <dxb2http://schemas.devexpress.com/winfx/2008/xaml/bars  #localclr-namespace:UDSStudio    �� 	Resources   �� MergedDictionaries �� Source   ProtectOnBrush �� Color  WarnOnBrush  NormalOnBrush � Color � Offset  
AFEOnBrush  BalanceOnBrush  
openSwitch 
� Center  OffBrush    UDSStudio.ReverseConverter  rc$   UDSStudio.HexToStringConverter  hsc'   !UDSStudio.ContentToEmptyConverter 	 cte"   UDSStudio.ByteToIntConverter 
 bic'   !UDSStudio.FlagToIsEnableConverter  ftc#   UDSStudio.RowToFocusConverter  rtc"   UDSStudio.DigitViewConverter  DigitViewConverter!   UDSStudio.TextViewConverter  TextViewConverter&	    UDSStudio.ExtremumValueConverter  ExtremumValueConverter#
   UDSStudio.VisibilityConverter  vbc$   UDSStudio.StatusColorConverter  StatusColorConverter   ProtectOnBrush   WarnOnBrush	   NormalOnBrush
   
AFEOnBrush   BalanceOnBrush   OffBrush)   #UDSStudio.BoolenToVisibilityConvert  BoolenToVisibilityConvert   UDSStudio.ZLGFuction 
 ZLGCAN �� 
TargetType  style_TextBlock �� Property �� Value&   DevExpress.Xpf.Core.SimpleButton  style_SimpleButton%  DevExpress.Xpf.Editors.TextEdit  style_TextEdit  ListBoxStyle1  ListBoxItemSelStyle �� 	Resources  VerGridSplitterStyle  HorGridSplitterStyle  MainBorderStyle  otherListBoxDataTemplate � ColumnDefinitions �� Padding �� Path `� 	Converter " CompactWarnListBoxDataTemplate � 	Resources �� Triggers �� Binding �� Value N� EnterActions g� RepeatBehavior g� 	BeginTime �� TargetProperty W� From	 W� To g� Duration  �� 	Converter " normalStateListBoxDataTemplate ! relayStateListBoxDataTemplate! � RowDefinitions   PackOnlineStateListBox'  !DevExpress.Xpf.Bars.BarStaticItem!  DevExpress.Xpf.Bars.BarItem3  -DevExpress.Xpf.Core.DXImageGrayscaleExtension"   Image:  4DevExpress.Xpf.Core.Native.DXImageGrayscaleConverter*  $DevExpress.Xpf.Core.DXImageExtension#   Image1  +DevExpress.Xpf.Core.Native.DXImageConverter��  �� $) #Languages\StringResource.zh-CN.xaml?��� $) #Languages\StringResource.en-US.xaml?��� $# ControlResource\ComboBox.xaml?��� $$ ControlResource\CheckEdit.xaml?��� $ Styles/DataGrid.xaml?�%b  &	        &	      &	 !     &	 z     &	 �     &	 ,    &	 z    &	 �    &	 �    &		 �    &	
 �    &	 �    &	 �    &	 �    &	 �    &	 �    &	 �    &	 �    7   7  7  7  7  7  &	 �    &	 �    &	 �    &	 0    &	 p    &	 �    &	     &	 i    &	 $    &	 �    &	 {    7  &	 �    7  7  &	 !
    7  &	 K    7  7  &	      �� $	 Red���� $ Yellow���� .��� $ #00FF00��$ 0Q�� $ White��$	 0.5Q�� $ #00FF00��$ 1Q��� .��� $ #FF0033��$ 0Q�� $ White��$	 0.5Q�� $ #FF0033��$ 1Q��� .��� $ #3399FF��$ 0Q�� $ White��$	 0.5Q�� $ #3399FF��$ 1Q�
� $ 0.5,0.5&�.��� $ White��$	 0.2Q�� $ 	#FF29F008��$ 1Q��� $ Transparent��                	  
    8   8  8	  8
  8  8      �� " ��.���  � ��$ Center=���  � ��$ Right=��� "  .���  � ��$ Center=���  � ��$ Right=��� "  .���  � ��$ Center=���  � ��$
 Left=��� " ��.���  � ��
 ���� ��  �@J�SnapsToDevicePixels . ��  � ��$ 3q��� " �� ���� $ Transparent��(��       * SystemColors.HighlightBrushKey+)�� $ Black��(��       *$"SystemColors.HighlightTextBrushKey+)�� $ Transparent��(��       *SystemColors.ControlBrushKey+)�� $ Black��(��       *" SystemColors.ControlTextBrushKey+).���  � ��$ Stretch=���  � ��$ Stretch=��� " ��.���  � ��$ 2����  � ��$ Center=���  � ��$ Stretch=���  �@��ShowsPreview .  ��  �@��ResizeDirection$ Columns=���  � ��
 ���� �� " ��.���  � ��$ 2����  � ��$ Stretch=���  � ��$ Center=���  �@��ShowsPreview .  ��  �@��ResizeDirection$
 Rows=���  � ��
 ���� �� " ��.���  � ��$ 1q���  � �� �	#FFADB9CD��  �@��CornerRadius$ 5����  � ��$ 3q���  �@J�SnapsToDevicePixels . �� .R�� .+��� $��5q�$��1q�
�������$	��150��._��  �� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0��$ 3q����� $ VarName��� $��1��$ 3q�$��5,0,5,0q�$��Right=���`� 8   .1��� $ VarValue��� $ VarScale��� $ VarUnit��� .R��  �� $
��Auto ��� $��* � ���� 'A�       " A�.���  �@J�Opacity$	 0.3Q� ��  �� $ 
StateValue� True �� .���� $ Forever��.��W� $ 0X�$ (UIElement.Opacity)�$ 1X�$ 0X�$ 00:00:02D�.a���  �@J�Opacity$	 1.0Q���  � �� �� $ VarPromptStatus�8    �� '��       " ��.���  �@J�Opacity$	 0.3Q� ��  �� $ 
StateValue� True.a���  �@J�Opacity$	 1.0Q���  � �� �� $ VarPromptStatus�8   .+�A� $��0��$��8,8,0,8q�$��14��$��14�����	#FF908C8C�� $��1��$��3,8,5,8q�
���   ����� $ 	StateName��� .R��  �� $
��Auto ��� $��* � ���� '��       " ��.���  �@�ToolTip$ 当前状态：OFF����  �@J�Opacity$	 0.3Q� ��  �� $ 
StateValue� True.a���  �@�ToolTip$ 当前状态：ON����  �@J�Opacity$	 1.0Q��� 'A�       " A�.���  �@J�Opacity$ 1Q� ��  �� $ 
StateValue� True.a���  � ��8   ��  �@J�Opacity$	 1.0Q�.+�A� $��0��$��8,8,0,8q�$��14��$��14�����	#FF908C8C�� $��1��$��3,8,5,8q�
���   ����� $ 	StateName��� .R��� 8��  ._�� $��80��$��80��$	��120��$	��120��! �� $��* ��� $��* � �� $��* ��� $��* � ���� '��       " ��.���  �@J�Opacity$	 0.3Q���  � ��$ 5q���  � ��
 � �����  � �� �� .R�(� .��F� 
��� ���$��1������Ѐ�� ��� � -1��� ׀����� ��� �� � -1��� ��� �� ���^��j � l��� �@����� ��2
��� ��� �� �@����� ��� �� ��2
��� �@ɢ��� 	 ��  �� *
StateValue+ True.a���  �@J�Opacity$ 1Q���  � ��
 ������  � �� �� .R�(� .��F� 
������$��1������Ѐ�� ��� � -1��� ׀����� ��� �� � -1��� ��� �� ���^��j � l���j �@����� ��2
��� ��� �� �@����� ��� �� ��2
��� �@ɢ��� 	�� '��       " ��.���  � ��$ Center=���  � ��$ Center=���  �@J�Opacity$	 0.3Q���  � ��$	 Off����  � ��$ 5q���  � ��
 � E�� ��  �� *
StateValue+ True.a���  �@J�Opacity$ 1Q���  � ��$ On����  � ��
 ����.+��� $��0��$��0���� $��0��$��1���� $��1��$��0��$��2��8�� 
����ح�._��� $��Center=�$��Center=���Z� ���� *	StateName+�� .R��  ���� '        "  .���  �@ Glyph   $" Apply_16x16.png ��  �@ Content �� *
PackName+ ��  �� *
IsOnline+ True.a���  �@ Glyph   $# Apply_16x16.png 