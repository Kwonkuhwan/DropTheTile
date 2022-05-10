using TbsFramework.Units;
using UnityEngine;
using Photon.Pun;
using System.Collections;

namespace TbsFramework.Tutorial
{
    public class SampleUnit : Unit
    {
        public Color LeadingColor;

        public override void Initialize()
        {
            base.Initialize();
            GetComponentInChildren<Renderer>().material.color = LeadingColor;
        }

        public override void MarkAsDestroyed()
        {
        }

        //public override void MarkAsFinished()
        //{
        //}

        //public override void MarkAsFriendly()
        //{
        //    GetComponent<Renderer>().material.color = LeadingColor + new Color(0.8f, 1, 0.8f);
        //}

        //public override void MarkAsReachableEnemy()
        //{
        //    GetComponent<Renderer>().material.color = LeadingColor + Color.red;
        //}

        public override void MarkAsSelected()
        {
            GetComponentInChildren<Renderer>().material.color = LeadingColor + Color.green;
        }

        public override void UnMark()
        {
            GetComponentInChildren<Renderer>().material.color = LeadingColor;
        }
    }
}
