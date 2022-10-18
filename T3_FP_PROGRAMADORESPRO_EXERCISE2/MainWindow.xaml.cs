using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace T3_FP_PROGRAMADORESPRO_EXERCISE2
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<double> p_numeros { get; set; } = new ObservableCollection<double>();
        public MainWindow()
        {
            InitializeComponent();
            p_button_agregarUnNumero.Click += AgregarNumeros;
            p_button_buscarNumero.Click += BuscarSiExisteElemento;
            p_button_eliminarUnNumero.Click += EliminarElemento;
            p_button_OrdenarDeFormaAscendente.Click += OrdenarAscendente;
            p_button_OrdenarDeFormaDescendente.Click += OrdenarDescendente;
        }
        //Agregar un número
        public void AgregarNumeros(object sender, EventArgs e)
        {
            double numero = 0;
            try
            {
                numero = Convert.ToDouble(p_textbox_agregarUnNumero.Text);
                p_numeros.Add(numero);
            }
            catch(Exception ex) when(ex is FormatException)
            {
                p_textbox_agregarUnNumero.Text = "Formato no válido.";
            }
            catch(Exception ex) when(ex.GetType() == typeof(OverflowException))
            {
                p_textbox_agregarUnNumero.Text = "Número muy grande o pequeño.";
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                p_textbox_agregarUnNumero.Text = "ingresa un dato válido.";
            }
        }
        //Buscar si un elemento existe
        public void BuscarSiExisteElemento(object sender, EventArgs e)
        {
            try
            {
                double elemento = Convert.ToDouble(p_txtbox_buscarNumero.Text);
                int elementosCoincidentes = BuscarNumero(p_numeros, elemento);
                if (elementosCoincidentes == 0)
                {
                    p_txtblock_mensajeNumeroEncontrado.Text = "0 elementos coinciden con tu búsqueda";
                }
                else
                {
                    p_txtblock_mensajeNumeroEncontrado.Text = $"{elementosCoincidentes} coinciden con tu búsqueda";
                }
            }
            catch(Exception ex) when (ex is FormatException)
            {
                p_txtblock_mensajeNumeroEncontrado.Text = "Formato inválido";
            }
            catch(Exception ex)
            {
                p_txtblock_mensajeNumeroEncontrado.Text = ex.GetType().Name;
            }
        }
        public int BuscarNumero(ObservableCollection<double> numeros, double elemento, int indice = 0, int elementosCoincidentes = 0)
        {
            if(indice >= numeros.Count)
            {
                return elementosCoincidentes;
            }
            if (elemento == numeros[indice]) elementosCoincidentes++;
            return BuscarNumero(numeros, elemento, indice + 1, elementosCoincidentes);
        }
        //Eliminar elemento
        public void EliminarElemento(object sender, EventArgs e)
        {
            if(p_listview_numeros.SelectedItem != null)
            {
                p_numeros.Remove((double) p_listview_numeros.SelectedItem);
            }
        }
        //ordenar elemento
        public void OrdenarAscendente(object sender, EventArgs e)
        {
            MergedSorted(p_numeros, true);
        }
        public void OrdenarDescendente(object sender, EventArgs e)
        {
            MergedSorted(p_numeros, false);
        }
        public void MergedSorted(ObservableCollection<double> numeros, bool ascendente = true)
        {
            //base case
            if (numeros.Count <= 1) return;
            //crear lista derecha e izquierda
            ObservableCollection<double> numerosIzquierda = new ObservableCollection<double>();
            ObservableCollection<double> numerosDerecha = new ObservableCollection<double>();
            //Llenar listas
            for(int i = 0; i < numeros.Count; i++)
            {
                if(i < numeros.Count / 2)
                {
                    numerosIzquierda.Add(numeros[i]);
                }
                else
                {
                    numerosDerecha.Add(numeros[i]);
                }
            }
            //recursividad
            MergedSorted(numerosIzquierda, ascendente);
            MergedSorted(numerosDerecha, ascendente);
            //merged
            Merged(numeros, numerosIzquierda, numerosDerecha, ascendente);
        }
        public void Merged(ObservableCollection<double> numeros, ObservableCollection<double> numerosIzquierda, ObservableCollection<double> numerosDerecha, bool ascendente)
        {
            numeros.Clear();
            int cuentaIzquierda = numerosIzquierda.Count;
            int cuentaDerecha = numerosDerecha.Count;
            int i = 0; int j = 0;
            if (ascendente)
            {
                while (i < cuentaIzquierda && j < cuentaDerecha)
                {
                    if (numerosIzquierda[i] < numerosDerecha[j])
                    {
                        numeros.Add(numerosIzquierda[i]);
                        i++;
                    }
                    else
                    {
                        numeros.Add(numerosDerecha[j]);
                        j++;
                    }
                }
            }
            else
            {
                while (i < cuentaIzquierda && j < cuentaDerecha)
                {
                    if (numerosIzquierda[i] > numerosDerecha[j])
                    {
                        numeros.Add(numerosIzquierda[i]);
                        i++;
                    }
                    else
                    {
                        numeros.Add(numerosDerecha[j]);
                        j++;
                    }
                }
            }
            while(i < numerosIzquierda.Count)
            {
                numeros.Add(numerosIzquierda[i]);
                i++;
            }
            while(j < numerosDerecha.Count)
            {
                numeros.Add(numerosDerecha[j]);
                j++;
            }
        }
    }
}
