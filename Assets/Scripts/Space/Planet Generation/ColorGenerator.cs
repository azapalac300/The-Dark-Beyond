using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator {

    BiomeColorSettings biomeColorSettings;
    private MeshRenderer _renderer;
    private Texture2D texture;

    const int textureResolution = 50;



    public void UpdateColorSettings(ref MeshRenderer renderer, BiomeColorSettings biomeColorSettings)
    {

        this.biomeColorSettings = biomeColorSettings;
        if (texture == null || texture.height != biomeColorSettings.biomes.Length)
        {
            texture = new Texture2D(biomeColorSettings.xResolution, 1);
        }

        Material material = new Material(renderer.sharedMaterial.shader);
        material.SetTexture("Ground", texture);
        renderer.material = material;
        _renderer = renderer;
    }

    public void UpdateElevation(MinMax terrainMinMax)
    {
        _renderer.sharedMaterials[0].SetVector("_elevationVect", new Vector4(terrainMinMax.min, terrainMinMax.max));
    }


    public float CalculateBiome(Vector3 pointOnSphere)
    {
        float heightPercent = (pointOnSphere.y + 1) / 2f;
        float biomeIndex = 0;
        int numBiomes = biomeColorSettings.biomes.Length;

       
        return biomeIndex / Mathf.Max(1, numBiomes - 1);
    }

    public void UpdateColors()
    {
        //Right now biomes only have 1 dimension - height
        //Fix  for biomes having 3 dimensions - height, moisture, temperature
        Color[] colors = new Color[biomeColorSettings.xResolution];
        int colorIndex = 0;
        for (int i = 0; i < biomeColorSettings.xResolution; i++) {
          
                Biome biome = biomeColorSettings.biomes[0];
                Color gradientColor = biome.gradient.Evaluate(i / (colors.Length - 1f));
                colors[colorIndex] = gradientColor;
                colorIndex++;
            
        }
        texture.SetPixels(colors);
        texture.Apply();
        _renderer.sharedMaterials[0].SetTexture("_planetTexture", texture);
         
    }
}
