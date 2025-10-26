using UnityEngine;

namespace KarmelitaPrime;

public class SpriteChanger
{
    private tk2dSprite bindSprite;
    private Texture2D[] currentTextures;
    
    public static SpriteChanger Initialize(tk2dSprite sprite, Texture2D[] textures)
    {
        SpriteChanger instance = new SpriteChanger(sprite, textures);
        instance.OverrideTextures(textures);
        return instance;
    }

    public void OverrideTextures(Texture2D[] overrideTextures)
    {
        var collection = bindSprite.Collection;
        collection.materials[0].mainTexture = overrideTextures[0];
        collection.materials[1].mainTexture = overrideTextures[1]; 
    }
    
    private SpriteChanger(tk2dSprite sprite, Texture2D[] textures)
    {
        bindSprite = sprite;
        currentTextures = textures;
    }
}