using UnityEngine;
/// <summary>
/// this script is to test pop up boxes and how they look in VR.
/// this is so we can get a preliminary idea of things we need to change 
/// regarding UI before playtest
/// </summary>
public class TextTestNew : MonoBehaviour
{
    // This game object is referencing the textbox itself
    public GameObject textBox;
    //this bool turns on and off the set active for the textbox
    public bool textAppear;

    /// <summary>
    /// the textbox starts turned off
    /// the bool starts false
    /// </summary>
    private void Awake()
    {
        textAppear = false;
        textBox.SetActive(false);
    }

    /// <summary>
    /// for now, this will call the pup up function
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        //PopUp();
    }

    /// <summary>
    /// when the player touches the collider object that the script is attached to,
    /// it will turn the bool to true
    /// </summary>
    /// <param name="collision"></param>
    /*private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Test")
        {
            textAppear = true;
        }
    }*/

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Test")
        {
            textAppear = true;
            textBox.SetActive(true);
        }
    }

    /// <summary>
    /// this will turn the text box on and off depending on the state of the bool
    /// </summary>
    /*private void PopUp()
    {
        if(textAppear == true)
        {
            textBox.SetActive(true);
        }
    }*/
}
