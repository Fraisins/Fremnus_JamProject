using System.Collections.Generic;
using Project.Game.Robot;
using UnityEngine;

namespace Project.Game.Other
{
    public class RobotsSpinning : MonoBehaviour
    {
        [Header("Rotation")]
        [SerializeField] private float rotationSpeed = 15f;
        
        [Header("Robot change")]
        [SerializeField] private List<RobotComponent> robots;
        [SerializeField] private List<Sprite> faceSprites;
        [SerializeField] private float changeDelay = 1f;

        private int _currentRobot;
        private float _elapsedDelayTime;

        private void Start()
        {
            foreach (RobotComponent robotComponent in robots)
            {
                robotComponent.gameObject.SetActive(false);
            }
            SetRobot(0);
        }

        private void Update()
        {
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
            
            _elapsedDelayTime += Time.deltaTime;
            if (_elapsedDelayTime >= changeDelay)
            {
                _elapsedDelayTime = 0f;
                HideRobot(_currentRobot);
                
                _currentRobot++;
                
                if (_currentRobot >= robots.Count)
                    _currentRobot = 0;
                
                SetRobot(_currentRobot);
            }
        }

        private void SetRobot(int robotIndex)
        {
            robots[robotIndex].gameObject.SetActive(true);

            if (faceSprites != null || faceSprites.Count > 0)
            {
                int randFaceIndex = Random.Range(0, faceSprites.Count);
                Sprite faceSprite = faceSprites[randFaceIndex];
                robots[robotIndex].SetFaceTexture(faceSprite);
            }
        }
        
        private void HideRobot(int robotIndex)
        {
            robots[robotIndex].gameObject.SetActive(false);
        }
    }
}