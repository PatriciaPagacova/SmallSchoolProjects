   M S B A M L   `   `   `  ����C  ?Projekt1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:Projekt1Projekt1  5   	       Projekt1.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Projekt1     5   	     � Title$  
MainWindow��5   	   $	��630��6   $
��1100��6)   .��5	      � .+�5
   
   �� -   5         ��Name$ canvas��5
      $
��Left=�6!   $	��581��6<   $��	10,10,0,0q�6I   $	��Top=�6\   $	��826��6t   .+�5      �� ���	#FFDADADA$	��561��6)   
���   �$	��806��6E    �� Left$ 10��6Q   
 �� Top$ 10��6b   5      �� 5   
   -   5         ��Name$ slider��5      $
��Left=�6!   $��882,393,0,0q�6<   $	��Top=�6Q   $	��148��6i    � Maximum$ 1Q�6u    �� TickFrequency$ 0.005Q�6�    �� IsSnapToTickEnabled .  � Value$	 0.5Q�6�   $��20��6�   ���	#FFE5E5E5���	#FFFCDCC1��6  Z� �� 5   
   -   5        	 ��Name$	 CheckBoxCasteljau��5      $��Casteljau Algoritmus��6.   $
��Left=�6M   $��882,348,0,0q�6h   $	��Top=�6}   
�������6�   	� 5   
   -   5        
 	�Name$
 RBPrerozdelenie��5      $��Subdivision��6/   $
��Left=�6E   $��882,255,0,0q�6`   $	��Top=�6u    d� 	IsChecked$
 TrueY�6�   $
��Bold"�6�   $��14(�6�   	� 5   
   -   5        $
 RBPriamyVypocet��5      $��Direct computation��6/   $
��Left=�6L   $��
882,56,0,0q�6g   $	��Top=�6{   $
��Bold"�6�   $��14(�6�   	� 5   
   -   5        $
 RBZvysovanieStupna��5      $��Degree elevation��62   $
��Left=�6M   $��882,166,0,0q�6h   $	��Top=�6}   $
��Bold"�6�   $��14(�6�   �� 5   
   -   5         ��Name$ MinusPriamyVypocet��5      $��-��6-   $
��Left=�69   $��970,107,0,0q�6T   $	��Top=�6i   $��30��6�   $��30��6�   �� 5   
   -   5        $ PlusPriamyVypocet��5      $��+��6,   $
��Left=�68   $��1032,107,0,0q�6S   $	��Top=�6i   $��30��6�   $��30��6�   �� 5   
   -	   5         ��Name$ PocetLOD��5      $
��Left=�6$   $��30��6?   $��1000,107,0,0q�6K    �� TextWrapping$
 Wrap=�6a   $��30��6u   $	��Top=�6   $��32��6�   $��Center=�6�   $��Center=�6�   ���	#FFFCDCC1{�.  6"  �� 5   
   -
   5        $ PocetR��5      $
��Left=�6"   $��30��6=   $��1000,205,0,0q�6I   $
 Wrap=�6_   $��20��6s   $	��Top=�6}   $��32��6�   $��Center=�6�   $��Center=�6�   ���	#FFFCDCC1{�.  6  �� 5   
   -   5        $ HlbkaM��5      $
��Left=�6"   $��30��6=   $��1000,292,0,0q�6I   $
 Wrap=�6_   $��5��6s   $	��Top=�6|   $��32��6�   $��Center=�6�   $��Center=�6�   ���	#FFFCDCC1{�.  6  �� 5   
   -   5        $ MinusZvysovanieStupna��5      $��-��60   $
��Left=�6<   $��970,205,0,0q�6W   $	��Top=�6l   $��30��6�   $��30��6�   6�   �� 5   
   -   5        $ PlusZvysovanieStupna��5      $��+��6/   $
��Left=�6;   $��1032,205,0,0q�6V   $	��Top=�6l   $��30��6�   $��30��6�   6�   �� 5   
   -   5        $ MinusPrerozdelenie��5      $��-��6-   $��970,292,0,0q�69   $	��Top=�6N   $
��Left=�6f   $��30��6�   $��30��6�   6�   �� 5   
   -   5        $ PlusPrerozdelenie��5      $��+��6,   $��1032,292,0,0q�68   $	��Top=�6N   $
��Left=�6g   $��30��6�   $��30��6�   6�   �� 5   
   -   5        $ Reset��5      $��Reset��6    $
��Left=�60   $��882,536,0,0q�6K   $	��Top=�6`   $	��146��6x   ���	#FFD71010$��27��6�   $��14(�6�   �� 5   
   -   5         ��Name$ 
ParameterT��5      $	��0.5��6$   $
��Left=�62   $��942,418,0,0q�6M   $	��Top=�6b   �� 5   
   $
��t  =��6   $
��Left=�6   $��912,418,0,0q�6:   $	��Top=�6O   $��30��6g   �� 5   
   $��Number of samples��6   $
��Left=�6,   $��861,107,0,0q�6G   $	��Top=�6\   $��98��6t   $��10(�6   �� 5   
   $��Number of steps��6   $
��Left=�6*   $��861,205,0,0q�6E   $	��Top=�6Z   $��10(�6r   �� 5    
   $��Number of steps��6   $
��Left=�6*   $��861,292,0,0q�6E   $	��Top=�6Z   $��10(�6r   5!      5"      