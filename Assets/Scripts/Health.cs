using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    #region Members
    public SpriteRenderer heartA;
    public SpriteRenderer heartB;
    public SpriteRenderer heartC;
    public PostProcessVolume ppv;

    private int health = 3;
    private float vignetteTimer = 0f;
    [SerializeField] private float vignetteDuration;
    [SerializeField] private float vignetteIntensity;
    private Vignette vignette;
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource hurtSound;
    private float deathYPos = -10;
    private float absoluteDeathYPos = -20;
    private bool deathSoundPlayed = false;
    #endregion

    private void Start()
    {
        ppv.profile.TryGetSettings(out vignette);
    }

    void Update()
    {
        if(ppv.enabled)
        {
            vignetteTimer += Time.deltaTime;
            vignette.intensity.Interp(vignetteIntensity, 0f, vignetteTimer / vignetteDuration);

            if (vignetteTimer >= vignetteDuration)
            {
                vignetteTimer = 0f;
                ppv.enabled = false;
            }
        }

        if(transform.position.y < deathYPos)
        {
            if (!deathSoundPlayed)
            {
                deathSound.Play();
                deathSoundPlayed = true;
            }
            renderHealth(health);
        }
        if(transform.position.y < absoluteDeathYPos)
        {
            Death();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag.Equals("hurt"))
        {
            hurtSound.Play();
            health--;
            renderHealth(health);
        }
    }

    private void renderHealth(int health)
    {
        switch(health)
        {
            case 3:
                {
                    ppv.enabled = true;
                    break;
                }
            case 2:
                {
                    heartC.color = new Color(1f, 0f, 0f, 0.5f);
                    ppv.enabled = true;
                    break;
                }
            case 1:
                {
                    heartB.color = new Color(1f, 0f, 0f, 0.5f);
                    ppv.enabled = true;
                    break;
                }
            case 0:
                {
                    heartA.color = new Color(1f, 0f, 0f, 0.5f);
                    ppv.enabled = true;
                    Death();
                    break;
                }
        }
    }

    private void Death()
    {
        SceneManager.LoadScene("EndScreen");
    }
}
