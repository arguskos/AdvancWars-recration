using System.Collections.Generic;

public interface IActionMenu
{

    void ShowMenu(List<BaseAbility> abilities);
    void HideMenu();
    void MoveSelectionUp();
    void MoveSelectionDown();
    void Confirm();
    void Cancel();
}