using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResourceDisplay : MonoBehaviour
{
    [SerializeField] float primaryStartPercent = 0.16f;
    [SerializeField] float PrimaryEndPercent = 0.76f;

    public PlayerController player;

    private Drawer drawer;
    private PlayerResource playerResource;
    private Image primaryResource;
    private Image primaryGlow;

    // Start is called before the first frame update
    void Start()
    {
        drawer = GetComponentInParent<Drawer>();
        playerResource = player.GetComponent<PlayerResource>();

        primaryResource = GetComponentsInChildren<Image>()
                            .Where(i => i.gameObject.name == "PrimaryDisplayFill")
                            .FirstOrDefault();
        primaryGlow = GetComponentsInChildren<Image>()
                            .Where(i => i.gameObject.name == "PrimaryDisplayGlow")
                            .FirstOrDefault();

        playerResource.resourceChanged += OnResourceChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnResourceChanged(object sender, EventArgs e)
    {
        switch (drawer.owner)
        {
            case Card.Owner.Luban:
                float percent = (float)playerResource.primaryResource /
                    (float)playerResource.maxPrimaryResource;
                float amount = primaryStartPercent
                    + (PrimaryEndPercent - primaryStartPercent) * percent;
                primaryResource.DOFillAmount(amount, 0.2f)
                               .SetEase(Ease.OutQuint);

                float glowStart = 90f - 360 * primaryStartPercent;
                float glowRotatoin = glowStart - 360 * (amount - primaryStartPercent);
                primaryGlow.transform.DORotate(
                    new Vector3(0, 0, glowRotatoin), 0.2f)
                    .SetEase(Ease.OutQuint);

                Image[] secondaryResource = new Image[3];
                for (int i = 0; i < 3; i++)
                {
                    secondaryResource[i] = GetComponentsInChildren<Image>()
                    .Where(image => image.gameObject.name 
                        == $"SecondaryDisplay{i + 1}Fill")
                    .FirstOrDefault();
                    if (i < playerResource.secondaryResource)
                    {
                        secondaryResource[i].DOFade(1f, 0.2f).SetEase(Ease.OutQuint);
                    }
                    else
                    {
                        secondaryResource[i].DOFade(0f, 0.2f).SetEase(Ease.OutQuint);
                    }
                }

                break;
            case Card.Owner.Daoshi:
                break;
            case Card.Owner.thirdChar:
                break;
            default:
                break;
        }
    }
}
