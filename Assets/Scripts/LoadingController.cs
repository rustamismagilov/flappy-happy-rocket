using System.Net.Sockets;
using UnityEngine;

public class LoadingController : MonoBehaviour
{
    Animator animator;

    bool isLoading = false;

    // Awake is called once before the Start
    void Awake()
    {
        animator = GetComponent<Animator>();
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
        isLoading = true;
        animator.SetBool("isLoading", isLoading);
        enableCanvas();
    }
    // stop loading
    public void StopLoading()
    {
        isLoading = false;
        animator.SetBool("isLoading", isLoading);
        Invoke(nameof(disableCanvas), 1f);
    }
    // enable canvas
    public void enableCanvas()
    {
        //this.enabled = true;
        this.gameObject.SetActive(true);
    }
    // disable canvas
    public void disableCanvas()
    {
        //this.enabled = false;
        this.gameObject.SetActive(false);
    }
}
