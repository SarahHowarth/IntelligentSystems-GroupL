using UnityEngine;

public class MayhemDemo : MonoBehaviour {
    CarFactory cf;

	void Start ()
    {
        cf = gameObject.GetComponent<CarFactory>();

        // Increase position by car size + .1f
        float incX = .6f;
        float incY = .6f;

        // Get screen bounds
        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Screen.width / Screen.height;

        // Left side, from the bottom up
        float posX = -camWidth - .55f;
        float posY = -camHeight + .5f;
        Quaternion rot = Quaternion.LookRotation(Vector3.forward);
        for (int i=0; i<16; i++)
        {
            GenerateCarLoop(posX, posY, rot);
            posY += incY;
        }

        // Top side, left to right
        posX = -camWidth + .5f;
        posY = camHeight + .55f;
        rot = Quaternion.Euler(0f, 0f, -90f);
        for (int i = 0; i < 29; i++)
        {
            GenerateCarLoop(posX, posY, rot);
            posX += incX;
        }

        // Right side, from the top down
        posX = camWidth + .55f;
        posY = camHeight - .5f;
        rot = Quaternion.Euler(0f, 0f, 180f);
        for (int i = 0; i < 16; i++)
        {
            GenerateCarLoop(posX, posY, rot);
            posY -= incY;
        }

        // Bottom side, right to left
        posX = camWidth - .5f;
        posY = -camHeight - .55f;
        rot = Quaternion.Euler(0f, 0f, 90f);
        for (int i = 0; i < 29; i++)
        {
            GenerateCarLoop(posX, posY, rot);
            posX -= incX;
        }
    }

    private void GenerateCarLoop(float posX, float posY, Quaternion rot)
    {
        Transform car = cf.GenerateCar(new Vector2(posX, posY), rot);
        car.gameObject.AddComponent<CarMoveMayhem>();
    }
}
