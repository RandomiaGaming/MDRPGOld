using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationHelper : MonoBehaviour
{
    private void Start()
    {

    }
    public static Vector2 DegToVector(float input)
    {
        input = DegClamp(input);
        float y = 0;
        float x = 0;
        if (DegDistance(input, 90) < DegDistance(input, 270))
        {
            x = DegDistance(input, 90) / 90;
        }
        else
        {
            x = DegDistance(input, 270) / 90;
            x *= -1;
        }
        y = 1 - Mathf.Abs(x);
        if (input <= 360 && input >= 180)
        {
            y *= -1;
        }
        return new Vector2(x, y);
    }
    public static float DegDistance(float a, float b)
    {
        a = DegClamp(a);
        b = DegClamp(b);
        if ((Mathf.Max(a, b) - Mathf.Min(a, b)) <= 180)
        {
            return Mathf.Max(a, b) - Mathf.Min(a, b);
        }
        else
        {
            return (360 - Mathf.Max(a, b)) + Mathf.Min(a, b);
        }
    }
    public static Vector2 VectorClamp(Vector2 input)
    {
        float x = Mathf.Abs(input.x) / (Mathf.Abs(input.x) + Mathf.Abs(input.y));
        return new Vector2(x * Mathf.Sign(input.x), (1 - x) * Mathf.Sign(input.y));
    }
    public static float DegClamp(float input)
    {
        while (input < 0 || input >= 360)
        {
            input -= 360 * Mathf.Sign(input);
        }
        return input;
    }
    public static float VectorToDeg(Vector2 input)
    {
        input = VectorClamp(input);
        float d = input.x * -90;
        if (Mathf.Sign(input.y) == -1)
        {
            d = 180 - d;
        }
        return DegClamp(d);
    }
}
