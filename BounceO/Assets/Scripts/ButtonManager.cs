using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{ 

    public void Pause()
    {
        Time.timeScale = 0.0f;
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
    }

    public void ButtonSound()
    {
        AudioManager.Instance.Play("ButtonClick");
    }

    public void MusicOnOff(Image image)
    {
        if(GameManager.Instance.MusicOn)
        {
            Color c = image.color;
            c.a = 0.59f;
            image.color = c;

            GameManager.Instance.MusicOn = false;


        }

        else
        {
            Color c = image.color;
            c.a = 1f;
            image.color = c;

            GameManager.Instance.MusicOn = true;
        }
    }
}
