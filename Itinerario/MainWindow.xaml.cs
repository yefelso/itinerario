using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Itinerario
{
    public partial class MainWindow : Window
    {
        private List<Evento> eventos;

        public MainWindow()
        {
            InitializeComponent();
            // Establecer el año y el mes actual como seleccionados por defecto
            cmbYear.SelectedItem = DateTime.Now.Year;
            cmbMonth.SelectedIndex = DateTime.Now.Month - 1;
            // Generar los botones de los días del mes actual
            GenerarEventosEjemplo();
            GenerateDayButtons(DateTime.Now.Year, DateTime.Now.Month);
        }

        private void cmbYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbMonth.SelectedItem != null)
            {
                GenerateDayButtons((int)cmbYear.SelectedItem, cmbMonth.SelectedIndex + 1);
            }
        }

        private void cmbMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbYear.SelectedItem != null)
            {
                GenerateDayButtons((int)cmbYear.SelectedItem, cmbMonth.SelectedIndex + 1);
            }
        }

        private void GenerarEventosEjemplo()
        {
            // Aquí puedes generar datos de ejemplo para los eventos
            eventos = new List<Evento>()
            {
                new Evento() { Nombre = "Reunión de trabajo", Fecha = new DateTime(2024, 3, 10), Completado = false },
                new Evento() { Nombre = "Comprar regalos", Fecha = new DateTime(2024, 3, 15), Completado = true },
                new Evento() { Nombre = "Entregar informe", Fecha = new DateTime(2024, 3, 23), Completado = false }
            };
        }

        private void GenerateDayButtons(int year, int month)
        {
            // Limpiar el panel de días si ya hay botones presentes
            panelDias.Children.Clear();

            // Obtener el primer día del mes
            DateTime firstDayOfMonth = new DateTime(year, month, 1);

            // Obtener el día de la semana del primer día del mes
            DayOfWeek firstDayOfWeek = firstDayOfMonth.DayOfWeek;

            // Obtener el último día del mes
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            // Crear un Grid para organizar los botones de los días en un diseño de calendario
            Grid calendarGrid = new Grid();
            calendarGrid.Width = 780;
            calendarGrid.HorizontalAlignment = HorizontalAlignment.Left;

            // Agregar columnas al Grid para los días de la semana
            for (int i = 0; i < 7; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                calendarGrid.ColumnDefinitions.Add(columnDefinition);
            }

            // Crear botones para los días de la semana
            for (int dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
            {
                // Crear un TextBlock para mostrar el nombre del día de la semana
                TextBlock dayOfWeekTextBlock = new TextBlock();
                dayOfWeekTextBlock.Text = ((DayOfWeek)((dayOfWeek + (int)firstDayOfWeek) % 7)).ToString();
                dayOfWeekTextBlock.TextAlignment = TextAlignment.Center;
                dayOfWeekTextBlock.Margin = new Thickness(5);
                Grid.SetColumn(dayOfWeekTextBlock, dayOfWeek);
                calendarGrid.Children.Add(dayOfWeekTextBlock);
            }

            // Calcular la fila y columna inicial para el primer día del mes
            int row = 1;
            int column = (int)firstDayOfWeek;

            // Crear botones para cada día del mes
            for (DateTime date = firstDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1))
            {
                Button button = new Button();
                button.Content = date.Day.ToString();
                button.Tag = date;
                button.Click += DayButton_Click;
                button.Margin = new Thickness(5);

                // Agregar el botón al Grid en la posición correspondiente
                Grid.SetColumn(button, column);
                Grid.SetRow(button, row);
                calendarGrid.Children.Add(button);

                // Avanzar a la siguiente columna
                column++;
                if (column >= 7)
                {
                    // Reiniciar la columna si alcanza el final de la semana
                    column = 0;
                    // Avanzar a la siguiente fila
                    row++;
                }
            }

            // Agregar el Grid al panel de días
            panelDias.Children.Add(calendarGrid);
        }

        private void DayButton_Click(object sender, RoutedEventArgs e)
        {
            // Aquí puedes manejar el evento de clic de los botones de los días
            Button button = (Button)sender;
            DateTime selectedDate = (DateTime)button.Tag;
            MessageBox.Show("Has seleccionado el día: " + selectedDate.ToShortDateString());
        }
    }

    public class Evento
    {
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public bool Completado { get; set; }
    }
}
