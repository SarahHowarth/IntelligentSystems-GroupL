using UnityEngine;

public class Demo : MonoBehaviour {
    CarFactory cf;

    void Start () {
        cf = gameObject.GetComponent<CarFactory>();

        float incX = 1f;
        float posX = -4.5f;
        float rot = -45f;

        for (int i=0; i<10; i++)
        {
            GenerateCarLoop(posX, 0f, Quaternion.Euler(0f, 0f, rot));
            posX += incX;
            rot -= 10f;
        }
    }

    private void GenerateCarLoop(float posX, float posY, Quaternion rot)
    {
        cf.GenerateCar(new Vector2(posX, posY), rot);
    }
}
