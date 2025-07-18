using UnityEngine;
using UnityEngine.UI;

public class LoadingHandler : MonoBehaviour
{
    [SerializeField] GameObject loadingObject;

    //bool isLoading = false;

    // Awake is called once before the Start
    void Awake()
    {
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    // start loading
    public void StartLoading()
    {
        //isLoading = true;
        enableCanvas();
        loadingObject.GetComponent<Image>().canvasRenderer.SetAlpha(0f);
        loadingObject.GetComponent<Image>().CrossFadeAlpha(1f, 1f, false);
    }
    // stop loading
    public void StopLoading()
    {
        //isLoading = false;
        enableCanvas();
        loadingObject.GetComponent<Image>().canvasRenderer.SetAlpha(1f);
        loadingObject.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
        Invoke(nameof(disableCanvas), 1f);
    }
    // enable canvas
    void enableCanvas()
    {
        //this.enabled = true;
        loadingObject.SetActive(true);
    }
    // disable canvas
    void disableCanvas()
    {
        //this.enabled = false;
        loadingObject.SetActive(false);
    }
}
