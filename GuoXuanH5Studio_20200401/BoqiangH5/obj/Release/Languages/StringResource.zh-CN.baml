   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089 
  biFile  biSelectDevice  coStart  coPause 
 coStop  coClear 
 coSave  coOperation  coFrame 	 coTimes 
 
coSendData  
coInterval 
 coSend  coDirection 
 coTime  scDevice  scDevicePara  scInitialPara 
 scType  
scDevIndex  scDevChannel  scTimer0  scTimer1  
scBaudrate  	scPattern  scCheckCode  scShieldCode  scSelect  scOK  scCanel  teNormal  teListenonly   tePrompt ! tePromptText1 " tePromptText2 # tePromptText3 $ 	teReceive % teStandardFrame & teExtendedFrame ' tbLoadProtocol ( tbLoadParam ) tbLoadAdjustOffset * tbUpLoadParam + tbUpLoadAdjustOffset , tbCommunicationRecords - tbRealtimeRecords . tbHistoryRecords / tbAdjustAndControl 0 tbDebug 1 	tbVerInfo 2 	tbAidInfo 3 
tbPackInfo 4 	tbSetting 5 
tbLanguage 6 	tbChinese 7 	tbEnglish 8 tbUnlock 9 	gcBatVolt : gcBatCur ; cbpMaxCellVolt < cbpMinCellVolt = cbpAvgCellVolt > 	glMaxTmpr 
? chVolt 	@ chCur 	A ttSoc 	B ttSoh 
C teMode D teInsulatio E 	teBatType F teManufacturers G 	teSoftVer H 	teHardVer I 
teBatCells J gbPackStatus 	K brOSI 
L brMode M undefinedMode N dxglRelayStatus O dxglInputSignals P dxglOutputSignals Q dxglWarnAndProtect R upLoadPromt S successPromt T sessionChangePromt U unLockSucessPromt V requestSeedPromt W clearDTCPromt X NoMatchRecordPromt Y errorUploadCompletedPromt Z 
txblOnline [ txblOffline \ eclpColorMaskProtect ] eclpColorMaskWarn ^ eclpColorMaskNormal _ txblProtection ` txblWarn a 
txblNormal b txblRecordCnt c bsiCurrentPack d gpbHighestVolt e gpbLowestVolt f gpbAverageVolt g gpbHighestTmpr h gpbLowestTmpr i txblPackAddr j txblPackSoftVer k txblPackHardVer l txblVoltagedifference m txblTemperaturedifference n txblBalanceMask o txblAFEMask p promtUpLoad q promtParameter r promtAdjust s promtUpLoadParam t promtUpLoadAdjust u promtAdjustAction v promtUpLoadAdjustOffset w tbicOpenParaFile x tbicSaveAsParaFile y tbicParaClose z tbicParaSetAll { tbicParaSave | tbicParaReset } grpbhNumericalparameter ~ grpbhFunctionswitch  grpbhOtherparameters � 
dxgacIndex � 	dxgacName � 
dxgacValue � 	dxgacunit � dxgacoperate � btndownload � promtSetAllPara � promtSavePara � promtResetPara � tbicOpenAdjustFile � tbicSaveAsAdjustFile � tbicAdjustClose � tbicAdjustSetAll � tbicAdjustSave � tbicAdjustReset � grpbhNumericaladjust � grpbhBmsClock � grpbhFunctionctrl � grpbhVersionInfo � btncReadTime � btncSetTime � readTimePromtInfo � setTimePromtInfo � grpbhRelayCtrl � tbicRealTimeRecordStart � tbicRealTimeRecordStop � tbicRealTimeRecordSave � tbicRealTimeRecordClear � tbicRealTimeRecordClose � dateRealPromt � timeRealPromt � modeRealPromt � auxInputSignalPromt � relayStatusPromt � warnRealPromt � 	datePromt � 	timePromt � 	warnPromt � clearDTC � readDTC � promtSessionSelect � promtUnlockOperation � promtChangeto � promtSessionType � promtSessionChange � promtRequestSeed � promtSendKey � promptNonProtocol � promptNonParameter � promptNonAdjust � promtNoFidProtocol � 
promtError � loadProtocolFile � protocolDefinitionFile � promtAllFile � promtProtocolLoading � promtParaFile � promtOpenParaFileSuccess � promtOpenParaFileFailed � promtAllParaUploading � promtAllAdjustUploading � promtPleaseWait � promtLoading � promtLoadingBasicSettings � promtLoaddingmeterSettings � promtLadingMasterSettings � promtLoadingPackSettings � promtInitializationTable � promtProtocolLoadingSucess�� D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     .sys&clr-namespace:System;assembly=mscorlib  %y  &	        &	      &	 &     &	 6     &	 F     &	 V     &	 f     &	 v     &	 �     &		 �     &	
 �     &	 �     &	 �     &	 �     &	     &	     &	 %    &	 ;    &	 T    &	 g    &	 z    &	 �    &	 �    &	 �    &	 �    &	 �    &	     &	     &	 1    &	 A    &	 Q    &	 a    &	  q    &	! �    &	" �    &	# �    &	$     &	%     &	& %    &	' 8    &	( N    &	) d    &	* �    &	+ �    &	, �    &	- �    &	. �    &	/ �    &	0     &	1     &	2 3    &	3 I    &	4 ]    &	5 m    &	6 }    &	7 �    &	8 �    &	9 �    &	: �    &	; �    &	< �    &	= �    &	> 	    &	?     &	@ *    &	A :    &	B P    &	C l    &	D |    &	E �    &	F �    &	G �    &	H �    &	I �    &	J �    &	K     &	L     &	M -    &	N @    &	O Y    &	P o    &	Q �    &	R �    &	S �    &	T �    &	U �    &	V �    &	W     &	X (    &	Y A    &	Z W    &	[ g    &	\ w    &	] �    &	^ �    &	_ �    &	` �    &	a �    &	b     &	c     &	d 0    &	e M    &	f j    &	g �    &	h �    &	i �    &	j �    &	k �    &	l �    &	m �    &	n 	    &	o '	    &	p ;	    &	q K	    &	r [	    &	s k	    &	t �	    &	u �	    &	v �	    &	w �	    &	x �	    &	y �	    &	z 
    &	{ 
    &	| 4
    &	} J
    &	~ `
    &	 v
    &	� �
    &	� �
    &	� �
    &	� �
    &	� �
    &	� �
    &	� �
    &	�     &	� !    &	� ;    &	� W    &	� j    &	� z    &	� �    &	� �    &	� �    &	� �    &	� �    &	�     &	�     &	� *    &	� @    &	� V    &	� o    &	� �    &	� �    &	� �    &	� �    &	� �    &	� �    &	� �    &	� �    &	�     &	� $    &	� =    &	� M    &	� ]    &	� m    &	� }    &	� �    &	� �    &	� �    &	� �    &	� �    &	� �    &	�     &	� '    &	� :    &	� S    &	� l    &	� �    &	� �    &	� �    &	�     &	� (    &	� >    &	� T    &	� j    &	� �    &	� �    &	� �    &	� �    &	� �    &	�     &	� 4    &	� S    &	� r    &	� �    &	� �    ��
文件����选择设备����
启动����
暂停����
停止����
清空����
保存����基本操作����
帧ID(HEX)����发送次数����数据(HEX)����发送间隔(ms)����
发送����
方向����
时间����
设备����设备参数����初始化参数����	类型：����	索引：����	通道：����定时器0：0x����定时器1：0x����波特率：����工作模式：����验收码：0x����屏蔽码：0x����	请选择����
确定����
取消����
正常����
只听����
提示����C?此类型CAN盒没有多余的设备或者通道可以打开！����打开CAN盒失败！����发送失败！����
接收����	标准帧����	扩展帧����加载协议����加在参数����加载校准偏移����上传参数����上传校准偏移值����通讯记录����实时记录����	DTC信息����校准和控制����
调试����版本信息����辅助信息����
Pack信息����
设置����
语言����
中文����
英文����
解锁����总电压(V)����	电流(A)����最大:����最小:����平均:����最高:����
电压����
电流����剩余容量����电池健康状态����
模式����绝缘阻值����电池类型:����厂商:����软件版本:����硬件版本:����串数:����
Pack状态����
绝缘����
模式����	未定义����继电器状态����输入信号����输出信号����告警和保护����
上传����
成功����会话切换成功����解锁成功����请求种子成功����DTC信息已清楚����无匹配记录����上传完成����
在线����
断开����颜色 标识--保护����颜色 标识--告警����颜色 标识--正常����
保护����
告警����
正常����实时记录:����当前pack:����最高单体电压:����最低单体电压:����平均电压:����最高温度:����最低温度:����地址:����软件版本:����硬件版本:����压差:����温差:����均衡标识:����
AFE标识:����
上传����
参数����
校准����上传参数����上传校准值����
校准����上传校准偏移值����打开参数文件����	另存为����
关闭����设置所有����设置保存����恢复出厂����数值参数����功能开关����其它参数����
序号����
名称����
数值����
单位����
操作����
下载����Set all parameters����Save parameters����Reset parameters����打开校准文件����	另存为����
关闭����设置所有����设置保存����恢复出厂����数值校准����保护板时钟����功能控制����版本信息����	读时间����设置时间����读BMS时间����设置BMS时间����继电气控制����
开始����
停止����
保存����
清除����
关闭����
日期����
时间����
模式����输入输出信号����继电器状态����
告警����
日期����
时间����
告警����	清除DTC����
读DTC����会话选择 $10����解锁操作 $27����
切换����会话类型:����会话切换����请求种子����	发送key����非协议文件����非参数文件����非校准文件����1-未发现协议文件，请导入协议文件����
错误����:6加载协议文件失败，请确认后重新加载。����协议定义文件����所有文件����加载协议����参数文件����打开参数文件成功.����打开参数文件失败.����上传所有参数����上传所有校准值.����	请等待����加载中...����加载基本设定...����加载仪表设置...����加载主控配置...����加载pack配置...����表格初始化...����协议加载成功��