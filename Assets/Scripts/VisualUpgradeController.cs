using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VisualUpgradeController : MonoBehaviour
{
    [SerializeField] private Color colourUpgrade;
    [SerializeField] private Color colourSkinPrimary;
    [SerializeField] private Color colourEye;
    [SerializeField] private Color colourSkinSecondary;
    [SerializeField] private Color colourClothesPrimary;
    [SerializeField] private Color colourClothesSecondary;
    [SerializeField] private Color colourClothesAlt;
    
    private UpgradeStatusController upgradeStatusController;
    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;

    [SerializeField] private ScreenShockWaveEffectController shockWaveEffectController;
    [SerializeField] private ParticleSystemEffectController effectControllerDrawLines;
    [SerializeField] private ParticleSystemEffectController effectControllerForceBurst;

    [SerializeField] private float upgradeDurationStart = 1;
    [SerializeField] private float upgradeDurationEnd = 1;
    [SerializeField] private float upgradeDurationElapsed = 0;

    public event Action<ParticleSystemEffectController> OnEffectForceBurst;
    public event Action<VisualUpgradeController, Vector3> OnForceBurst;

    private void Awake()
    {
        upgradeStatusController = GetComponent<UpgradeStatusController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        upgradeStatusController.UpgradeEnter += StartUpgradeEffects;

        colourSkinPrimary = spriteRenderer.material.GetColor("_Color_Skin_Primary");
        colourSkinSecondary = spriteRenderer.material.GetColor("_Color_Skin_Secondary");
        colourEye = spriteRenderer.material.GetColor("_Color_Eye");
        colourClothesPrimary = spriteRenderer.material.GetColor("_Color_Clothes_Primary");
        colourClothesSecondary = spriteRenderer.material.GetColor("_Color_Clothes_Secondary");
        colourClothesAlt = spriteRenderer.material.GetColor("_Color_Clothes_Alt");
    }

    public void StartUpgradeEffects(UpgradeStatusController upgradeStatusController, Vector3 pos)
    {
        ParticleSystemEffectController particleSystemEffect = ObjectPool.instance.GetObjectFromPool(effectControllerDrawLines.name).GetComponent<ParticleSystemEffectController>();
        particleSystemEffect.transform.position = transform.position;
        particleSystemEffect.gameObject.SetActive(true);

        StartUpgradeRoutine();
    }

    public void StartUpgradeRoutine()
    {
        if (!playerController.GetIsUpgrading())
            StartCoroutine("UpgradeRoutine");
    }

    IEnumerator UpgradeRoutine()
    {
        playerController.SetIsUpgrading(true);

        while(upgradeDurationElapsed < upgradeDurationStart)
        {
            upgradeDurationElapsed += Time.deltaTime;
            spriteRenderer.material.SetColor("_Color_Skin_Primary", Color.Lerp(colourSkinPrimary, colourUpgrade, upgradeDurationElapsed / upgradeDurationStart));
            spriteRenderer.material.SetColor("_Color_Skin_Secondary", Color.Lerp(colourSkinSecondary, colourUpgrade, upgradeDurationElapsed / upgradeDurationStart));
            spriteRenderer.material.SetColor("_Color_Eye", Color.Lerp(colourEye, colourUpgrade, upgradeDurationElapsed / upgradeDurationStart));
            spriteRenderer.material.SetColor("_Color_Clothes_Primary", Color.Lerp(colourClothesPrimary, colourUpgrade, upgradeDurationElapsed / upgradeDurationStart));
            spriteRenderer.material.SetColor("_Color_Clothes_Secondary", Color.Lerp(colourClothesSecondary, colourUpgrade, upgradeDurationElapsed / upgradeDurationStart));
            spriteRenderer.material.SetColor("_Color_Clothes_Alt", Color.Lerp(colourClothesAlt, colourUpgrade, upgradeDurationElapsed / upgradeDurationStart));
            yield return null;
        }

        upgradeDurationElapsed = 0;

        while (upgradeDurationElapsed < upgradeDurationEnd)
        {
            upgradeDurationElapsed += Time.deltaTime;
            spriteRenderer.material.SetColor("_Color_Skin_Primary", Color.Lerp(colourUpgrade, colourSkinPrimary, upgradeDurationElapsed / upgradeDurationEnd));
            spriteRenderer.material.SetColor("_Color_Skin_Secondary", Color.Lerp(colourUpgrade, colourSkinSecondary, upgradeDurationElapsed / upgradeDurationEnd));
            spriteRenderer.material.SetColor("_Color_Eye", Color.Lerp(colourUpgrade, colourEye, upgradeDurationElapsed / upgradeDurationEnd));
            spriteRenderer.material.SetColor("_Color_Clothes_Primary", Color.Lerp(colourUpgrade, colourClothesPrimary, upgradeDurationElapsed / upgradeDurationEnd));
            spriteRenderer.material.SetColor("_Color_Clothes_Secondary", Color.Lerp(colourUpgrade, colourClothesSecondary, upgradeDurationElapsed / upgradeDurationEnd));
            spriteRenderer.material.SetColor("_Color_Clothes_Alt", Color.Lerp(colourUpgrade, colourClothesAlt, upgradeDurationElapsed / upgradeDurationEnd));
            yield return null;
        }

        upgradeDurationElapsed = 0;

        shockWaveEffectController.transform.position = transform.position;
        shockWaveEffectController.gameObject.SetActive(true);

        ParticleSystemEffectController particleSystemForceBurst = ObjectPool.instance.GetObjectFromPool(effectControllerForceBurst.name).GetComponent<ParticleSystemEffectController>();
        particleSystemForceBurst.transform.position = transform.position;
        particleSystemForceBurst.gameObject.SetActive(true);

        if (OnEffectForceBurst != null)
            OnEffectForceBurst(particleSystemForceBurst);

        if (OnForceBurst != null)
            OnForceBurst(this, transform.position);

        playerController.SetIsUpgrading(false);
    }
}

