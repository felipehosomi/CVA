using CVA.Hybel.Monitor.IntegSkaSap.BLL;
using CVA.Hybel.Monitor.IntegSkaSap.MODEL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CVA.Hybel.Monitor.IntegSkaSap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static bool AutoRefresh;
        private static int RefreshSeconds;
        private static int TimerSeconds;
        private static int RefreshMinutes;
        private static int SearchMinutes;
        DispatcherTimer Timer = new DispatcherTimer();

        public MainWindow()
        {
            AutoRefresh = true;
            RefreshMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["RefreshMinutes"]);
            SearchMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["SearchMinutes"]);
            RefreshSeconds = RefreshMinutes * 60;
            
            InitializeComponent();
            this.tbxFrequencia.Value = RefreshMinutes;

            this.RefreshGrid();

            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Tick;
            Timer.Start();
            //this.StartTask();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.lblRefresh.Content = "Próxima atualização: " + DateTime.Today.AddSeconds(RefreshSeconds - TimerSeconds).ToString("HH:mm:ss");
            TimerSeconds++;
            if (RefreshSeconds - TimerSeconds == 0)
            {
                this.RefreshGrid();
                TimerSeconds = 0;
            }
        }

        private async void StartTask()
        {
            await Task.Run(() => KeepRefreshingGrid(this));
        }

        private async Task<string> KeepRefreshingGrid(MainWindow form)
        {
            while (true)
            {
                //IntegracaoList = new ObservableCollection<IntegracaoModel>(IntegracaoBLL.GetList());
                if (AutoRefresh)
                {
                    form.RefreshGrid();
                }
                await Task.Delay(TimeSpan.FromMinutes(GetMinutes(form)));
                RefreshSeconds = RefreshMinutes * 60;
            }
            return "";
        }

        private int GetMinutes(MainWindow form)
        {
            Dispatcher.Invoke(() =>
            {
                if (form.tbxFrequencia.Value.HasValue)
                {
                    RefreshMinutes = (int)form.tbxFrequencia.Value.Value;
                }
            });
            return RefreshMinutes;
        }

        private void RefreshGrid()
        {
            Dispatcher.Invoke(() =>
            {
                FiltroModel model = new FiltroModel();
                model.DataDe = DateTime.Now.AddMinutes(SearchMinutes * -1);
                model.DataAte = DateTime.Now;
                this.lstIntegracao.ItemsSource = new ObservableCollection<IntegracaoModel>(IntegracaoBLL.GetList(model));
            });
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            this.SetAutoRefresh();
        }

        private void SetAutoRefresh()
        {
            RefreshMinutes = this.tbxFrequencia.Value.Value;
            RefreshSeconds = RefreshMinutes * 60;
            TimerSeconds = 0;

            if (Timer.IsEnabled)
            {
                Timer.Stop();
                AutoRefresh = false;
                this.btnStart.Content = "Iniciar";
            }
            else
            {
                Timer.Start();
                AutoRefresh = true;
                this.btnStart.Content = "Pausar";
            }
        }

        private void btnFiltro_Click(object sender, RoutedEventArgs e)
        {
            if (AutoRefresh)
            {
                if (MessageBox.Show("Deseja pausar a atualização automática?", "Alerta", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    this.SetAutoRefresh();
                }
            }
            this.Filtrar();
        }

        private void Filtrar()
        {
            FiltroModel filtroModel = new FiltroModel();
            filtroModel.DataDe = this.tbxDataDe.Value;
            filtroModel.DataAte = this.tbxDataAte.Value;
            if (this.cbxStatus.Text != "Todos")
            {
                filtroModel.Status = this.cbxStatus.Text;
            }
            filtroModel.OP = this.tbxOP.Text;
            filtroModel.BELPOS_ID = this.tbxIdOper.Text;
            filtroModel.POS_ID = this.tbxIdOper.Text;
            filtroModel.OPER = this.tbxOperacao.Text;
            filtroModel.OPERADOR = this.tbxOperador.Text;
            filtroModel.MAQ = this.tbxMaq.Text;
            filtroModel.CODPECA = this.tbxCodPeca.Text;

            this.lstIntegracao.ItemsSource = new ObservableCollection<IntegracaoModel>(IntegracaoBLL.GetList(filtroModel));
        }

        private void btnAtualizar_Click(object sender, RoutedEventArgs e)
        {
            RefreshMinutes = this.tbxFrequencia.Value.Value;
            RefreshSeconds = RefreshMinutes * 60;
            TimerSeconds = 0;
            FiltroModel model = new FiltroModel();
            model.DataDe = DateTime.Now.AddMinutes(SearchMinutes * -1);
            model.DataAte = DateTime.Now;
            this.lstIntegracao.ItemsSource = new ObservableCollection<IntegracaoModel>(IntegracaoBLL.GetList(model));
        }

        private void btnReprocessar_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<IntegracaoModel> list = this.lstIntegracao.ItemsSource as ObservableCollection<IntegracaoModel>;
            IntegracaoBLL.SetStatus(list.Where(l => l.Reprocessar).ToList());
            if (AutoRefresh)
            {
                FiltroModel model = new FiltroModel();
                model.DataDe = DateTime.Now.AddMinutes(SearchMinutes * -1);
                model.DataAte = DateTime.Now;
                this.lstIntegracao.ItemsSource = new ObservableCollection<IntegracaoModel>(IntegracaoBLL.GetList(model));
            }
            else
            {
                this.Filtrar();
            }
        }

        private void cbxReprocessar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
