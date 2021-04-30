using UnityEngine;

public class ButtonSpell : MonoBehaviour
{
    //script para que los botones pasaran correctamente así mismo y prefabs.
    public delegate void ClickedSpell(Spell card);
    public static ClickedSpell spellButton;
    public void OnClick()
    {
        spellButton?.Invoke(gameObject.GetComponent<Card>() as Spell);
    }
}
