using UnityEngine;

namespace EzySlice
{
    /**
     * TextureRegion defines a region of a specific texture which can be used
     * for custom UV Mapping Routines.
     * 
     * TextureRegions are always stored in normalized UV Coordinate space between
     * 0.0f and 1.0f
     */
    public struct TextureRegion
    {
        public TextureRegion(float startX, float startY, float endX, float endY)
        {
            this.startX = startX;
            this.startY = startY;
            this.endX = endX;
            this.endY = endY;
        }

        public float startX { get; }

        public float startY { get; }

        public float endX { get; }

        public float endY { get; }

        public Vector2 start => new(startX, startY);
        public Vector2 end => new(endX, endY);

        /**
         * Perform a mapping of a UV coordinate (computed in 0,1 space)
         * into the new coordinates defined by the provided TextureRegion
         */
        public Vector2 Map(Vector2 uv)
        {
            return Map(uv.x, uv.y);
        }

        /**
         * Perform a mapping of a UV coordinate (computed in 0,1 space)
         * into the new coordinates defined by the provided TextureRegion
         */
        public Vector2 Map(float x, float y)
        {
            var mappedX = MAP(x, 0.0f, 1.0f, startX, endX);
            var mappedY = MAP(y, 0.0f, 1.0f, startY, endY);

            return new Vector2(mappedX, mappedY);
        }

        /**
         * Our mapping function to map arbitrary values into our required texture region
         */
        private static float MAP(float x, float in_min, float in_max, float out_min, float out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }
    }

    /**
     * Define our TextureRegion extension to easily calculate
     * from a Texture2D Object.
     */
    public static class TextureRegionExtension
    {
        /**
         * Helper function to quickly calculate the Texture Region from a material.
         * This extension function will use the mainTexture component to perform the
         * calculation.
         * 
         * Will throw a null exception if the texture does not exist. See
         * Texture.getTextureRegion() for function details.
         */
        public static TextureRegion GetTextureRegion(this Material mat,
            int pixX,
            int pixY,
            int pixWidth,
            int pixHeight)
        {
            return mat.mainTexture.GetTextureRegion(pixX, pixY, pixWidth, pixHeight);
        }

        /**
         * Using a Texture2D, calculate and return a specific TextureRegion
         * Coordinates are provided in pixel coordinates where 0,0 is the
         * bottom left corner of the texture.
         * 
         * The texture region will automatically be calculated to ensure that it
         * will fit inside the provided texture.
         */
        public static TextureRegion GetTextureRegion(this Texture tex,
            int pixX,
            int pixY,
            int pixWidth,
            int pixHeight)
        {
            var textureWidth = tex.width;
            var textureHeight = tex.height;

            // ensure we are not referencing out of bounds coordinates
            // relative to our texture
            var calcWidth = Mathf.Min(textureWidth, pixWidth);
            var calcHeight = Mathf.Min(textureHeight, pixHeight);
            var calcX = Mathf.Min(Mathf.Abs(pixX), textureWidth);
            var calcY = Mathf.Min(Mathf.Abs(pixY), textureHeight);

            var startX = calcX / (float)textureWidth;
            var startY = calcY / (float)textureHeight;
            var endX = (calcX + calcWidth) / (float)textureWidth;
            var endY = (calcY + calcHeight) / (float)textureHeight;

            // texture region is a struct which is allocated on the stack
            return new TextureRegion(startX, startY, endX, endY);
        }
    }
}