// Copyright (c) Georg Jung. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FaceAiSharp.Abstractions;
using FaceAiSharp.Extensions;
using SixLabors.ImageSharp;

namespace FaceAiSharp;

public sealed class FaceOnnxDetector : IFaceDetector, IDisposable
{
    private readonly FaceONNX.FaceDetector _faceDetector = new();

    public float ConfidenceThreshold { get; set; } = 0.95f;

    /// <summary>
    /// Gets or sets NonMaxSuppression threshold.
    /// </summary>
    public float NmsThreshold { get; set; } = 0.5f;

    public void Dispose() => _faceDetector.Dispose();

    public float GetFaceAlignmentAngle(IReadOnlyList<PointF> landmarks) => throw new NotSupportedException();

    public IReadOnlyCollection<(RectangleF Box, IReadOnlyList<PointF>? Landmarks, float? Confidence)> Detect(Image image)
    {
        _faceDetector.ConfidenceThreshold = ConfidenceThreshold;
        _faceDetector.NmsThreshold = NmsThreshold;
        var img = image.ToFaceOnnxFloatArray();
        var res = _faceDetector.Forward(img);

        // FaceONNX does not return any confidence values but just lets us define a threshold before.
        // Landmarks detection is performed in a second step.
        static (RectangleF Box, IReadOnlyList<PointF>? Landmarks, float? Confidence) ToReturnType(System.Drawing.Rectangle r)
            => (new RectangleF(r.X, r.Y, r.Width, r.Height), null, null);

        return res.Select(ToReturnType).ToList();
    }
}
