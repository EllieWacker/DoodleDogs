   M S B A M L   `   `   `  ����  WpfPresentation'    !WpfPresentation.frmUpdatePassword   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Update Your Password��5   	   $	��300��6&   $	��440��63    � WindowStartupLocation$ CenterScreen=�5   	   .��5      � 
������� � RowDefinitions 5   
   �� 5      6   �� 5	      6   �� 5
      6   �� 5      6   �� 5      6   �� 5      6    � ColumnDefinitions 5   
   �� 5      6!   �� 5      $	��300 �6   6-   .+�5   
   �� -   5         ��Name$ 
tbkMessage��5      $
��Wrap=�6(    �� TextAlignment$ Center=�5      $��25 10q�6+   $��0��5      $��2��6!   $��13(�65   $
��Bold"�5       J� RenderTransformOrigin$ 0.5,0.5&�6&   $��Change your password.��6F   �� 5   
   $��1��6   $��0��6   $��10q�6-   $��Right=�69   $��Center=�5      .��6+   Enter your Email:6>   �� 5   
   $��2��6   $��0��6   $��10q�6-   $��Right=�69   $��Center=�5      .��6+   Current Password:6>   �� 5   
   $��3��6   $��0��6   $��10q�6-   $��Right=�69   $��Center=�5      .��6+   New Password:6:   �� 5   
   $��4��6   $��0��6   $��10q�6-   $��Right=�69   $��Center=�5       .��6+   Retype Password:6=   �� 5"   
   -   5         ��Name$ txtEmail��5"      $��1��6$   $��1��61   $
��10 5q�6A   G� 5#   
   -   5         G�Name$ pwdOldPassword��5#      $��2��6.   $��1��6;   $
��10 5q�6K   G� 5$   
   -   5        $ pwdNewPassword��5$      $��3��6.   $��1��6;   $
��10 5q�6K   G� 5%   
   -   5        $ pwdRetypePassword��5%      $��4��61   $��1��6>   $
��10 5q�6N   �� 5&   
   -   5        	 ��Name$	 	btnSubmit��5&      $��5��6$   $��2��61   $��150 10q�5'      $��Submit��6!   62   5(      5)      