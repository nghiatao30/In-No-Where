using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFill : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] PlayerHealth playerHealth;
    Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = playerHealth.CurrentHealth;
    }
}
