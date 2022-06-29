using Photon.Bolt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TagColors { err, red, green, blue, magenta, yellow }
//public static class TagRange { public const int firstIndex = 0; public const int lastIndex = 5; }

public class Box : EntityBehaviour<IPickableItem>
{
    [SerializeField]
    List<GameObject> boxes;

    List<MeshRenderer> tags;

    [SerializeField]
    Material red, green, blue, magenta, yellow;

    private TagColors currentTagColor;

    public TagColors TagColor { get => currentTagColor; set => state.TagColor = (int)value; }

    //public TagColors CurrentTagColor { get => currentTagColor; set => ChangeColor(value); }

    private void Awake()
    {
        tags = new List<MeshRenderer>();

        foreach (var b in boxes)
        {
            foreach (var mesh in b.GetComponentsInChildren<MeshRenderer>())
            {
                if (mesh.gameObject.CompareTag("boxTag"))
                {
                    tags.Add(mesh.GetComponent<MeshRenderer>());
                }
            }
        }

        //red = Resources.Load<Material>("Boxes/tag_red.mat");
        //green = Resources.Load<Material>("Boxes/tag_green.mat");
        //blue = Resources.Load<Material>("Boxes/tag_blue.mat");
        //magenta = Resources.Load<Material>("Boxes/tag_magenta.mat");
        //yellow = Resources.Load<Material>("Boxes/tag_yellow.mat");
    }

    void Start()
    {
        boxes[Random.Range(0, boxes.Count)].SetActive(true);
        //CurrentTagColor = (TagColors)Random.Range(0,5);
    }

    private void ChangeColor()
    {
        currentTagColor = (TagColors)state.TagColor;
        Debug.LogError("currentTagColor: " + currentTagColor);
        switch (currentTagColor)
        {
            case TagColors.red: ChangeTagColors(red); break; 
            case TagColors.green: ChangeTagColors(green); break; 
            case TagColors.blue: ChangeTagColors(blue); break; 
            case TagColors.magenta: ChangeTagColors(magenta); break; 
            case TagColors.yellow: ChangeTagColors(yellow); break;
            //case TagColors.err: state.TagColor = Random.Range(TagRange.firstIndex+1, TagRange.lastIndex); ChangeColor(); break; 
            case TagColors.err: ChangeTagColors(yellow); break;
        }
    }

    private void ChangeTagColors(Material newColor)
    {
        tags.ForEach(t => t.material = newColor);
    }

    public override void Attached()
    {
        if (entity.IsOwner)
        {
            state.TagColor = (int)currentTagColor;
        }

        state.AddCallback("TagColor", ChangeColor);
    }
}
