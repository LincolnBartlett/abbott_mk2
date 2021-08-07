using System;
using UnityEngine.Assertions.Must;

namespace GameCreator.Core
{
    using System.Collections;
    using UnityEngine;

    [AddComponentMenu("")]
    public class IgniterAxisDown : Igniter
    {
        public string axisName;
        public enum AxisDirection
        {
            Positive,
            Negative
        }
        public AxisDirection direction = AxisDirection.Positive;
        public float inputCooldown;
        private bool axisInUse;
        
#if UNITY_EDITOR
        public new static string NAME = "Input/On Axis Down";
#endif

        private void Update()
        {
            if (Input.GetAxisRaw(axisName) == 1)
            {
                if (direction == AxisDirection.Positive)
                {
                    if (axisInUse == false)
                    {
                        axisInUse = true;
                        this.ExecuteTrigger(gameObject);
                        StartCoroutine(ResetInputAxis());
                    }
                }
            }
            else if (Input.GetAxisRaw(axisName) == -1)
            {
                if (direction == AxisDirection.Negative)
                {
                    if (axisInUse == false)
                    {
                        axisInUse = true;
                        this.ExecuteTrigger(gameObject);
                        StartCoroutine(ResetInputAxis());
                    }
                }
            }

            
        }
        IEnumerator ResetInputAxis()
        {
            yield return new WaitForSeconds(inputCooldown);
            axisInUse = false;
        }
    }
}