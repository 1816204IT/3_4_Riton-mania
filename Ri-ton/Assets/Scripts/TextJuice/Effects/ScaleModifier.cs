using UnityEngine;

namespace BrunoMikoski.TextJuicer.Effects
{
    [AddComponentMenu("UI/Text Juicer/Effects/Scale")]
    public class ScaleModifier : VertexModifier
    {
        [SerializeField]
        private AnimationCurve curve = new AnimationCurve(new Keyframe(0, 1));
        public override void Apply(CharacterData characterData, ref UIVertex uiVertex)
        {
            uiVertex.position.y = curve.Evaluate(characterData.progress)*uiVertex.position.y;
            uiVertex.position.x = curve.Evaluate(characterData.progress) *uiVertex.position.x;
        }
    }
}