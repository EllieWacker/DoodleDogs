   M S B A M L   `   `   `  ����  WpfPresentation$    WpfPresentation.frmPuppyDetail   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Puppy Detail View��5   	   $	��340��6#   $	��480��60    � WindowStartupLocation$ CenterScreen=�5   	   .��5      � 
������� � RowDefinitions 5   
   �� 5      $	��150 �6   �� 5	      $��40 �6   �� 5
      $��40 �6   �� 5      $��40 �6    � ColumnDefinitions 5   
   �� 5      $��10 �6   �� 5      $	��120 �6   �� 5      $	��220 �6   �� 5      $	��120 �6   �� 5      $
��auto �6   .+�5   
   �� -   5         ��Name$ imgPup��5      $��0��6    $��2��6-   $��
Totoro.png��6=   �� 5   
   -   5         ��Name$ lblPupAvail��5      $��1��6%   $��1��62   $
��Left=�6B   $��Center=�5      $��Available: ��6+   �� 5   
   -   5        $ lblPupBreed��5      $��1��6%   $��2��62   $��Center=�6B   $��Center=�5      $��Aussiedoodle��6+   �� 5   
   -   5        $ lblPupMicro��5      $��1��6%   $��3��62   $
��Left=�6B   $��Center=�5      $��Microchip: ��6,   �� 5   
   -   5        $ lblPupPrice��5      $��2��6%   $��1��62   $
��Left=�6B   $��Center=�5      $��Price: ��6,   �� 5   
   -   5        $ lblPupTreatment��5      $��2��6*   $��2��67   $��Center=�6G   $��Center=�5      $��Treatment: ��6,   4  .System.Windows.Automation.AutomationProperties   IsColumnHeader$
 True��6B   �� 5    
   -   5        $ lblPupWeight��5       $��2��6'   $��3��64   $
��Left=�6D   $��Center=�5!      $��Weight: ��6,   �� 5"   
   -	   5         ��Name$ btnPupAdopt��5"      $��3��6&   $��2��63   $��Center=�6C   $��Center=�5#      $��Adopt��6,   $��50��6<   $��30��6G   
�������6f   5&      5'      