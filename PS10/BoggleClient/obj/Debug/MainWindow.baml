   M S B A M L   `   `   `  ����  BoggleClient    BoggleClient.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  BOGGLE - Connect��5   	   $	��700��6"   $	��500��6/    � 
ResizeMode$ NoResize=�6;    � Icon$# Resource/boggleAlphabet/B.png��6Q   .��5      � -   5        ��5   
   �� 5       �� EndPoint$ 0.5,1&�6"    � MappingMode$ RelativeToBoundingBox=�63    �� 
StartPoint$ 0.5,0&�6W   .��5      �  � Color$ 	#FF46A042��6   � 5	      $ 	#FF39AC9F��6    � Offset$ 0.749Q�61   5
      .+�5   
   �� -   5         ��Name$ IPAddress_Text_Box��5      $��Center=�6.   $
��Left=�6P   $��45��6k   $��228,437,0,0q�6w   	 �� TextWrapping$
	 Wrap=�6�   $	��Top=�6�   $	��163��6�   $��20(�6�   $��	localhost��6�   �� 5   
   -   5        $ Name_Text_Box��5      $��Center=�6)   $
��Left=�6K   $��41��6f   $��228,507,0,0q�6r   $
	 Wrap=�6�   $	��Top=�6�   $	��163��6�   $��20(�6�   �� 5   
   $��IP Address:��6   $
��Left=�6&   $��
86,437,0,0q�6A   $	��Top=�6U   
 J� RenderTransformOrigin$
 0.042,-0.344&�6m   $��45��6�   $��24(�6�   �� 5   
   $��Name:��6   $
��Left=�6    $��126,507,0,0q�6;   $	��Top=�6P   $��41��6h   $��87��6t   $��24(�6   �� 5   
   -   5         ��Name$ Play_Button��5      $
��Play��6&   $
��Left=�65   $��
86,584,0,0q�6P   $	��Top=�6d   $	��305��6|   $��45��6�   $��20(�6�   �� 5   
   $��Boggle��6   $��Center=�6!   $
��Left=�6E   $	��Top=�6`   $��106,-13,0,0q�6   $	��Top=�6�   $
 -5.503,-0.383&�6�   $	��114��6�   $	��270��6�   $��72(�6�   ���	#FFFFFDFD$
��Bold"�6  �� 5   
   $
��Left=�6   $��
43,106,0,0q�6/   $
��Wrap=�6C   $	��Top=�6W   $
 -3.284,-1.896&�6o   $	��305��6�   $	��410��6�   $��16(�6�   �� 6�   $��20(�6�   $�Rules:��6�   �� 6�   �� 6�   �� 6�   $0�*- Words with 2 letters or less are ignored��6�   �� 6/  �� 6;  $.�(- 3 and 4 letter words count for 1 point��6?  �� 6q  �� 6}  $)�#- 5 letter words count for 2 points��6�  �� 6�  �� 6�  $)�#- 6 letter words count for 3 points��6�  �� 6�  �� 6�  $)�#- 7 letter words count for 5 points��6�  �� 6(  �� 64  $+�%- 8+ letter words count for 11 points��68  �� 6g  �� 6s  $+�%- Incorrect words count for -1 points��6w  �� 6�  �� 6�  $6�0- Scoring causes your opponents letters to spin!��6�  �� 6�  �� 6�  �� 6  �� 6  $>�8To play, enter an IP address or URL and your name below.��6  �� 6T  �� 6`  $/�)Once two players join, a game will start.��6d  �� 6�  �� 6�  $�
Good Luck!��6�  6�  5      5      