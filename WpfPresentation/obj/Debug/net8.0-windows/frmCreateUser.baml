   M S B A M L   `   `   `  ����  WpfPresentation#    WpfPresentation.frmCreateUser   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Create Account��5   	   $	��300��6    $	��440��6-    � WindowStartupLocation$ CenterScreen=�5   	   .��5      � 
������� � RowDefinitions 5   
   �� 5      6   �� 5	      6   �� 5
      6   �� 5      6   �� 5      6   �� 5      6    � ColumnDefinitions 5   
   �� 5      �� 5      $	��300 �6   6-   .+�5   
   �� -   5         ��Name$ 
tbkMessage��5      $
��Wrap=�6(    �� TextAlignment$ Center=�5      $��25,10,25,10q�6+   $��0��5      $��2��6!   $��13(�65   $
��Bold"�5       J� RenderTransformOrigin$ 0.5,0.5&�6&   $��Enter Your Information��6F   �� 5   
   $��1��6   $��0��6   $
��Left=�6-   $��Center=�5      $��First Name:��6+   $��25,0,0,0q�6A   �� 5   
   $��2��6   $��0��6   $��25,0,0,0q�6-   $
��Left=�6?   $��Center=�5      $��
Last Name:��6+   �� 5   
   $��3��6   $��0��6   $
��Left=�6-   $��Phone Number:��6H   $��Center=�6`   $��25,0,0,0q�6{   �� 5   
   $��4��6   $��0��6   $��28,0,0,0q�6-   $
��Left=�6?   $��Center=�5      $��Email:��6+   �� 5!   
   -   5         ��Name$ txtFirstName��5!      $��1��6(   $��1��65   $��	10,5,10,5q�6E   �� 5"   
   -   5        $ txtLastName��5"      $��2��6'   $��1��64   $��	10,5,10,5q�6D   �� 5#   
   -   5        $ txtPhoneNumber��5#      $��3��6*   $��1��67   $��	10,5,10,5q�6G   �� 5$   
   -   5        $ txtEmail��5$      $��4��6$   $��1��61   $��	10,5,10,5q�6A   �� 5%   
   -   5         ��Name$ 	btnSubmit��5%      $��5��6$   $��10,10,150,10q�5&      $��Submit��6'   $��1��6P   5'      5(      