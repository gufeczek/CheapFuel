package com.example.fuel.ui.fragment.map;

import android.content.res.Resources;
import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Typeface;
import android.graphics.drawable.BitmapDrawable;

public class Test {
    public static BitmapDrawable getTextIcon(Resources resources, final String pText) {
        final Paint background = new Paint();
        background.setColor(Color.WHITE);
        final Paint p = new Paint();
        p.setTextSize(24);
        p.setColor(Color.BLACK);
        p.setAntiAlias(true);
        p.setTypeface(Typeface.DEFAULT_BOLD);
        p.setTextAlign(Paint.Align.LEFT);
        final int width = (int) (p.measureText(pText) + 0.5f);
        final float baseline = (int) (-p.ascent() + 0.5f);
        final int height = (int) (baseline + p.descent() + 0.5f);
        final Bitmap image = Bitmap.createBitmap(width, height, Bitmap.Config.ARGB_8888);
        final Canvas c = new Canvas(image);
        c.drawPaint(background);
        c.drawText(pText, 0, baseline, p);
        return new BitmapDrawable(resources, image);
    }
}
