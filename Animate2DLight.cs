using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Animate2DLight : MonoBehaviour
{
    public Light2D targetLight; // R�f�rence � la lumi�re 2D que vous souhaitez animer
    public float minIntensity = 0.5f; // Intensit� minimale
    public float maxIntensity = 2.0f; // Intensit� maximale
    public float animationSpeed = 1.0f; // Vitesse de l'animation

    private float startIntensity;
    private bool increasingIntensity = true;

    void Start()
    {
        // Assurez-vous que vous avez assign� une lumi�re 2D dans l'inspecteur
        if (targetLight == null)
        {
            Debug.LogError("Veuillez assigner une lumi�re 2D dans l'inspecteur.");
            enabled = false; // D�sactivez le script s'il n'y a pas de lumi�re assign�e
            return;
        }

        startIntensity = targetLight.intensity;
    }

    void FixedUpdate()
    {
        // Animez l'intensit� de la lumi�re
        float newIntensity = targetLight.intensity + (increasingIntensity ? 1 : -1) * animationSpeed * Time.fixedDeltaTime;

        // Inversez la direction de l'animation si l'intensit� d�passe les valeurs min ou max
        if (newIntensity > maxIntensity || newIntensity < minIntensity)
        {
            increasingIntensity = !increasingIntensity;
        }

        targetLight.intensity = newIntensity;
    }
}