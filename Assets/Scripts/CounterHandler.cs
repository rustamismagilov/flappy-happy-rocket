using System.Collections;
using TMPro;
using UnityEngine;

public class CounterHandler : MonoBehaviour
{
    [SerializeField] GameObject counterObject;

    TextMeshProUGUI counterTextbox;

    // Awake is called once before the Start
    void Awake()
    {
        counterTextbox = counterObject.GetComponentInChildren<TextMeshProUGUI>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    // start Counter (3, 2, 1..)
    public IEnumerator RunCounter()
    {
        enableCanvas();
        // use last 3 seconds for the counter
        int counter = 3;
        while (counter > 0)
        {
            counterTextbox.text = counter.ToString();
            yield return new WaitForSeconds(1);
            counter--;
        }
        disableCanvas();
    }
    // enable canvas
    void enableCanvas()
    {
        //this.enabled = true;
        counterObject.SetActive(true);
    }
    // disable canvas
    void disableCanvas()
    {
        //this.enabled = false;
        counterObject.SetActive(false);
    }
}
