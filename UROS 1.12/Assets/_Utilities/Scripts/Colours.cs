using UnityEngine;

// Holds all the colour values for ease of access
public static class Colours
{
    // ********************************************************************************************************************************
    // ********************************************************************************************************************************
    //                              1). NORMAL COLOURS NEED TO BE SET IN EACH SECTION - SEE BELOW
    // 
    //                              2). KEY AND CHEST COLOUR BLIND COLOURS NEED TO BE SET - SEE BELOW
    // 
    //                              COLOURS THAT NEED TO BE MODIFIED CAN BE DONE SO FROM THIS CLASS
    //                              ONCE COLOURS ARE SET - REBUILD PROJECT 
    // ********************************************************************************************************************************
    // ********************************************************************************************************************************


    // FLOOR COLOURS
    public static Color floorColour = new Color(RGBClamp(191), RGBClamp(171), RGBClamp(133)); // SET COLOUR NORMAL
    public static Color floorColourRedSafe = new Color(RGBClamp(191), RGBClamp(171), RGBClamp(133));
    public static Color floorColourGreenSafe = new Color(RGBClamp(204), RGBClamp(204), RGBClamp(204));
    public static Color floorColourBlueSafe = new Color(RGBClamp(227), RGBClamp(255), RGBClamp(193));

    // EDGE COLOURS
    public static Color edgeColour = new Color(RGBClamp(48), RGBClamp(48), RGBClamp(48)); // SET NORMAL COLOUR
    public static Color edgeColourRedSafe = new Color(RGBClamp(48), RGBClamp(48), RGBClamp(48));
    public static Color edgeColourGreenSafe = new Color(RGBClamp(45), RGBClamp(31), RGBClamp(0));
    public static Color edgeColourBlueSafe = new Color(RGBClamp(0), RGBClamp(17), RGBClamp(4));

    // BORDER COLOURS
    public static Color borderColour = new Color(edgeColour.r, edgeColour.g, edgeColour.b); // SET NORMAL COLOUR - COPIED FROM ABOVE
    public static Color borderColourRedSafe = new Color(edgeColourRedSafe.r, edgeColourRedSafe.g, edgeColourRedSafe.b);
    public static Color borderColourGreenSafe = new Color(edgeColourGreenSafe.r, edgeColourGreenSafe.g, edgeColourGreenSafe.b);
    public static Color borderColourBlueSafe = new Color(edgeColourBlueSafe.r, edgeColourBlueSafe.g, edgeColourBlueSafe.b);

    // ROCK COLOURS
    public static Color rockColour = new Color(RGBClamp(117), RGBClamp(95), RGBClamp(43)); // SET NORMAL COLOUR
    public static Color rockColourRedSafe = new Color(RGBClamp(117), RGBClamp(95), RGBClamp(43));
    public static Color rockColourGreenSafe = new Color(RGBClamp(61), RGBClamp(61), RGBClamp(61));
    public static Color rockColourBlueSafe = new Color(RGBClamp(56), RGBClamp(73), RGBClamp(4));

    // ROCK DAMAGED COLOURS
    public static Color rockDamagedColour = new Color(RGBClamp(181), RGBClamp(151), RGBClamp(83)); // SET NORMAL COLOUR
    public static Color rockDamagedColourRedSafe = new Color(RGBClamp(181), RGBClamp(151), RGBClamp(83));
    public static Color rockDamagedColourGreenSafe = new Color(RGBClamp(132), RGBClamp(132), RGBClamp(132));
    public static Color rockDamagedColourBlueSafe = new Color(RGBClamp(94), RGBClamp(123), RGBClamp(6));

    // ROCK DAMAGED PARTICLE SYSTEM COLOURS
    public static Color rockDamagedPSColour = new Color(rockColour.r, rockColour.g, rockColour.b); // SET NORMAL COLOUR - COPIED FROM ABOVE
    public static Color rockDamagedPSColourRedSafe = new Color(rockColourRedSafe.r, rockColourRedSafe.g, rockColourRedSafe.b);
    public static Color rockDamagedPSColourGreenSafe = new Color(rockColourGreenSafe.r, rockColourGreenSafe.g, rockColourGreenSafe.b);
    public static Color rockDamagedPSColourBlueSafe = new Color(rockColourBlueSafe.r, rockColourBlueSafe.g, rockColourBlueSafe.b);

    // CRYSTAL COLOURS
    public static Color crystalColour = new Color(RGBClamp(0), RGBClamp(255), RGBClamp(229)); // SET NORMAL COLOUR
    public static Color crystalColourRedSafe = new Color(RGBClamp(0), RGBClamp(255), RGBClamp(229));
    public static Color crystalColourGreenSafe = new Color(RGBClamp(191), RGBClamp(0), RGBClamp(255));
    public static Color crystalColourBlueSafe = new Color(RGBClamp(96), RGBClamp(212), RGBClamp(255));

    // CRYSTAL DAMGED PARTICLE SYSTEM COLOURS
    public static Color crystalDamagedPSColour = new Color(crystalColour.r, crystalColour.g, crystalColour.b); // SET NORMAL COLOUR - COPIED FROM ABOVE
    public static Color crystalDamagedPSColourRedSafe = new Color(crystalColourRedSafe.r, crystalColourRedSafe.g, crystalColourRedSafe.b);
    public static Color crystalDamagedPSColourGreenSafe = new Color(crystalColourGreenSafe.r, crystalColourGreenSafe.g, crystalColourGreenSafe.b);
    public static Color crystalDamagedPSColourBlueSafe = new Color(crystalColourBlueSafe.r, crystalColourBlueSafe.g, crystalColourBlueSafe.b);

    // PICK UP COLOURS
    public static Color pickUpColour = new Color(RGBClamp(255), RGBClamp(215), RGBClamp(0)); // SET NORMAL COLOUR
    public static Color pickUpColourRedSafe = new Color(RGBClamp(255), RGBClamp(215), RGBClamp(0));
    public static Color pickUpColourGreenSafe = new Color(RGBClamp(218), RGBClamp(165), RGBClamp(32));
    public static Color pickUpColourBlueSafe = new Color(RGBClamp(192), RGBClamp(192), RGBClamp(192));

    // BASIC MONSTER COLOURS
    public static Color basicMonColour = new Color(RGBClamp(255), RGBClamp(0), RGBClamp(0)); // SET NORMAL COLOUR
    public static Color basicMonColourRedSafe = new Color(RGBClamp(226), RGBClamp(0), RGBClamp(0));
    public static Color basicMonColourGreenSafe = new Color(RGBClamp(87), RGBClamp(155), RGBClamp(0));
    public static Color basicMonColourBlueSafe = new Color(RGBClamp(0), RGBClamp(221), RGBClamp(255));

    // JUICY MONSTER COLOURS
    public static Color juicyMonColour = new Color(RGBClamp(0), RGBClamp(255), RGBClamp(0)); // SET NORMAL COLOUR
    public static Color juicyMonColourRedSafe = new Color(RGBClamp(87), RGBClamp(155), RGBClamp(0));
    public static Color juicyMonColourGreenSafe = new Color(RGBClamp(226), RGBClamp(0), RGBClamp(0));
    public static Color juicyMonColourBlueSafe = new Color(RGBClamp(200), RGBClamp(162), RGBClamp(200));

    // Chest and key colours - NORMAL VISION
    public static Color chestKey1 = new Color(RGBClamp(7), RGBClamp(115), RGBClamp(179));
    public static Color chestKey2 = new Color(RGBClamp(95), RGBClamp(178), RGBClamp(230));
    public static Color chestKey3 = new Color(RGBClamp(211), RGBClamp(96), RGBClamp(96));
    public static Color chestKey4 = new Color(RGBClamp(231), RGBClamp(159), RGBClamp(39));
    public static Color chestKey5 = new Color(RGBClamp(242), RGBClamp(230), RGBClamp(70));
    public static Color chestKey6 = new Color(RGBClamp(0), RGBClamp(160), RGBClamp(116));
    public static Color[] chestKeyColours = new Color[]
    {
        chestKey1,
        chestKey2,
        chestKey3,
        chestKey4,
        chestKey5,
        chestKey6
    };


    // ****************************************************************************************************************************
    // ****************************************************************************************************************************
    //                      KEY AND CHEST COLOUR BLIND COLOURS NEED TO BE SET - SEE BELOW
    // ****************************************************************************************************************************
    // ****************************************************************************************************************************
    // Chest and key colours - CVD RED SAFE
    public static Color chestKeyRedSafe1 = new Color(RGBClamp(107), RGBClamp(107), RGBClamp(179));
    public static Color chestKeyRedSafe2 = new Color(RGBClamp(170), RGBClamp(170), RGBClamp(229));
    public static Color chestKeyRedSafe3 = new Color(RGBClamp(114), RGBClamp(114), RGBClamp(42));
    public static Color chestKeyRedSafe4 = new Color(RGBClamp(168), RGBClamp(168), RGBClamp(42));
    public static Color chestKeyRedSafe5 = new Color(RGBClamp(231), RGBClamp(231), RGBClamp(70));
    public static Color chestKeyRedSafe6 = new Color(RGBClamp(149), RGBClamp(149), RGBClamp(116));
    public static Color[] chestKeyColoursRedSafe = new Color[]
    {
        chestKeyRedSafe1,
        chestKeyRedSafe2,
        chestKeyRedSafe3,
        chestKeyRedSafe4,
        chestKeyRedSafe5,
        chestKeyRedSafe6
    };


    // Chest and key colours - CVD GREEN SAFE
    public static Color chestKeyGreenSafe1 = new Color(RGBClamp(94), RGBClamp(94), RGBClamp(179));
    public static Color chestKeyGreenSafe2 = new Color(RGBClamp(157), RGBClamp(157), RGBClamp(230));
    public static Color chestKeyGreenSafe3 = new Color(RGBClamp(137), RGBClamp(137), RGBClamp(31));
    public static Color chestKeyGreenSafe4 = new Color(RGBClamp(182), RGBClamp(182), RGBClamp(34));
    public static Color chestKeyGreenSafe5 = new Color(RGBClamp(233), RGBClamp(133), RGBClamp(70));
    public static Color chestKeyGreenSafe6 = new Color(RGBClamp(132), RGBClamp(132), RGBClamp(118));
    public static Color[] chestKeyColoursGreenSafe = new Color[]
    {
        chestKeyGreenSafe1,
        chestKeyGreenSafe2,
        chestKeyGreenSafe3,
        chestKeyGreenSafe4,
        chestKeyGreenSafe5,
        chestKeyGreenSafe6
    };


    // Chest and key colours - CVD BLUE SAFE
    public static Color chestKeyBlueSafe1 = new Color(RGBClamp(0), RGBClamp(120), RGBClamp(157));
    public static Color chestKeyBlueSafe2 = new Color(RGBClamp(92), RGBClamp(179), RGBClamp(219));
    public static Color chestKeyBlueSafe3 = new Color(RGBClamp(214), RGBClamp(85), RGBClamp(109));
    public static Color chestKeyBlueSafe4 = new Color(RGBClamp(237), RGBClamp(143), RGBClamp(159));
    public static Color chestKeyBlueSafe5 = new Color(RGBClamp(253), RGBClamp(253), RGBClamp(236));
    public static Color chestKeyBlueSafe6 = new Color(RGBClamp(42), RGBClamp(147), RGBClamp(187));
    public static Color[] chestKeyColoursBlueSafe = new Color[]
    {
        chestKeyBlueSafe1,
        chestKeyBlueSafe2,
        chestKeyBlueSafe3,
        chestKeyBlueSafe4,
        chestKeyBlueSafe5,
        chestKeyBlueSafe6
    };


    // ****************************************************************************************************************************
    // ****************************************************************************************************************************

    // UI COLOURS
    public static Color UITextColour = new Color(RGBClamp(255), RGBClamp(255), RGBClamp(255)); // SET NORMAL COLOUR
    public static Color UITextColourRedSafe = new Color(RGBClamp(255), RGBClamp(254), RGBClamp(239));
    public static Color UITextColourGreenSafe = new Color(RGBClamp(255), RGBClamp(188), RGBClamp(204));
    public static Color UITextColourBlueSafe = new Color(RGBClamp(232), RGBClamp(255), RGBClamp(232));

    // UI BACKGROUND COLOURS
    public static Color UIBackgroundColour = new Color(RGBClamp(40), RGBClamp(40), RGBClamp(40)); // SET NORMAL COLOUR
    public static Color UIBackgroundColourRedSafe = new Color(RGBClamp(33), RGBClamp(22), RGBClamp(0));
    public static Color UIBackgroundColourGreenSafe = new Color(RGBClamp(9), RGBClamp(0), RGBClamp(56));
    public static Color UIBackgroundColourBlueSafe = new Color(RGBClamp(2), RGBClamp(42), RGBClamp(0));

    // ROCK GRADIENTS
    public static Gradient rockDamagedColourGrad = new Gradient();
    public static Gradient rockDamagedColourGradRedSafe = new Gradient();
    public static Gradient rockDamagedColourGradGreenSafe = new Gradient();
    public static Gradient rockDamagedColourGradBlueSafe = new Gradient();

    // CRYSTAL GRADIENTS
    public static Gradient crystalDamagedColourGrad = new Gradient();
    public static Gradient crystalDamagedColourGradRedSafe = new Gradient();
    public static Gradient crystalDamagedColourGradGreenSafe = new Gradient();
    public static Gradient crystalDamagedColourGradBlueSafe = new Gradient();

    // Set particle system colour gradient
    public static void ColoursPSGradient()
    {
        // Normal
        rockDamagedColourGrad.SetKeys
        (
            new GradientColorKey[] { new GradientColorKey(rockDamagedColour, 0.0f), new GradientColorKey(rockDamagedColour, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );

        // Red safe
        rockDamagedColourGradRedSafe.SetKeys
        (
            new GradientColorKey[] { new GradientColorKey(rockDamagedPSColourRedSafe, 0.0f), new GradientColorKey(rockDamagedPSColourRedSafe, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );

        // Green safe
        rockDamagedColourGradGreenSafe.SetKeys
        (
            new GradientColorKey[] { new GradientColorKey(rockDamagedPSColourGreenSafe, 0.0f), new GradientColorKey(rockDamagedPSColourGreenSafe, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );

        // Blue safe
        rockDamagedColourGradBlueSafe.SetKeys
        (
            new GradientColorKey[] { new GradientColorKey(rockDamagedPSColourBlueSafe, 0.0f), new GradientColorKey(rockDamagedPSColourBlueSafe, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );

        // Normal
        crystalDamagedColourGrad.SetKeys
        (
            new GradientColorKey[] { new GradientColorKey(crystalColour, 0.0f), new GradientColorKey(crystalColour, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );

        // Red safe
        crystalDamagedColourGradRedSafe.SetKeys
        (
            new GradientColorKey[] { new GradientColorKey(crystalColourRedSafe, 0.0f), new GradientColorKey(crystalColourRedSafe, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );

        // green safe
        crystalDamagedColourGradGreenSafe.SetKeys
        (
            new GradientColorKey[] { new GradientColorKey(crystalColourGreenSafe, 0.0f), new GradientColorKey(crystalColourGreenSafe, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );

        // Blue safe
        crystalDamagedColourGradBlueSafe.SetKeys
        (
            new GradientColorKey[] { new GradientColorKey(crystalColourBlueSafe, 0.0f), new GradientColorKey(crystalColourBlueSafe, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );
    }

    // Set R/G/B values to between 0.0f - 1.0f from 0 - 255
    public static float RGBClamp(int rgbValue)
    {
        if (rgbValue > 255)
            rgbValue = 255;

        if (rgbValue < 0)
            rgbValue = 0;

        return rgbValue / 255.0f;
    }
}