   M S B A M L   `   `   `  ���� |  xSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting   Chart Control In WPF   WindowsTimer.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     �my{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit       � Title$  Windows Timer��$��854.03��$
��1137��.���� $
��Auto=�$
��Auto=�$��
0,-28,0,28q�.��� $	��836�� � 	Resources C   =System.Windows.Controls.DataVisualization.Charting.LinearAxis=   7System.Windows.Controls.DataVisualization.Charting.Axis   OrientationD   >System.Windows.Controls.DataVisualization.Charting.DisplayAxis   Title 	  YAxis��%3   &	          $ Y=�$ 
Procenty %��$
��Left=�.+�>   8System.Windows.Controls.DataVisualization.Charting.Chart  -   $��Chart1��$	��771��$
��Left=�$��10,0,-0.4,0q�   Title$$ Czas działania okien ogółem��$��Bottom=�$
��1104��$y�Hidden=�   Series. E   ?System.Windows.Controls.DataVisualization.Charting.ColumnSeries  -   $��MyChart1��H   BSystem.Windows.Controls.DataVisualization.Charting.DataPointSeries   DependentValuePath$ Value��?   9System.Windows.Controls.DataVisualization.Charting.Series   Title$+ %Kliknij w słupek by uzyskać nazwę!��   IndependentValuePath$	 Key���   �System.Windows.Controls.DataVisualization.Charting.ColumnBarBaseSeries`1[[System.Windows.Controls.DataVisualization.Charting.ColumnDataPoint, System.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]	   DependentRangeAxis#	 [  
   IsSelectionEnabled
 .    ItemsSource �� &	   System.Windows.Controls.DataGrid	  -    	 Name$ 	DataGrid1��$��0,65,-0.4,0q�$
��Bold"� 	  AutoGenerateColumns .   	  
IsReadOnly . $y�Hidden=� 	  Columns 0
  *System.Windows.Controls.DataGridTextColumn
  ,  &System.Windows.Controls.DataGridColumn   Header$ 
Nazwa okna��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *nazwa+
  $ Czas korzystania [s]�� �� *	sekundy+�� -    ��Name$ 	graph1Btn��$��Tabela��$��Right=�$	��Top=�$	��156��$��23��$��0,37,-0.4,0q��� -   $ 	graph2Btn��$��Wykres słupkowy��$��Right=�$��0,60,-0.4,0q�$	��Top=�$	��156��$��23���� -   $ startBtn��$��Start��$
��Left=�$	��Top=�$	��156��$��23��$��0,37,0,0q��� -   $ 	cancelBtn��$
��Stop��$
��Left=�$��0,60,0,0q�$	��Top=�$	��156��$��23��