using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TransitionType
{
    FadeIn,
    FadeOut
}

public class TransitionManager : Singleton<TransitionManager>
{
    [Header("Animator Settings")]
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private TransitionType startTransition = TransitionType.FadeIn;
    private HandleAnimations HandleAnimations;

    private bool isTransitioning;


    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        HandleAnimations = GetComponent<HandleAnimations>();
    }



    private void Start()
    {
        if (playOnStart)
            PlayTransition(startTransition);
    }

    /// <summary>
    /// Llama una animación de transición por su tipo.
    /// </summary>
    public void PlayTransition(TransitionType type)
    {
        if (isTransitioning)
            return;

        HandleAnimations.ChangeAnimationState(type.ToString());

    }

    /// <summary>
    /// Ejecuta una transición y espera su final (útil en corrutinas).
    /// </summary>
    public IEnumerator PlayTransitionAndWait(TransitionType type, float duration)
    {
        if (isTransitioning)
            yield break;

        HandleAnimations.ChangeAnimationState(type.ToString());

        yield return new WaitForSeconds(duration);

        isTransitioning = false;
    }

    public void PlayTransitionAndLoadScene(TransitionType type, int sceneIndex)
    {
        StartCoroutine(PlayTransitionAndLoadSceneCoroutine(type, sceneIndex));
    }

    public IEnumerator PlayTransitionAndLoadSceneCoroutine(TransitionType type, int sceneIndex)
    {
        if (isTransitioning)
            yield break;

        isTransitioning = true;

        HandleAnimations.ChangeAnimationState(type.ToString());
        yield return null; 

        float fadeOutDuration = HandleAnimations.GetCurrentAnimationLength();
        yield return new WaitForSeconds(fadeOutDuration);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
            yield return null;

        asyncLoad.allowSceneActivation = true;

        HandleAnimations.ChangeAnimationState(TransitionType.FadeIn.ToString());
        yield return null; 
        yield return new WaitForSeconds(HandleAnimations.GetCurrentAnimationLength());

        isTransitioning = false;
    }
}
