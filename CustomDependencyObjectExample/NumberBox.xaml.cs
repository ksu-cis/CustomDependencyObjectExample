using System.Windows;
using System.Windows.Controls;

namespace CustomDependencyObjectExample
{
    /// <summary>
    /// Interaction logic for NumberBox.xaml
    /// </summary>
    public partial class NumberBox : UserControl
    {

        #region Dependency Properties

        /// <summary>
        /// Identifies the NumberBox.Step XAML attached property
        /// </summary>
        public static DependencyProperty StepProperty = DependencyProperty.Register("Step", typeof(double), typeof(NumberBox), new PropertyMetadata(1.0));
       
        /// <summary>
        /// The amount each increment or decrement operation should change the value by
        /// </summary>
        public double Step
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        /// <summary>
        /// Identifies the NumberBox.MaxValue property
        /// </summary>
        public static DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(double), typeof(NumberBox), new PropertyMetadata(double.MaxValue));

        /// <summary>
        /// The maximum value this NumberBox will allow
        /// </summary>
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        /// <summary>
        /// Identifies the NumberBox.MaxValue property
        /// </summary>
        public static DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(double), typeof(NumberBox), new PropertyMetadata(double.MinValue));

        /// <summary>
        /// The maximum value this NumberBox will allow
        /// </summary>
        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        /// <summary>
        /// Identifies the NumberBox.Value XAML attached property
        /// </summary>
        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(NumberBox), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, HandleValueChanged));

        /// <summary>
        /// The NumberBox's displayed value
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        #endregion

        #region Routed Events

        /// <summary>
        /// Identifies the NumberBox.ValueClamped event
        /// </summary>
        public static readonly RoutedEvent ValueClampedEvent = EventManager.RegisterRoutedEvent("ValueClamped", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumberBox));

        /// <summary>
        /// Event that is triggered when the value of this NumberBox is clamped to the 
        /// range between MinValue and MaxValue
        /// </summary>
        public event RoutedEventHandler ValueClamped
        {
            add { AddHandler(ValueClampedEvent, value); }
            remove { RemoveHandler(ValueClampedEvent, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new NumberBox
        /// </summary>
        public NumberBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructs a new NumberBox
        /// </summary>
        /// <param name="value">The initial value of the NumberBox</param>
        public NumberBox(double value)
        {
            InitializeComponent();
            Value = value;
        }

        /// <summary>
        /// Constructs a new NumberBox
        /// </summary>
        /// <param name="value">The initial value of the NumberBox</param>
        /// <param name="min">The minimum allowable value</param>
        /// <param name="max">The maximum allowable value</param>
        public NumberBox(double value, double min, double max)
        {
            InitializeComponent();
            Value = value;
            MinValue = min;
            MaxValue = max;
        }

        #endregion

        #region Event Handlers and Callbacks

        /// <summary>
        /// Handles the click of the increment or decrement button
        /// </summary>
        /// <param name="sender">The button clicked</param>
        /// <param name="e">The event arguments</param>
        void HandleButtonClick(object sender, RoutedEventArgs e)
        {
            if(sender is Button button)
            {
                switch(button.Name)
                {
                    case "Increment":
                        Value += Step;
                        break;
                    case "Decrement":
                        Value -= Step;
                        break;
                }
            }
            e.Handled = true;
        }

        /// <summary>
        /// Callback for the ValueProperty, which clamps the Value to the range 
        /// defined by MinValue and MaxValue
        /// </summary>
        /// <param name="sender">The NumberBox whose value is changing</param>
        /// <param name="e">The event args</param>
        static void HandleValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if(e.Property.Name == "Value" && sender is NumberBox box)
            {
                if(box.Value < box.MinValue)
                {
                    box.Value = box.MinValue;
                    box.RaiseEvent(new RoutedEventArgs(ValueClampedEvent));
                }
                if(box.Value > box.MaxValue)
                {
                    box.Value = box.MaxValue;
                    box.RaiseEvent(new RoutedEventArgs(ValueClampedEvent));
                }
            }
        }

        #endregion
    }
}
