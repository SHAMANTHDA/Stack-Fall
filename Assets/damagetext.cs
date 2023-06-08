using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class damagetext : MonoBehaviour
{
    public GameObject homeui;
    private Player _player;

    private void Awake()
    {
        if(ScoreHandler.instance)
        {
            Destroy(homeui);
        }
    }

    public float damageAmount;
    public float displayTime;

    private Text textComponent;

    private void Start()
    {
        textComponent = GetComponent<Text>();
        //DontDestroyOnLoad(gameObject); // Mark the text object as persistent
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            textComponent.text = "-" + damageAmount;
            Invoke("ClearText", displayTime);
        }
    }

    private void ClearText()
    {
        textComponent.text = "";
        Destroy(gameObject); // Destroy the text object when the damage effect is complete
    }

}
