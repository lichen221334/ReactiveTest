using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReactiveTest
{
    public partial class Form1 : Form, IViewFor<AddViewModel>
    {
        public Form1()
        {
            InitializeComponent();
            this.WhenActivated(a => 
            {
                a(this.Bind(ViewModel, vm => vm.Num1, v => v.textBox1.Text));
                a(this.Bind(ViewModel, vm => vm.Num2, v => v.textBox3.Text));
                a(this.OneWayBind(ViewModel, vm => vm.Sum, v => v.textBox2.Text));
                a(this.BindCommand(ViewModel, vm => vm.TestCommand, v => v.button1));
            });

            ViewModel.WhenAnyValue(x => x.Num1).Where(v=>v>10).Subscribe(x=>MessageBox.Show(x.ToString()));
        }

        public AddViewModel ViewModel
        {
            get;
            set;
        } = new AddViewModel();

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (AddViewModel)value;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.ViewModel.Num1 = 10;
            this.ViewModel.Num2 = 20;
        }
    }

    public class AddViewModel : ReactiveObject
    {
        public AddViewModel()
        {
            this.TestCommand = ReactiveCommand.Create(TestClick, this.WhenAnyValue(vm => vm.Sum).Select(s => s> 10));
        }

        private double num1;
        private double num2;
        private double sum;
        public ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit> TestCommand { get; }
        public double Num1
        {
            get => num1;
            set
            {
                if (value != num1)
                {
                    this.RaiseAndSetIfChanged(ref num1, value);
                    this.Sum = num1 + num2;
                }
            }
        }

        public double Num2
        {
            get => num2;
            set
            {
                if (value != num2)
                {
                    this.RaiseAndSetIfChanged(ref num2, value);
                    this.Sum = num1 + num2;
                }
            }
        }

        public double Sum
        {
            get => sum;
            set => this.RaiseAndSetIfChanged(ref sum, value);
        }

        private void TestClick()
        {
            MessageBox.Show($"结果：{this.sum:N3}");
        }
    }
}
