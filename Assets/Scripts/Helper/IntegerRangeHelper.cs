using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Hackman {
    public static class IntegerRangeHelper {

        /// <summary>
        /// startからendまでにある整数を取得する
        /// <para>(v1=13.0, v2=14.0) ==> [13, 14]</para>
        /// <para>(v1=13.5, v2=14.0) ==> [14]</para>
        /// <para>(v1=13.0, v2=14.5) ==> [13, 14]</para>
        /// <para>(v1=13.5, v2=14.5) ==> [14]</para>
        /// <para>(v1=13.5, v2=13.8) ==> []</para>
        /// </summary>
        /// <param name="start">開始</param>
        /// <param name="end">終了</param>
        /// <returns>startからendまでの間に含まれるすべての整数が、昇順で格納された配列</returns>
        public static int[] GetIntegerRange(float value1, float value2, float eps = 1e-6f) {
            float start = Mathf.Min(value1, value2);
            float end = Mathf.Max(value1, value2);
            int startInteger = Mathf.CeilToInt(start - eps);
            int endInteger = Mathf.FloorToInt(end + eps);
            return Enumerable.Range(startInteger, Mathf.Max(0, endInteger - startInteger + 1)).ToArray();
        }

        /// <summary>
        /// startからendまでにある整数を取得する
        /// <para>(v1=13.0, v2=14.0) ==> [13]</para>
        /// <para>(v1=13.5, v2=14.0) ==> [13]</para>
        /// <para>(v1=13.0, v2=14.5) ==> [13, 14]</para>
        /// <para>(v1=13.5, v2=14.5) ==> [13, 14]</para>
        /// <para>(v1=13.5, v2=13.8) ==> [13]</para>
        /// </summary>
        /// <param name="start">開始</param>
        /// <param name="end">終了</param>
        /// <returns>startからendまでの間に含まれるすべての整数が、昇順で格納された配列</returns>
        public static int[] GetIntegerRange2(float value1, float value2, float eps = 1e-6f) {
            float start = Mathf.Min(value1, value2);
            float end = Mathf.Max(value1, value2);
            int startInteger = Mathf.FloorToInt(start + eps);
            int endInteger = Mathf.CeilToInt(end - eps);
            return Enumerable.Range(startInteger, Mathf.Max(0, endInteger - startInteger)).ToArray();
        }

        /// <summary>
        /// startからendまでにある整数を取得する
        /// <para>(v1=13.0, v2=14.0) ==> [13, 14]</para>
        /// <para>(v1=13.5, v2=14.0) ==> [13, 14]</para>
        /// <para>(v1=13.0, v2=14.5) ==> [13, 14]</para>
        /// <para>(v1=13.5, v2=14.5) ==> [13, 14]</para>
        /// <para>(v1=13.5, v2=13.8) ==> [13]</para>
        /// </summary>
        /// <param name="start">開始</param>
        /// <param name="end">終了</param>
        /// <returns>startからendまでの間に含まれるすべての整数が、昇順で格納された配列</returns>
        public static int[] GetIntegerRangeWide(float value1, float value2, float eps = 1e-6f) {
            float start = Mathf.Min(value1, value2);
            float end = Mathf.Max(value1, value2);
            int startInteger = Mathf.FloorToInt(start + eps);
            int endInteger = Mathf.FloorToInt(end + eps);
            return Enumerable.Range(startInteger, endInteger - startInteger + 1).ToArray();
        }

        public static bool IsRange(this int value, int border1, int border2) {
            int minBorder = Mathf.Min(border1, border2);
            int maxBorder = Mathf.Max(border1, border2);
            return minBorder <= value && value <= maxBorder;
        }

        /// <summary>
        ///     任意の座標が、マップ上のどのタイル座標に相当するかを返す
        /// </summary>
        /// <param name="pos">対象の座標</param>
        /// <returns>相当するタイル座標</returns>
        public static IEnumerable<Vector2Int> GetIntegerPositions(Vector2 pos)
        {
            var ix = IntegerRangeHelper.GetIntegerRange2(pos.x, pos.x + 1f);
            var iy = IntegerRangeHelper.GetIntegerRange2(pos.y, pos.y + 1f);
            return ix.Select(x => iy.Select(y => new Vector2Int(x, y))).SelectMany(x => x).ToArray();
        }
    }
}
