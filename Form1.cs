using System.Net.Http.Headers;
using System.Text;

namespace Invoke_Auth_Rest_Write;

public partial class ARRL : Form
{
    private string? _URLEndpoint;
    private string? _AuthToken;
    private string? _OutFile;

    public ARRL()
    {
        InitializeComponent();
    }

    #region CheckBox Enable Fields
    private void CheckBox_Select_Out_File_CheckedChanged(object sender, EventArgs e)
    {
        bool isChecked = checkBoxSelectOutFile.Checked;

        textBoxOutFile.Enabled = !isChecked;
        button_Select_File.Enabled = isChecked;
    }

    private void CheckBoxAuthToken_CheckedChanged(object sender, EventArgs e)
    {
        bool isChecked = checkBoxAuthToken.Checked;

        textBoxAuthToken.Enabled = isChecked;
    }
    #endregion

    #region Text Input State
    private void textBoxAuthToken_TextChanged(object sender, EventArgs e)
    {
        _AuthToken = textBoxAuthToken.Text;
    }

    private void textBoxURL_TextChanged(object sender, EventArgs e)
    {
        _URLEndpoint = textBoxURL.Text;
    }

    private void textBoxOutFile_TextChanged(object sender, EventArgs e)
    {
        _OutFile = textBoxOutFile.Text;
    }
    #endregion

    #region Buttons
    private void Button_Select_File_Click(object sender, EventArgs e)
    {
        // Show the Open File Dialog, if the user clicks OK insert the filename to text box
        if (fileDialogWrite.ShowDialog() == DialogResult.OK)
        {
            // TODO: fileDialogWrite.RestoreDirectory (bool saves last dir used in Dialog)
            // Inserts selected filename to Out-File text box
            textBoxOutFile.Text = fileDialogWrite.FileName;
        }
    }

    private async void Button_Send_Req_Click(object sender, EventArgs e)
    {
        using HttpClient client = new();
        // TODO: handle null OutFile, handle null URL
        if (_OutFile == null || _URLEndpoint == null)
        {
            return;
        }
        if (!string.IsNullOrWhiteSpace(_AuthToken))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _AuthToken);
        }
        HttpResponseMessage response = await client.GetAsync(_URLEndpoint);
        string responseBody = await response.Content.ReadAsStringAsync();
        await File.WriteAllTextAsync(_OutFile, responseBody, Encoding.UTF8);
    }
    #endregion
}
