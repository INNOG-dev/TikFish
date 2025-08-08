using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Animate2DLight : MonoBehaviour
{
    public Light2D targetLight; // Référence à la lumière 2D que vous souhaitez animer
    public float minIntensity = 0.5f; // Intensité minimale
    public float maxIntensity = 2.0f; // Intensité maximale
    public float animationSpeed = 1.0f; // Vitesse de l'animation

    private float startIntensity;
    private bool increasingIntensity = true;

    void Start()
    {
        // Assurez-vous que vous avez assigné une lumière 2D dans l'inspecteur
        if (targetLight == null)
        {
            Debug.LogError("Veuillez assigner une lumière 2D dans l'inspecteur.");
            enabled = false; // Désactivez le script s'il n'y a pas de lumière assignée
            return;
        }

        startIntensity = targetLight.intensity;
    }

    void FixedUpdate()
    {
        // Animez l'intensité de la lumière
        float newIntensity = targetLight.intensity + (increasingIntensity ? 1 : -1) * animationSpeed * Time.fixedDeltaTime;

        // Inversez la direction de l'animation si l'intensité dépasse les valeurs min ou max
        if (newIntensity > maxIntensity || newIntensity < minIntensity)
        {
            increasingIntensity = !increasingIntensity;
        }

        targetLight.intensity = newIntensity;
    }
}