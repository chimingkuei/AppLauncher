   M S B A M L   `   `   `  ����[  WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms  5   	   F BAppLauncher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:AppLauncherAppLauncher 5   	      AppLauncher.MainWindow   5   	   ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   X TXceed.Wpf.Toolkit, Version=4.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   Iwf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms  5   	   %localclr-namespace:AppLauncher  5   	   -   5          � 
ResizeMode$  CanMinimize=�5   	    � Title$ Template��5   	   $	��700��6   $
��1000��6'   ���	#FF3E6FB3 � WindowStartupLocation$ CenterScreen=�6K   .��5      � .+�5   
   �� -   5         ��Name$	 Log��5      $
��Left=�6"   $	��172��6=   $��	5,485,0,0q�6J   $	��Top=�6]   $	��790��6u   ���	#FFE6F0FF � 	Resources 5       �� 
TargetType �� Property �� Value��%d  '��       'J� �     �� 5      " ��6   .�5      �� # �@��VerticalScrollBarVisibility$
 Auto=�6D   �� 5       � ��$ 0q�6/   �� 5       �@��
IsReadOnly . 63   �� 5       � ��$ Bottom=�67   5      �� 5      " J�6   .�5      ��  �@��Margin$ 0q�6/   �� 5       � ��$ 16(�61   5      5      .@�5      �� $��一般��6      BaseTabcontrol#��[  6"   .��5      �� -   5         ��Name$ richTextBoxGeneral��5      ���	#FF43B5BF6:   5      �� 5      $��警告��6   #��[  6"   .��5      �� -   5        $ richTextBoxWarning��5      ���	#FF43AABF6:   5       �� 5!      $��錯誤��6   #��[  6"   .��5"      �� -   5        $ richTextBoxError��5"      ���	#FF43A0BF68   5#      �� 5$      $��除錯��6   #��[  6"   .��5%      �� -   5        $ richTextBoxDebug��5%      ���	#FF4396BF68   5&      5'      �� 5(   
   $
��Left=�6   $��5,3,0,0q�60   $	��Top=�6A   $	��478��6Y   $	��978��6f   ���	#FFE6F0FF.@�5)      �� $��	主畫面��6   #��[  6#   .��5*      � .+�5+      �� -   5        	 ��Name$
	 Demo��5+        
BaseButton#��[ 6+   $��785,83,45,307q�6O   $
��Demo��6f   $��18(�6u   ���	#FF3E6FB36�   �� 5,      -   5        
 ��Name$
 
Parameter1��5,        BaseTextBox#��[ 62   
 � Tag$ 請輸入內容...��6W   $��790,193,52,212q�6f   �� 5-      -	   5        $
 
Parameter2��5-      #��[ 62   $ 請輸入內容...��6W   $��793,267,49,139q�6f   5/      50      �� 51      $��參數畫面��6   #��[  6$   .��52      � 54      55      56      57      58      