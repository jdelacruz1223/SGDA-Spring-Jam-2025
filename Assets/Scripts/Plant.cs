using System.Runtime;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Plant : MonoBehaviour
{
    private enum AgeState { Sprout, Mature }
    private AgeState age;
    private GameObject bug; // initialize when plant is matured
    private enum PlantType { Fern } // expandable
    private bool HasBug;
    [SerializeField] private GameObject model; // assign in editor
    

    private void SetModel() {
        if (age == 0) {
            model.SetActive(false);
            return;
        }
        model.SetActive(true);
    }

    private void SpawnBug(AgeState age, bool HasBug) {

    }

    // private Bug SetBugType(PlantType plantType) {
    //     switch (plantType) {
    //         case PlantType.Fern:
    //             //set bug's type to fernbug
    //             return;      
    //     }
    // }

    private void Update() {
        SetModel();
        if (Input.GetKeyDown(KeyCode.L)) {
            age++;
            Debug.Log(age);
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            age--;
            Debug.Log(age);
        }
    }

}
