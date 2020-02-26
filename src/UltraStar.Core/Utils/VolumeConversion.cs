#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;

namespace UltraStar.Core.Utils
{
    /// <summary>
    /// This class provides conversions to different translation curves for volume values.
    /// </summary>
    /// <remarks>
    /// This class currently supports the linear curve and the loudness curve.
    /// The linear curve is usually the default curve when working with audio drivers.
    /// The loudness curve adapts the linear curve values to the perceived loudness.
    /// 
    /// As creating correct calculations for the perceived loudness can quite lengthy,
    /// the approach here was to measure the loudness changes from Windows10 and create
    /// a polinomal function which approximates the measurement.
    /// </remarks>
    internal static class VolumeConversion
    {
        #region Constants

        /// <summary>
        /// Coefficients for the conversion from loudness to linear curve.
        /// </summary>
        private static readonly double[] convertToLinearCoefficients = new double[]
        {
             0.00000000000000E+00,
             6.83177164535032E-04,
             1.26079500834042E-04,
            -4.84930156814667E-07,
             1.56988146008503E-09
        };

        /// <summary>
        /// Coefficients for the conversion from linear to loudness curve.
        /// This produces valid outputs in the range 0.122145 ... 1.
        /// </summary>
        private static readonly double[] convertToLoudnessCoefficientsUpperPart = new double[]
        {
             0.00000000000000E+00,
             3.56430895149707E+02,
            -1.26624393652379E+03,
             3.19018730875849E+03,
            -4.45076602423191E+03,
             3.16833290344476E+03,
            -8.98112340480089E+02
        };

        /// <summary>
        /// Coefficients for the conversion from linear to loudness curve.
        /// This produces valid outputs in the range 0 ... 0.122145.
        /// </summary>
        private static readonly double[] convertToLoudnessCoefficientsLowerPart = new double[]
        {
             0.00000000000000E+00,
             9.24436079828999E+02,
            -3.17665542649030E+04,
             7.59441666347504E+05,
            -9.73594714703369E+06,
             6.20359399711914E+07,
            -1.54320292452148E+08
        };

        #endregion Constants

        /// <summary>
        /// Calculates a value of a one-dimensional polynomial.
        /// </summary>
        /// <param name="x">x-value used for calculations.</param>
        /// <param name="a">Coefficients used for calculations.</param>
        /// <returns>The value of a one-dimensional polynomial.</returns>
        private static double polynomialResult(double x, double[] a)
        {
            double y = 0, xPower = 1;
            foreach (double coeffA in a)
            {
                // Multiply current x with the coefficient
                y += coeffA * xPower;
                // Power up x
                xPower *= x;
            }
            return y;
        }

        /// <summary>
        /// Converts the volume from a loudness curve value to a linear curve value.
        /// </summary>
        /// <param name="loudnessVolume">
        /// The value from the loudness curve.
        /// This parameter should be in the range 0 ... 1; otherwise it will be trim'ed to fit into the range.
        /// The precision for the value is in 0.01 steps.
        /// </param>
        /// <returns>
        /// The converted from the linear curve.
        /// This parameter is in the range 0 ... 1.
        /// The full precision of float can be considered for this value.
        /// </returns>
        public static float ConvertLoudnessToLinear(float loudnessVolume)
        {
            // Adapt float to be in range
            if (loudnessVolume < +0) loudnessVolume = +0;
            if (loudnessVolume > 1) loudnessVolume = 1;
            // Convert
            loudnessVolume *= 100;
            float result = (float)polynomialResult(Math.Round(loudnessVolume), convertToLinearCoefficients);
            if (result < +0) result = +0;
            if (result > 1) result = 1;
            // Return
            return result;
        }

        /// <summary>
        /// Converts the volume from a linear curve value to a loudness curve value.
        /// </summary>
        /// <param name="linearVolume">
        /// The value from the linear curve.
        /// This parameter should be in the range 0 ... 1; otherwise it will be trim'ed to fit into the range.
        /// The full precision of float can be considered for this value.
        /// </param>
        /// <returns>
        /// The converted from the loudness curve.
        /// This parameter is in the range 0 ... 1.
        /// The precision for the value is in 0.01 steps.
        /// </returns>
        public static float ConvertLinearToLoudness(float linearVolume)
        {
            // Adapt float to be in range
            if (linearVolume < +0) linearVolume = +0;
            if (linearVolume > 1) linearVolume = 1;
            // Convert
            float result;
            if (linearVolume > 0.122145)
                result = (float)polynomialResult(linearVolume, convertToLoudnessCoefficientsUpperPart);
            else
                result = (float)polynomialResult(linearVolume, convertToLoudnessCoefficientsLowerPart);
            result = (float)Math.Round(result) / 100;
            if (result < +0) result = +0;
            if (result > 1) result = 1;
            // Return
            return result;
        }
    }
}
