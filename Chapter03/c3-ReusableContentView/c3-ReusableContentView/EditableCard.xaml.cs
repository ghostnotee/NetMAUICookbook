namespace c3_ReusableContentView;

public partial class EditableCard : ContentView
{
    private static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(EditableCard));

    private bool _isEditing;

    public EditableCard()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        init => SetValue(TextProperty, value);
    }

    private void OnEditButtonClicked(object sender, EventArgs e)
    {
        _isEditing = !_isEditing;
        if (_isEditing)
        {
            Editor.IsReadOnly = false;
            Editor.Focus();
            Editor.CursorPosition = Editor.Text == null ? 0 : Editor.Text.Length;
            EditButton.Text = "Save";
        }
        else
        {
            Editor.IsReadOnly = true;
            EditButton.Focus();
            EditButton.Text = "Edit";
        }
    }
}