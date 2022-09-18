package com.example.fuel;

import android.graphics.drawable.GradientDrawable;

import androidx.annotation.NonNull;

import lombok.AccessLevel;
import lombok.NoArgsConstructor;

@NoArgsConstructor(access = AccessLevel.PRIVATE)
public class CornerDrawable extends GradientDrawable {

    private float percent;

    public static CornerDrawable getInstance(float percent, int color) {
        CornerDrawable drawable = new CornerDrawable();
        drawable.setCornerRadiusPercent(percent);
        drawable.setColor(color);
        return drawable;
    }

    public void setCornerRadiusPercent(float percent) {
        this.percent = 100 / percent;
    }

    @Override
    public void setBounds(int left, int top, int right, int bottom) {
        super.setBounds(left, top, right, bottom);
        setCornerRadius(getBounds().height() / percent);
    }

    @Override
    public void setBounds(@NonNull android.graphics.Rect bounds) {
        super.setBounds(bounds);
        setCornerRadius(getBounds().height() / percent);
    }


}