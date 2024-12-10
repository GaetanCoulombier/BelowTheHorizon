using Godot;

public class ProceduralWorldGenerator
{
    private FastNoiseLite _noise; // Générateur de bruit Perlin/Simplex
    private float _noiseScale; // Échelle du bruit
    private float _threshold; // Seuil de génération des blocs solides

    public ProceduralWorldGenerator(float noiseScale = 0.1f, float threshold = 0.5f, int seed = 0)
    {
        _noise = new FastNoiseLite();
        _noiseScale = noiseScale;
        _threshold = threshold;

        // Définir une graine pour rendre la génération reproductible
        _noise.Seed = seed == 0 ? (int)GD.Randi() : seed;
    }

    public void Generate(World world)
    {
        if (world == null)
        {
            GD.PrintErr("Le monde à générer est nul.");
            return;
        }

        for (int z = 0; z < world.Depth; z++)
        {
            for (int y = 0; y < world.Height; y++)
            {
                for (int x = 0; x < world.Width; x++)
                {
                    // Générer la valeur du bruit pour la position actuelle
                    float noiseValue = _noise.GetNoise3D(x * _noiseScale, y * _noiseScale, z * _noiseScale);

                    // Vérifier si la valeur dépasse le seuil pour placer un bloc solide
                    if (noiseValue > _threshold)
                    {
                        Wall block = new Wall(new Vector3I(x, y, z));
                        world.SetBlock(block);
                    }
                    else
                    {
                        // Optionnel : Ajouter des blocs vides explicites (généralement non nécessaire)
                        world.SetBlock(new Void(new Vector3I(x, y, z)));
                    }
                }
            }
        }
    }
}
