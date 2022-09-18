package com.example.fuel;

import android.graphics.drawable.ColorDrawable;
import android.graphics.drawable.GradientDrawable;
import android.view.View;

import androidx.annotation.NonNull;

import lombok.AccessLevel;
import lombok.NoArgsConstructor;

@NoArgsConstructor(access = AccessLevel.PRIVATE)
public class CornerDrawable extends GradientDrawable {

    private float percent;

    public static void roundCorners(View view, float percent) {
        CornerDrawable drawable = new CornerDrawable();
        drawable.setCornerRadiusPercent(percent);
        drawable.setColor(getColorFromView(view));
        view.setBackground(drawable);
    }

    private static int getColorFromView(View view) {
        ColorDrawable colorDrawable = (ColorDrawable) view.getBackground();
        return colorDrawable.getColor();
    }

    private void setCornerRadiusPercent(float percent) {
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