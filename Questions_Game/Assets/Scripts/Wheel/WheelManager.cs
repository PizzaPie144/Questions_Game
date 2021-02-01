using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PizzaPie.QuestionsGame.Events;


using System.Threading.Tasks;

namespace PizzaPie.QuestionsGame.Wheel
{
    public class WheelManager : MonoBehaviour, ISubscriber<States.StateChangedEventArgs>, ISubscriber<StopWheelButtonEventArgs>
    {
        [SerializeField]
        private float spinSpeed;
        [SerializeField]
        private float spinDeccelaration;
        [SerializeField]
        private float offset = .9f;
        [SerializeField]
        private GameObject wheel;
        [SerializeField]
        private int pixelsPerUnit = 200;
        [SerializeField]
        private Color tintColor;
        [SerializeField]
        private float exitDelay;
        [SerializeField]
        private AudioClip wheelSpinningClip;
        [SerializeField]
        private AudioClip wheelStopClip;

        private bool stop;

        private SpriteRenderer spriteRenderer;
        private Material mat;
        private Texture2D texture;
        private int size;

        private List<Questions.QuestionCategory> categories;
        
        private const string TARGET_COLOR = "_TargetColor";
        private const string TINT_COLOR = "_ColorTint";

        private void Start()
        {
            spriteRenderer = wheel.GetComponent<SpriteRenderer>();
            mat = spriteRenderer.material;
            Init();
            wheel.SetActive(false);
            Services.Instance.EventAggregator.Subscribe<States.StateChangedEventArgs>(this);
            Services.Instance.EventAggregator.Subscribe<StopWheelButtonEventArgs>(this);
        }

        private void Init()
        {
            mat.SetColor(TINT_COLOR, tintColor);
            texture = InitTexture();
            spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit, 50, SpriteMeshType.FullRect);
        }

        #region Event Handlers

        public void Handler(object sender, States.StateChangedEventArgs e)
        {
            if (e.StateType != States.StateType.WHEEL)
                return;

            wheel.transform.eulerAngles = Vector3.zero;
            stop = false;
            List<Color> colors;
            var percentages =  GeneratePercentages(Services.Instance.QuestionsProvider.GetQuestionsRemainingCounts(), out colors);

            UpdateTexture(percentages, colors);
            wheel.SetActive(true);
            StartCoroutine(SpinRoutine(percentages, colors));
        }

        public void Handler(object sender, StopWheelButtonEventArgs e)
        {
            stop = true;
        }

        #endregion

        private IEnumerator SpinRoutine(List<float> percentages, List<Color> colors)
        {
            Services.Instance.SoundService.PlayClip(wheelSpinningClip, QuestionsGame.Sound.AudioType.SOUND_FX, true);

            var angle = 0f;
            var spinSpeed = this.spinSpeed;
            int currentIndex = 0;

            while (spinSpeed >= 0.1f)
            {
                if (stop)
                    spinSpeed -= spinDeccelaration;

                angle += spinSpeed * Time.deltaTime;
                wheel.transform.eulerAngles += new Vector3(0, 0, 1) * spinSpeed * Time.deltaTime;

                var minAngle = 0f;
                var maxAngle = 0f;
                
                for (int i = 0; i < percentages.Count; i++)
                {
                    minAngle = maxAngle;
                    maxAngle += percentages[i] * 360;
                    if (minAngle < angle && angle < maxAngle)
                    {
                        mat.SetColor(TARGET_COLOR, colors[i]);
                        currentIndex = i;
                        break;
                    }
                }
                angle %= 360;
                yield return null;

            }

            Services.Instance.SoundService.PlayClip(wheelStopClip, QuestionsGame.Sound.AudioType.SOUND_FX, false);

            yield return new WaitForSeconds(exitDelay);

            var cocurrentRoutine = new Unity.Utils.CocurrentRoutineHandler();
            cocurrentRoutine.AddOnFinishCallback(OnExit);

            Services.Instance.EventAggregator.Invoke(this, new WheelStopSpinEventArgs(categories[currentIndex], cocurrentRoutine));
            cocurrentRoutine.Start();
        }

        private void OnExit()
        {
            wheel.gameObject.SetActive(false);
        }

        private List<float> GeneratePercentages(Dictionary<Questions.QuestionCategory, int> remainingQuestionsCounts, out List<Color> colors)
        {
            float total = 0;
            List<float> percentages = new List<float>();
            
            categories = new List<Questions.QuestionCategory>();

            colors = new List<Color>();
            
            foreach (var remaining in remainingQuestionsCounts.Values)
                total += remaining;

            foreach(var kvp in remainingQuestionsCounts)
            {
                colors.Add(Services.Instance.QuestionsProvider.GetCategoryDefinition(kvp.Key).Color);
                categories.Add(kvp.Key);

                if(total != 0)
                    percentages.Add(kvp.Value / total);
            }

            return percentages;
        }

        #region Wheel Texture
        private Texture2D InitTexture()
        {
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float camHalfHeight = Camera.main.orthographicSize;
            float camHalfWidth = screenAspect * camHalfHeight;
            float camHeight = 2f * camHalfHeight;
            float camWidth = 2f * camHalfWidth;

            float refSize = 0f;
            if (camWidth < camHeight)
                refSize = camWidth;
            else
            {
                refSize = camHeight /2f;
            }

            size = (int)((float)refSize * offset * (float)pixelsPerUnit);
            
            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, Color.clear);
            tex.Resize(size, size);
            return tex;
        }

        private void UpdateTexture(List<float> percentages, List<Color> colors)
        {
            GenerateWheelTexture(texture,percentages, colors);
        }

        private async void GenerateWheelTexture(Texture2D texture,List<float> percentages,List<Color> colors)
        {
            int size = texture.width;
            Color[] texRaw = await Task.Run<Color[]>(() => GenenerateTexData(size, size, size/2, percentages, colors));
            texture.SetPixels(texRaw);
            texture.Apply();
        }

        private Color[] GenenerateTexData(int width, int height, int radius, List<float> percentages, List<Color> palette)
        {
            Color[] tex = new Color[width * height];

            Vector2 up = Vector2.up;
            Vector2 center = Vector2.one * Mathf.FloorToInt((float)width / 2f);

            var radiusSqr = Mathf.Pow(((float)radius), 2);

            int index = 0;

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (Mathf.Pow(((float)i) - center.x, 2) + Mathf.Pow(((float)j) - center.y, 2) <= radiusSqr)
                    {
                        var x = (float)i - center.x;
                        var y = (float)j - center.y;
                        var angle = Mathf.Atan2(x * up.y - y * up.x, x * up.x + y * up.y) * 180 / Mathf.PI;
                        angle += 180;
                        var minAngle = 0f;
                        var maxAngle = 0f;

                        for (int k = 0; k < percentages.Count; k++)
                        {
                            if (k != 0)
                                minAngle = maxAngle;

                            maxAngle += 360 * percentages[k];
                            if (minAngle < angle && maxAngle > angle)
                            {
                                tex[index] = palette[k];
                                break;
                            }
                        }
                    }
                    else
                        tex[index] = Color.clear;
                    index++;

                }
            }

            return tex;
        }
        #endregion
    }
}
