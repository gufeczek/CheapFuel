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

class FuelStationMarker(res: Resources, text: CharSequence) : Drawable() {
    private val defaultColor = Color.WHITE;
    private val defaultTextSize = 15;

    private var mPaint: Paint
    private var mText: CharSequence
    private var mIntrinsicWidth = 0
    private var mIntrinsicHeight = 0

    init {
        mText = text
        mPaint = Paint(Paint.ANTI_ALIAS_FLAG)
        mPaint.color = defaultColor
        mPaint.textAlign = Paint.Align.CENTER
        var textSize = TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_SP,
            defaultTextSize.toFloat(), res.displayMetrics)
        mPaint.textSize = textSize
        mIntrinsicWidth = (mPaint.measureText(mText, 0, mText.length) + .5).toInt()
        mIntrinsicHeight = mPaint.getFontMetricsInt(null)
    }


    override fun draw(canvas: Canvas) {
        val bounds: Rect = bounds
//        canvas.drawRect(mIntrinsicHeight.toFloat(), mIntrinsicHeight.toFloat(), mIntrinsicWidth.toFloat() + mIntrinsicHeight.toFloat(), mIntrinsicWidth.toFloat() + mIntrinsicHeight.toFloat(), mPaint)
        mPaint.textAlign = Paint.Align.CENTER

//        val rect = Rect(
//            bounds.centerX() - (mIntrinsicWidth / 2),
//            bounds.centerY() - mPaint.textSize.toInt(),
//            mIntrinsicWidth + bounds.centerX()  - (mIntrinsicWidth / 2),
//            mIntrinsicHeight + bounds.centerY() - mPaint.textSize.toInt())
//        canvas.drawRect(rect, mPaint)
        val borderPaint = Paint().apply {
            color = Color.GRAY
            isAntiAlias = true
            pathEffect = CornerPathEffect(16F)
            strokeCap = Paint.Cap.ROUND
            style = Paint.Style.STROKE
            strokeWidth = 3F
        }
        drawMarker(canvas, borderPaint)

        val fillPaint = Paint().apply {
            color = Color.WHITE
            isAntiAlias = true
            pathEffect = CornerPathEffect(16F)
            strokeCap = Paint.Cap.ROUND
            style = Paint.Style.FILL
        }
        drawMarker(canvas, fillPaint)

        mPaint.color = Color.BLUE
        canvas.drawText(mText, 0, mText.length, bounds.centerX().toFloat(), bounds.centerY().toFloat(), mPaint)
    }

    private fun drawMarker(canvas: Canvas, paint: Paint) {
        val path = Path()

        val marginLeftRight = 30f
        val marginTopBottom = 20f

        val xStart = (bounds.centerX() - (mIntrinsicWidth / 2)).toFloat() - marginLeftRight
        val yStart = (bounds.centerY() - mPaint.textSize.toInt()).toFloat() - marginTopBottom
        val xEnd = (mIntrinsicWidth + bounds.centerX()  - (mIntrinsicWidth / 2)).toFloat() + marginLeftRight
        val yEnd = (mIntrinsicHeight + bounds.centerY() - mPaint.textSize.toInt()).toFloat() + marginTopBottom

        path.moveTo(xStart, yStart)
        path.lineTo(xEnd, yStart)
        path.lineTo(xEnd, yEnd)
        path.lineTo(xStart, yEnd)
        path.lineTo(xStart, yStart)
        path.close()

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