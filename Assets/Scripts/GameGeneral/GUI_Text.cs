using UnityEngine;
using System.Collections;

public class GUI_Text : MonoBehaviour
{

    public GUIText yourText;
    //public GUITexture yourTexture;

    void OnTriggerEnter(Collider hit)
    {
        if (this.gameObject.name == "text1")
        {
            yourText.text = "<b><color=red>Move</color></b> with <b><color=red>WASD</color></b> or <b><color=red>Arrow</color></b> keys" +
                "\n <b><color=blue>Jump</color></b> with <b><color=blue>W</color></b>, <b><color=blue>SPACE</color></b> or <b><color=blue>UP Arrow</color></b> keys" +
            "\n <b><color=green>Shoot</color></b> by dragging and dropping <b><color=green>MOUSE1</color></b>"+
            "\n <b><color=black>Run</color></b> by holding <b><color=black>SHIFT</color></b>";
        }
        else if (this.gameObject.name == "text2")
        {
            yourText.text = "<b>Shoot <color=blue>EVERYTHING</color></b> that moves" +
                "\n <b><color=red>HAVE FUN!</color></b>";
        }
        else if (this.gameObject.name == "text3")
        {
            yourText.text = "<b><color=black>This is the end of the Tutorial!</color></b>" +
                "\n <b><color=black>Let's get right on it!</color></b>";
        }
        else
        {

        }
    }

    void OnTriggerExit(Collider hit)
    {
        yourText.text = "";
    }
}