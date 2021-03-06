﻿#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;

namespace UltraStar.Core.Unmanaged.FFmpeg
{
    /// <summary>
    /// Represents errors that occur within a device.
    /// </summary>
    public class FFmpegException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FFmpegException" /> class.
        /// </summary>
        /// <remarks>
        /// This constructor initializes the <see cref="Exception.Message" /> property of the new instance 
        /// to a system-supplied message that describes the error and takes into account the 
        /// current system culture.<br />The following table shows the initial property values for an instance 
        /// of <see cref="FFmpegException" />.
        /// <list type="table">
        /// <listheader>
        /// <term>Property</term>
        /// <description>Value</description>
        /// </listheader>
        /// <item>
        /// <term><see cref="Exception.InnerException" /></term>
        /// <description><see langword="null" /></description>
        /// </item>
        /// <item>
        /// <term><see cref="Exception.Message" /></term>
        /// <description>A system-supplied localized description.</description>
        /// </item>
        /// </list>
        /// </remarks>
        public FFmpegException() : base()
        {
            ErrorCode = int.MinValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FFmpegException" /> 
        /// class with a specified error message.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <remarks>
        /// This constructor initializes the <see cref="Exception.Message" /> property of the new instance 
        /// using the <paramref name="message" /> parameter.<br />The following table shows the 
        /// initial property values for an instance of <see cref="FFmpegException" />.
        /// <list type="table">
        /// <listheader>
        /// <term>Property</term>
        /// <description>Value</description>
        /// </listheader>
        /// <item>
        /// <term><see cref="Exception.InnerException" /></term>
        /// <description><see langword="null" /></description>
        /// </item>
        /// <item>
        /// <term><see cref="Exception.Message" /></term>
        /// <description>The error message string.</description>
        /// </item>
        /// </list>
        /// </remarks>
        public FFmpegException(string message) : base(message)
        {
            ErrorCode = int.MinValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FFmpegException" /> class 
        /// with a specified error message and a reference to the inner exception that is the cause of 
        /// this exception.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter 
        /// is not <see langword="null" />, the current exception is raised in a catch block that handles 
        /// the inner exception.
        /// </param>
        /// <remarks>
        /// An exception that is thrown as a direct result of a previous exception should include 
        /// a reference to the previous exception in the <see cref="Exception.InnerException" /> property. The 
        /// <see cref="Exception.InnerException" /> 
        /// property returns the same value that is passed into the constructor, or <see langword="null" /> 
        /// if the <see cref="Exception.InnerException" /> property does not supply the inner exception value to the constructor. 
        /// <br />The following table shows the initial property values for an instance of <see cref="FFmpegException" />.
        /// <list type="table">
        /// <listheader>
        /// <term>Property</term>
        /// <description>Value</description>
        /// </listheader>
        /// <item>
        /// <term><see cref="Exception.InnerException" /></term>
        /// <description>The inner exception reference.</description>
        /// </item>
        /// <item>
        /// <term><see cref="Exception.Message" /></term>
        /// <description>The error message string.</description>
        /// </item>
        /// </list>
        /// </remarks>
        public FFmpegException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = int.MinValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FFmpegException" /> class.
        /// </summary>
        /// <param name="errorCode">
        /// The original FFmpeg error code.
        /// </param>
        /// <remarks>
        /// This constructor initializes the <see cref="Exception.Message" /> property of the new instance 
        /// to a system-supplied message that describes the error and takes into account the 
        /// current system culture.<br />The following table shows the initial property values for an instance 
        /// of <see cref="FFmpegException" />.
        /// <list type="table">
        /// <listheader>
        /// <term>Property</term>
        /// <description>Value</description>
        /// </listheader>
        /// <item>
        /// <term><see cref="Exception.InnerException" /></term>
        /// <description><see langword="null" /></description>
        /// </item>
        /// <item>
        /// <term><see cref="Exception.Message" /></term>
        /// <description>A system-supplied localized description.</description>
        /// </item>
        /// </list>
        /// </remarks>
        public FFmpegException(int errorCode) : base("The FFmpeg library returned an error: " + FFmpeg.AVGetErrorDescription(errorCode) + "("+ errorCode.ToString() + ").")
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FFmpegException" /> 
        /// class with a specified error message.
        /// </summary>
        /// <param name="errorCode">
        /// The original FFmpeg error code.
        /// </param>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <remarks>
        /// This constructor initializes the <see cref="Exception.Message" /> property of the new instance 
        /// using the <paramref name="message" /> parameter.<br />The following table shows the 
        /// initial property values for an instance of <see cref="FFmpegException" />.
        /// <list type="table">
        /// <listheader>
        /// <term>Property</term>
        /// <description>Value</description>
        /// </listheader>
        /// <item>
        /// <term><see cref="Exception.InnerException" /></term>
        /// <description><see langword="null" /></description>
        /// </item>
        /// <item>
        /// <term><see cref="Exception.Message" /></term>
        /// <description>The error message string.</description>
        /// </item>
        /// </list>
        /// </remarks>
        public FFmpegException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FFmpegException" /> class 
        /// with a specified error message and a reference to the inner exception that is the cause of 
        /// this exception.
        /// </summary>
        /// <param name="errorCode">
        /// The original FFmpeg error code.
        /// </param>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter 
        /// is not <see langword="null" />, the current exception is raised in a catch block that handles 
        /// the inner exception.
        /// </param>
        /// <remarks>
        /// An exception that is thrown as a direct result of a previous exception should include 
        /// a reference to the previous exception in the <see cref="Exception.InnerException" /> property. The 
        /// <see cref="Exception.InnerException" /> 
        /// property returns the same value that is passed into the constructor, or <see langword="null" /> 
        /// if the <see cref="Exception.InnerException" /> property does not supply the inner exception value to the constructor. 
        /// <br />The following table shows the initial property values for an instance of <see cref="FFmpegException" />.
        /// <list type="table">
        /// <listheader>
        /// <term>Property</term>
        /// <description>Value</description>
        /// </listheader>
        /// <item>
        /// <term><see cref="Exception.InnerException" /></term>
        /// <description>The inner exception reference.</description>
        /// </item>
        /// <item>
        /// <term><see cref="Exception.Message" /></term>
        /// <description>The error message string.</description>
        /// </item>
        /// </list>
        /// </remarks>
        public FFmpegException(int errorCode, string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        public int ErrorCode { get; }
    }
}
