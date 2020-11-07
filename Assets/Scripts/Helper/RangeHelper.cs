using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Helper
{
    public static class RangeHelper
    {
        /// <summary>
        ///     startからendまでにある整数を取得する
        ///     <para>(v1=13.0, v2=14.0) ==> [13, 14]</para>
        ///     <para>(v1=13.5, v2=14.0) ==> [14]</para>
        ///     <para>(v1=13.0, v2=14.5) ==> [13, 14]</para>
        ///     <para>(v1=13.5, v2=14.5) ==> [14]</para>
        ///     <para>(v1=13.5, v2=13.8) ==> []</para>
        /// </summary>
        /// <param name="value1">開始</param>
        /// <param name="value2">終了</param>
        /// <param name="eps">誤差</param>
        /// <returns>startからendまでの間に含まれるすべての整数が、昇順で格納された配列</returns>
        public static IEnumerable<int> GetIntegerRange(float value1, float value2, float eps = 1e-6f)
        {
            var start = Mathf.Min(value1, value2);
            var end = Mathf.Max(value1, value2);
            var startInteger = Mathf.CeilToInt(start - eps);
            var endInteger = Mathf.FloorToInt(end + eps);
            return Enumerable.Range(startInteger, Mathf.Max(0, endInteger - startInteger + 1)).ToArray();
        }

        /// <summary>
        ///     startからendまでにある整数を取得する
        ///     <para>(v1=13.0, v2=14.0) ==> [13]</para>
        ///     <para>(v1=13.5, v2=14.0) ==> [13]</para>
        ///     <para>(v1=13.0, v2=14.5) ==> [13, 14]</para>
        ///     <para>(v1=13.5, v2=14.5) ==> [13, 14]</para>
        ///     <para>(v1=13.5, v2=13.8) ==> [13]</para>
        /// </summary>
        /// <param name="value1">開始</param>
        /// <param name="value2">終了</param>
        /// <param name="eps">誤差</param>
        /// <returns>startからendまでの間に含まれるすべての整数が、昇順で格納された配列</returns>
        public static int[] GetIntegerRange2(float value1, float value2, float eps = 1e-6f)
        {
            var start = Mathf.Min(value1, value2);
            var end = Mathf.Max(value1, value2);
            var startInteger = Mathf.FloorToInt(start + eps);
            var endInteger = Mathf.CeilToInt(end - eps);
            return Enumerable.Range(startInteger, Mathf.Max(0, endInteger - startInteger)).ToArray();
        }

        /// <summary>
        ///     startからendまでにある整数を取得する
        ///     <para>(v1=13.0, v2=14.0) ==> [13, 14]</para>
        ///     <para>(v1=13.5, v2=14.0) ==> [13, 14]</para>
        ///     <para>(v1=13.0, v2=14.5) ==> [13, 14]</para>
        ///     <para>(v1=13.5, v2=14.5) ==> [13, 14]</para>
        ///     <para>(v1=13.5, v2=13.8) ==> [13]</para>
        /// </summary>
        /// <param name="value1">開始</param>
        /// <param name="value2">終了</param>
        /// <param name="eps">誤差</param>
        /// <returns>startからendまでの間に含まれるすべての整数が、昇順で格納された配列</returns>
        public static IEnumerable<int> GetIntegerRangeWide(float value1, float value2, float eps = 1e-6f)
        {
            var start = Mathf.Min(value1, value2);
            var end = Mathf.Max(value1, value2);
            var startInteger = Mathf.FloorToInt(start + eps);
            var endInteger = Mathf.FloorToInt(end + eps);
            return Enumerable.Range(startInteger, endInteger - startInteger + 1).ToArray();
        }

        public static bool IsRange(this int value, int border1, int border2)
        {
            var minBorder = Mathf.Min(border1, border2);
            var maxBorder = Mathf.Max(border1, border2);
            return minBorder <= value && value <= maxBorder;
        }

        /// <summary>
        ///     任意の座標が、マップ上のどのタイル座標に相当するかを返す
        /// </summary>
        /// <param name="pos">対象の座標</param>
        /// <returns>相当するタイル座標</returns>
        public static IEnumerable<Vector2Int> GetIntegerPositions(Vector2 pos)
        {
            var ix = GetIntegerRange2(pos.x, pos.x + 1f);
            var iy = GetIntegerRange2(pos.y, pos.y + 1f);
            return ix.Select(x => iy.Select(y => new Vector2Int(x, y))).SelectMany(x => x).ToArray();
        }

        public static bool IsRange(this Vector2 value, Vector2 border1, Vector2 border2, float eps = 0)
        {
            var min = new Vector2(Mathf.Min(border1.x, border2.x), Mathf.Min(border1.y, border2.y));
            var max = new Vector2(Mathf.Max(border1.x, border2.x), Mathf.Max(border1.y, border2.y));
            return min.x - eps <= value.x && value.x < max.x + eps && min.y - eps <= value.y && value.y < max.y + eps;
        }
    }
}