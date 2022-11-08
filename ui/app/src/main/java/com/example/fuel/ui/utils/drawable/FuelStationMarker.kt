package com.example.fuel.ui.utils.drawable

import android.content.res.Resources
import android.graphics.Canvas
import android.graphics.Color
import android.graphics.ColorFilter
import android.graphics.CornerPathEffect
import android.graphics.Paint
import android.graphics.Path
import android.graphics.Rect
import android.graphics.drawable.Drawable
import android.util.TypedValue

class FuelStationMarker(res: Resources, text: CharSequence, textColor: Int, isBold: Boolean) : Drawable() {
    private val defaultTextSize = 15;

    private var mPaint: Paint
    private var mText: CharSequence
    private var mIntrinsicWidth = 0
    private var mIntrinsicHeight = 0

    init {
        mText = text

        mPaint = Paint(Paint.ANTI_ALIAS_FLAG)
        mPaint.color = textColor
        mPaint.isFakeBoldText = isBold
        mPaint.textAlign = Paint.Align.CENTER
        mPaint.textSize  = TypedValue.applyDimension(
            TypedValue.COMPLEX_UNIT_SP,
            defaultTextSize.toFloat(),
            res.displayMetrics)

        mIntrinsicWidth = (mPaint.measureText(mText, 0, mText.length) + .5).toInt()
        mIntrinsicHeight = mPaint.getFontMetricsInt(null)
    }


    override fun draw(canvas: Canvas) {
        val bounds: Rect = bounds

        mPaint.textAlign = Paint.Align.CENTER

        val borderPaint = Paint().apply {
            color = Color.GRAY
            isAntiAlias = true
            pathEffect = CornerPathEffect(10F)
            strokeCap = Paint.Cap.ROUND
            style = Paint.Style.STROKE
            strokeWidth = 3F
        }
        drawMarker(canvas, borderPaint)

        val fillPaint = Paint().apply {
            color = Color.WHITE
            isAntiAlias = true
            pathEffect = CornerPathEffect(10F)
            strokeCap = Paint.Cap.ROUND
            style = Paint.Style.FILL
        }
        drawMarker(canvas, fillPaint)

        canvas.drawText(mText,
            0,
            mText.length,
            bounds.centerX().toFloat(),
            bounds.centerY().toFloat() + mPaint.textSize + (mPaint.textSize / 3),
            mPaint)
    }

    private fun drawMarker(canvas: Canvas, paint: Paint) {
        val path = Path()

        val marginLeftRight = 20f
        val marginTopBottom = 10f

        val xStart = (bounds.centerX() - (mIntrinsicWidth / 2)).toFloat() - marginLeftRight
        val yStart = (bounds.centerY() - mPaint.textSize.toInt()).toFloat() - marginTopBottom
        val xEnd = (mIntrinsicWidth + bounds.centerX()  - (mIntrinsicWidth / 2)).toFloat() + marginLeftRight
        val yEnd = (mIntrinsicHeight + bounds.centerY() - mPaint.textSize.toInt()).toFloat() + marginTopBottom

        val xTriangleCenter = bounds.centerX().toFloat()
        val yTriangleCenter = yEnd + 20
        val xTriangleStart = xTriangleCenter - 20
        val xTriangleEnd = xTriangleCenter + 20

        path.moveTo(xStart, yStart)
        path.lineTo(xEnd, yStart)
        path.lineTo(xEnd, yEnd)
        path.lineTo(xTriangleEnd, yEnd)
        path.lineTo(xTriangleCenter, yTriangleCenter)
        path.lineTo(xTriangleStart, yEnd)
        path.lineTo(xStart, yEnd)
        path.lineTo(xStart, yStart)
        path.close()

        path.offset(0F, bounds.centerY().toFloat() - mPaint.textSize)

        canvas.drawPath(path, paint)
    }

    override fun getIntrinsicHeight(): Int {
        return mIntrinsicHeight
    }

    override fun getIntrinsicWidth(): Int {
        return mIntrinsicWidth
    }

    override fun setAlpha(p0: Int) {
        mPaint.alpha = p0
    }

    override fun setColorFilter(p0: ColorFilter?) {
        mPaint.colorFilter = p0
    }

    override fun getOpacity(): Int {
        return mPaint.alpha
    }
}