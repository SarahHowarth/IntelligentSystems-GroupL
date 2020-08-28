using UnityEngine;

public class CarFactory : MonoBehaviour {

    public Transform prefab;
    private int carNo = 1;

	public Transform GenerateCar(Vector2 position, Quaternion rotation)
    {
        string spriteName = "Car" + carNo.ToString().PadLeft(2,'0');

        // Loop through all ten cars in order
        carNo++;
        if (carNo > 10)
        {
            carNo = 1;
        }

        return InstantiateCar(position, rotation, spriteName);
    }

    public Transform GenerateRandomCar(Vector2 position, Quaternion rotation)
    {
        string spriteName = "8bit-Cars-Lite-Sprite-Sheet_" + UnityEngine.Random.Range(1, 10).ToString();
        return InstantiateCar(position, rotation, spriteName);
    }

    private Transform InstantiateCar(Vector2 position, Quaternion rotation, string spriteName)
    {
        Sprite carSprite = Resources.Load<Sprite>("Sprites/" + spriteName);
        prefab.GetComponent<SpriteRenderer>().sprite = carSprite;
        return Instantiate(prefab, position, rotation);
    }
}
