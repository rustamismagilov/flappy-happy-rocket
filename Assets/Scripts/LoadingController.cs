using UnityEngine;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    Animator animator;
    Image loadingImage;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        loadingImage = canvas.GetComponentInChildren<Image>(true);
        //loadingImage.gameObject.SetActive(false);
    }

    public void DarkToTransparent()
    {
        //loadingImage.gameObject.SetActive(true);
        animator.SetTrigger("isSceneLoaded");
    }

    public void TransparentToDark()
    {
        animator.SetTrigger("isSceneUnloaded");
        Invoke(nameof(HideImage), 1f);
    }

    void HideImage()
    {
        loadingImage.gameObject.SetActive(false);
    }
}
