using System.Collections.Generic;
using UnityEngine;

public class ShipGeneratoScript : MonoBehaviour
{
    public float TopLeftX;
    public float TopLeftY;

    public float TileWidth;
    public float TileHeight;

    public string ShipMapResourceName;
    public char ShipMapLineSeparator;
    public char ShipMapLayerSeparator;

    public GameObject SheathingRect;
    public char SheathingRectSymbol;

    public GameObject SheathingRoundCornerTopLeft;
    public char SheathingRoundCornerTopLeftSymbol;

    public GameObject SheathingRoundCornerTopRight;
    public char SheathingRoundCornerTopRightSymbol;

    public GameObject SheathingRoundCornerBotRight;
    public char SheathingRoundCornerBotRightSymbol;

    public GameObject SheathingRoundCornerBotLeft;
    public char SheathingRoundCornerBotLeftSymbol;

    public GameObject SheathingTriangleTopLeft;
    public char SheathingTriangleTopLeftSymbol;

    public GameObject SheathingTriangleTopRight;
    public char SheathingTriangleTopRightSymbol;

    public GameObject SheathingTriangleBotRight;
    public char SheathingTriangleBotRightSymbol;

    public GameObject SheathingTriangleBotLeft;
    public char SheathingTriangleBotLeftSymbol;

    public GameObject SheathingTriangleTop;
    public char SheathingTriangleTopSymbol;

    public GameObject SheathingTriangleRight;
    public char SheathingTriangleRightSymbol;

    public GameObject SheathingTriangleBot;
    public char SheathingTriangleBotSymbol;

    public GameObject SheathingTriangleLeft;
    public char SheathingTriangleLeftSymbol;


    public GameObject SheathingBackgroundWindow;
    public char SheathingBackgroundWindowSymbol;

    public GameObject SheathingBackground;
    public char SheathingBackgroundSymbol;


    public GameObject Battary;
    public char BattarySymbol;

    public GameObject Box;
    public char BoxSymbol;

    public GameObject Ladder;
    public char LadderSymbol;


    public GameObject CanonTop;
    public char CanonTopSymbol;

    public GameObject CanonRight;
    public char CanonRightSymbol;

    public GameObject CanonBot;
    public char CanonBotSymbol;

    public GameObject CanonLeft;
    public char CanonLeftSymbol;

    public GameObject ScreenAnimation;
    public char ScreenAnimationSymbol;


    public GameObject ExhaustPipeTop;
    public char ExhaustPipeTopSymbol;

    public GameObject ExhaustPipeRight;
    public char ExhaustPipeRightSymbol;

    public GameObject ExhaustPipeBot;
    public char ExhaustPipeBotSymbol;

    public GameObject ExhaustPipeLeft;
    public char ExhaustPipeLeftSymbol;


    public GameObject ExhaustAnimationTop;
    public char ExhaustAnimationTopSymbol;

    public GameObject ExhaustAnimationRight;
    public char ExhaustAnimationRightSymbol;

    public GameObject ExhaustAnimationBot;
    public char ExhaustAnimationBotSymbol;

    public GameObject ExhaustAnimationLeft;
    public char ExhaustAnimationLeftSymbol;

    // Start is called before the first frame update
    public void Start()
    {
        Dictionary<char, Object> TileSet = new Dictionary<char, Object>()
        {
            { SheathingRectSymbol, SheathingRect },

            { SheathingRoundCornerTopLeftSymbol, SheathingRoundCornerTopLeft },
            { SheathingRoundCornerTopRightSymbol, SheathingRoundCornerTopRight },
            { SheathingRoundCornerBotRightSymbol, SheathingRoundCornerBotRight },
            { SheathingRoundCornerBotLeftSymbol, SheathingRoundCornerBotLeft },

            { SheathingTriangleTopLeftSymbol, SheathingTriangleTopLeft },
            { SheathingTriangleTopRightSymbol, SheathingTriangleTopRight },
            { SheathingTriangleBotRightSymbol, SheathingTriangleBotRight },
            { SheathingTriangleBotLeftSymbol, SheathingTriangleBotLeft },

            { SheathingTriangleTopSymbol, SheathingTriangleTop },
            { SheathingTriangleRightSymbol, SheathingTriangleRight },
            { SheathingTriangleBotSymbol, SheathingTriangleBot },
            { SheathingTriangleLeftSymbol, SheathingTriangleLeft },

            { SheathingBackgroundWindowSymbol, SheathingBackgroundWindow },
            { SheathingBackgroundSymbol, SheathingBackground },

            { BattarySymbol, Battary },
            { BoxSymbol, Box },
            { LadderSymbol, Ladder },
            { ScreenAnimationSymbol, ScreenAnimation },

            { CanonTopSymbol, CanonTop },
            { CanonRightSymbol, CanonRight },
            { CanonBotSymbol, CanonBot },
            { CanonLeftSymbol, CanonLeft },

            { ExhaustPipeTopSymbol, ExhaustPipeTop },
            { ExhaustPipeRightSymbol, ExhaustPipeRight },
            { ExhaustPipeBotSymbol, ExhaustPipeBot },
            { ExhaustPipeLeftSymbol, ExhaustPipeLeft },

            { ExhaustAnimationTopSymbol, ExhaustAnimationTop },
            { ExhaustAnimationRightSymbol, ExhaustAnimationRight },
            { ExhaustAnimationBotSymbol, ExhaustAnimationBot },
            { ExhaustAnimationLeftSymbol, ExhaustAnimationLeft }
        };

        TextAsset ShipMapText = Resources.Load(ShipMapResourceName) as TextAsset;
        string[] layers = ShipMapText.text.Replace("\r\n", "").Split(ShipMapLayerSeparator);

        for (int l = 0; l < layers.Length; l++)
        {
            string[] lines = layers[l].Split(ShipMapLineSeparator);
            for (int i = 0; i < lines.Length; i++)
                for (int j = 0; j < lines[i].Length; j++)
                    if (TileSet.ContainsKey(lines[i][j]))
                    {
                        Instantiate(
                            TileSet[lines[i][j]],
                            new Vector3(TopLeftX + j * TileWidth, TopLeftY - i * TileHeight, 0),
                            new Quaternion()
                        );
                    }
        }
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
}
