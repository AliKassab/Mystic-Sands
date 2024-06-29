using UnityEngine;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour
{
    private ICommand _command;
    Button button;
    object _parameter;

    private void OnEnable()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnClick);
    }

    // Method to set the command for this menu item
    public void SetCommand(ICommand command, object parameter = null)
    {
        _command = command;
        _parameter = parameter;
    }

    // Method called when the menu item is clicked
    public void OnClick()
    {
        _command.Execute(_parameter);
    }
}
