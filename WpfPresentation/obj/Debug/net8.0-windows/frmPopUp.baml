   M S B A M L   `   `   `  ����  WpfPresentation    WpfPresentation.frmPopUp   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Approve Application��5   	   $	��200��6%   $	��300��62    � WindowStartupLocation$ CenterScreen=�5   	   .��5      � $��6��6   $��3��6    � RowDefinitions 5   
   �� 5      $��30 �6   �� 5	      $��70 �6   �� 5
      $��50 �6   �� 5      $
��auto �6    � ColumnDefinitions 5   
   �� 5      $	��150 �6   �� 5      $	��150 �6   .+�5   
   �� $��0��6   $��0��6$   $��2��61   $��Application Details��6E    �� Padding$ 10,10,10,10q�6`   $	��120��6v   $
��Wrap=�6�   $
��auto��6�   �� 5   
   -   5         ��Name$ txtApplicationDetails��5      $��0��63   $��1��6C   $��2��6P   $��Application Details��6d   $ 25,10,25,10q�6   $
��Wrap=�6�   $
��auto��6�   $��0,0,10,0q�6�   �� 5   
   -   5         ��Name$ 
btnApprove��5      $��Approve��6%   $��0��68   $��2��6H   $��Right=�6U   $��5,5,5,5q�6q   $	��Top=�6�   $��	10,0,10,0q�6�   ���	#FFF4F2EC���	#FFDDDADA6�   �� 5   
   -   5        $ btnDeny��5      $
��Deny��6"   $��1��62   $��2��6B   $
��Left=�6O   $	��Top=�6j   $��5,5,5,5q�6�   $��	10,0,10,0q�6�   ���	#FFF4F2EC���	#FFDDDADA6�   5      5      